namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class RegainAbilityIfMaxxedOutOverriddenRule : Rule, IConfigWritable<Dictionary<string, bool>>, IMultiplayerSafe
    {
        public override string Description => "RegainAbilityIfMaxxedOut settings are overridden";

        private readonly Dictionary<string, bool> _adjustments;
        private Dictionary<string, bool> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegainAbilityIfMaxxedOutOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of a StatModifier to whether a player gets back
        /// a card after using it when the affected ability is already maxed.</param>
        public RegainAbilityIfMaxxedOutOverriddenRule(Dictionary<string, bool> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<string, bool>();
        }

        public Dictionary<string, bool> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            _originals = ReplaceStatModifiers(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceStatModifiers(_originals);
        }

        private static Dictionary<string, bool> ReplaceStatModifiers(Dictionary<string, bool> replacements)
        {
            var originals = new Dictionary<string, bool>();

            var statModifierType = AccessTools.TypeByName("Boardgame.GameplayEffects.StatModifier");
            var statModifiers = Resources.FindObjectsOfTypeAll(statModifierType);
            foreach (var replacement in replacements)
            {
                var statModifier = statModifiers.First(c => c.name.Equals($"{replacement.Key}(Clone)"));
                originals[replacement.Key] = Traverse.Create(statModifier).Field<bool>("regainAbilityIfMaxxedOut").Value;
                Traverse.Create(statModifier).Field<bool>("regainAbilityIfMaxxedOut").Value = replacement.Value;
            }

            return originals;
        }
    }
}
