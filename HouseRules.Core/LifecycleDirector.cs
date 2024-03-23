namespace HouseRules.Core
{
    using System;
    using System.Linq;
    using System.Text;
    using Boardgame;
    using Boardgame.BoardgameActions;
    using Boardgame.Networking;
    using Boardgame.NonVR.Ui.Settings;
    using HarmonyLib;
    using HouseRules.Core.Types;
    using Photon.Pun;
    using Photon.Realtime;
    using UnityEngine;

    internal static class LifecycleDirector
    {
        private const string ModdedRoomPropertyKey = "modded";

        private static float welcomeMessageDurationSeconds = 15f;
        private static GameContext _gameContext;
        private static bool _isCreatingGame;
        private static bool _isLoadingGame;
        private static string _roomCode;
        private static string _lastCode;

        internal static bool IsRulesetActive { get; private set; }

        internal static bool IsReconnect { get; private set; }

        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateMachine), "OnRoomJoined"),
                postfix: new HarmonyMethod(typeof(LifecycleDirector), nameof(GameStateMachine_OnRoomJoined_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(LifecycleDirector), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(CreatingGameState), "TryCreateRoom"),
                prefix: new HarmonyMethod(typeof(LifecycleDirector), nameof(CreatingGameState_TryCreateRoom_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(CreatingGameState), "OnJoinedRoom"),
                prefix: new HarmonyMethod(typeof(LifecycleDirector), nameof(CreatingGameState_OnJoinedRoom_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(PlayingState), "OnMasterClientChanged"),
                prefix: new HarmonyMethod(typeof(LifecycleDirector), nameof(PlayingGameState_OnMasterClientChanged_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateMachine), "GoToPlayingState"),
                postfix: new HarmonyMethod(typeof(LifecycleDirector), nameof(GameStateMachine_GoToPlayingState_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateMachine), "GoToShoppingState"),
                postfix: new HarmonyMethod(typeof(LifecycleDirector), nameof(GameStateMachine_GoToShoppingState_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(PostGameControllerBase), "OnPlayAgainClicked"),
                postfix: new HarmonyMethod(typeof(LifecycleDirector), nameof(PostGameControllerBase_OnPlayAgainClicked_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(GameStateMachine), "EndGame"),
                prefix: new HarmonyMethod(typeof(LifecycleDirector), nameof(GameStateMachine_EndGame_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "DisconnectLocalPlayer"),
                prefix: new HarmonyMethod(typeof(LifecycleDirector), nameof(SerializableEventQueue_DisconnectLocalPlayer_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(ReconnectState), "OnClickLeaveGameAfterReconnect"),
                postfix: new HarmonyMethod(typeof(LifecycleDirector), nameof(ReconnectState_OnClickLeaveGameAfterReconnect_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(NonVrGameSettingsPageController), "ToggleGamePrivacy"),
                prefix: new HarmonyMethod(
                    typeof(LifecycleDirector),
                    nameof(NonVrGameSettingsPageController_ToggleGamePrivacy_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(
                    typeof(HandSettingsPageController),
                    "<SetupGameButtons>g__ToggleGamePrivacy|19_4"),
                prefix: new HarmonyMethod(
                    typeof(LifecycleDirector),
                    nameof(HandSettingsPageController_ToggleGamePrivacy_Prefix)));
        }

        private static void GameStartup_InitializeGame_Postfix(GameStartup __instance)
        {
            var gameContext = Traverse.Create(__instance).Field<GameContext>("gameContext").Value;
            _gameContext = gameContext;
        }

        private static void ReconnectState_OnClickLeaveGameAfterReconnect_Postfix()
        {
            DeactivateReconnect();
        }

        private static void GameStateMachine_OnRoomJoined_Postfix()
        {
            if (!IsReconnect)
            {
                return;
            }

            _lastCode = PhotonNetwork.CurrentRoom.Name;
            if (_lastCode != _roomCode)
            {
                HouseRulesCoreBase.LogWarning($"Room {_lastCode} doesn't match original room {_roomCode}. Deactivating reconnection rules!");
                DeactivateReconnect();
            }
        }

        private static void CreatingGameState_TryCreateRoom_Prefix()
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            var createGameMode = Traverse.Create(_gameContext.gameStateMachine)
                .Field<CreateGameMode>("createGameMode").Value;
            if (createGameMode != CreateGameMode.Private)
            {
                return;
            }

            var gameStateTraverse = Traverse.Create(_gameContext.gameStateMachine).Field("creatingGameState");
            if (!gameStateTraverse.FieldExists())
            {
                HouseRulesCoreBase.LogError("Failed to find required \"creatingGameState\" field.");
                return;
            }

            var gameState = gameStateTraverse.GetValue();
            var roomOptions = Traverse.Create(gameState).Field<RoomOptions>("roomOptions").Value;
            AddModdedRoomProperties(roomOptions);
        }

        private static void CreatingGameState_OnJoinedRoom_Prefix()
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (_gameContext.gameStateMachine.goBackToMenuState)
            {
                return;
            }

            var createGameMode = Traverse.Create(_gameContext.gameStateMachine)
                .Field<CreateGameMode>("createGameMode").Value;
            if (createGameMode != CreateGameMode.Private)
            {
                return;
            }

            var createdGameFromSave =
                Traverse.Create(_gameContext.gameStateMachine).Field<bool>("createdGameFromSave").Value;
            if (createdGameFromSave)
            {
                _isLoadingGame = true;
            }
            else
            {
                _isCreatingGame = true;
            }

            var levelSequence = Traverse.Create(_gameContext.gameStateMachine).Field<LevelSequence>("levelSequence").Value;
            MotherbrainGlobalVars.CurrentConfig = levelSequence.gameConfig;

            if (IsReconnect)
            {
                DeactivateReconnect();
            }

            _roomCode = PhotonNetwork.CurrentRoom.Name;
            HouseRulesCoreBase.LogDebug($"New game in room {_roomCode} started");
            ActivateRuleset();
            OnPreGameCreated();
        }

        private static void PlayingGameState_OnMasterClientChanged_Prefix()
        {
            if (!IsReconnect)
            {
                return;
            }

            if (!GameStateMachine.IsMasterClient)
            {
                return;
            }

            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (_gameContext.gameStateMachine.goBackToMenuState)
            {
                return;
            }

            HouseRulesCoreBase.LogWarning($"<--- Resuming ruleset after disconnection from room {_roomCode} --->");

            ActivateRuleset();
            OnPreGameCreated();
            OnPostGameCreated();
            IsReconnect = false;
        }

        private static void GameStateMachine_GoToPlayingState_Postfix()
        {
            if (!_isCreatingGame)
            {
                return;
            }

            _isCreatingGame = false;
            OnPostGameCreated();
        }

        private static void GameStateMachine_GoToShoppingState_Postfix()
        {
            if (!_isLoadingGame)
            {
                return;
            }

            _isLoadingGame = false;
            OnPostGameCreated();
        }

        private static void PostGameControllerBase_OnPlayAgainClicked_Postfix()
        {
            var createGameMode = Traverse.Create(_gameContext.gameStateMachine)
                .Field<CreateGameMode>("createGameMode").Value;
            if (createGameMode != CreateGameMode.Private)
            {
                return;
            }

            ActivateRuleset();
            _isCreatingGame = true;
            OnPreGameCreated();
        }

        private static void GameStateMachine_EndGame_Prefix()
        {
            DeactivateRuleset();
        }

        private static void SerializableEventQueue_DisconnectLocalPlayer_Prefix(BoardgameActionOnLocalPlayerDisconnect.DisconnectContext context)
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (GameHub.GetGameMode != GameHub.GameMode.Multiplayer)
            {
                DeactivateRuleset();
                return;
            }

            if (!GameStateMachine.IsMasterClient)
            {
                return;
            }

            if (context == BoardgameActionOnLocalPlayerDisconnect.DisconnectContext.ReconnectState)
            {
                HouseRulesCoreBase.LogWarning($"<- Disconnected from room {_roomCode} ->");
                IsReconnect = true;
                DeactivateRuleset();
            }
            else
            {
                HouseRulesCoreBase.LogDebug($"<- MANUALLY disconnected from room {_roomCode} ->");
                IsReconnect = true;
                DeactivateRuleset();
            }
        }

        private static bool NonVrGameSettingsPageController_ToggleGamePrivacy_Prefix()
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return true;
            }

            // Don't allow PC-Edition privacy settings to change from Private to Public.
            return false;
        }

        private static bool HandSettingsPageController_ToggleGamePrivacy_Prefix()
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return true;
            }

            // Don't allow PCVR privacy settings to change from Private to Public.
            return false;
        }

        /// <summary>
        /// Add properties to the room to indicate its modded nature.
        /// </summary>
        /// <remarks>
        /// Properties may be used by other mods to distinguish modded rooms from non-modded rooms.
        /// </remarks>
        private static void AddModdedRoomProperties(RoomOptions roomOptions)
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (roomOptions.CustomRoomPropertiesForLobby.Contains(ModdedRoomPropertyKey))
            {
                HouseRulesCoreBase.LogWarning($"Room options already include custom property: {ModdedRoomPropertyKey}");
                return;
            }

            var newOptions = new string[roomOptions.CustomRoomPropertiesForLobby.Length + 1];
            newOptions[0] = ModdedRoomPropertyKey;
            roomOptions.CustomRoomPropertiesForLobby.CopyTo(newOptions, 1);
            roomOptions.CustomRoomPropertiesForLobby = newOptions;
            roomOptions.CustomRoomProperties.Add(ModdedRoomPropertyKey, true);
        }

        private static void ActivateRuleset()
        {
            if (IsRulesetActive && !IsReconnect)
            {
                HouseRulesCoreBase.LogWarning("Ruleset activation was attempted whilst a ruleset was already activated. This should not happen. Please report this to HouseRules developers.");
                return;
            }

            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (GameHub.GetGameMode == GameHub.GameMode.Multiplayer && !HR.SelectedRuleset.IsSafeForMultiplayer)
            {
                HouseRulesCoreBase.LogWarning($"The selected ruleset [{HR.SelectedRuleset.Name}] is not safe for multiplayer games. Skipping activation.");
                return;
            }

            IsRulesetActive = true;

            var setName = HR.SelectedRuleset.Name;
            HouseRulesCoreBase.LogWarning($"Activating ruleset: {setName} (with {HR.SelectedRuleset.Rules.Count} rules)");
            foreach (var rule in HR.SelectedRuleset.Rules)
            {
                try
                {
                    var isDisabled = rule is IDisableOnReconnect;
                    if (IsReconnect && isDisabled)
                    {
                        HouseRulesCoreBase.LogDebug($"Skip activating rule type: {rule.GetType()}");
                        continue;
                    }
                    else
                    {
                        HouseRulesCoreBase.LogDebug($"Activating rule type: {rule.GetType()}");
                        rule.OnActivate(_gameContext);
                    }
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    HouseRulesCoreBase.LogWarning($"Failed to activate rule [{rule.GetType()}]: {e}");
                }
            }
        }

        private static void DeactivateRuleset()
        {
            if (!IsRulesetActive)
            {
                return;
            }

            if (!IsReconnect)
            {
                IsRulesetActive = false;
            }

            var setName = HR.SelectedRuleset.Name;
            HouseRulesCoreBase.LogDebug($"Deactivating ruleset: {setName} (with {HR.SelectedRuleset.Rules.Count} rules)");
            foreach (var rule in HR.SelectedRuleset.Rules)
            {
                try
                {
                    var isDisabled = rule is IDisableOnReconnect;
                    if (IsReconnect && isDisabled)
                    {
                        HouseRulesCoreBase.LogDebug($"Skip deactivating rule type: {rule.GetType()}");
                        continue;
                    }
                    else
                    {
                        HouseRulesCoreBase.LogDebug($"Deactivating rule type: {rule.GetType()}");
                        rule.OnDeactivate(_gameContext);
                    }
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    HouseRulesCoreBase.LogWarning($"Failed to deactivate rule [{rule.GetType()}]: {e}");
                }
            }
        }

        public static void DeactivateReconnect()
        {
            IsReconnect = false;
            IsRulesetActive = false;

            HouseRulesCoreBase.LogWarning($"Deactivating reconnection: {HR.SelectedRuleset.Name} (with {HR.SelectedRuleset.Rules.Count} rules)");

            foreach (var rule in HR.SelectedRuleset.Rules)
            {
                try
                {
                    var isDisabled = rule is IDisableOnReconnect;
                    if (isDisabled)
                    {
                        HouseRulesCoreBase.LogDebug($"Deactivating reconnection for rule type: {rule.GetType()}");
                        rule.OnDeactivate(_gameContext);
                    }
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    HouseRulesCoreBase.LogWarning($"Failed to deactivate reconnection for rule [{rule.GetType()}]: {e}");
                }
            }
        }

        private static void OnPreGameCreated()
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (!IsRulesetActive)
            {
                return;
            }

            foreach (var rule in HR.SelectedRuleset.Rules)
            {
                try
                {
                    var isDisabled = rule is IDisableOnReconnect;
                    if (IsReconnect && isDisabled)
                    {
                        HouseRulesCoreBase.LogDebug($"Skip OnPreGameCreated for rule type: {rule.GetType()}");
                        continue;
                    }
                    else
                    {
                        HouseRulesCoreBase.LogDebug($"Calling OnPreGameCreated for rule type: {rule.GetType()}");
                        rule.OnPreGameCreated(_gameContext);
                    }
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    HouseRulesCoreBase.LogWarning($"Failed to successfully call OnPreGameCreated on rule [{rule.GetType()}]: {e}");
                }
            }

            ShowWelcomeMessage();
        }

        private static void OnPostGameCreated()
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (!IsRulesetActive)
            {
                return;
            }

            foreach (var rule in HR.SelectedRuleset.Rules)
            {
                try
                {
                    var isDisabled = rule is IDisableOnReconnect;
                    if (IsReconnect && isDisabled)
                    {
                        HouseRulesCoreBase.LogDebug($"Skip OnPostGameCreated for rule type: {rule.GetType()}");
                        continue;
                    }
                    else
                    {
                        HouseRulesCoreBase.LogDebug($"Calling OnPostGameCreated for rule type: {rule.GetType()}");
                        rule.OnPostGameCreated(_gameContext);
                    }
                }
                catch (Exception e)
                {
                    // TODO(orendain): Consider rolling back or disable rule.
                    HouseRulesCoreBase.LogWarning($"Failed to successfully call OnPostGameCreated on rule [{rule.GetType()}]: {e}");
                }
            }
        }

        private static void ShowWelcomeMessage()
        {
            if (HR.SelectedRuleset == Ruleset.None)
            {
                return;
            }

            if (!IsRulesetActive)
            {
                GameUI.ShowCameraMessage(NotSafeForMultiplayerMessage(), 30);
                return;
            }

            GameUI.ShowCameraMessage(RulesetActiveMessage(), welcomeMessageDurationSeconds);
        }

        private static string NotSafeForMultiplayerMessage()
        {
            Color orange = new(1f, 0.499f, 0f);
            return new StringBuilder()
                .Append(ColorizeString("*** ", orange))
                .Append(ColorizeString("ATTENTION", Color.red))
                .AppendLine(ColorizeString(" ***", orange))
                .AppendLine()
                .AppendLine(ColorizeString("The HouseRules ruleset you selected is", Color.yellow))
                .Append(ColorizeString("not safe", Color.cyan))
                .AppendLine(ColorizeString(" for multiplayer games!", Color.yellow))
                .AppendLine()
                .Append(ColorizeString("The ruleset was ", Color.yellow))
                .Append(ColorizeString("NOT", Color.white))
                .AppendLine(ColorizeString(" activated!", Color.yellow))
                .ToString();
        }

        private static string RulesetActiveMessage()
        {
            Color violet = new(0.8f, 0f, 0.8f);
            Color lightblue = new(0f, 0.75f, 1f);
            Color orange = new(1f, 0.499f, 0f);
            Color gold = new(1f, 1f, 0.6f);
            var sb = new StringBuilder();
            sb.AppendLine(ColorizeString("Welcome to a game using", Color.cyan));
            sb.Append(ColorizeString("H", violet));
            sb.Append(ColorizeString("o", lightblue));
            sb.Append(ColorizeString("u", Color.green));
            sb.Append(ColorizeString("s", Color.yellow));
            sb.Append(ColorizeString("e", orange));
            sb.Append(ColorizeString("-", Color.red));
            sb.Append(ColorizeString("R", orange));
            sb.Append(ColorizeString("u", Color.yellow));
            sb.Append(ColorizeString("l", Color.green));
            sb.Append(ColorizeString("e", lightblue));
            sb.AppendLine(ColorizeString("s", violet));
            sb.AppendLine();

            if (MotherbrainGlobalVars.IsRunningOnNonVRPlatform)
            {
                sb.AppendLine(ColorizeString($"{HR.SelectedRuleset.Name}:", Color.yellow));
                sb.AppendLine(ColorizeString(HR.SelectedRuleset.Description, Color.white));
                sb.AppendLine();
                if (!string.IsNullOrEmpty(HR.SelectedRuleset.Longdesc))
                {
                    sb.AppendLine(ColorizeString($"<========== Ruleset Creator's Description ==========>", orange));
                    sb.AppendLine(ColorizeString($"{HR.SelectedRuleset.Longdesc}", gold));
                }
                else
                {
                    sb.AppendLine(ColorizeString($"{HR.SelectedRuleset.Rules.Count} Rules loaded!", gold));
                }
            }


            // Pad lines to raise text higher on PC-Edition screen
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }

        private static string ColorizeString(string text, Color color)
        {
            return string.Concat(new string[]
            {
        "<color=#",
        ColorUtility.ToHtmlStringRGB(color),
        ">",
        text,
        "</color>",
            });
        }
    }
}
