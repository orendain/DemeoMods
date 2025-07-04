﻿namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using DataKeys;
    using HouseRules.Core.Types;

    public sealed class AbilityRandomPieceListRule : Rule, IConfigWritable<Dictionary<AbilityKey, List<BoardPieceId>>>
    {
        public override string Description => "Ability random summons are replaced (Skirmish Only)";

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

        protected override void OnPreGameCreated(Context context)
        {
            _originals = ReplaceAbilities(context, _adjustments);
        }

        protected override void OnDeactivate(Context context)
        {
            ReplaceAbilities(context, _originals);
        }

        private static Dictionary<AbilityKey, List<BoardPieceId>> ReplaceAbilities(
            Context context,
            Dictionary<AbilityKey, List<BoardPieceId>> replacements)
        {
            var originals = new Dictionary<AbilityKey, List<BoardPieceId>>();

            foreach (var replacement in replacements)
            {
                var abilityPromise = context.AbilityFactory.LoadAbility(replacement.Key);
                abilityPromise.OnLoaded(ability =>
                {
                    originals[replacement.Key] = ability.randomPieceList.ToList();
                    ability.randomPieceList = replacement.Value.ToArray();
                });
            }

            // Theoretically, there can be a race condition as there's no guarantee the promise above is fulfilled by
            // the return value is used. Realistically, there's no concern since the value isn't used until after the
            // promise has long been fulfilled.
            return originals;
        }
    }
}
