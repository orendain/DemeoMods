namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class StatModifiersOverridenRule : Rule, IConfigWritable<Dictionary<string, int>>, IMultiplayerSafe
    {
        public override string Description => "StatModifiersOverridenRule are overridden";

        private readonly Dictionary<string, int> _adjustments;
        private readonly Dictionary<string, int> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatModifiersOverridenRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of a StatModifier and the new additiveBonus setting.
        /// Overwrites existing settings. Some StatModifiers require negative values.</param>
        public StatModifiersOverridenRule(Dictionary<string, int> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<string, int>();
        }

        public Dictionary<string, int> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            var typeByName = AccessTools.TypeByName("Boardgame.GameplayEffects.StatModifier");
            var statModifiers = Resources.FindObjectsOfTypeAll(typeByName);
            foreach (var item in _adjustments)
            {
                var statModifier = statModifiers.First(c => c.name.Equals($"{item.Key}(Clone)"));
                _originals[item.Key] = Traverse.Create(statModifier).Field<int>("additiveBonus").Value;
                Traverse.Create(statModifier).Field<int>("additiveBonus").Value = item.Value;
            }
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            var typeByName = AccessTools.TypeByName("Boardgame.GameplayEffects.StatModifier");
            var statModifiers = Resources.FindObjectsOfTypeAll(typeByName);
            foreach (var item in _originals)
            {
                var statModifier = statModifiers.First(c => c.name.Equals($"{item.Key}(Clone)"));
                _originals[item.Key] = Traverse.Create(statModifier).Field<int>("additiveBonus").Value;
                Traverse.Create(statModifier).Field<int>("additiveBonus").Value = item.Value;
            }
        }
    }
}
