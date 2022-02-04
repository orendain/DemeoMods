namespace Rules.Rule
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame.BoardEntities.Abilities;
    using UnityEngine;

    public sealed class AbilityActionCostAdjustedRule : RulesAPI.Rule
    {
        public override string Description => "Ability AP costs are adjusted";

        private readonly Dictionary<string, bool> _adjustments;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityActionCostAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of an entity to the number of action points
        /// added to their base. Negative numbers are allowed.</param>
        public AbilityActionCostAdjustedRule(Dictionary<string, bool> adjustments)
        {
            _adjustments = adjustments;
        }

        protected override void OnPostGameCreated()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            foreach (var item in _adjustments)
            {
                var ability = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                ability.costActionPoint= item.Value;
            }
        }

        protected override void OnDeactivate()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            foreach (var item in _adjustments)
            {
                var ability = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                ability.costActionPoint = !item.Value;
            }
        }
    }
}
