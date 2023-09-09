namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.Board;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.AI;
    using Boardgame.LevelLoading;
    using Boardgame.TurnOrder;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class DarknessRule : Rule, IConfigWritable<Dictionary<BoardPieceId, int>>,
        IPatchable, IMultiplayerSafe, IDisableOnReconnect
    {
        public override string Description => "Some Heroes have modified vision range";

        private readonly Dictionary<BoardPieceId, int> _adjustments;
        private static Dictionary<BoardPieceId, int> _globalAdjustments;
        private static bool _isActivated;
        private static bool _check;
        private static List<Piece> _playerPieces;

        public DarknessRule(Dictionary<BoardPieceId, int> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<BoardPieceId, int> GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            _isActivated = false;
        }

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(LevelLoaderAndInitializer), "GetFloorTileEffects"),
                postfix: new HarmonyMethod(
                    typeof(DarknessRule),
                    nameof(LevelLoaderAndInitializer_GetFloorTileEffects_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(
                    typeof(DarknessRule),
                    nameof(Piece_CreatePiece_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(LevelLoaderAndInitializer), "RecreatePieceOnNewLevel"),
                postfix: new HarmonyMethod(
                    typeof(DarknessRule),
                    nameof(Piece_CreatePiece_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(MotherTracker), "TrackUnitDefeated"),
                prefix: new HarmonyMethod(
                    typeof(DarknessRule),
                    nameof(MotherTracker_TrackUnitDefeated_Prefix)));

            harmony.Patch(
                original: AccessTools.Constructor(typeof(RearrangePlayerTurnOrder), new[] { typeof(TurnQueue) }),
                postfix: new HarmonyMethod(
                    typeof(DarknessRule),
                    nameof(RearrangePlayerTurnOrder_Constructor_Postfix)));
        }

        private static void RearrangePlayerTurnOrder_Constructor_Postfix(
            RearrangePlayerTurnOrder __instance,
            TurnQueue turnQueue)
        {
            if (!_isActivated)
            {
                return;
            }

            _playerPieces = turnQueue.GetPlayerPieces();
        }

        private static void LevelLoaderAndInitializer_GetFloorTileEffects_Postfix(out float prob, out List<TileEffect> list)
        {
            if (!_isActivated)
            {
                if (MotherbrainGlobalVars.CurrentConfig == GameConfigType.Desert)
                {
                    prob = 0.05f;
                    list = new List<TileEffect> { TileEffect.Corruption };
                    return;
                }
                else if (MotherbrainGlobalVars.CurrentConfig == GameConfigType.Forest)
                {
                    prob = 0.1f;
                    list = new List<TileEffect> { TileEffect.Water };
                    return;
                }
                else
                {
                    prob = 0;
                    list = new List<TileEffect> { };
                    return;
                }
            }

            prob = 0.2f;
            list = new List<TileEffect> { TileEffect.Acid };
        }

        private static void Piece_CreatePiece_Postfix(ref Piece __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!_globalAdjustments.ContainsKey(__result.boardPieceId))
            {
                return;
            }

            foreach (var replacement in _globalAdjustments)
            {
                if (__result.boardPieceId == replacement.Key)
                {
                    __result.effectSink.TrySetStatBaseValue(Stats.Type.VisionRange, replacement.Value);
                }
            }
        }

        private static void MotherTracker_TrackUnitDefeated_Prefix(Piece defeatedUnit, Piece attackerUnit)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!defeatedUnit.IsCreature())
            {
                return;
            }

            if (!attackerUnit.IsPlayer())
            {
                Piece piece2;
                var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
                if (attackerUnit.boardPieceId == BoardPieceId.WarlockMinion && attackerUnit.GetHealth() > 0)
                {
                    PieceAI pieceAI = attackerUnit.pieceAI;
                    if (pieceAI == null)
                    {
                        return;
                    }
                    else if (pieceAI.memory.TryGetAssociatedPiece(gameContext.pieceAndTurnController, out piece2))
                    {
                        attackerUnit = piece2;
                    }
                    else
                    {
                        return;
                    }
                }
                else if (attackerUnit.boardPieceId == BoardPieceId.SellswordArbalestierActive)
                {
                    PieceAI pieceAI = attackerUnit.pieceAI;
                    if (pieceAI == null)
                    {
                        return;
                    }
                    else if (pieceAI.memory.TryGetAssociatedPiece(gameContext.pieceAndTurnController, out piece2))
                    {
                        _check = true;
                        attackerUnit = piece2;
                    }
                    else
                    {
                        return;
                    }
                }
                else if (attackerUnit.boardPieceId == BoardPieceId.Verochka)
                {
                    foreach (var piece in _playerPieces)
                    {
                        if (piece.boardPieceId == BoardPieceId.HeroHunter)
                        {
                            _check = true;
                            attackerUnit = piece;
                        }
                    }

                    if (!_check)
                    {
                        return;
                    }
                }
                else if (attackerUnit.boardPieceId == BoardPieceId.Tornado)
                {
                    foreach (var piece in _playerPieces)
                    {
                        if (piece.boardPieceId == BoardPieceId.HeroBard)
                        {
                            _check = true;
                            attackerUnit = piece;
                        }
                    }

                    if (!_check)
                    {
                        return;
                    }
                }
                else if (attackerUnit.boardPieceId == BoardPieceId.GrapplingTotem)
                {
                    foreach (var piece in _playerPieces)
                    {
                        if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                        {
                            _check = true;
                            attackerUnit = piece;
                        }
                    }

                    if (!_check)
                    {
                        return;
                    }
                }
                else if (attackerUnit.boardPieceId == BoardPieceId.SwordOfAvalon)
                {
                    foreach (var piece in _playerPieces)
                    {
                        if (piece.boardPieceId == BoardPieceId.HeroRogue)
                        {
                            _check = true;
                            attackerUnit = piece;
                        }
                    }

                    if (!_check)
                    {
                        return;
                    }
                }
                else if (attackerUnit.boardPieceId == BoardPieceId.SmiteWard)
                {
                    foreach (var piece in _playerPieces)
                    {
                        if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                        {
                            _check = true;
                            attackerUnit = piece;
                        }
                    }

                    if (!_check)
                    {
                        return;
                    }
                }
                else if (attackerUnit.HasEffectState(EffectStateType.ConfusedPermanentVisualOnly) && (attackerUnit.boardPieceId == BoardPieceId.IceElemental || attackerUnit.boardPieceId == BoardPieceId.FireElemental))
                {
                    foreach (var piece in _playerPieces)
                    {
                        if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                        {
                            _check = true;
                            attackerUnit = piece;
                        }
                    }

                    if (!_check)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                if (!attackerUnit.IsPlayer())
                {
                    return;
                }
            }

            if (attackerUnit.HasEffectState(EffectStateType.TorchPlayer))
            {
                var torched = attackerUnit.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.TorchPlayer);
                if (torched < 8)
                {
                    attackerUnit.effectSink.RemoveStatusEffect(EffectStateType.TorchPlayer);
                    attackerUnit.effectSink.AddStatusEffect(EffectStateType.TorchPlayer, torched + 1);
                }
                else
                {
                    attackerUnit.effectSink.RemoveStatusEffect(EffectStateType.TorchPlayer);
                    attackerUnit.effectSink.AddStatusEffect(EffectStateType.TorchPlayer, 8);
                }
            }
            else if (_check && attackerUnit.HasEffectState(EffectStateType.TorchPlayer))
            {
                attackerUnit.effectSink.RemoveStatusEffect(EffectStateType.TorchPlayer);
                attackerUnit.effectSink.AddStatusEffect(EffectStateType.TorchPlayer, 1);
            }
            else
            {
                attackerUnit.effectSink.RemoveStatusEffect(EffectStateType.TorchPlayer);
                attackerUnit.effectSink.AddStatusEffect(EffectStateType.TorchPlayer, 2);
            }

            _check = false;
        }
    }
}
