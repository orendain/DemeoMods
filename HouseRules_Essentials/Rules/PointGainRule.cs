namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.Data;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PointGainRule : Rule, IConfigWritable<PointGainRule.Points>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Players can gain points for certain actions";

        private static Points _globalConfig;
        private static bool _isActivated;

        internal static int Player1 { get; private set; }

        internal static int Player2 { get; private set; }

        internal static int Player3 { get; private set; }

        internal static int Player4 { get; private set; }

        private readonly Points _config;

        public struct Points
        {
            public int KillEnemy;
            public int KillPlayer;
            public int HurtPlayer;
            public int Keyholder;
            public int UnlockDoor;
            public int KillBoss;
            public int HurtSelf;
            public int KillSelf;
        }

        public PointGainRule(Points points)
        {
            _config = points;
        }

        public Points GetConfigObject() => _config;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalConfig = _config;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            _isActivated = false;
        }

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(
                    typeof(PointGainRule),
                    nameof(CreatePiece_Revolutions_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Ability), "GenerateAttackDamage"),
                postfix: new HarmonyMethod(
                    typeof(PointGainRule),
                    nameof(Ability_GenerateAttackDamage_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(MotherTracker), "TrackUnitDefeated"),
                prefix: new HarmonyMethod(
                    typeof(PointGainRule),
                    nameof(MotherTracker_TrackUnitDefeated_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Interactable), "OnInteraction", new[] { typeof(int), typeof(IntPoint2D), typeof(GameContext), typeof(int) }),
                prefix: new HarmonyMethod(
                    typeof(PointGainRule),
                    nameof(Interactable_OnInteraction_Prefix)));
        }

        private static void Interactable_OnInteraction_Prefix(
            int pieceId,
            GameContext gameContext,
            IntPoint2D targetTile)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!gameContext.pieceAndTurnController.GetInteractableAtPosition(targetTile))
            {
                return;
            }

            var interactable = gameContext.pieceAndTurnController.GetInteractableAtPosition(targetTile);
            if (!gameContext.pieceAndTurnController.TryGetPiece(pieceId, out var piece))
            {
                return;
            }

            if (interactable.type == Interactable.Type.LevelExit)
            {
                if (!piece.IsPlayer() || !piece.HasEffectState(EffectStateType.Key))
                {
                    return;
                }

                EssentialsMod.Logger.Msg($"{piece.boardPieceId} [ID: {piece.networkID}] unlocked door (+{_globalConfig.UnlockDoor})");
                var startPoints = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
                if (startPoints > 998)
                {
                    startPoints = 0;
                }

                var pointCount = startPoints;
                pointCount += _globalConfig.UnlockDoor;
                if (pointCount != startPoints)
                {
                    EssentialsMod.Logger.Msg($"{piece.boardPieceId} [ID: {piece.networkID}] total points: {pointCount}");
                    piece.effectSink.RemoveStatusEffect(EffectStateType.StrengthInNumbers);
                    piece.effectSink.AddStatusEffect(EffectStateType.StrengthInNumbers, pointCount);
                }
            }
        }

        private static void MotherTracker_TrackUnitDefeated_Prefix(Piece defeatedUnit, Piece attackerUnit)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!attackerUnit.IsPlayer())
            {
                return;
            }

            var startPoints = attackerUnit.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
            if (startPoints > 998)
            {
                startPoints = 0;
            }

            var pointCount = startPoints;
            if (!defeatedUnit.IsPlayer())
            {
                EssentialsMod.Logger.Msg($"{attackerUnit.boardPieceId} [ID: {attackerUnit.networkID}] killed enemy {defeatedUnit.boardPieceId} (+{_globalConfig.KillEnemy})");
                pointCount += _globalConfig.KillEnemy;
                if (defeatedUnit.HasPieceType(PieceType.Boss))
                {
                    EssentialsMod.Logger.Msg($"Enemy was the BOSS (+{_globalConfig.KillBoss})");
                    pointCount += _globalConfig.KillBoss;
                }

                if (attackerUnit.HasEffectState(EffectStateType.Key))
                {
                    EssentialsMod.Logger.Msg($"Keyholder bonus (+{_globalConfig.Keyholder})");
                    pointCount += _globalConfig.Keyholder;
                }
            }
            else if (defeatedUnit != attackerUnit)
            {
                EssentialsMod.Logger.Msg($"{attackerUnit.boardPieceId} [ID: {attackerUnit.networkID}] killed PLAYER {defeatedUnit.boardPieceId} (+{_globalConfig.KillPlayer})");
                pointCount += _globalConfig.KillPlayer;
                if (attackerUnit.HasEffectState(EffectStateType.Key))
                {
                    EssentialsMod.Logger.Msg($"Keyholder bonus (+{_globalConfig.Keyholder})");
                    pointCount += _globalConfig.Keyholder;
                }
            }
            else
            {
                if (pointCount > 0)
                {
                    EssentialsMod.Logger.Msg($"{attackerUnit.boardPieceId} [ID: {attackerUnit.networkID}] killed self {defeatedUnit.boardPieceId} ({_globalConfig.KillSelf})");
                    pointCount += _globalConfig.KillSelf;
                }
            }

            if (pointCount != startPoints)
            {
                EssentialsMod.Logger.Msg($"{attackerUnit.boardPieceId} [ID: {attackerUnit.networkID}] total points: {pointCount}");
                attackerUnit.effectSink.RemoveStatusEffect(EffectStateType.StrengthInNumbers);
                attackerUnit.effectSink.AddStatusEffect(EffectStateType.StrengthInNumbers, pointCount);
            }
        }

        private static void Ability_GenerateAttackDamage_Postfix(Piece source, Dice.Outcome diceResult, Piece[] targets)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!source.IsPlayer())
            {
                return;
            }

            var startPoints = source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
            if (startPoints > 998)
            {
                startPoints = 0;
            }

            var pointCount = startPoints;
            if (source != null && targets.Length != 0)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i].IsPlayer() && targets[i] != source && !targets[i].IsDowned() && !targets[i].IsImmuneToDamage())
                    {
                        EssentialsMod.Logger.Msg($"{source.boardPieceId} [ID: {source.networkID}] hurt player {targets[i].boardPieceId} (+{_globalConfig.HurtPlayer})");
                        pointCount += _globalConfig.HurtPlayer;
                        if (source.HasEffectState(EffectStateType.Key))
                        {
                            EssentialsMod.Logger.Msg($"Keyholder bonus (+{_globalConfig.Keyholder})");
                            pointCount += _globalConfig.Keyholder;
                        }
                    }
                    else if (targets[i].IsPlayer() && targets[i] == source && !source.IsDowned() && diceResult != Dice.Outcome.None)
                    {
                        if (pointCount > 0)
                        {
                            EssentialsMod.Logger.Msg($"{source.boardPieceId} [ID: {source.networkID}] hurt self ({_globalConfig.HurtSelf})");
                            pointCount += _globalConfig.HurtSelf;
                        }
                    }
                }
            }

            if (pointCount != startPoints)
            {
                EssentialsMod.Logger.Msg($"{source.boardPieceId} [ID: {source.networkID}] total points: {pointCount}");
                source.effectSink.RemoveStatusEffect(EffectStateType.StrengthInNumbers);
                source.effectSink.AddStatusEffect(EffectStateType.StrengthInNumbers, pointCount);
            }
        }

        private static void CreatePiece_Revolutions_Postfix(ref Piece __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!__result.IsPlayer())
            {
                return;
            }

            __result.effectSink.AddStatusEffect(EffectStateType.StrengthInNumbers, 0);
        }
    }
}
