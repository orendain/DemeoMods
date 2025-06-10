namespace HouseRules.Essentials.Rules
{
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

        protected override void OnPreGameCreated(Context context)
        {
            var abilityPromise = context.AbilityFactory.LoadAbility(AbilityKey.SpawnRat);
            abilityPromise.OnLoaded(ability =>
            {
                ability.pieceToSpawn = BoardPieceId.GoldPile;
                _originalSpawnAmount = ability.spawnMaxAmount;
                ability.spawnMaxAmount = _pileCount;
            });
        }

        protected override void OnDeactivate(Context context)
        {
            var abilityPromise = context.AbilityFactory.LoadAbility(AbilityKey.SpawnRat);
            abilityPromise.OnLoaded(ability =>
            {
                ability.pieceToSpawn = BoardPieceId.Rat;
                ability.spawnMaxAmount = _originalSpawnAmount;
            });
        }
    }
}
