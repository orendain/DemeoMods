namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core.Types;
    using Utils;

    // adds the deck to the base game deck instead of overwritting it.
    public sealed class HeroesMonsterDeckAdditionRule : Rule, IConfigWritable<HeroesMonsterDeckAdditionRule.MyDeckConfig>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Enemy spawn randomness is adjusted";

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

        // adds the deck to the base game deck instead of overwritting it.
        public HeroesMonsterDeckAdditionRule(MyDeckConfig adjustments)
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
                    typeof(HeroesMonsterDeckAdditionRule),
                    nameof(AIDirectorDeckConstructor_ConstructMonsterDeck_Postfix)));
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
                for (var i = 0; i < deckItemConfig.Value - 1; i++)
                {
                    mySubDeck.Add(deckItem);
                }
            }

            return mySubDeck;
        }

        private static void AIDirectorDeckConstructor_ConstructMonsterDeck_Postfix(ref MonsterDeck __result, int floorIndex, IRnd rng)
        {
            if (!_isActivated)
            {
                return;
            }

            List<MonsterDeck.MonsterDeckEntry> standardDeck;
            List<MonsterDeck.MonsterDeckEntry> spikeDeck;
            MonsterDeck.MonsterDeckEntry keyHolder;

            //public Dictionary<BoardPieceId, int> EntranceDeckFloor1;
            //public Dictionary<BoardPieceId, int> ExitDeckFloor1;
            //public Dictionary<BoardPieceId, int> EntranceDeckFloor2;
            //public Dictionary<BoardPieceId, int> ExitDeckFloor2;
            //public Dictionary<BoardPieceId, int> BossDeck;

            //public BoardPieceId KeyHolderFloor1;
            // bool skipKeyHolderFloor1 = true;

            //public BoardPieceId KeyHolderFloor2;
            // bool skipKeyHolderFloor2 = true;

            // keyHolder = new MonsterDeck.MonsterDeckEntry();

            if (floorIndex == 1)
            {
                standardDeck = CreateSubDeck(_globalAdjustments.EntranceDeckFloor1);
                spikeDeck = CreateSubDeck(_globalAdjustments.ExitDeckFloor1);
                keyHolder = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = _globalAdjustments.KeyHolderFloor1, enemyWeight = EnemyWeight.Medium, isRedrawEnabled = false };

                // check if a keyholder was specified.
                //if (_globalAdjustments.KeyHolderFloor1 == BoardPieceId.None)
                //{
                //    skipKeyHolderFloor1 = true;
                //}
                //else
                //{
                //    skipKeyHolderFloor1 = false;
                //    keyHolder = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = _globalAdjustments.KeyHolderFloor1, enemyWeight = EnemyWeight.Medium, isRedrawEnabled = false };
                //}
            }
            else
            {
                standardDeck = CreateSubDeck(_globalAdjustments.EntranceDeckFloor2);
                spikeDeck = CreateSubDeck(_globalAdjustments.ExitDeckFloor2);
                keyHolder = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = _globalAdjustments.KeyHolderFloor2, enemyWeight = EnemyWeight.Medium, isRedrawEnabled = false };

                // check if a keyholder was specified.
                //if (_globalAdjustments.KeyHolderFloor2 == BoardPieceId.None)
                //{
                //    skipKeyHolderFloor2 = true;
                //}
                //else
                //{
                //    skipKeyHolderFloor2 = false;
                //    keyHolder = new MonsterDeck.MonsterDeckEntry() { BoardPieceId = _globalAdjustments.KeyHolderFloor2, enemyWeight = EnemyWeight.Medium, isRedrawEnabled = false };
                //}
            }

            List<MonsterDeck.MonsterDeckEntry> bossDeck = CreateSubDeck(_globalAdjustments.BossDeck);



            MonsterDeck monsterDeck = new MonsterDeck
            {
                monsterDeckStandard = standardDeck,
                monsterDeckSpike = spikeDeck,
                monsterDeckBoss = bossDeck,
                Keyholder = keyHolder, // need to check if a keyholder was specified. If not then we don't want to add nothing, it may overwrite the existing keyholder with nothing?
            };


            // if a keyholder was specified, add to the deck.
            //if (skipKeyHolderFloor1 || skipKeyHolderFloor2)
            //{
            //    monsterDeck.Keyholder = keyHolder;
            //}

            // adds the deck to the base game deck instead of overwritting it.
            __result.AddDeck(monsterDeck);
            monsterDeck.ShuffleSubDecks(rng);

            // return false; // We returned an user-adjusted config.
        }
    }
}
