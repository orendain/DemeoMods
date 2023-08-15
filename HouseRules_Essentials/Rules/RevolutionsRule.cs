namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class RevolutionsRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "A Demeo Revolutions style game is enabled";

        private static bool _isActivated;
        private static bool _isReconnect;
        private static bool _checkPlayers;
        private static int _keyResist;
        private static int _numPlayers = 1;

        public RevolutionsRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext)
        {
            _isActivated = false;
            _isReconnect = false;
            _checkPlayers = false;
            _keyResist = 0;
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
                if (piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) != 69)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamageExtraDamage, 69);
                    _isReconnect = false;
                }
                else if (_numPlayers > 1)
                {
                    piece.effectSink.AddStatusEffect(EffectStateType.Invulnerable1);
                    piece.effectSink.SetStatusEffectDuration(EffectStateType.Invulnerable1, 1);
                    _numPlayers--;
                }
            }

            bool rev_progr = false;
            if (HR.SelectedRuleset.Name.Contains("PROGRESSIVE") || HR.SelectedRuleset.Name.Contains("TEST GAME"))
            {
                rev_progr = true;
            }

            // Handle fixing character stats and cards when the Host reconnects and becomes the Master Client again
            if (!piece.IsDead() && piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) != 69)
            {
                _isReconnect = true;
                _checkPlayers = true;
                int mage = 0;
                int runner = 0;
                int diff = 0;
                if (HR.SelectedRuleset.Name.Contains("(EASY"))
                {
                    diff = 2;
                }
                else if (HR.SelectedRuleset.Name.Equals("Demeo Revolutions"))
                {
                    diff = 1;
                }

                if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    mage = 1;
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
                    }

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 3);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 6 + diff);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 6 + diff);
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
                        abilityKey = AbilityKey.Implode,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.Deflect,
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

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 3);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 6 + diff);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 6 + diff);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                {
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.Lure,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

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
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 2);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 5);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MoveRange, 5);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 7 + diff);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 7 + diff);
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

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.PiercingVoice,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

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

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MoveRange, 5);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, 2);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 5);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 7 + diff);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 7 + diff);
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

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.FlashBomb,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

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
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 7 + diff);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 7 + diff);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
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

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.CritDamage, 9);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 7 + diff);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 7 + diff);
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

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.WarCry,
                        flags = 0,
                        originalOwner = -1,
                        replenishCooldown = 0,
                    });

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

                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, 8 + diff);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, 8 + diff);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamage, 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 255);
                }

                piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, 5 + mage);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, 5);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, 5 + runner);

                if (rev_progr)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, 2);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.CritChance, 1);
                    piece.EnableEffectState(EffectStateType.Flying);
                    piece.effectSink.SetStatusEffectDuration(EffectStateType.Flying, 1);
                }

                piece.AddGold(0);
            }

            // Handle keyholder gets 1 damage resist and 1 counter-attack damage
            if (piece.HasEffectState(EffectStateType.Locked) && !piece.HasEffectState(EffectStateType.Key))
            {
                piece.effectSink.TrySetStatBaseValue(Stats.Type.DamageResist, _keyResist);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.DamageResist, 1);
                if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamage, 1);
                }
                else
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamage, 0);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 0);
                }

                piece.DisableEffectState(EffectStateType.Locked);
            }
            else if (piece.HasEffectState(EffectStateType.Key))
            {
                if (!piece.HasEffectState(EffectStateType.Locked))
                {
                    _keyResist = 0;
                    piece.effectSink.AddStatusEffect(EffectStateType.Locked, -1);
                    if (piece.GetStat(Stats.Type.DamageResist) > 0)
                    {
                        _keyResist = 1;
                    }
                }

                piece.effectSink.TrySetStatMaxValue(Stats.Type.DamageResist, 2);
                piece.effectSink.TrySetStatBaseValue(Stats.Type.DamageResist, 1 + _keyResist);
                if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamage, 2);
                }
                else
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamage, 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 255);
                }
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
                        if (value.abilityKey == AbilityKey.LightningBolt)
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
                            abilityKey = AbilityKey.LightningBolt,
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
                        if (value.abilityKey == AbilityKey.FretsOfFire)
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
                            abilityKey = AbilityKey.FretsOfFire,
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
                var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
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
                else if (ruleSet.Contains("PROGRESSIVE") || ruleSet.Equals("TEST GAME") || ruleSet.Contains("(LEGENDARY"))
                {
                    if (gameContext.levelManager.GetLevelSequence().CurrentLevelIndex == 3)
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
                    else if (gameContext.levelManager.GetLevelSequence().CurrentLevelIndex == 5)
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

                return;
            }

            __result.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamageExtraDamage, 69);
        }
    }
}
