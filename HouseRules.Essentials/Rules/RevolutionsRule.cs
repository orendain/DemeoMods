namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class RevolutionsRule : Rule, IConfigWritable<int>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "A Reloaded/Revolutions style game is enabled";

        private static bool _isActivated;
        private static float _globalGameType;
        private static bool _isReconnect;
        private static bool _checkPlayers;
        private static int _numPlayers = 1;
        private readonly int _gameType;

        public RevolutionsRule(int gameType)
        {
            _gameType = gameType;
        }

        public int GetConfigObject() => _gameType;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalGameType = _gameType;
            _isActivated = true;
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
                    typeof(RevolutionsRule),
                    nameof(CreatePiece_Revolutions_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Inventory), "RestoreReplenishables"),
                prefix: new HarmonyMethod(
                    typeof(RevolutionsRule),
                    nameof(Inventory_RestoreReplenishables_Prefix)));
        }

        private static bool Inventory_RestoreReplenishables_Prefix(ref bool __result, Piece piece)
        {
            if (!_isActivated)
            {
                return true;
            }

            if (!piece.IsPlayer())
            {
                return true;
            }

            var ruleSet = HR.SelectedRuleset.Name;
            if (!ruleSet.Contains("Demeo Re") && !ruleSet.Contains("TEST GAME"))
            {
                return true;
            }

            bool rev_progr = false;
            bool reloaded = false;
            if (ruleSet.Contains("PROGRESSIVE") || ruleSet.Contains("TEST GAME"))
            {
                rev_progr = true;
            }
            else if (ruleSet.Equals("Demeo Reloaded"))
            {
                reloaded = true;
            }

            Inventory.Item value;
            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
            if (_checkPlayers)
            {
                _numPlayers = gameContext.pieceAndTurnController.GetNumberOfPlayerPieces();
                _checkPlayers = false;
            }

            // Handle Host reconnect makes returning players invulnerable when becoming master client again
            if (_isReconnect)
            {
                if (piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) != 42 && piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) != 69)
                {
                    if (reloaded)
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamageExtraDamage, 42);
                    }
                    else
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamageExtraDamage, 69);
                    }

                    _isReconnect = false;
                }
                else if (_numPlayers > 1)
                {
                    piece.effectSink.AddStatusEffect(EffectStateType.Invulnerable1);
                    piece.effectSink.SetStatusEffectDuration(EffectStateType.Invulnerable1, 1);
                    _numPlayers--;
                }
            }

            // Handle fixing character stats and cards for Revolutions/Reloaded game when the Host reconnects and becomes the Master Client again
            if (GameStateMachine.IsMasterClient && !piece.IsDead() && piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) != 42 && piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) != 69)
            {
                _isReconnect = true;
                _checkPlayers = true;
                int mage = 0;
                int runner = 0;
                int diff = 0;
                if (ruleSet.Contains("(EASY"))
                {
                    diff = 2;
                }
                else if (ruleSet.Equals("Demeo Revolutions"))
                {
                    diff = 1;
                }

                if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    mage = 1;
                    if (reloaded)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.SummonElemental,
                            flags = 0,
                            originalOwner = -1,
                            replenishCooldown = 0,
                        });
                    }
                    else
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Vortex,
                            flags = 0,
                            originalOwner = -1,
                            replenishCooldown = 0,
                        });

                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Banish,
                            flags = 0,
                            originalOwner = -1,
                            replenishCooldown = 0,
                        });
                    }

                    if (!rev_progr)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Electricity,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });

                        if (piece.inventory.HasAbility(AbilityKey.Overcharge))
                        {
                            for (var i = 0; i < piece.inventory.Items.Count; i++)
                            {
                                value = piece.inventory.Items[i];
                                if (value.abilityKey == AbilityKey.Overcharge)
                                {
                                    piece.inventory.Items.Remove(value);
                                    break;
                                }
                            }
                        }
                    }

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 3);
                    if (reloaded)
                    {
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 11);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 11);
                    }
                    else
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, 1);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 6 + diff);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 6 + diff);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    if (piece.inventory.HasAbility(AbilityKey.MinionCharge))
                    {
                        for (var i = 0; i < piece.inventory.Items.Count; i++)
                        {
                            value = piece.inventory.Items[i];
                            if (value.abilityKey == AbilityKey.MinionCharge)
                            {
                                piece.inventory.Items.Remove(value);
                                break;
                            }
                        }
                    }

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.Deflect,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    if (!reloaded)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Implode,
                            flags = 0,
                            originalOwner = -1,
                            replenishCooldown = 0,
                        });

                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.GuidingLight,
                            flags = 0,
                            originalOwner = -1,
                            replenishCooldown = 0,
                        });
                    }

                    if (!rev_progr)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.MinionCharge,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                    }

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 2);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 5);
                    if (reloaded)
                    {
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 11);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 11);
                    }
                    else
                    {
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 6 + diff);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 6 + diff);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                {
                    if (!reloaded)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Lure,
                            flags = 0,
                            originalOwner = -1,
                            replenishCooldown = 0,
                        });
                    }

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.CallCompanion,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    if (!rev_progr)
                    {
                        for (var i = 0; i < piece.inventory.Items.Count; i++)
                        {
                            value = piece.inventory.Items[i];
                            if (value.abilityKey == AbilityKey.Arrow)
                            {
                                piece.inventory.Items.Remove(value);
                                break;
                            }
                        }

                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.EnemyFireball,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                    }

                    runner = 1;
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MoveRange, 5);
                    if (reloaded)
                    {
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 14);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 14);
                    }
                    else
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 2);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 5);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 7 + diff);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 7 + diff);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBard)
                {
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.ShatteringVoice,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    if (!reloaded)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.PiercingVoice,
                            flags = 0,
                            originalOwner = -1,
                            replenishCooldown = 0,
                        });
                    }

                    if (!rev_progr)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.EnemyFlashbang,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                    }

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 2);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 5);
                    if (reloaded)
                    {
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 12);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 12);
                    }
                    else
                    {
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 7 + diff);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 7 + diff);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                {
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.CursedDagger,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    if (!reloaded)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.FlashBomb,
                            flags = 0,
                            originalOwner = -1,
                            replenishCooldown = 0,
                        });
                    }

                    if (!rev_progr)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.DiseasedBite,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                    }

                    runner = 1;
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MoveRange, 5);
                    if (reloaded)
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 9);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 13);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 13);
                    }
                    else
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 8);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 7 + diff);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 7 + diff);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                {
                    if (reloaded)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.GrapplingSmash,
                            flags = 0,
                            originalOwner = -1,
                            replenishCooldown = 0,
                        });
                    }
                    else
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.GrapplingTotem,
                            flags = 0,
                            originalOwner = -1,
                            replenishCooldown = 0,
                        });

                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.GrapplingPush,
                            flags = 0,
                            originalOwner = -1,
                            replenishCooldown = 0,
                        });
                    }

                    if (!rev_progr)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Net,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                    }

                    if (reloaded)
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 5);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 13);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 15);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 15);
                    }
                    else
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 9);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 7 + diff);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 7 + diff);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.Charge,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    if (!reloaded)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.WarCry,
                            flags = 0,
                            originalOwner = -1,
                            replenishCooldown = 0,
                        });
                    }

                    if (!rev_progr)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Grab,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                    }

                    if (reloaded)
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 4);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 9);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 16);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 16);
                    }
                    else
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 7);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 8 + diff);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 8 + diff);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamage, 1);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 255);
                    }
                }

                if (!reloaded)
                {
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, 5 + mage);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, 5);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, 5 + runner);
                }

                if (rev_progr)
                {
                    if (ruleSet.Contains("(LEGENDARY"))
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, 3);
                    }
                    else
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, 2);
                    }

                    piece.effectSink.TrySetStatMaxValue(Stats.Type.CritChance, 1);
                    piece.EnableEffectState(EffectStateType.Flying);
                    piece.effectSink.SetStatusEffectDuration(EffectStateType.Flying, 1);
                }

                piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamageExtraDamage, _globalGameType);
                piece.AddGold(0);
            }

            // Remove One-Time replenishables if used
            if (piece.boardPieceId == BoardPieceId.HeroHunter)
            {
                if (piece.inventory.HasAbility(AbilityKey.EnemyFrostball) || piece.inventory.HasAbility(AbilityKey.Bone))
                {
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.EnemyFrostball || value.abilityKey == AbilityKey.Bone)
                        {
                            if (value.IsReplenishing)
                            {
                                Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                piece.inventory.Items.Remove(value);
                                piece.AddGold(0);
                            }
                        }
                    }
                }
            }
            else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
            {
                if (piece.inventory.HasAbility(AbilityKey.WaterBottle))
                {
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.WaterBottle)
                        {
                            if (value.IsReplenishing)
                            {
                                Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                piece.inventory.Items.Remove(value);
                                piece.AddGold(0);
                            }

                            break;
                        }
                    }
                }
            }
            else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
            {
                if (piece.inventory.HasAbility(AbilityKey.SpellPowerPotion))
                {
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.SpellPowerPotion)
                        {
                            if (value.IsReplenishing)
                            {
                                Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                piece.inventory.Items.Remove(value);
                                piece.AddGold(0);
                            }

                            break;
                        }
                    }
                }
            }
            else if (piece.boardPieceId == BoardPieceId.HeroBard)
            {
                if (piece.inventory.HasAbility(AbilityKey.PanicPowder))
                {
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.PanicPowder)
                        {
                            if (value.IsReplenishing)
                            {
                                Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                piece.inventory.Items.Remove(value);
                                piece.AddGold(0);
                            }

                            break;
                        }
                    }
                }
            }
            else if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
            {
                if (piece.inventory.HasAbility(AbilityKey.SpawnRandomLamp))
                {
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.SpawnRandomLamp)
                        {
                            if (value.IsReplenishing)
                            {
                                Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                piece.inventory.Items.Remove(value);
                                piece.AddGold(0);
                            }

                            break;
                        }
                    }
                }
            }

            // Energy Potion cards added per class
            if (piece.HasEffectState(EffectStateType.ExtraEnergy))
            {
                bool hasPower = false;
                if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                {
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.ImplosionExplosionRain)
                        {
                            hasPower = true;
                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.ImplosionExplosionRain,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.LeapHeavy)
                        {
                            hasPower = true;
                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.LeapHeavy,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                {
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.PVPMissileSwarm)
                        {
                            hasPower = true;
                            break;
                        }

                        if (value.abilityKey == AbilityKey.Zap)
                        {
                            piece.inventory.Items.Remove(value);
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.PVPMissileSwarm,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBard)
                {
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.PVPBlink)
                        {
                            hasPower = true;
                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.PVPBlink,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.DeathBeam)
                        {
                            hasPower = true;
                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.DeathBeam,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                {
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.PVPFireball)
                        {
                            hasPower = true;
                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.PVPFireball,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.WeakeningShout)
                        {
                            hasPower = true;
                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.WeakeningShout,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
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
                var ruleSet = HR.SelectedRuleset.Name;
                if (ruleSet.Contains("Demeo Re") || ruleSet.Equals("TEST GAME"))
                {
                    if (__result.boardPieceId == BoardPieceId.FireElemental || __result.boardPieceId == BoardPieceId.ServantOfAlfaragh)
                    {
                        __result.effectSink.AddStatusEffect(EffectStateType.FireImmunity, 99);
                    }
                    else if (__result.boardPieceId == BoardPieceId.Tornado || __result.boardPieceId == BoardPieceId.GasLamp)
                    {
                        __result.effectSink.AddStatusEffect(EffectStateType.Overcharge, 99);
                    }
                    else if (__result.boardPieceId == BoardPieceId.IceElemental)
                    {
                        __result.effectSink.AddStatusEffect(EffectStateType.IceImmunity, 99);
                    }
                    else if (__result.boardPieceId.ToString().Contains("SummoningRift"))
                    {
                        __result.effectSink.AddStatusEffect(EffectStateType.Corruption, 99);
                    }
                    else if (ruleSet.Contains("PROGRESSIVE") || ruleSet.Contains("(LEGENDARY") || ruleSet.Equals("TEST GAME"))
                    {
                        var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
                        if (gameContext.levelLoaderAndInitializer.GetLevelSequence().CurrentLevelIndex == 3)
                        {
                            if (__result.boardPieceId == BoardPieceId.ReptileMutantWizard || __result.boardPieceId == BoardPieceId.TheUnseen)
                            {
                                __result.effectSink.AddStatusEffect(EffectStateType.MagicShield, 99);
                            }
                            else if (__result.boardPieceId.ToString().Contains("Goblin") || __result.boardPieceId.ToString().Contains("Elven"))
                            {
                                __result.effectSink.AddStatusEffect(EffectStateType.Courageous, 99);
                            }
                        }
                        else if (gameContext.levelLoaderAndInitializer.GetLevelSequence().CurrentLevelIndex == 5)
                        {
                            if (__result.boardPieceId == BoardPieceId.ReptileMutantWizard || __result.boardPieceId == BoardPieceId.TheUnseen)
                            {
                                __result.effectSink.AddStatusEffect(EffectStateType.MagicShield, 99);
                            }
                            else if (__result.boardPieceId.ToString().Contains("The"))
                            {
                                __result.effectSink.AddStatusEffect(EffectStateType.Courageous, 99);
                            }
                            else if (__result.boardPieceId.ToString().Contains("Goblin") || (__result.boardPieceId != BoardPieceId.ElvenQueen && __result.boardPieceId.ToString().Contains("Elven")))
                            {
                                __result.effectSink.AddStatusEffect(EffectStateType.Heroic, 99);
                            }
                            else if (__result.boardPieceId.ToString().Contains("Druid"))
                            {
                                __result.effectSink.AddStatusEffect(EffectStateType.Recovery, 99);
                            }
                        }
                    }
                }

                return;
            }

            __result.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamageExtraDamage, _globalGameType);
        }
    }
}
