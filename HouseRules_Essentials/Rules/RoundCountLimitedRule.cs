namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class RoundCountLimitedRule : Rule, IConfigWritable<int>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Round count is limited";

        private const float RoundsLeftMessageDurationSeconds = 5f;
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
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

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
        }

        private static void Piece_CreatePiece_Postfix(ref Piece __result)
        {
            if (!_isActivated || !__result.IsPlayer())
            {
                return;
            }

            __result.EnableEffectState(EffectStateType.SelfDestruct);
            __result.effectSink.SetStatusEffectDuration(EffectStateType.SelfDestruct, _globalRoundLimit);
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
                _gameContext.serializableEventQueue.SendResponseEvent(new SerializableEventEndGame());
            }
        }

        private static void ShowRoundsLeft()
        {
            var roundsLeft = _globalRoundLimit - _globalRoundsPlayed;
            GameUI.ShowCameraMessage(
                $"You have {roundsLeft} rounds left to escape...",
                RoundsLeftMessageDurationSeconds);
        }
    }
}
