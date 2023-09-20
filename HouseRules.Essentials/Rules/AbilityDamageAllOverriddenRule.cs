namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HouseRules.Core.Types;

    public sealed class AbilityDamageAllOverriddenRule : Rule, IConfigWritable<Dictionary<AbilityKey, List<int>>>, IMultiplayerSafe
    {
        public override string Description => "Some abilities have had their damage adjusted";

        private readonly Dictionary<AbilityKey, List<int>> _adjustments;
        private Dictionary<AbilityKey, List<int>> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityDamageAllOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">KV pairs of AbilityKeys and list of targetDamage and critDamage
        /// to replace default values.</param>
        public AbilityDamageAllOverriddenRule(Dictionary<AbilityKey, List<int>> adjustments)
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

        private static Dictionary<AbilityKey, List<int>> ReplaceAbilities(
            Dictionary<AbilityKey, List<int>> replacements)
        {
            var originals = new Dictionary<AbilityKey, List<int>>();

            foreach (var replacement in replacements)
            {
                if (!AbilityFactory.TryGetAbility(replacement.Key, out var ability))
                {
                    throw new InvalidOperationException(
                        $"AbilityKey [{replacement.Key}] does not have a corresponding ability.");
                }

                originals[replacement.Key] = new List<int>
                    { ability.abilityDamage.targetDamage, ability.abilityDamage.critDamage, ability.abilityDamage.splashDamage, ability.abilityDamage.critSplashDamage };
                ability.abilityDamage.targetDamage = replacement.Value[0];
                ability.abilityDamage.critDamage = replacement.Value[1];
                ability.abilityDamage.splashDamage = replacement.Value[2];
                ability.abilityDamage.critSplashDamage = replacement.Value[3];
            }

            return originals;
        }
    }
}
