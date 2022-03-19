namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HouseRules.Types;

    public sealed class AbilityBackstabAdjustedRule : Rule, IConfigWritable<Dictionary<AbilityKey, bool>>, IMultiplayerSafe
    {
        public override string Description => "Ability backstab enablement is adjusted";

        private readonly Dictionary<AbilityKey, bool> _adjustments;
        private Dictionary<AbilityKey, bool> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityBackstabAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs of abilityKey and whether backstab bonus is enabled or not.</param>
        public AbilityBackstabAdjustedRule(Dictionary<AbilityKey, bool> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<AbilityKey, bool> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            _originals = ReplaceAbilities(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceAbilities(_originals);
        }

        private static Dictionary<AbilityKey, bool> ReplaceAbilities(Dictionary<AbilityKey, bool> replacements)
        {
            var originals = new Dictionary<AbilityKey, bool>();

            foreach (var replacement in replacements)
            {
                if (!AbilityFactory.TryGetAbility(replacement.Key, out var ability))
                {
                    EssentialsMod.Logger.Warning($"Provided AbilityKey [{replacement.Key}] does not have a corresponding ability. Skipping that ability.");
                }

                originals[replacement.Key] = ability.enableBackstabBonus;
                ability.enableBackstabBonus = replacement.Value;
            }

            return originals;
        }
    }
}
