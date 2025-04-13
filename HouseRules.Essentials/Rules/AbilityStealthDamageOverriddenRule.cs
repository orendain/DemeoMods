namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HouseRules.Core.Types;

    public sealed class AbilityStealthDamageOverriddenRule : Rule, IConfigWritable<Dictionary<AbilityKey, int>>, IMultiplayerSafe
    {
        public override string Description => "Ability stealth bonus damage values are adjusted";

        private readonly Dictionary<AbilityKey, int> _adjustments;
        private Dictionary<AbilityKey, int> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityStealthDamageOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs of abilityKey and extra stealth damage.</param>
        public AbilityStealthDamageOverriddenRule(Dictionary<AbilityKey, int> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, int>();
        }

        public Dictionary<AbilityKey, int> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            _originals = ReplaceAbilities(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceAbilities(_originals);
        }

        private static Dictionary<AbilityKey, int> ReplaceAbilities(Dictionary<AbilityKey, int> replacements)
        {
            var originals = new Dictionary<AbilityKey, int>();
            IAssetContextInterface? assetContext = null;
            AbilityFactory abilityFactory = new(assetContext);
            foreach (var replacement in replacements)
            {
                if (!abilityFactory.TryGetAbility(replacement.Key, out var ability))
                {
                    throw new InvalidOperationException($"AbilityKey [{replacement.Key}] does not have a corresponding ability.");
                }

                originals[replacement.Key] = ability.stealthBonusDamage;
                ability.stealthBonusDamage = replacement.Value;
            }

            return originals;
        }
    }
}
