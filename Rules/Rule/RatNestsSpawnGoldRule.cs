namespace Rules.Rule
{
    using System.Linq;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using MelonLoader.TinyJSON;
    using UnityEngine;

    public sealed class RatNestsSpawnGoldRule : RulesAPI.Rule, RulesAPI.IConfigWritable
    {
        public override string Description => "Rat nests spawn gold";

        private readonly int _maxPileCount;
        private int _originalSpawnAmount;

        public RatNestsSpawnGoldRule(int maxPileCount)
        {
            _maxPileCount = maxPileCount;
        }

        public static RatNestsSpawnGoldRule FromConfigString(string configString)
        {
            JSON.MakeInto(JSON.Load(configString), out int conf);
            return new RatNestsSpawnGoldRule(conf);
        }

        public string ToConfigString()
        {
            return JSON.Dump(_maxPileCount, EncodeOptions.NoTypeHints);
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
