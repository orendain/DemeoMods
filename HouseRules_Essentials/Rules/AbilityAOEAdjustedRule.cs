namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class AbilityAoeAdjustedRule : Rule, IConfigWritable<Dictionary<string, int>>, IMultiplayerSafe
    {
        public override string Description => "Ability AOE Ranges are adjusted";

        private readonly Dictionary<string, int> _adjustments;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityAoeAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of an ability and the AOE range
        /// added to their base. Adding '1' to a 3x3 spell will make a 5x5. Negative values will reduce AOE.</param>
        public AbilityAoeAdjustedRule(Dictionary<string, int> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<string, int> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            foreach (var item in _adjustments)
            {
                var ability = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                var aoe = Traverse.Create(ability).Field<AreaOfEffect>("areaOfEffect").Value;
                Traverse.Create(aoe).Field<int>("range").Value += item.Value; // Adjust the yellow AOE outline when casting.
                ability.areaOfEffectRange += item.Value; // Adjust value displayed on the card.
            }
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            foreach (var item in _adjustments)
            {
                var ability = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                var aoe = Traverse.Create(ability).Field<AreaOfEffect>("areaOfEffect").Value;
                Traverse.Create(aoe).Field<int>("range").Value -= item.Value; // Adjust the yellow AOE outline when casting.
                ability.areaOfEffectRange -= item.Value; // Adjust value displayed on the card.
            }
        }
    }
}
