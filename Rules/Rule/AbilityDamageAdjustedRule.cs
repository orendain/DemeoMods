namespace Rules.Rule
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame.BoardEntities.Abilities;
    using UnityEngine;

    public sealed class AbilityDamageAdjustedRule : RulesAPI.Rule, RulesAPI.IConfigWritable<Dictionary<string, int>>
    {
        public override string Description => "Ability damage is adjusted";

        private readonly Dictionary<string, int> _adjustments;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityDamageAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of an entity to the number of action points
        /// added to their base. Negative numbers are allowed.</param>
        public AbilityDamageAdjustedRule(Dictionary<string, int> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<string, int> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            foreach (var item in _adjustments)
            {
                var ability = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                ability.abilityDamage.targetDamage += item.Value;
                ability.abilityDamage.critDamage = ability.abilityDamage.targetDamage * 2;
            }
        }

        protected override void OnDeactivate()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            foreach (var item in _adjustments)
            {
                var ability = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                ability.abilityDamage.targetDamage -= item.Value;
                ability.abilityDamage.critDamage = ability.abilityDamage.targetDamage * 2;
            }
        }
    }
}
