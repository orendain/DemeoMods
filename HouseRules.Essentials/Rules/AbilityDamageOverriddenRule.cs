namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Core.Types;

    public sealed class AbilityDamageOverriddenRule : Rule, IConfigWritable<Dictionary<AbilityKey, List<int>>>, IMultiplayerSafe
    {
        public override string Description => "Ability damage is adjusted";

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

        protected override void OnPreGameCreated(Context context)
        {
            _originals = ReplaceAbilities(context, _adjustments);
        }

        protected override void OnDeactivate(Context context)
        {
            ReplaceAbilities(context, _originals);
        }

        private static Dictionary<AbilityKey, List<int>> ReplaceAbilities(
            Context context,
            Dictionary<AbilityKey, List<int>> replacements)
        {
            var originals = new Dictionary<AbilityKey, List<int>>();

            foreach (var replacement in replacements)
            {
                var abilityPromise = context.AbilityFactory.LoadAbility(replacement.Key);
                abilityPromise.OnLoaded(ability =>
                {
                    originals[replacement.Key] =
                        [ability.abilityDamage.targetDamage, ability.abilityDamage.critDamage];
                    ability.abilityDamage.targetDamage = replacement.Value[0];
                    ability.abilityDamage.critDamage = replacement.Value[1];
                });
            }

            // Theoretically, there can be a race condition as there's no guarantee the promise above is fulfilled by
            // the return value is used. Realistically, there's no concern since the value isn't used until after the
            // promise has long been fulfilled.
            return originals;
        }
    }
}
