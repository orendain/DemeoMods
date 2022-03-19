﻿namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class AbilityAoeAdjustedRule : Rule, IConfigWritable<Dictionary<AbilityKey, int>>, IMultiplayerSafe
    {
        public override string Description => "Ability AOE Ranges are adjusted";

        private readonly Dictionary<AbilityKey, int> _adjustments;
        private Dictionary<AbilityKey, int> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityAoeAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of an ability and the AOE range
        /// added to their base. Adding '1' to a 3x3 spell will make a 5x5. Negative values will reduce AOE.</param>
        public AbilityAoeAdjustedRule(Dictionary<AbilityKey, int> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, int>();
        }

        public Dictionary<AbilityKey, int> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            _originals = ReplaceAbility(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceAbility(_originals);
        }

        private static Dictionary<AbilityKey, int> ReplaceAbility(Dictionary<AbilityKey, int> replacements)
        {
            var originals = new Dictionary<AbilityKey, int>();

            foreach (var replacement in replacements)
            {
                if (!AbilityFactory.TryGetAbility(replacement.Key, out var ability))
                {
                    EssentialsMod.Logger.Warning($"Provided AbilityKey [{replacement.Key}] does not have a corresponding ability. Skipping that ability.");
                }

                originals[replacement.Key] = ability.areaOfEffectRange;
                var aoe = Traverse.Create(ability).Field<AreaOfEffect>("areaOfEffect").Value;
                Traverse.Create(aoe).Field<int>("range").Value += replacement.Value; // Adjust the AOE outline when casting.
                ability.areaOfEffectRange += replacement.Value; // Adjust value displayed on the card.
            }

            return originals;
        }
    }
}
