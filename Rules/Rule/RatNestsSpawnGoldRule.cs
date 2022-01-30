namespace Rules.Rule
{
    using System.Linq;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using UnityEngine;

    public sealed class RatNestsSpawnGoldRule : RulesAPI.Rule
    {
        public override string Description => "Rat nests spawn gold";

        private readonly int _maxPileCount;
        private int _originalSpawnAmount;

        public RatNestsSpawnGoldRule(int maxPileCount)
        {
            _maxPileCount = maxPileCount;
        }

        protected override void OnPostGameCreated()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            var ability = abilities.First(c => c.name.Equals("SpawnRat(Clone)"));
            ability.pieceToSpawn = BoardPieceId.GoldPile;
            _originalSpawnAmount = ability.spawnMaxAmount;
            ability.spawnMaxAmount = _maxPileCount;
        }

        protected override void OnDeactivate()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            var ability = abilities.First(c => c.name.Equals("SpawnRat(Clone)"));
            ability.pieceToSpawn = BoardPieceId.Rat;
            ability.spawnMaxAmount = _originalSpawnAmount;
        }
    }
}
