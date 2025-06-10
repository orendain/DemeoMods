namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Core.Types;

    public sealed class AbilityBackstabAdjustedRule : Rule, IConfigWritable<Dictionary<AbilityKey, bool>>,
        IMultiplayerSafe
    {
        public override string Description => "Hero ability backstab bonus damage is adjusted";

        private readonly Dictionary<AbilityKey, bool> _adjustments;
        private Dictionary<AbilityKey, bool> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityBackstabAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs of abilityKey and whether backstab bonus is enabled or not.</param>
        public AbilityBackstabAdjustedRule(Dictionary<AbilityKey, bool> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, bool>();
        }

        public Dictionary<AbilityKey, bool> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(Context context)
        {
            _originals = ReplaceAbilities(context, _adjustments);
        }

        protected override void OnDeactivate(Context context)
        {
            ReplaceAbilities(context, _originals);
        }

        private static Dictionary<AbilityKey, bool> ReplaceAbilities(
            Context context,
            Dictionary<AbilityKey, bool> replacements)
        {
            var originals = new Dictionary<AbilityKey, bool>();

            foreach (var replacement in replacements)
            {
                var abilityPromise = context.AbilityFactory.LoadAbility(replacement.Key);
                abilityPromise.OnLoaded(ability =>
                {
                    originals[replacement.Key] = ability.enableBackstabBonus;
                    ability.enableBackstabBonus = replacement.Value;
                });
            }

            // Theoretically, there can be a race condition as there's no guarantee the promise above is fulfilled by
            // the return value is used. Realistically, there's no concern since the value isn't used until after the
            // promise has long been fulfilled.
            return originals;
        }
    }
}
