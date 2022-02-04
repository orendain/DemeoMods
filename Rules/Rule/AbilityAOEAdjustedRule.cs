namespace Rules.Rule
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame.BoardEntities.Abilities;
    using HarmonyLib;
    using UnityEngine;

    public sealed class AbilityAOEAdjustedRule : RulesAPI.Rule
    {
        public override string Description => "Ability AOE Ranges are adjusted";

        private readonly Dictionary<string, int> _adjustments;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityAOEAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of an ability and the AOE range
        /// added to their base. Adding '1' to a 3x3 spell will make a 5x5. Negative values will reduce AOE.</param>
        public AbilityAOEAdjustedRule(Dictionary<string, int> adjustments)
        {
            _adjustments = adjustments;
        }

        protected override void OnPostGameCreated()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            foreach (var item in _adjustments)
            {
                var myAbility = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                var myAOE = Traverse.Create(myAbility).Field<AreaOfEffect>("areaOfEffect").Value;
                Traverse.Create(myAOE).Field<int>("range").Value += item.Value; // Adjust the yellow AOE outline when casting.
                myAbility.areaOfEffectRange += item.Value; // Adjust value displayed on the card.
            }
        }

        protected override void OnDeactivate()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            foreach (var item in _adjustments)
            {
                var myAbility = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                var myAOE = Traverse.Create(myAbility).Field<AreaOfEffect>("areaofEffect").Value;
                Traverse.Create(myAOE).Field<int>("range").Value -= item.Value; // Adjust the yellow AOE outline when casting.
                myAbility.areaOfEffectRange -= item.Value; // Adjust value displayed on the card.
            }
        }
    }
}
