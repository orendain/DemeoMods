namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class StartCardsModifiedRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>>,
        IPatchable, IMultiplayerSafe, IDisableOnReconnect
    {
        public override string Description => "Hero start cards are modified";

        private static Dictionary<BoardPieceId, List<CardConfig>> _globalHeroStartCards;
        private static bool _isActivated;
        private static bool _isReconnect;
        private static int _numPlayers = 1;

        private readonly Dictionary<BoardPieceId, List<CardConfig>> _heroStartCards;

        public struct CardConfig
        {
            public AbilityKey Card;
            public int ReplenishFrequency;
        }

        public StartCardsModifiedRule(Dictionary<BoardPieceId, List<CardConfig>> heroStartCards)
        {
            _heroStartCards = heroStartCards;
        }

        public Dictionary<BoardPieceId, List<CardConfig>> GetConfigObject() => _heroStartCards;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalHeroStartCards = _heroStartCards;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(PieceSpawner), "CreatePieceInternal"),
                prefix: new HarmonyMethod(
                    typeof(StartCardsModifiedRule),
                    nameof(Piece_CreatePieceInternal_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Inventory), "RestoreReplenishables"),
                prefix: new HarmonyMethod(
                    typeof(StartCardsModifiedRule),
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
            bool rev_progr = false;
            if (HR.SelectedRuleset.Name.Contains("(PROGRESSIVE"))
            {
                rev_progr = true;
            }

            if (piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69 || HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
                _numPlayers = gameContext.pieceAndTurnController.GetNumberOfPlayerPieces();

                if (!piece.IsDead() && piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) != 69)
                {
                    _isReconnect = true;
                    int mage = 0;
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
                        /*if (piece.inventory.HasAbility(AbilityKey.Overcharge))
                        {
                            for (var i = 0; i < piece.inventory.Items.Count; i++)
                            {
                                value = piece.inventory.Items[i];
                                if (value.abilityKey == AbilityKey.Overcharge)
                                {
                                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                    piece.inventory.Items.Remove(value);
                                    break;
                                }
                            }
                        }*/

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
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, 5);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamageExtraDamage, 69);
                    if (rev_progr)
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, 2);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.CritChance, 1);
                        piece.EnableEffectState(EffectStateType.Flying);
                        piece.effectSink.SetStatusEffectDuration(EffectStateType.Flying, 1);
                    }

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

                // Handle Host reconnect makes returning players invulnerable when becoming master client again
                if (!piece.IsDead() && _numPlayers > 1 && _isReconnect && !gameContext.pieceAndTurnController.IsMyTurn())
                {
                    piece.effectSink.AddStatusEffect(EffectStateType.Invulnerable1);
                    piece.effectSink.SetStatusEffectDuration(EffectStateType.Invulnerable1, 1);
                    _numPlayers--;
                    if (_numPlayers == 1)
                    {
                        _isReconnect = false;
                    }
                }
            }

            // Progressive Extra Actions and Action Point cost changes per character class
            if (rev_progr)
            {
                int level = piece.GetStatMax(Stats.Type.CritChance);
                if (level > 7)
                {
                    piece.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP + 1);
                }

                if (level < 2)
                {
                    if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.Grapple, out var ability);
                        ability.costActionPoint = true;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroBard)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.CourageShanty, out var ability);
                        ability.costActionPoint = true;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.Sneak, out var ability);
                        ability.costActionPoint = true;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.Zap, out var ability);
                        AbilityFactory.TryGetAbility(AbilityKey.LightningBolt, out var ability2);
                        ability.costActionPoint = true;
                        ability2.costActionPoint = true;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.Arrow, out var ability);
                        ability.costActionPoint = true;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.MinionCharge, out var ability);
                        ability.costActionPoint = true;
                    }
                }
                else
                {
                    if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.Grapple, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroBard)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.CourageShanty, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.Sneak, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.Zap, out var ability);
                        AbilityFactory.TryGetAbility(AbilityKey.LightningBolt, out var ability2);
                        ability.costActionPoint = false;
                        ability2.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.Arrow, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.MinionCharge, out var ability);
                        ability.costActionPoint = false;
                    }
                }
            }

            __result = false;
            for (var i = 0; i < piece.inventory.Items.Count; i++)
            {
                value = piece.inventory.Items[i];

                if (!value.IsReplenishing)
                {
                    continue;
                }

                // Fix for adding new cards late in the game with longer than 1 cooldown
                AbilityKey levelTen = value.abilityKey;
                if (levelTen == AbilityKey.EnemyFlashbang || levelTen == AbilityKey.DiseasedBite || levelTen == AbilityKey.Net)
                {
                    if (value.replenishCooldown < 0)
                    {
                        value.replenishCooldown = 2;
                        piece.inventory.Items[i] = value;
                    }
                }
                else if (levelTen == AbilityKey.Petrify || levelTen == AbilityKey.AcidSpit || levelTen == AbilityKey.DropChest || levelTen == AbilityKey.Shockwave || levelTen == AbilityKey.DeathFlurry)
                {
                    if (value.replenishCooldown < 0)
                    {
                        value.replenishCooldown = 5;
                        piece.inventory.Items[i] = value;
                    }
                }

                // Bypass problem with replenishCooldown somehow being set to -1 by Demeo
                foreach (var card in _globalHeroStartCards[piece.boardPieceId])
                {
                    if (value.abilityKey == card.Card && card.ReplenishFrequency > 1 && value.replenishCooldown < 0)
                    {
                        value.replenishCooldown = card.ReplenishFrequency - 1;
                        piece.inventory.Items[i] = value;
                    }
                }

                bool skipReplenishing = false;
                if (!AbilityFactory.TryGetAbility(value.abilityKey, out Ability ability))
                {
                    throw new Exception("Failed to get ability prefab from ability key while attempting to replenish hand!");
                }

                int j = 0;
                int count = ability.effectsPreventingReplenished.Count;
                while (j < count)
                {
                    if (piece.HasEffectState(ability.effectsPreventingReplenished[j]))
                    {
                        skipReplenishing = true;
                        break;
                    }

                    j++;
                }

                if (!skipReplenishing)
                {
                    if (value.replenishCooldown > 0)
                    {
                        value.replenishCooldown -= 1;
                        piece.inventory.Items[i] = value;

                        // Force inventory sync to clients
                        piece.AddGold(0);
                    }
                    else
                    {
                        // If we reached our desired turn count we can unset isReplenishing and return true
                        value.flags &= (Inventory.ItemFlag)(-3); // unsets isReplenishing (bit1 ) allowing card to be used again.
                        piece.inventory.Items[i] = value;
                        __result = true;

                        // Force inventory sync to clients
                        piece.AddGold(0);
                    }
                }
            }

            return false;
        }

        private static void Piece_CreatePieceInternal_Prefix(PieceSpawnSettings spawnSettings)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!_globalHeroStartCards.ContainsKey(spawnSettings.boardPieceId))
            {
                return;
            }

            var inventory = CreateInventory(spawnSettings.boardPieceId);
            Traverse.Create(spawnSettings).Property<Inventory>("Inventory").Value = inventory;
            Traverse.Create(spawnSettings).Property<bool>("HasInventory").Value = true;
        }

        private static Inventory CreateInventory(BoardPieceId boardPieceId)
        {
            var inventory = new Inventory();

            foreach (var card in _globalHeroStartCards[boardPieceId])
            {
                // flag bits
                // 0 : isReplenishable
                // 1 : isReplenishing
                // 2 : abilityDisabledOnStatusEffect
                // 3 : disableCooldown
                Inventory.ItemFlag flags = 0;

                if (HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
                {
                    // Only add Torch in Rat King adventure
                    if (MotherbrainGlobalVars.CurrentConfig != GameConfigType.Sewers && card.Card == AbilityKey.Torch)
                    {
                        continue;
                    }
                }

                if (card.ReplenishFrequency > 0)
                {
                    Traverse.Create(inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                    flags = (Inventory.ItemFlag)1;
                }

                inventory.Items.Add(new Inventory.Item
                {
                    abilityKey = card.Card,
                    flags = flags,
                    originalOwner = -1,
                    replenishCooldown = card.ReplenishFrequency,
                });
            }

            return inventory;
        }
    }
}
