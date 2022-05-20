﻿namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.GameplayEffects;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class RegroupAlliesRule : Rule, IConfigWritable<int>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Allies are included in Regroup.";

        private static int _maxNum;
        private static bool _isActivated;

        private readonly int _adjustments;

        private enum TeleportTargetPiece
        {
            Attacker,
            Defenders,
            MyTeam,
        }

        public RegroupAlliesRule(int adjustments)
        {
            _adjustments = adjustments;
        }

        public int GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _maxNum = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Teleport), "GetPiecesToTeleport"),
                prefix: new HarmonyMethod(
                    typeof(RegroupAlliesRule),
                    nameof(Teleport_GetPiecesToTeleport_Prefix)));
        }

        private static bool Teleport_GetPiecesToTeleport_Prefix(ref List<Target> __result, TeleportTargetPiece mode, int maxNum, AbilityContext abilityContext, PieceAndTurnController pieceAndTurnController)
        {
            if (!_isActivated)
            {
                return true;
            }

            List<Target> list = new List<Target>(maxNum);
            if (mode == TeleportTargetPiece.Attacker || mode == TeleportTargetPiece.Defenders)
            {
                return true;
            }
            else if (mode == TeleportTargetPiece.MyTeam)
            {
                var attacker = AccessTools.StructFieldRefAccess<AbilityContext, Target>(ref abilityContext, "attacker");
                int me = attacker.piece.networkID;
                PieceType type = attacker.piece.IsPlayer() ? PieceType.Player : PieceType.Enemy;
                int count = 0;
                pieceAndTurnController.ForEachPiece(
                    delegate(Piece piece)
                {
                    if (piece.HasPieceType(type) && piece.networkID != me && count < _maxNum)
                    {
                        list.Add(new Target(piece.effectSink));
                        count++;
                    }

                    if (type == PieceType.Player && (piece.IsPlayerFaction() ||
                        piece.HasEffectState(EffectStateType.ConfusedPermanent)) &&
                        !piece.HasEffectState(EffectStateType.SelfDestruct) &&
                        !piece.IsPlayer() &&
                        !piece.IsProp() &&
                        count < _maxNum)
                    {
                        list.Add(new Target(piece.effectSink));
                        count++;
                    }
                },
                    PieceAndTurnController.SearchPiece.IncludeDisabled);
            }

            __result = list;

            return false; // We returned an user-adjusted config.
        }
    }
}
