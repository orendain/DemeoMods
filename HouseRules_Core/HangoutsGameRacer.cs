namespace HouseRules
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Reflection.Emit;
    using Boardgame.Social;
    using Bowser;
    using Bowser.Core;
    using Bowser.GameIntegration;
    using HarmonyLib;
    using HouseRules.Types;
    using Photon.Pun;

    internal static class HangoutsGameRacer
    {
        private static readonly Stopwatch StopWatch = new Stopwatch();

        private static GameStateHobbyShop _gameStateHobbyShop;
        private static GameStateHobbyShopData _coroutineSpinner;

        private static bool _hasJoinedTheRace;
        private static JoinParameters _joinParameters;

        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateHobbyShop), "Start"),
                postfix: new HarmonyMethod(
                    typeof(HangoutsGameRacer),
                    nameof(GameStateHobbyShop_Start_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GroupLaunchTable), "OnStartButtonPressed"),
                prefix: new HarmonyMethod(
                    typeof(HangoutsGameRacer),
                    nameof(GroupLaunchTable_OnStartButtonPressed_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GroupLaunchTable), "OnStartGroupLaunchRPC"),
                prefix: new HarmonyMethod(
                    typeof(HangoutsGameRacer),
                    nameof(GroupLaunchTable_OnStartGroupLaunchRPC_Prefix)));

            harmony.Patch(
                original: AccessTools.Constructor(
                    typeof(SocialProviderParea),
                    new[] { typeof(StringToRoomCode), typeof(Action<ISocialProvider, JoinParameters>) }),
                prefix: new HarmonyMethod(typeof(HangoutsGameRacer), nameof(SocialProviderParea_Constructor_Prefix)));

            harmony.Patch(
                original: AccessTools
                    .Inner(typeof(GameStateMachine), "CreatingGameState").GetTypeInfo()
                    .GetDeclaredMethod("OnJoinedRoom"),
                prefix: new HarmonyMethod(typeof(HangoutsGameRacer), nameof(CreatingGameState_OnJoinedRoom_Prefix)));

            harmony.Patch(
                original: AccessTools
                    .Method(typeof(PlayWithFriendsController).GetNestedType("<TryCreateTheRoom>d__25"), "MoveNext"),
                transpiler: new HarmonyMethod(typeof(HangoutsGameRacer), nameof(PlayWithFriendsController_TryCreateTheRoom_Transpiler)));
        }

        private static void GameStateHobbyShop_Start_Postfix(GameStateHobbyShop __instance, GameStateData baseData)
        {
            _gameStateHobbyShop = __instance;
            _coroutineSpinner = (GameStateHobbyShopData)baseData;
        }

        private static bool GroupLaunchTable_OnStartButtonPressed_Prefix(GroupLaunchTable __instance)
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return true;
            }

            HR.Logger.Msg("[HangoutGameRacer] Attempting to race out of Hangouts as table host by front-loading computation.");
            _hasJoinedTheRace = true;

            var groupId = Guid.NewGuid().ToString();
            var moduleType = FindSelectedModelType(__instance);
            RaceOutOfHangouts(__instance, groupId, moduleType, isTableHost: true);

            return false;
        }

        private static bool GroupLaunchTable_OnStartGroupLaunchRPC_Prefix(GroupLaunchTable __instance, string groupId, int selectedModuleType, int randomIndex)
        {
            StopWatch.Restart();

            if (HR.SelectedRuleset == Ruleset.None)
            {
                return true;
            }

            if (_hasJoinedTheRace)
            {
                return false;
            }

            HR.Logger.Msg("[HangoutGameRacer] Attempting to race out of Hangouts as table non-host.");
            _hasJoinedTheRace = true;

            var moduleType = (GroupLaunchModuleData.ModuleType)selectedModuleType;
            if (moduleType == GroupLaunchModuleData.ModuleType.Random)
            {
                moduleType = (GroupLaunchModuleData.ModuleType)randomIndex;
            }

            RaceOutOfHangouts(__instance, groupId, moduleType, isTableHost: false);
            return false;
        }

        private static bool SocialProviderParea_Constructor_Prefix(SocialProviderParea __instance, Action<ISocialProvider, JoinParameters> onJoinReceived)
        {
            if (!_hasJoinedTheRace)
            {
                return true;
            }

            Traverse.Create(__instance).Field<JoinParameters>("joinParameters").Value = _joinParameters;
            Traverse.Create(__instance).Field<Action<ISocialProvider, JoinParameters>>("onJoinReceived").Value = onJoinReceived;
            return false;
        }

        private static void CreatingGameState_OnJoinedRoom_Prefix()
        {
            _hasJoinedTheRace = false;

            if (!StopWatch.IsRunning)
            {
                return;
            }

            StopWatch.Stop();
            var timeElapsed = StopWatch.Elapsed;
            HR.Logger.Msg($"[HangoutGameRacer] Time to join game from Hangouts: {timeElapsed.Seconds:00}.{timeElapsed.Milliseconds:00}s");
        }

        private static IEnumerable<CodeInstruction> PlayWithFriendsController_TryCreateTheRoom_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_I4 && instruction.operand.ToString().Contains("500"))
                {
                    yield return new CodeInstruction(OpCodes.Ldc_I4, 50);
                    continue;
                }

                yield return instruction;
            }
        }

        private static void RaceOutOfHangouts(GroupLaunchTable groupLaunchTable, string groupId, GroupLaunchModuleData.ModuleType moduleType, bool isTableHost)
        {
            var destination = ConvertModuleTypeToDestination(moduleType);
            var stringToRoomCode = new StringToRoomCode(_coroutineSpinner);
            stringToRoomCode.GetRoomCodeFromString(groupId, delegate(string roomCode)
            {
                _joinParameters = new JoinParameters
                {
                    groupId = GetPareaRoomCode(roomCode),
                    gameId = string.Empty,
                    destination = destination,
                };

                HR.Logger.Msg("[HangoutGameRacer] Pre-fetched room code.");
                PrepareToLeaveHangouts(groupLaunchTable);

                if (isTableHost)
                {
                    // Originally from `GroupLaunchTable.OnStartButtonPressed`.
                    var playersInParty = Traverse.Create(groupLaunchTable).Field<int[]>("playersInParty").Value;
                    var groupLaunchTableData = Traverse.Create(groupLaunchTable).Field<GroupLaunchTableData>("data").Value;
                    groupLaunchTableData.photonView.RPC("BowserStartGroupLaunchRPC", RpcTarget.All, playersInParty[0], playersInParty[1], playersInParty[2], playersInParty[3], groupId, (int)moduleType, -1);
                }

                LeaveHangouts();
            });
        }

        private static void PrepareToLeaveHangouts(GroupLaunchTable groupLaunchTable)
        {
            Traverse.Create(groupLaunchTable).Field<bool>("leavingBowser").Value = true;

            // Recreating `GameStateHobbyShop.ExitBowser`
            var teleport = Traverse.Create(_gameStateHobbyShop).Field<Teleport>("teleport").Value;
            teleport.SetTeleportationEnabled(enabled: false);
            Traverse.Create<BowserTriggerHandler>().Method("StopHobbyShopAmbience").GetValue();
        }

        private static void LeaveHangouts()
        {
            PhotonNetwork.SendAllOutgoingCommands();
            BowserIntegration.ExitBowser();
        }

        private static GroupLaunchModuleData.ModuleType FindSelectedModelType(GroupLaunchTable groupLaunchTable)
        {
            var modules = Traverse.Create(groupLaunchTable).Field<GroupLaunchModuleData[]>("modules").Value;
            var selectedModuleIndex = Traverse.Create(groupLaunchTable).Field<int>("selectedModuleIndex").Value;
            var moduleType = modules[selectedModuleIndex].moduleType;
            if (moduleType == GroupLaunchModuleData.ModuleType.Random)
            {
                moduleType = (GroupLaunchModuleData.ModuleType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(GroupLaunchModuleData.ModuleType)).Length);
            }

            return moduleType;
        }

        private static PlayWithFriendsController.Destination ConvertModuleTypeToDestination(GroupLaunchModuleData.ModuleType moduleType)
        {
            switch (moduleType)
            {
                case GroupLaunchModuleData.ModuleType.ElvenQueen:
                    return PlayWithFriendsController.Destination.TheBlackSarcophagus;
                case GroupLaunchModuleData.ModuleType.RatKing:
                    return PlayWithFriendsController.Destination.RealmOfTheRatKing;
                case GroupLaunchModuleData.ModuleType.RootsOfEvil:
                    return PlayWithFriendsController.Destination.RootsOfEvil;
                default:
                    return PlayWithFriendsController.Destination.TheBlackSarcophagus;
            }
        }

        // Recreated from `SocialProviderParea`.
        private static string GetPareaRoomCode(string response)
        {
            return response.Length > 5 ? response.Substring(1, 5) : "12345";
        }
    }
}
