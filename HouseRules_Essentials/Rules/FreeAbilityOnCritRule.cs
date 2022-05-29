﻿namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class FreeAbilityOnCritRule : Rule, IConfigWritable<Dictionary<BoardPieceId, AbilityKey>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Critical Hit gives free card.";

        private static Dictionary<BoardPieceId, AbilityKey> _globalAdjustments;
        private static bool _isActivated;

        private readonly Dictionary<BoardPieceId, AbilityKey> _adjustments;

        public FreeAbilityOnCritRule(Dictionary<BoardPieceId, AbilityKey> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<BoardPieceId, AbilityKey> GetConfigObject() => _adjustments;

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
                    typeof(FreeAbilityOnCritRule),
                    nameof(Ability_GenerateAttackDamage_Prefix)));
        }

        private static bool Ability_GenerateAttackDamage_Prefix(Piece source, Dice.Outcome diceResult)
        {
            if (!_isActivated)
            {
                return true;
            }

            if (diceResult == Dice.Outcome.Crit)
            {
                if (source.IsPlayer() && _globalAdjustments.ContainsKey(source.boardPieceId))
                {
                    source.TryAddAbilityToInventory(_globalAdjustments[source.boardPieceId], isReplenishable: false);
                    HR.ScheduleBoardSync();
                }
            }

            return true; // Allow the regular GenerateAttackDamage function to run afterwards.
        }
    }
}