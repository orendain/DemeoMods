namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class StatModifiersOverriddenRule : Rule, IConfigWritable<Dictionary<AbilityKey, int>>,
        IMultiplayerSafe
    {
        public override string Description => "Stat modifier abilities are adjusted";

        private readonly Dictionary<AbilityKey, int> _adjustments;
        private Dictionary<AbilityKey, int> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatModifiersOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of a StatModifier and the new additiveBonus setting.
        /// Overwrites existing settings. Some StatModifiers require negative values.</param>
        public StatModifiersOverriddenRule(Dictionary<AbilityKey, int> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, int>();
        }

        public Dictionary<AbilityKey, int> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(Context context)
        {
            _originals = ReplaceStatModifiers(_adjustments);
        }

        protected override void OnDeactivate(Context context)
        {
            ReplaceStatModifiers(_originals);
        }

        private static Dictionary<AbilityKey, int> ReplaceStatModifiers(Dictionary<AbilityKey, int> replacements)
        {
            var originals = new Dictionary<AbilityKey, int>();

            var statModifierType = AccessTools.TypeByName("Boardgame.GameplayEffects.StatModifier");
            foreach (var replacement in replacements)
            {
                if (!AbilityFactory.TryGetAbility(replacement.Key, out var ability))
                {
                    throw new InvalidOperationException(
                        $"AbilityKey [{replacement.Key}] does not have a corresponding ability.");
                }

                if (!ability.TryGetComponent(statModifierType, out var statModifier))
                {
                    throw new InvalidOperationException(
                        $"AbilityKey [{replacement.Key}] does not have a corresponding StatModifier.");
                }

                originals[replacement.Key] = Traverse.Create(statModifier).Field<int>("additiveBonus").Value;
                Traverse.Create(statModifier).Field<int>("additiveBonus").Value = replacement.Value;
            }

            return originals;
        }
    }
}
