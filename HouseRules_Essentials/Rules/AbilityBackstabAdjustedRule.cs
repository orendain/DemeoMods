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
        public override string Description => "Ability AP costs are adjusted";

        private readonly Dictionary<string, bool> _adjustments;

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
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            foreach (var item in _adjustments)
            {
                var ability = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                ability.enableBackstabBonus = item.Value;
            }
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            foreach (var item in _adjustments)
            {
                var ability = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                ability.enableBackstabBonus = !item.Value;
            }
        }
    }
}
