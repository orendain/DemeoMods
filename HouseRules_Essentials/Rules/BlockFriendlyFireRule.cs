namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.Data;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class BlockFriendlyFireRule : Rule, IConfigWritable<bool>, IPatchable,
        IMultiplayerSafe
    {
        public override string Description => "Friendly fire is blocked.";

        private static bool _globalAdjustments;
        private static bool _isActivated;

        private readonly bool _adjustments;

        public BlockFriendlyFireRule(bool adjustments)
        {
            _adjustments = adjustments;
        }

        public bool GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Ability), "GenerateAttackDamage"),
                prefix: new HarmonyMethod(
                    typeof(BlockFriendlyFireRule),
                    nameof(Ability_GenerateAttackDamage_Prefix)));
        }

        private static bool Ability_GenerateAttackDamage_Prefix(Ability __instance, Piece source, Piece mainTarget, Dice.Outcome diceResult, BoardModel boardModel, Piece[] targets, ref Damage __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            bool isBackStab = false;
            if (source != null && source.IsRogue() && __instance.enableBackstabBonus)
            {
                isBackStab = Traverse.Create(__instance).Method("IsBackstab", paramTypes: new[] { typeof(Piece), typeof(Piece), typeof(BoardModel) }, arguments: new object[] { source, mainTarget, boardModel }).GetValue<bool>();
            }

            if (source != null && targets.Length != 0)
            {
                List<Piece> list = new List<Piece>();
                for (int i = 0; i < targets.Length; i++)
                {
                    var isBlockedFromFront = Traverse.Create(__instance).Method("IsBlockedFromFront", paramTypes: new[] { typeof(Piece), typeof(Piece), typeof(BoardModel) }, arguments: new object[] { source, targets[i], boardModel }).GetValue<bool>();
                    if ((Enum.IsDefined(typeof(FrontBlockingPieces), (int)targets[i].boardPieceId) && isBlockedFromFront && __instance.isBlockable) || (source.IsPlayer() && targets[i].IsPlayer()))
                    {
                        list.Add(targets[i]);
                    }
                }

                __result = new Damage(__instance, source, diceResult, isBackStab, list);
                return false;
            }

            __result = new Damage(__instance, source, diceResult, isBackStab, null);
            return false;
        }
    }
}
