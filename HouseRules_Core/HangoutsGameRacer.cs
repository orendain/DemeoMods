namespace HouseRules
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using Boardgame.Social;
    using Bowser;
    using Bowser.Core;
    using Bowser.GameIntegration;
    using HarmonyLib;
    using Photon.Pun;

    internal static class HangoutsGameRacer
    {
        private static readonly Stopwatch StopWatch = new Stopwatch();

        private static GameStateHobbyShop _gameStateHobbyShop;
        private static GameStateHobbyShopData _coroutineSpinner;

        private static bool _isStartingHangoutsGame;
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
        }

        private static void GameStateHobbyShop_Start_Postfix(GameStateHobbyShop __instance, GameStateData baseData)
        {
            _gameStateHobbyShop = __instance;
            _coroutineSpinner = (GameStateHobbyShopData)baseData;
        }

        private static bool GroupLaunchTable_OnStartButtonPressed_Prefix(GroupLaunchTable __instance)
        {
            HR.Logger.Msg("[HangoutGameRacer] Attempting to race out of Hangouts by front-loading computation.");
            _isStartingHangoutsGame = true;

            var moduleType = FindSelectedModelType(__instance);
            var destination = ConvertModuleTypeToDestination(moduleType);
            var uniqueId = Guid.NewGuid().ToString();

            var stringToRoomCode = new StringToRoomCode(_coroutineSpinner);
            stringToRoomCode.GetRoomCodeFromString(uniqueId, delegate(string roomCode)
            {
                _joinParameters = new JoinParameters();
                _joinParameters.groupId = GetPareaRoomCode(roomCode);
                _joinParameters.gameId = string.Empty;
                _joinParameters.destination = destination;

                HR.Logger.Msg("[HangoutGameRacer] Pre-fetched room code.");
                NotifyPlayersAndLeaveHangouts(__instance, _joinParameters.groupId, moduleType);
            });

            return false;
        }

        private static void NotifyPlayersAndLeaveHangouts(GroupLaunchTable groupLaunchTable, string groupId, GroupLaunchModuleData.ModuleType moduleType)
        {
            HR.Logger.Msg("[HangoutGameRacer] Finally notifying players of race.");
            Traverse.Create(groupLaunchTable).Field<bool>("leavingBowser").Value = true;

            // Recreating `GameStateHobbyShop.ExitBowser`
            var teleport = Traverse.Create(_gameStateHobbyShop).Field<Teleport>("teleport").Value;
            teleport.SetTeleportationEnabled(enabled: false);
            Traverse.Create<BowserTriggerHandler>().Method("StopHobbyShopAmbience").GetValue();

            // Originally from `GroupLaunchTable.OnStartButtonPressed`.
            var playersInParty = Traverse.Create(groupLaunchTable).Field<int[]>("playersInParty").Value;
            var groupLaunchTableData = Traverse.Create(groupLaunchTable).Field<GroupLaunchTableData>("data").Value;
            groupLaunchTableData.photonView.RPC("BowserStartGroupLaunchRPC", RpcTarget.All, playersInParty[0], playersInParty[1], playersInParty[2], playersInParty[3], groupId, (int)moduleType, -1);

            PhotonNetwork.SendAllOutgoingCommands();
            BowserIntegration.ExitBowser();
        }

        private static void GroupLaunchTable_OnStartGroupLaunchRPC_Prefix()
        {
            StopWatch.Restart();
        }

        private static bool SocialProviderParea_Constructor_Prefix(SocialProviderParea __instance, Action<ISocialProvider, JoinParameters> onJoinReceived)
        {
            if (!_isStartingHangoutsGame)
            {
                return true;
            }

            Traverse.Create(__instance).Field<JoinParameters>("joinParameters").Value = _joinParameters;
            Traverse.Create(__instance).Field<Action<ISocialProvider, JoinParameters>>("onJoinReceived").Value = onJoinReceived;
            return false;
        }

        private static void CreatingGameState_OnJoinedRoom_Prefix()
        {
            _isStartingHangoutsGame = false;

            StopWatch.Stop();
            var timeElapsed = StopWatch.Elapsed;
            HR.Logger.Msg($"Time to join game from Hangouts: {timeElapsed.Seconds:00}.{timeElapsed.Milliseconds:00}s");
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
