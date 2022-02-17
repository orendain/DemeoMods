namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.SerializableEvents;
    using Boardgame.TurnOrder;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class RoundCountLimitedRule : Rule, IConfigWritable<int>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Round count is limited";

        private const float RoundsLeftMessageDurationSeconds = 5f;
        private static int _globalRoundLimit;
        private static int _globalRoundsPlayed;
        private static bool _isActivated;

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
                original: AccessTools.Method(typeof(SerializableEventQueue), "RespondToRequest"),
                postfix: new HarmonyMethod(
                    typeof(RoundCountLimitedRule),
                    nameof(SerializableEventQueue_RespondToRequest_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(PieceAndTurnController), "EndCurrentTurn"),
                prefix: new HarmonyMethod(
                    typeof(RoundCountLimitedRule),
                    nameof(PieceAndTurnController_EndCurrentTurn_Prefix)));
        }

        private static void SerializableEventQueue_RespondToRequest_Postfix(SerializableEvent request)
        {
            if (!_isActivated)
            {
                return;
            }

            if (request.type != SerializableEvent.Type.StartRound)
            {
                return;
            }

            ShowRoundsLeft();
        }

        private static void PieceAndTurnController_EndCurrentTurn_Prefix(PieceAndTurnController __instance)
        {
            if (!_isActivated)
            {
                return;
            }

            var turnQueue = Traverse.Create(__instance).Field<TurnQueue>("turnQueue").Value;
            if (!turnQueue.CurrentPieceIsLastInTurnOrder())
            {
                return;
            }

            _globalRoundsPlayed += 1;
            if (_globalRoundsPlayed < _globalRoundLimit)
            {
                return;
            }

            DownAllPlayers(__instance);
        }

        private static void ShowRoundsLeft()
        {
            var roundsLeft = _globalRoundLimit - _globalRoundsPlayed;
            GameUI.ShowCameraMessage($"You have {roundsLeft} rounds left to escape...", RoundsLeftMessageDurationSeconds);
        }

        private static void DownAllPlayers(PieceAndTurnController pieceAndTurnController)
        {
            pieceAndTurnController.GetPlayerPieces().Do(p => p.SetIsDowned(true, pieceAndTurnController));
        }
    }
}
