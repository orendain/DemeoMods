namespace RulesAPI.Essentials.Rules
{
    using System.Linq;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using UnityEngine;

    public sealed class RatNestsSpawnGoldRule : Rule, IConfigWritable<int>
    {
        public override string Description => "Rat nests spawn gold";

        private readonly int _pileCount;
        private int _originalSpawnAmount;

        public RatNestsSpawnGoldRule(int pileCount)
        {
            _pileCount = pileCount;
        }

        public int GetConfigObject() => _pileCount;

        protected override void OnPostGameCreated()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            var ability = abilities.First(c => c.name.Equals("SpawnRat(Clone)"));
            ability.pieceToSpawn = BoardPieceId.GoldPile;
            _originalSpawnAmount = ability.spawnMaxAmount;
            ability.spawnMaxAmount = _pileCount;
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
