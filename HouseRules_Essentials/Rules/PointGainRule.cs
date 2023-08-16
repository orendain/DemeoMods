namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.BoardEntities.AI;
    using Boardgame.BoardgameActions;
    using Boardgame.Data;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PointGainRule : Rule, IConfigWritable<PointGainRule.Points>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Players can gain points for certain actions";

        internal static Points _globalConfig;
        private static bool _isActivated;

        internal static int Player1 { get; private set; }

        internal static int Player2 { get; private set; }

        internal static int Player3 { get; private set; }

        internal static int Player4 { get; private set; }

        private readonly Points _config;

        public struct Points
        {
            public int KillEnemy;
            public int HurtEnemy;
            public int KillPlayer;
            public int HurtPlayer;
            public int Keyholder;
            public int UnlockDoor;
            public int HurtBoss;
            public int KillBoss;
            public int HurtSelf;
            public int KillSelf;
            public int LootGold;
            public int LootChest;
            public int LootStand;
            public int OpenDoor;
            public int UseFountain;
            public int RevivePlayer;
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
                    nameof(Piece_CreatePiece_Postfix)));

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

            harmony.Patch(
                original: AccessTools.Method(typeof(MotherTracker), "TrackRevive"),
                prefix: new HarmonyMethod(
                    typeof(PointGainRule),
                    nameof(MotherTracker_TrackRevive_Prefix)));

            harmony.Patch(
                original: AccessTools.Constructor(typeof(BoardgameActionPiecePickup), new[] { typeof(GameContext), typeof(int), typeof(IntPoint2D), typeof(int), typeof(int) }),
                prefix: new HarmonyMethod(
                    typeof(PointGainRule),
                    nameof(BoardgameActionPiecePickup_Prefix)));
        }

        private static void BoardgameActionPiecePickup_Prefix(GameContext gameContext, int pieceId)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!gameContext.pieceAndTurnController.TryGetPiece(pieceId, out var piece))
            {
                return;
            }

            Piece piece2 = null;
            if (piece.boardPieceId == BoardPieceId.WarlockMinion)
            {
                PieceAI pieceAI = piece.pieceAI;
                if (pieceAI == null)
                {
                    return;
                }
                else if (pieceAI.memory.TryGetAssociatedPiece(gameContext.pieceAndTurnController, out piece2))
                {
                    EssentialsMod.Logger.Msg($"{piece.boardPieceId} [ID: {piece2.networkID}]'s minion is attacking...");
                    piece = piece2;
                }
            }

            var pointCount = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
            if (pointCount > 998)
            {
                pointCount = 0;
            }

            EssentialsMod.Logger.Msg($"{piece.boardPieceId} [ID: {piece.networkID}] looted gold ({_globalConfig.LootGold})");
            pointCount += _globalConfig.LootGold;
            if (piece.HasEffectState(EffectStateType.Key))
            {
                EssentialsMod.Logger.Msg($"Keyholder bonus ({_globalConfig.Keyholder})");
                pointCount += _globalConfig.Keyholder;
            }

            if (pointCount < 0)
            {
                pointCount = 0;
            }

            EssentialsMod.Logger.Msg($"{piece.boardPieceId} [ID: {piece.networkID}] total points: {pointCount}");
            piece.effectSink.RemoveStatusEffect(EffectStateType.StrengthInNumbers);
            piece.effectSink.AddStatusEffect(EffectStateType.StrengthInNumbers, pointCount);
        }

        private static void MotherTracker_TrackRevive_Prefix(Piece revivedPiece, Piece sourcePiece)
        {
            if (!_isActivated)
            {
                return;
            }

            if (revivedPiece == sourcePiece)
            {
                return;
            }

            var pointCount = sourcePiece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
            if (pointCount > 998)
            {
                pointCount = 0;
            }

            EssentialsMod.Logger.Msg($"{sourcePiece.boardPieceId} [ID: {sourcePiece.networkID}] revived player {revivedPiece.boardPieceId} ({_globalConfig.RevivePlayer})");
            pointCount += _globalConfig.RevivePlayer;

            if (pointCount < 0)
            {
                pointCount = 0;
            }

            EssentialsMod.Logger.Msg($"{sourcePiece.boardPieceId} [ID: {sourcePiece.networkID}] total points: {pointCount}");
            sourcePiece.effectSink.RemoveStatusEffect(EffectStateType.StrengthInNumbers);
            sourcePiece.effectSink.AddStatusEffect(EffectStateType.StrengthInNumbers, pointCount);

            pointCount = revivedPiece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
            if (pointCount > 998)
            {
                pointCount = 0;
            }

            EssentialsMod.Logger.Msg($"{revivedPiece.boardPieceId} [ID: {revivedPiece.networkID}] was revived by player {sourcePiece.boardPieceId} (-{_globalConfig.RevivePlayer})");
            pointCount -= _globalConfig.RevivePlayer;
            if (pointCount < 0)
            {
                pointCount = 0;
            }

            EssentialsMod.Logger.Msg($"{revivedPiece.boardPieceId} [ID: {revivedPiece.networkID}] total points: {pointCount}");
            revivedPiece.effectSink.RemoveStatusEffect(EffectStateType.StrengthInNumbers);
            revivedPiece.effectSink.AddStatusEffect(EffectStateType.StrengthInNumbers, pointCount);
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

            if (!piece.IsPlayer())
            {
                return;
            }

            var pointCount = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
            if (pointCount > 998)
            {
                pointCount = 0;
            }

            bool flag = false;
            if (interactable.type == Interactable.Type.LevelExit)
            {
                if (!piece.HasEffectState(EffectStateType.Key))
                {
                    return;
                }

                if (piece.HasEffectState(EffectStateType.Locked))
                {
                    var keyCount = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.Locked);
                    if (keyCount > 0)
                    {
                        EssentialsMod.Logger.Msg($"{piece.boardPieceId} [ID: {piece.networkID}] unlocked the exit door ({keyCount})");
                        flag = true;
                        pointCount += keyCount;
                    }
                }
                else
                {
                    EssentialsMod.Logger.Msg($"{piece.boardPieceId} [ID: {piece.networkID}] unlocked the exit door ({_globalConfig.UnlockDoor})");
                    flag = true;
                    pointCount += _globalConfig.UnlockDoor;
                }
            }
            else if (interactable.type == Interactable.Type.Chest)
            {
                EssentialsMod.Logger.Msg($"{piece.boardPieceId} [ID: {piece.networkID}] looted a chest ({_globalConfig.LootChest})");
                flag = true;
                pointCount += _globalConfig.LootChest;
            }
            else if (interactable.type == Interactable.Type.PotionStand)
            {
                EssentialsMod.Logger.Msg($"{piece.boardPieceId} [ID: {piece.networkID}] looted a potion stand ({_globalConfig.LootStand})");
                flag = true;
                pointCount += _globalConfig.LootStand;
            }
            else if (interactable.type == Interactable.Type.Door)
            {
                EssentialsMod.Logger.Msg($"{piece.boardPieceId} [ID: {piece.networkID}] opened a door ({_globalConfig.OpenDoor})");
                flag = true;
                pointCount += _globalConfig.OpenDoor;
            }
            else if (interactable.type == Interactable.Type.AltarOfBlessing)
            {
                EssentialsMod.Logger.Msg($"{piece.boardPieceId} [ID: {piece.networkID}] used a fountain ({_globalConfig.UseFountain})");
                flag = true;
                pointCount += _globalConfig.UseFountain;
            }

            if (flag && piece.HasEffectState(EffectStateType.Key) && interactable.type != Interactable.Type.LevelExit)
            {
                EssentialsMod.Logger.Msg($"Keyholder bonus ({_globalConfig.Keyholder})");
                pointCount += _globalConfig.Keyholder;
            }

            if (pointCount < 0)
            {
                pointCount = 0;
            }

            if (flag)
            {
                EssentialsMod.Logger.Msg($"{piece.boardPieceId} [ID: {piece.networkID}] total points: {pointCount}");
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
                if (attackerUnit.boardPieceId == BoardPieceId.WarlockMinion)
                {
                    Piece piece2 = null;
                    var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
                    PieceAI pieceAI = attackerUnit.pieceAI;
                    if (pieceAI == null)
                    {
                        return;
                    }
                    else if (pieceAI.memory.TryGetAssociatedPiece(gameContext.pieceAndTurnController, out piece2))
                    {
                        EssentialsMod.Logger.Msg($"[ID: {piece2.networkID}]'s minion is attacking...");
                        attackerUnit = piece2;
                    }
                }
                else
                {
                    return;
                }
            }

            var pointCount = attackerUnit.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
            if (pointCount > 998)
            {
                pointCount = 0;
            }

            bool flag = false;
            if (!defeatedUnit.IsPlayer())
            {
                EssentialsMod.Logger.Msg($"{attackerUnit.boardPieceId} [ID: {attackerUnit.networkID}] killed enemy {defeatedUnit.boardPieceId} ({_globalConfig.KillEnemy})");
                flag = true;
                pointCount += _globalConfig.KillEnemy;
                if (defeatedUnit.HasPieceType(PieceType.Boss))
                {
                    EssentialsMod.Logger.Msg($"Enemy was the BOSS ({_globalConfig.KillBoss})");
                    pointCount += _globalConfig.KillBoss;
                }
            }
            else if (defeatedUnit != attackerUnit)
            {
                EssentialsMod.Logger.Msg($"{attackerUnit.boardPieceId} [ID: {attackerUnit.networkID}] killed PLAYER {defeatedUnit.boardPieceId} ({_globalConfig.KillPlayer})");
                flag = true;
                pointCount += _globalConfig.KillPlayer;
            }
            else
            {
                EssentialsMod.Logger.Msg($"{attackerUnit.boardPieceId} [ID: {attackerUnit.networkID}] killed self {defeatedUnit.boardPieceId} ({_globalConfig.KillSelf})");
                flag = true;
                pointCount += _globalConfig.KillSelf;
            }

            if (flag && attackerUnit.HasEffectState(EffectStateType.Key))
            {
                EssentialsMod.Logger.Msg($"Keyholder bonus ({_globalConfig.Keyholder})");
                pointCount += _globalConfig.Keyholder;
            }

            if (pointCount < 0)
            {
                pointCount = 0;
            }

            if (flag)
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
                if (source.boardPieceId == BoardPieceId.WarlockMinion)
                {
                    Piece piece2 = null;
                    var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
                    PieceAI pieceAI = source.pieceAI;
                    if (pieceAI == null)
                    {
                        return;
                    }
                    else if (pieceAI.memory.TryGetAssociatedPiece(gameContext.pieceAndTurnController, out piece2))
                    {
                        EssentialsMod.Logger.Msg($"[ID: {piece2.networkID}]'s minion is attacking...");
                        source = piece2;
                    }
                }
                else
                {
                    return;
                }
            }

            var pointCount = source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
            if (pointCount > 998)
            {
                pointCount = 0;
            }

            bool flag = false;
            if (source != null && targets.Length != 0)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i].IsPlayer() && targets[i] != source && !targets[i].IsDowned() && !targets[i].IsImmuneToDamage())
                    {
                        EssentialsMod.Logger.Msg($"{source.boardPieceId} [ID: {source.networkID}] hurt/buffed player {targets[i].boardPieceId} ({_globalConfig.HurtPlayer})");
                        flag = true;
                        pointCount += _globalConfig.HurtPlayer;
                        if (source.HasEffectState(EffectStateType.Key))
                        {
                            EssentialsMod.Logger.Msg($"Keyholder bonus ({_globalConfig.Keyholder})");
                            pointCount += _globalConfig.Keyholder;
                        }
                    }
                    else if (targets[i] == source && !source.IsDowned() && diceResult != Dice.Outcome.None)
                    {
                        EssentialsMod.Logger.Msg($"{source.boardPieceId} [ID: {source.networkID}] hurt/buffed themself ({_globalConfig.HurtSelf})");
                        flag = true;
                        pointCount += _globalConfig.HurtSelf;
                    }
                    else if (!targets[i].IsPlayer() && targets[i].HasPieceType(PieceType.Boss))
                    {
                        EssentialsMod.Logger.Msg($"{source.boardPieceId} [ID: {source.networkID}] hurt a boss {targets[i].boardPieceId} ({_globalConfig.HurtBoss})");
                        flag = true;
                        pointCount += _globalConfig.HurtBoss;
                    }
                    else if (!targets[i].IsPlayer() && !targets[i].IsBot() && !targets[i].IsProp())
                    {
                        EssentialsMod.Logger.Msg($"{source.boardPieceId} [ID: {source.networkID}] hurt an enemy {targets[i].boardPieceId} ({_globalConfig.HurtEnemy})");
                        flag = true;
                        pointCount += _globalConfig.HurtEnemy;
                        if (source.HasEffectState(EffectStateType.Key))
                        {
                            EssentialsMod.Logger.Msg($"Keyholder bonus ({_globalConfig.Keyholder})");
                            pointCount += _globalConfig.Keyholder;
                        }
                    }
                }
            }

            if (pointCount < 0)
            {
                pointCount = 0;
            }

            if (flag)
            {
                EssentialsMod.Logger.Msg($"{source.boardPieceId} [ID: {source.networkID}] total points: {pointCount}");
                source.effectSink.RemoveStatusEffect(EffectStateType.StrengthInNumbers);
                source.effectSink.AddStatusEffect(EffectStateType.StrengthInNumbers, pointCount);
            }
        }

        private static void Piece_CreatePiece_Postfix(ref Piece __result)
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
