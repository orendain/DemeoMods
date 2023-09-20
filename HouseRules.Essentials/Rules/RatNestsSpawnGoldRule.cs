namespace HouseRules.Essentials.Rules
{
    using System;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HouseRules.Core.Types;

    public sealed class RatNestsSpawnGoldRule : Rule, IConfigWritable<int>
    {
        public override string Description => "Rat nests spawn gold (Skirmish Only)";

        private readonly int _pileCount;
        private int _originalSpawnAmount;

        public RatNestsSpawnGoldRule(int pileCount)
        {
            _pileCount = pileCount;
        }

        public int GetConfigObject() => _pileCount;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            if (!AbilityFactory.TryGetAbility(AbilityKey.SpawnRat, out var ability))
            {
                throw new InvalidOperationException(
                    $"AbilityKey [{AbilityKey.SpawnRat}] does not have a corresponding ability.");
            }

            ability.pieceToSpawn = BoardPieceId.GoldPile;
            _originalSpawnAmount = ability.spawnMaxAmount;
            ability.spawnMaxAmount = _pileCount;
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            if (!AbilityFactory.TryGetAbility(AbilityKey.SpawnRat, out var ability))
            {
                throw new InvalidOperationException(
                    $"AbilityKey [{AbilityKey.SpawnRat}] does not have a corresponding ability.");
            }

            ability.pieceToSpawn = BoardPieceId.Rat;
            ability.spawnMaxAmount = _originalSpawnAmount;
        }
    }
}
