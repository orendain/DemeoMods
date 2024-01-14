namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class HeroesRule : Rule, IConfigWritable<int>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "A Heroes style game (featuring Revolutions) is enabled";

        private static bool _isActivated;
        private static float _globalGameType;
        private static bool _isReconnect;
        private static bool _checkPlayers;
        private static int _numPlayers = 1;
        private readonly int _gameType;

        public HeroesRule(int gameType)
        {
            _gameType = gameType;
        }

        public int GetConfigObject() => _gameType;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalGameType = _gameType;
            _isActivated = true;

            Traverse.Create(typeof(AIDirectorConfig)).Field<float>("CardEnergy_EnergyRequiredToGetNewCard").Value = 5.0f;
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            _isActivated = false;
            _isReconnect = false;
            _checkPlayers = false;
            _numPlayers = 1;
        }

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(
                    typeof(HeroesRule),
                    nameof(CreatePiece_Revolutions_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Inventory), "RestoreReplenishables"),
                prefix: new HarmonyMethod(
                    typeof(HeroesRule),
                    nameof(Inventory_RestoreReplenishables_Prefix)));
        }

        private static bool Inventory_RestoreReplenishables_Prefix(Piece piece)
        {
            if (!_isActivated)
            {
                return true;
            }

            if (piece == null || !piece.IsPlayer())
            {
                return true;
            }

            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;

            if (_checkPlayers)
            {
                _numPlayers = gameContext.pieceAndTurnController.GetNumberOfPlayerPieces();
                _checkPlayers = false;
            }

            // Handle Host reconnect makes returning players invulnerable when becoming master client again
            if (_isReconnect)
            {
                if (piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) != 55)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamageExtraDamage, 55);
                    _isReconnect = false;
                }
                else if (_numPlayers > 1)
                {
                    piece.effectSink.AddStatusEffect(EffectStateType.Invulnerable1);
                    piece.effectSink.SetStatusEffectDuration(EffectStateType.Invulnerable1, 1);
                    _numPlayers--;
                }
            }

            // Handle fixing character stats for and cards Heroes game when the Host reconnects and becomes the Master Client again
            if (GameStateMachine.IsMasterClient && !piece.IsDead() && piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) != 55)
            {
                _isReconnect = true;
                _checkPlayers = true;

                // reset the sorcerer.
                if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    // reset the sorcerer hand.
                    piece.inventory.Items.Clear();
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value = 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.EnemyArrow,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.SnakeBossLongRange,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.ScrollOfCharm,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.HealingPotion,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 4);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 5);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 5);
                }

                // reset the warlock.
                else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    piece.inventory.Items.Clear();
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value = 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.MagicMissile,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.EnemyArrow,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.HailOfArrows,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.Barrage,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.HealingPotion,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 2); // warlock starting damage.
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 4);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 5);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 5);
                }

                // reset the hunter.
                else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                {
                    piece.inventory.Items.Clear();
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value = 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.LetItRain,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 4,
                    });

                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.Whip,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.EarthShatter,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.HealingPotion,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 2); //reset hunter stats.
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 4);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 5);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 5);
                }

                // reset the bard.
                else if (piece.boardPieceId == BoardPieceId.HeroBard)
                {
                    piece.inventory.Items.Clear();
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value = 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.CourageShanty,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.DrainingKiss,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 2,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.Weaken,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.HealingPotion,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 2); //reset bard stats.
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 3);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 5);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 5);
                }

                // reset the assassin.
                else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                {
                    piece.inventory.Items.Clear();
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value = 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.Sneak,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.ExtraActionPotion,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 4,
                    });

                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.EnemyJavelin,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.WhirlwindAttack,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.HealingPotion,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 5); // reset assassin stats.
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 6);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 6);
                }

                // reset barbarian.
                else if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                {
                    piece.inventory.Items.Clear();
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value = 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.Grapple,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.ExplodingLampPlaceholder,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.TauntingScream,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 3,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.HealingPotion,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 5); // reset barbarian stats.
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 7);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 7);
                }

                // reset guardian.
                else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    piece.inventory.Items.Clear();
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value = 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.ReplenishArmor,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.BlindingLight,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 2,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.MagicShield,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.HealingPotion,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 4); // reset guardian stats.
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 6);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 6);
                }

                piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, 2);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.CritChance, 1);
                piece.EnableEffectState(EffectStateType.Flying);
                piece.effectSink.SetStatusEffectDuration(EffectStateType.Flying, 1); // flying is the level tracker.
                piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamageExtraDamage, _globalGameType);
                piece.AddGold(0);
            }

            return false;
        }

        private static void CreatePiece_Revolutions_Postfix(ref Piece __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!__result.IsPlayer())
            {
                if (__result.boardPieceId == BoardPieceId.FireElemental)
                {
                    __result.effectSink.AddStatusEffect(EffectStateType.FireImmunity, 99);
                }
                else if (__result.boardPieceId == BoardPieceId.IceElemental)
                {
                    __result.effectSink.AddStatusEffect(EffectStateType.IceImmunity, 99);
                }

                return;
            }
            else
            {
                __result.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamageExtraDamage, _globalGameType);
            }
        }
    }
}
