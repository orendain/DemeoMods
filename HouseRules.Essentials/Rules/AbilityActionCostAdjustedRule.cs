namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HouseRules.Core.Types;

    public sealed class AbilityActionCostAdjustedRule : Rule, IConfigWritable<Dictionary<AbilityKey, bool>>,
        IMultiplayerSafe
    {
        public override string Description => "Hero ability Action Point costs are adjusted";

        private readonly Dictionary<AbilityKey, bool> _adjustments;
        private Dictionary<AbilityKey, bool> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityActionCostAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs of abilityKey and whether the ability costs any actions points.</param>
        public AbilityActionCostAdjustedRule(Dictionary<AbilityKey, bool> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, bool>();
        }

        public Dictionary<AbilityKey, bool> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(Context context)
        {
            _originals = ReplaceAbilities(_adjustments);
        }

        protected override void OnDeactivate(Context context)
        {
            ReplaceAbilities(_originals);
        }

        private static Dictionary<AbilityKey, bool> ReplaceAbilities(Dictionary<AbilityKey, bool> replacements)
        {
            var originals = new Dictionary<AbilityKey, bool>();

            foreach (var replacement in replacements)
            {
                if (!AbilityFactory.TryGetAbility(replacement.Key, out var ability))
                {
                    throw new InvalidOperationException(
                        $"AbilityKey [{replacement.Key}] does not have a corresponding ability.");
                }

                originals[replacement.Key] = ability.costActionPoint;
                ability.costActionPoint = replacement.Value;
            }

            return originals;
        }
    }
}
