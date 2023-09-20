namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardgameActions;
    using Boardgame.LevelLoading;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using global::Types;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class RoundCountLimitedRule : Rule, IConfigWritable<int>, IPatchable, IMultiplayerSafe
    {
        private const float RoundsLeftMessageDurationSeconds = 5f;

        public override string Description => "Round count is limited";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.StatusEffectDataModified;

        private readonly List<StatusEffectData> _adjustments = new List<StatusEffectData> { new StatusEffectData { effectStateType = EffectStateType.SelfDestruct, durationTurns = 2, damagePerTurn = 0, killOnExpire = true, clearOnNewLevel = false, tickWhen = StatusEffectsConfig.TickWhen.EndTurn } };
        private List<StatusEffectData> _originals;
        private static int _globalRoundLimit;
        private static int _globalRoundsPlayed;
        private static bool _isActivated;
        private static GameContext _gameContext;
        private readonly int _roundLimit;

        public RoundCountLimitedRule(int roundLimit)
        {
            _roundLimit = roundLimit;
        }

        public int GetConfigObject() => _roundLimit;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalRoundLimit = _roundLimit;
            _globalRoundsPlayed = 0;
            _originals = new List<StatusEffectData>();
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            UpdateStatusEffectConfig(_originals);
            _isActivated = false;
        }

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            _originals = UpdateStatusEffectConfig(_adjustments);
        }

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameStartup), "InitializeGame"),
                postfix: new HarmonyMethod(typeof(RoundCountLimitedRule), nameof(GameStartup_InitializeGame_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "RespondToRequest"),
                postfix: new HarmonyMethod(
                    typeof(RoundCountLimitedRule),
                    nameof(SerializableEventQueue_RespondToRequest_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(
                    typeof(RoundCountLimitedRule),
                    nameof(Piece_CreatePiece_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(LevelLoaderAndInitializer), "RecreatePieceOnNewLevel"),
                postfix: new HarmonyMethod(
                    typeof(RoundCountLimitedRule),
                    nameof(Piece_CreatePiece_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(BoardgameActionRecreateReconnectedPlayerPiece), "RecreatePlayerPiece"),
                postfix: new HarmonyMethod(
                    typeof(RoundCountLimitedRule),
                    nameof(Piece_Reconnect_Postfix)));
        }

        private static void Piece_Reconnect_Postfix(Piece disconnectedPiece)
        {
            if (!_isActivated || !disconnectedPiece.IsPlayer())
            {
                return;
            }

            disconnectedPiece.EnableEffectState(EffectStateType.SelfDestruct);
            disconnectedPiece.effectSink.SetStatusEffectDuration(EffectStateType.SelfDestruct, _globalRoundLimit - _globalRoundsPlayed);
        }

        private static void Piece_CreatePiece_Postfix(ref Piece __result)
        {
            if (!_isActivated || !__result.IsPlayer())
            {
                return;
            }

            __result.EnableEffectState(EffectStateType.SelfDestruct);
            __result.effectSink.SetStatusEffectDuration(EffectStateType.SelfDestruct, _globalRoundLimit - _globalRoundsPlayed);
        }

        private static void GameStartup_InitializeGame_Postfix(GameStartup __instance)
        {
            var gameContext = Traverse.Create(__instance).Field<GameContext>("gameContext").Value;
            _gameContext = gameContext;
        }

        private static void SerializableEventQueue_RespondToRequest_Postfix(SerializableEvent request)
        {
            if (!_isActivated)
            {
                return;
            }

            if (request.type == SerializableEvent.Type.StartRound)
            {
                ShowRoundsLeft();
                return;
            }

            if (request.type == SerializableEvent.Type.EndRound)
            {
                EssentialsMod.Logger.Msg("Next turn!");
                _globalRoundsPlayed += 1;
                if (_globalRoundsPlayed < _globalRoundLimit)
                {
                    EssentialsMod.Logger.Msg($"<--- {_globalRoundLimit - _globalRoundsPlayed} turns left --->");
                    return;
                }

                EssentialsMod.Logger.Msg("EVERYONE DIES NOW!");
            }
        }

        private static List<StatusEffectData> UpdateStatusEffectConfig(List<StatusEffectData> adjustments)
        {
            var effectsConfigs = Traverse.Create(typeof(StatusEffectsConfig)).Field<StatusEffectData[]>("effectsConfig").Value;
            var previousConfigs = new List<StatusEffectData>();
            for (var i = 0; i < effectsConfigs.Length; i++)
            {
                var matchingIndex = adjustments.FindIndex(a => a.effectStateType == effectsConfigs[i].effectStateType);
                if (matchingIndex < 0)
                {
                    continue;
                }

                previousConfigs.Add(effectsConfigs[i]);
                effectsConfigs[i] = adjustments[matchingIndex];
            }

            return previousConfigs;
        }

        private static void ShowRoundsLeft()
        {
            var roundsLeft = _globalRoundLimit - _globalRoundsPlayed;
            if (roundsLeft == 1)
            {
                GameUI.ShowCameraMessage(
                    $"<color=#FFFF00>This is your</color> <color=#B31106FF><b>LAST</b></color> <color=#FFFF00>round left to escape!</color>",
                    RoundsLeftMessageDurationSeconds);
            }
            else if (roundsLeft > 0)
            {
                GameUI.ShowCameraMessage(
                    $"<color=#7CFC00>You have</color> <color=#FFC125><b>{roundsLeft}</b></color> <color=#7CFC00>rounds left to escape...</color>",
                    RoundsLeftMessageDurationSeconds);
            }
            else
            {
                GameUI.ShowCameraMessage(
                    $"<color=#B31106><b>YOU HAVE FAILED</b></color>",
                    RoundsLeftMessageDurationSeconds);
            }
        }
    }
}
