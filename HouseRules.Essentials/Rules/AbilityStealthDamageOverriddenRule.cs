namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
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

        protected override void OnPreGameCreated(Context context)
        {
            _originals = ReplaceAbilities(context, _adjustments);
        }

        protected override void OnDeactivate(Context context)
        {
            ReplaceAbilities(context, _originals);
        }

        private static Dictionary<AbilityKey, int> ReplaceAbilities(
            Context context,
            Dictionary<AbilityKey, int> replacements)
        {
            var originals = new Dictionary<AbilityKey, int>();

            foreach (var replacement in replacements)
            {
                var abilityPromise = context.AbilityFactory.LoadAbility(replacement.Key);
                abilityPromise.OnLoaded(ability =>
                {
                    originals[replacement.Key] = ability.stealthBonusDamage;
                    ability.stealthBonusDamage = replacement.Value;
                });
            }

            // Theoretically, there can be a race condition as there's no guarantee the promise above is fulfilled by
            // the return value is used. Realistically, there's no concern since the value isn't used until after the
            // promise has long been fulfilled.
            return originals;
        }
    }
}
