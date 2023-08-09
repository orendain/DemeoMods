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

                EssentialsMod.Logger.Msg($"{piece.boardPieceId} unlocked door (+{_globalConfig.UnlockDoor})");
                var pointCount = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
                if (pointCount > 998)
                {
                    pointCount = 0;
                }

                pointCount += _globalConfig.UnlockDoor;
                EssentialsMod.Logger.Msg($"{piece.boardPieceId} total points: {pointCount}");
                piece.effectSink.RemoveStatusEffect(EffectStateType.StrengthInNumbers);
                piece.effectSink.AddStatusEffect(EffectStateType.StrengthInNumbers, pointCount);
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

            var pointCount = attackerUnit.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
            if (pointCount > 998)
            {
                pointCount = 0;
            }

            if (!defeatedUnit.IsPlayer())
            {
                EssentialsMod.Logger.Msg($"{attackerUnit.boardPieceId} killed enemy {defeatedUnit.boardPieceId} (+{_globalConfig.KillEnemy})");
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
                EssentialsMod.Logger.Msg($"{attackerUnit.boardPieceId} killed PLAYER {defeatedUnit.boardPieceId} (+{_globalConfig.KillPlayer})");
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
                    EssentialsMod.Logger.Msg($"{attackerUnit.boardPieceId} killed self {defeatedUnit.boardPieceId} ({_globalConfig.KillSelf})");
                    pointCount += _globalConfig.KillSelf;
                }
            }

            EssentialsMod.Logger.Msg($"{attackerUnit.boardPieceId} total points: {pointCount}");
            attackerUnit.effectSink.RemoveStatusEffect(EffectStateType.StrengthInNumbers);
            attackerUnit.effectSink.AddStatusEffect(EffectStateType.StrengthInNumbers, pointCount);
        }

        private static void Ability_GenerateAttackDamage_Postfix(Piece source, Piece[] targets)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!source.IsPlayer())
            {
                return;
            }

            var pointCount = source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
            if (pointCount > 998)
            {
                pointCount = 0;
            }

            if (source != null && targets.Length != 0)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i].IsPlayer() && targets[i] != source && !targets[i].IsDowned() && !targets[i].IsImmuneToDamage())
                    {
                        EssentialsMod.Logger.Msg($"{source.boardPieceId} hurt player {targets[i].boardPieceId} (+{_globalConfig.HurtPlayer})");
                        pointCount += _globalConfig.HurtPlayer;
                    }
                    else if (targets[i].IsPlayer() && targets[i] == source && !source.IsDowned())
                    {
                        if (pointCount > 0)
                        {
                            EssentialsMod.Logger.Msg($"{source.boardPieceId} hurt self ({_globalConfig.HurtSelf})");
                            pointCount += _globalConfig.HurtSelf;
                        }
                    }
                }
            }

            EssentialsMod.Logger.Msg($"{source.boardPieceId} total points: {pointCount}");
            source.effectSink.RemoveStatusEffect(EffectStateType.StrengthInNumbers);
            source.effectSink.AddStatusEffect(EffectStateType.StrengthInNumbers, pointCount);
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
