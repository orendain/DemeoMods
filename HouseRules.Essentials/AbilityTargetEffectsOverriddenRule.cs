namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using DataKeys;
    using HouseRules.Core.Types;

    public sealed class AbilityTargetEffectsOverriddenRule : Rule, IConfigWritable<Dictionary<AbilityKey, List<EffectStateType>>>, IMultiplayerSafe
    {
        public override string Description => "Some abilities have added secondary effects.";

        private readonly Dictionary<AbilityKey, List<EffectStateType>> _adjustments;
        private Dictionary<AbilityKey, List<EffectStateType>> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityTargetEffectsOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs of abilityKey and List<EffectStateType>.</param>
        public AbilityTargetEffectsOverriddenRule(Dictionary<AbilityKey, List<EffectStateType>> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, List<EffectStateType>>();
        }

        public Dictionary<AbilityKey, List<EffectStateType>> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(Context context)
        {
            _originals = ReplaceAbilities(context, _adjustments);
        }

        protected override void OnDeactivate(Context context)
        {
            ReplaceAbilities(context, _originals);
        }

        private static Dictionary<AbilityKey, List<EffectStateType>> ReplaceAbilities(Context context, Dictionary<AbilityKey, List<EffectStateType>> replacements)
        {
            var originals = new Dictionary<AbilityKey, List<EffectStateType>>();
            foreach (var replacement in replacements)
            {
                var abilityPromise = context.AbilityFactory.LoadAbility(replacement.Key);
                abilityPromise.OnLoaded(ability =>
                {
                    originals[replacement.Key] = ability.targetEffects.ToList();
                    ability.targetEffects = replacement.Value.ToArray();
                });
            }

            // Theoretically, there can be a race condition as there's no guarantee the promise above is fulfilled by
            // the return value is used. Realistically, there's no concern since the value isn't used until after the
            // promise has long been fulfilled.
            return originals;
        }
    }
}
