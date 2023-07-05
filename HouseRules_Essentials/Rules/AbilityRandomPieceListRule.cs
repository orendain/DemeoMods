namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HouseRules.Types;

    public sealed class AbilityRandomPieceListRule : Rule, IConfigWritable<Dictionary<AbilityKey, List<BoardPieceId>>>, IMultiplayerSafe
    {
        public override string Description => "Random summon abilities are adjusted";

        private readonly Dictionary<AbilityKey, List<BoardPieceId>> _adjustments;
        private Dictionary<AbilityKey, List<BoardPieceId>> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityRandomPieceListRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of ability name string and list of BoardPieceIDs.</param>
        /// <remarks>Replaces the list of pieces that certain abilities will spawn (e.g. BeastWhisperer, SpawnCultists etc).</remarks>
        public AbilityRandomPieceListRule(Dictionary<AbilityKey, List<BoardPieceId>> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, List<BoardPieceId>>();
        }

        public Dictionary<AbilityKey, List<BoardPieceId>> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            _originals = ReplaceAbilities(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceAbilities(_originals);
        }

        private static Dictionary<AbilityKey, List<BoardPieceId>> ReplaceAbilities(
            Dictionary<AbilityKey, List<BoardPieceId>> replacements)
        {
            var originals = new Dictionary<AbilityKey, List<BoardPieceId>>();

            foreach (var replacement in replacements)
            {
                if (!AbilityFactory.TryGetAbility(replacement.Key, out var ability))
                {
                    throw new InvalidOperationException(
                        $"AbilityKey [{replacement.Key}] does not have a corresponding ability.");
                }

                originals[replacement.Key] = ability.randomPieceList.ToList();
                ability.randomPieceList = replacement.Value.ToArray();
            }

            return originals;
        }
    }
}
