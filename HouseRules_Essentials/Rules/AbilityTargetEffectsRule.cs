namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HouseRules.Types;

    public sealed class AbilityTargetEffectsRule : Rule, IConfigWritable<Dictionary<AbilityKey, List<EffectStateType>>>, IMultiplayerSafe
    {
        public override string Description => "Ability effects on hit are modified.";

        private readonly Dictionary<AbilityKey, List<EffectStateType>> _adjustments;
        private Dictionary<AbilityKey, List<EffectStateType>> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityTargetEffectsRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs of abilityKey and List<EffectStateType>.</param>
        public AbilityTargetEffectsRule(Dictionary<AbilityKey, List<EffectStateType>> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, List<EffectStateType>>();
        }

        public Dictionary<AbilityKey, List<EffectStateType>> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            _originals = ReplaceAbilities(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceAbilities(_originals);
        }

        private static Dictionary<AbilityKey, List<EffectStateType>> ReplaceAbilities(Dictionary<AbilityKey, List<EffectStateType>> replacements)
        {
            var originals = new Dictionary<AbilityKey, List<EffectStateType>>();

            // var pieceTypes = new List<PieceType> { PieceType.Creature, PieceType.Prop };
            // var invalidPieces = new List<PieceType> { PieceType.Player };
            // var teamMode = new List<Ability.TeamMode> { Ability.TeamMode.OpponentTeam };
            foreach (var replacement in replacements)
            {
                if (!AbilityFactory.TryGetAbility(replacement.Key, out var ability))
                {
                    throw new InvalidOperationException($"AbilityKey [{replacement.Key}] does not have a corresponding ability.");
                }

                originals[replacement.Key] = ability.targetEffects.ToList();
                ability.targetEffects = replacement.Value.ToArray();

                // ability.validEffectTargets = pieceTypes;
                // ability.invalidEffectTargets = invalidPieces;
                // ability.validEffectTeams = teamMode;
            }

            // Barbarian can Grapple and Throw Lamps in same turn
            AbilityFactory.TryGetAbility(AbilityKey.Grapple, out var grapple);
            grapple.effectAppliedToSelf = EffectStateType.It;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingIceLamp, out var launchIce);
            launchIce.effectAppliedToSelf = EffectStateType.It;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingIceLamp, out var launchFire);
            launchFire.effectAppliedToSelf = EffectStateType.It;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingIceLamp, out var launchVortex);
            launchVortex.effectAppliedToSelf = EffectStateType.It;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingIceLamp, out var launchGas);
            launchGas.effectAppliedToSelf = EffectStateType.It;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingIceLamp, out var launchWater);
            launchWater.effectAppliedToSelf = EffectStateType.It;

            return originals;
        }
    }
}
