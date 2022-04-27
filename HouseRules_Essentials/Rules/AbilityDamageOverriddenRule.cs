namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HouseRules.Types;

    public sealed class AbilityDamageOverriddenRule: Rule, IConfigWritable<Dictionary<AbilityKey, List<int>>>, IMultiplayerSafe
    {
        public override string Description => "Ability damage values are overridden";

        private readonly Dictionary<AbilityKey, List<int>> _adjustments;
        private Dictionary<AbilityKey, List<int>> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityDamageOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">KV pairs of AbilityKeys and list of targetDamage and critDamage
        /// to replace default values.</param>
        public AbilityDamageOverriddenRule(Dictionary<AbilityKey, List<int>> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, List<int>>();
        }

        public Dictionary<AbilityKey, List<int>> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            _originals = ReplaceAbilities(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceAbilities(_originals);
        }

        private static Dictionary<AbilityKey, List<int>> ReplaceAbilities(Dictionary<AbilityKey, List<int>> replacements)
        {
            var originals = new Dictionary<AbilityKey, List<int>>();

            foreach (var replacement in replacements)
            {
                if (!AbilityFactory.TryGetAbility(replacement.Key, out var ability))
                {
                    throw new InvalidOperationException($"AbilityKey [{replacement.Key}] does not have a corresponding ability.");
                }

                originals[replacement.Key] = new List<int> { ability.abilityDamage.targetDamage, ability.abilityDamage.critDamage };
                ability.abilityDamage.targetDamage = replacement.Value[0];
                ability.abilityDamage.critDamage = replacement.Value[1];
            }

            return originals;
        }
    }
}
