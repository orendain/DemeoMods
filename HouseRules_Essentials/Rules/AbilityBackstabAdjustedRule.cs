namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class AbilityBackstabAdjustedRule : Rule, IConfigWritable<Dictionary<string, bool>>, IMultiplayerSafe
    {
        public override string Description => "Ability backstab enablement is adjusted";

        private readonly Dictionary<string, bool> _adjustments;
        private Dictionary<string, bool> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityBackstabAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs of abilitykey and bool for whether backstab bonus
        /// is enabled or not.</param>
        public AbilityBackstabAdjustedRule(Dictionary<string, bool> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<string, bool> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            _originals = UpdateAbilities(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            UpdateAbilities(_originals);
        }

        private static Dictionary<string, bool> UpdateAbilities(Dictionary<string, bool> adjustments)
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            var previousValues = new Dictionary<string, bool>();
            foreach (var item in adjustments)
            {
                var ability = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                previousValues.Add(item.Key, ability.enableBackstabBonus);
                ability.enableBackstabBonus = item.Value;
            }

            return previousValues;
        }
    }
}
