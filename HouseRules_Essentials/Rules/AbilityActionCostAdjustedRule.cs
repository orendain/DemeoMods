namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class AbilityActionCostAdjustedRule : Rule, IConfigWritable<Dictionary<string, bool>>, IMultiplayerSafe
    {
        public override string Description => "Ability AP costs are adjusted";

        private readonly Dictionary<string, bool> _adjustments;
        private Dictionary<string, bool> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityActionCostAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs of abilitykey and bool that controls
        /// the costActionPoint setting.</param>
        public AbilityActionCostAdjustedRule(Dictionary<string, bool> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<string, bool> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            _originals = UpdateActionPoints(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            UpdateActionPoints(_originals);
        }

        private static Dictionary<string, bool> UpdateActionPoints(Dictionary<string, bool> adjustments)
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            var previousValues = new Dictionary<string, bool>();
            foreach (var item in adjustments)
            {
                var ability = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                previousValues.Add(item.Key, ability.costActionPoint);
                ability.costActionPoint = item.Value;
            }

            return previousValues;
        }
    }
}
