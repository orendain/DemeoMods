namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class RegainAbilityIfMaxxedOutOverriddenRule : Rule, IConfigWritable<Dictionary<AbilityKey, bool>>, IMultiplayerSafe
    {
        public override string Description => "RegainAbilityIfMaxxedOut settings are overridden";

        private readonly Dictionary<AbilityKey, bool> _adjustments;
        private Dictionary<AbilityKey, bool> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegainAbilityIfMaxxedOutOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of a StatModifier to whether a player gets back
        /// a card after using it when the affected ability is already maxed.</param>
        public RegainAbilityIfMaxxedOutOverriddenRule(Dictionary<AbilityKey, bool> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, bool>();
        }

        public Dictionary<AbilityKey, bool> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            _originals = ReplaceStatModifiers(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceStatModifiers(_originals);
        }

        private static Dictionary<AbilityKey, bool> ReplaceStatModifiers(Dictionary<AbilityKey, bool> replacements)
        {
            var originals = new Dictionary<AbilityKey, bool>();

            var statModifierType = AccessTools.TypeByName("Boardgame.GameplayEffects.StatModifier");
            foreach (var replacement in replacements)
            {
                if (!AbilityFactory.TryGetAbility(replacement.Key, out var ability))
                {
                    throw new InvalidOperationException($"AbilityKey [{replacement.Key}] does not have a corresponding ability.");
                }

                if (!ability.TryGetComponent(statModifierType, out var statModifier))
                {
                    throw new InvalidOperationException($"AbilityKey [{replacement.Key}] does not have a corresponding StatModifier.");
                }

                originals[replacement.Key] = Traverse.Create(statModifier).Field<bool>("regainAbilityIfMaxxedOut").Value;
                Traverse.Create(statModifier).Field<bool>("regainAbilityIfMaxxedOut").Value = replacement.Value;
            }

            return originals;
        }
    }
}
