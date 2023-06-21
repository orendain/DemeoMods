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

            Inventory.Item value;
            if (piece.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69 || HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
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
            }

            // Handle Extra Actions and Action Point cost changes per character class when card hand is active
            if (HR.SelectedRuleset.Name.Contains("(PROGRESSIVE"))
            {
                int level = piece.GetStat(Stats.Type.BonusCorruptionDamage);
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
                if (value.abilityKey == AbilityKey.EnemyFlashbang || value.abilityKey == AbilityKey.DiseasedBite || value.abilityKey == AbilityKey.Net)
                {
                    if (value.replenishCooldown < 0)
                    {
                        value.replenishCooldown = 2;
                        piece.inventory.Items[i] = value;
                    }
                }
                else if (value.abilityKey == AbilityKey.Petrify)
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
