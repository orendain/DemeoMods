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
        private readonly Dictionary<string, bool> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegainAbilityIfMaxxedOutOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of a StatModifier and the new additiveBonus setting.
        /// Overwrites existing settings. Some StatModifiers require negative values.</param>
        public RegainAbilityIfMaxxedOutOverriddenRule(Dictionary<string, bool> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<string, bool>();
        }

        public Dictionary<string, bool> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            var typeByName = AccessTools.TypeByName("Boardgame.GameplayEffects.StatModifier");
            var statModifiers = Resources.FindObjectsOfTypeAll(typeByName);
            foreach (var item in _adjustments)
            {
                var statModifier = statModifiers.First(c => c.name.Equals($"{item.Key}(Clone)"));
                _originals[item.Key] = Traverse.Create(statModifier).Field<bool>("regainAbilityIfMaxxedOut").Value;
                Traverse.Create(statModifier).Field<bool>("regainAbilityIfMaxxedOut").Value = item.Value;
            }
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            var typeByName = AccessTools.TypeByName("Boardgame.GameplayEffects.StatModifier");
            var statModifiers = Resources.FindObjectsOfTypeAll(typeByName);
            foreach (var item in _originals)
            {
                var statModifier = statModifiers.First(c => c.name.Equals($"{item.Key}(Clone)"));
                _originals[item.Key] = Traverse.Create(statModifier).Field<bool>("regainAbilityIfMaxxedOut").Value;
                Traverse.Create(statModifier).Field<bool>("regainAbilityIfMaxxedOut").Value = item.Value;
            }
        }
    }
}
