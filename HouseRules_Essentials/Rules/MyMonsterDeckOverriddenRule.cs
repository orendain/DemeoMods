namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using Utils;

    public sealed class MyMonsterDeckOverriddenRule : Rule, IConfigWritable<MyMonsterDeckOverriddenRule.MyDeckConfig>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "My MonsterDeck creation is overriden";

        private static MyDeckConfig _globalAdjustments;
        private static bool _isActivated;

        private readonly MyDeckConfig _adjustments;

        public struct MyDeckConfig
        {
            public Dictionary<BoardPieceId, int> EntranceDeckFloor1;
            public Dictionary<BoardPieceId, int> ExitDeckFloor1;
            public Dictionary<BoardPieceId, int> EntranceDeckFloor2;
            public Dictionary<BoardPieceId, int> ExitDeckFloor2;
            public Dictionary<BoardPieceId, int> BossDeck;
            public BoardPieceId KeyHolderFloor1;
            public BoardPieceId KeyHolderFloor2;
        }

        public MyMonsterDeckOverriddenRule(MyDeckConfig adjustments)
        {
            _adjustments = adjustments;
        }

        public MyDeckConfig GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(AIDirectorDeckConstructor), "ConstructMonsterDeck"),
                prefix: new HarmonyMethod(
                    typeof(MyMonsterDeckOverriddenRule),
                    nameof(AIDirectorDeckConstructor_ConstructMonsterDeck_Prefix)));
        }

        private static List<MonsterDeck.MonsterDeckEntry> CreateSubDeck(Dictionary<BoardPieceId, int> subdeck)
        {
            var mySubDeck = new List<MonsterDeck.MonsterDeckEntry>();
            foreach (var deckItemConfig in subdeck)
            {
                var deckItem = new MonsterDeck.MonsterDeckEntry
                {
                    BoardPieceId = deckItemConfig.Key,
                    enemyWeight = EnemyWeight.Light,
                    isRedrawEnabled = false,
                };
                if (deckItemConfig.Value == 0)
                {
                    deckItem.isRedrawEnabled = true;
                }

                mySubDeck.Add(deckItem);
                for (int i = 0; i < deckItemConfig.Value - 1; i++)
                {
                    mySubDeck.Add(deckItem);
                }
            }

            return mySubDeck;
        }

        private static bool AIDirectorDeckConstructor_ConstructMonsterDeck_Prefix(ref MonsterDeck __result, int floorIndex, IRnd rng)
        {
            if (!_isActivated)
            {
                return true;
            }

            List<MonsterDeck.MonsterDeckEntry> standardDeck;
            List<MonsterDeck.MonsterDeckEntry> spikeDeck;
            MonsterDeck.MonsterDeckEntry keyHolder;
            if (floorIndex == 1)
            {
                standardDeck = CreateSubDeck(_globalAdjustments.EntranceDeckFloor1);
                spikeDeck = CreateSubDeck(_globalAdjustments.ExitDeckFloor1);
                keyHolder = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = _globalAdjustments.KeyHolderFloor1, enemyWeight = EnemyWeight.Light, isRedrawEnabled = false };
            }
            else
            {
                standardDeck = CreateSubDeck(_globalAdjustments.EntranceDeckFloor2);
                spikeDeck = CreateSubDeck(_globalAdjustments.ExitDeckFloor2);
                keyHolder = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = _globalAdjustments.KeyHolderFloor2, enemyWeight = EnemyWeight.Light, isRedrawEnabled = false };
            }

            List<MonsterDeck.MonsterDeckEntry> bossDeck = CreateSubDeck(_globalAdjustments.BossDeck);

            MonsterDeck monsterDeck = new MonsterDeck
            {
                monsterDeckStandard = standardDeck,
                monsterDeckSpike = spikeDeck,
                monsterDeckBoss = bossDeck,
                Keyholder = keyHolder,
            };
            monsterDeck.ShuffleSubDecks(rng);
            __result = monsterDeck;
            return false; // We returned an user-adjusted config.
        }
    }
}
