namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Threading;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class PieceProgressRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Hero progression levels are enabled";

        private static bool _isActivated;
        private static bool _dropchest;

        public PieceProgressRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(
                    typeof(PieceProgressRule),
                    nameof(CreatePiece_Progression_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "RespondToRequest"),
                prefix: new HarmonyMethod(
                    typeof(PieceProgressRule),
                    nameof(SerializableEventQueue_RespondToRequest_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Inventory), "RestoreReplenishables"),
                prefix: new HarmonyMethod(
                    typeof(PieceProgressRule),
                    nameof(Inventory_RestoreReplenishables_Prefix)));
        }

        private static void CreatePiece_Progression_Postfix(ref Piece __result)
        {
            if (!_isActivated || !__result.IsPlayer())
            {
                return;
            }

            __result.effectSink.TrySetStatMaxValue(Stats.Type.CritChance, 1);
            __result.EnableEffectState(EffectStateType.Flying);
            __result.effectSink.SetStatusEffectDuration(EffectStateType.Flying, 1);
        }

        public static class RandomProvider
        {
            private static int seed = Environment.TickCount;

            private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(
                () => new Random(Interlocked.Increment(ref seed)));

            public static Random GetThreadRandom()
            {
                return randomWrapper.Value;
            }
        }

        private static void SerializableEventQueue_RespondToRequest_Prefix(
            SerializableEventQueue __instance,
            ref SerializableEvent request)
        {
            if (!_isActivated)
            {
                return;
            }

            if (request.type != SerializableEvent.Type.AddCardToPiece)
            {
                return;
            }

            var addCardToPieceEvent = (SerializableEventAddCardToPiece)request;
            var gameContext = Traverse.Create(__instance).Property<GameContext>("gameContext").Value;
            var pieceId = Traverse.Create(addCardToPieceEvent).Field<int>("pieceId").Value;
            var cardSource = Traverse.Create(addCardToPieceEvent).Field<int>("cardSource").Value;

            if (cardSource != (int)MotherTracker.Context.Energy)
            {
                return;
            }

            if (!gameContext.pieceAndTurnController.TryGetPiece(pieceId, out var piece))
            {
                return;
            }

            if (!piece.IsPlayer())
            {
                return;
            }

            Inventory.Item value;
            int nextLevel = piece.GetStatMax(Stats.Type.CritChance);
            if (nextLevel < 10)
            {
                piece.effectSink.TrySetStatMaxValue(Stats.Type.CritChance, nextLevel + 1);
                nextLevel++;
                piece.effectSink.Heal(2);
                if (piece.HasEffectState(EffectStateType.Downed))
                {
                    piece.effectSink.RemoveStatusEffect(EffectStateType.Downed);
                    piece.effectSink.RemoveStatusEffect(EffectStateType.Stunned);
                    piece.effectSink.RemoveStatusEffect(EffectStateType.Frozen);
                }

                piece.DisableEffectState(EffectStateType.ExtraEnergy);
                piece.EnableEffectState(EffectStateType.ExtraEnergy, 1);
                piece.effectSink.SetStatusEffectDuration(EffectStateType.Flying, nextLevel);

                if (piece.GetHealth() < piece.GetMaxHealth())
                {
                    piece.DisableEffectState(EffectStateType.Heal);
                    piece.EnableEffectState(EffectStateType.Heal, 1);
                }

                GameUI.ShowCameraMessage("<color=#F0F312>The party has</color> <color=#00FF00>LEVELED UP</color><color=#F0F312>!</color>", 8);
                if (nextLevel == 3)
                {
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 1);
                }
                else if (nextLevel == 6)
                {
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 2);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 2);
                }
                else if (nextLevel == 4)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, piece.GetStat(Stats.Type.DownedCounter) - 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedTimer, piece.GetStat(Stats.Type.DownedTimer) + 1);
                }
                else if (nextLevel == 8)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, piece.GetStat(Stats.Type.DownedCounter) - 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedTimer, piece.GetStat(Stats.Type.DownedTimer) + 1);
                    int randAbil = RandomProvider.GetThreadRandom().Next(5);
                    if (randAbil == 4 && _dropchest)
                    {
                        randAbil = RandomProvider.GetThreadRandom().Next(4);
                    }

                    if (randAbil == 0)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Petrify,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 7,
                        });
                    }
                    else if (randAbil == 1)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.AcidSpit,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 7,
                        });
                    }
                    else if (randAbil == 2)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.DeathFlurry,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 7,
                        });
                    }
                    else if (randAbil == 3)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Shockwave,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 7,
                        });
                    }
                    else if (randAbil == 4)
                    {
                        _dropchest = true;
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.DropChest,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 7,
                        });
                    }

                    piece.AddGold(0);
                }
                else if (nextLevel == 2)
                {
                    if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Net,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 3,
                        });
                        piece.AddGold(0);

                        AbilityFactory.TryGetAbility(AbilityKey.Grapple, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroBard)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.EnemyFlashbang,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 3,
                        });
                        piece.AddGold(0);

                        AbilityFactory.TryGetAbility(AbilityKey.CourageShanty, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Grab,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.DiseasedBite,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 3,
                        });
                        piece.AddGold(0);

                        AbilityFactory.TryGetAbility(AbilityKey.Sneak, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroHunter)
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
                        piece.AddGold(0);

                        AbilityFactory.TryGetAbility(AbilityKey.Arrow, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                    {
                        int overcharge = 0;
                        if (piece.HasEffectState(EffectStateType.Overcharge))
                        {
                            overcharge = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.Overcharge);
                            piece.effectSink.RemoveStatusEffect(EffectStateType.Overcharge);
                        }

                        for (var i = 0; i < piece.inventory.Items.Count; i++)
                        {
                            value = piece.inventory.Items[i];
                            if (value.abilityKey == AbilityKey.Overcharge)
                            {
                                if (value.IsReplenishing)
                                {
                                    value.flags &= (Inventory.ItemFlag)(-3);
                                    piece.inventory.Items[i] = value;
                                }

                                piece.inventory.Items.Remove(value);
                                break;
                            }
                        }

                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Electricity,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });

                        if (overcharge > 0)
                        {
                            piece.effectSink.AddStatusEffect(EffectStateType.Overcharge);
                            piece.effectSink.SetStatusEffectDuration(EffectStateType.Overcharge, overcharge);
                        }

                        piece.AddGold(0);

                        AbilityFactory.TryGetAbility(AbilityKey.Zap, out var ability);
                        AbilityFactory.TryGetAbility(AbilityKey.LightningBolt, out var ability2);
                        ability.costActionPoint = false;
                        ability2.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.MinionCharge,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);

                        AbilityFactory.TryGetAbility(AbilityKey.MinionCharge, out var ability);
                        ability.costActionPoint = false;
                    }
                }
                else if (nextLevel == 5)
                {
                    if (piece.boardPieceId == BoardPieceId.HeroSorcerer || piece.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) + 1);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) + 1);
                    }
                    else
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) + 1);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) + 1);
                    }
                }
                else if (nextLevel == 10)
                {
                    if (piece.boardPieceId == BoardPieceId.HeroSorcerer || piece.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) + 2);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) + 2);
                    }
                    else
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) + 2);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) + 2);
                    }

                    piece.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP + 1);
                }
                else if (nextLevel == 7)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Speed, piece.GetStat(Stats.Type.Speed) + 2);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, piece.GetStatMax(Stats.Type.Speed) + 2);
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

            // Health Regeneration, Extra Actions, and Action Point cost changes per character class
            if (piece.HasEffectState(EffectStateType.ConfusedPermanentVisualOnly))
            {
                piece.DisableEffectState(EffectStateType.ConfusedPermanentVisualOnly);
                piece.DisableEffectState(EffectStateType.Corruption);
            }

            int level = piece.GetStatMax(Stats.Type.CritChance);
            if (level > 9)
            {
                piece.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                piece.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP + 1);
            }

            if (level > 8 && piece.GetHealth() > 0 && piece.GetHealth() < piece.GetMaxHealth())
            {
                if (!piece.HasEffectState(EffectStateType.Downed) && !piece.HasEffectState(EffectStateType.Diseased) && !piece.HasEffectState(EffectStateType.Petrified))
                {
                    piece.effectSink.Heal(1);
                    piece.DisableEffectState(EffectStateType.Heal);
                    piece.EnableEffectState(EffectStateType.Heal, 1);
                }
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
                    AbilityFactory.TryGetAbility(AbilityKey.LightningBolt, out var ability2);
                    ability.costActionPoint = true;
                    ability2.costActionPoint = false;
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
                    AbilityFactory.TryGetAbility(AbilityKey.LightningBolt, out var ability2);
                    ability.costActionPoint = false;
                    ability2.costActionPoint = false;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.MinionCharge, out var ability);
                    ability.costActionPoint = false;
                }
            }

            return false;
        }
    }
}
