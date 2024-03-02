namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Threading;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.BoardEntities.AI;
    using Boardgame.SerializableEvents;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class HeroesPieceProgressRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Hero progression levels are enabled";

        private static bool _isActivated;
        private static bool _dropchest;
        private static bool _heroesLogDisplayOn;
        private static bool _heroesEasy;
        private static bool _heroesInsane;

        public HeroesPieceProgressRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext)
        {
            _isActivated = true;

            // Heroes *Easy* PROGRESSIVE
            if (HR.SelectedRuleset.Name.Contains("Heroes ") && HR.SelectedRuleset.Name.Contains("EASY PROGRESSIVE"))
            {
                _heroesEasy = true;
            }

            // Heroes *Insane* PROGRESSIVE
            if (HR.SelectedRuleset.Name.Contains("Heroes ") && HR.SelectedRuleset.Name.Contains("INSANE PROGRESSIVE"))
            {
                _heroesInsane = true;
            }
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(
                    typeof(HeroesPieceProgressRule),
                    nameof(CreatePiece_Progression_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "RespondToRequest"),
                prefix: new HarmonyMethod(
                    typeof(HeroesPieceProgressRule),
                    nameof(SerializableEventQueue_RespondToRequest_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Inventory), "RestoreReplenishables"),
                prefix: new HarmonyMethod(
                    typeof(HeroesPieceProgressRule),
                    nameof(Inventory_RestoreReplenishables_Prefix)));
        }

        private static void CreatePiece_Progression_Postfix(ref Piece __result)
        {
            if (!_isActivated || !__result.IsPlayer())
            {
                return;
            }

            // track the use of group boost.
            __result.effectSink.TrySetStatMaxValue(Stats.Type.InnateCounterDirections, 10);
            __result.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 0);

            if (_heroesEasy)
            {
                // start at level 3.
                __result.effectSink.TrySetStatMaxValue(Stats.Type.CritChance, 3);
                __result.EnableEffectState(EffectStateType.Flying);
                __result.effectSink.SetStatusEffectDuration(EffectStateType.Flying, 3);
            }
            else
            {
                __result.effectSink.TrySetStatMaxValue(Stats.Type.CritChance, 1);
                __result.EnableEffectState(EffectStateType.Flying);
                __result.effectSink.SetStatusEffectDuration(EffectStateType.Flying, 1);
            }
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

            // **************** Heroes Debug Logger *******************
            // toggle for displaying debug/test info in the log window.
            _heroesLogDisplayOn = true;

            // lets level up, and go higher than 10!
            // Inventory.Item value;
            int nextLevel = piece.GetStatMax(Stats.Type.CritChance);
            if (nextLevel < 100)
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

                piece.effectSink.SetStatusEffectDuration(EffectStateType.Flying, nextLevel); // uses flying as the level indicator.

                if (piece.GetHealth() < piece.GetMaxHealth())
                {
                    piece.DisableEffectState(EffectStateType.Heal);
                    piece.EnableEffectState(EffectStateType.Heal, 1);
                }

                // increase exp needed per level. It should take longer to level as they gain levels.
                // This gets called for each hero that levels up. So it actualy makes it more difficult when there are more heroes playing.
                float expChangePercent1 = 0.75f;
                float expChangePercent2 = 0.90f;
                float expChangePercent3 = 0.99f;
                if (_heroesEasy)
                {
                    expChangePercent1 = 0.85f;
                    expChangePercent2 = 0.95f;
                    expChangePercent3 = 1.0f;
                }
                else if (_heroesInsane)
                {
                    expChangePercent1 = 0.80f;
                    expChangePercent2 = 0.95f;
                    expChangePercent3 = 0.99f;
                }

                if (nextLevel < 3)
                {
                    var originalValue_DealDam = AIDirectorConfig.CardEnergy_EnergyToGetFromDealingDamage;
                    Traverse.Create(typeof(AIDirectorConfig)).Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = originalValue_DealDam * expChangePercent1;
                    if (_heroesLogDisplayOn)
                    {
                        HouseRulesEssentialsBase.LogDebug($"Exp from dealing damage changed from {originalValue_DealDam} to {originalValue_DealDam * expChangePercent1}");
                    }

                    var originalValue_IndirectKill = AIDirectorConfig.CardEnergy_EnergyToGetFromIndirectKill;
                    Traverse.Create(typeof(AIDirectorConfig)).Field<float>("CardEnergy_EnergyToGetFromIndirectKill").Value = originalValue_IndirectKill * expChangePercent1;
                    if (_heroesLogDisplayOn)
                    {
                        HouseRulesEssentialsBase.LogDebug($"CardEnergy_EnergyToGetindirect kill changed from {originalValue_IndirectKill} to {originalValue_IndirectKill * expChangePercent1}");
                    }
                }
                else if (nextLevel < 4)
                {
                    var originalValue_DealDam = AIDirectorConfig.CardEnergy_EnergyToGetFromDealingDamage;
                    Traverse.Create(typeof(AIDirectorConfig)).Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = originalValue_DealDam * expChangePercent2;
                    if (_heroesLogDisplayOn)
                    {
                        HouseRulesEssentialsBase.LogDebug($"Exp from dealing damage changed from {originalValue_DealDam} to {originalValue_DealDam * expChangePercent2}");
                    }

                    var originalValue_IndirectKill = AIDirectorConfig.CardEnergy_EnergyToGetFromIndirectKill;
                    Traverse.Create(typeof(AIDirectorConfig)).Field<float>("CardEnergy_EnergyToGetFromIndirectKill").Value = originalValue_IndirectKill * expChangePercent2;
                    if (_heroesLogDisplayOn)
                    {
                        HouseRulesEssentialsBase.LogDebug($"CardEnergy_EnergyToGetindirect kill changed from {originalValue_IndirectKill} to {originalValue_IndirectKill * expChangePercent2}");
                    }
                }
                else if (nextLevel < 6)
                {
                    var originalValue_DealDam = AIDirectorConfig.CardEnergy_EnergyToGetFromDealingDamage;
                    Traverse.Create(typeof(AIDirectorConfig)).Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = originalValue_DealDam * expChangePercent3;
                    if (_heroesLogDisplayOn)
                    {
                        HouseRulesEssentialsBase.LogDebug($"Exp from dealing damage changed from {originalValue_DealDam} to {originalValue_DealDam * expChangePercent3}");
                    }

                    var originalValue_IndirectKill = AIDirectorConfig.CardEnergy_EnergyToGetFromIndirectKill;
                    Traverse.Create(typeof(AIDirectorConfig)).Field<float>("CardEnergy_EnergyToGetFromIndirectKill").Value = originalValue_IndirectKill * expChangePercent3;
                    if (_heroesLogDisplayOn)
                    {
                        HouseRulesEssentialsBase.LogDebug($"CardEnergy_EnergyToGetindirect kill changed from {originalValue_IndirectKill} to {originalValue_IndirectKill * expChangePercent3}");
                    }
                }

                GameUI.ShowCameraMessage("<color=#F0F312>The party has</color> <color=#00FF00>LEVELED UP</color><color=#F0F312>!</color>", 8);

                // extra stats and refreshables become 0AP at level 2
                if (nextLevel == 2)
                {
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 1);
                    // piece.GetPieceConfig().StartHealth += 1; // test HP

                    /* Give Overcharge cards (for testing).
                    if (!piece.inventory.HasAbility(AbilityKey.Overcharge)) // check if the have it already.
                    {
                        // track the use of group boost. give the card and 1 counter.
                        piece.TryAddAbilityToInventory(AbilityKey.Overcharge);
                        piece.DisableEffectState(EffectStateType.Wet);
                        piece.AddGold(0);
                    }*/

                    if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.TauntingScream, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroBard)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.CourageShanty, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                    {
                        piece.TryAddAbilityToInventory(AbilityKey.DropChest);
                        piece.AddGold(0);

                        AbilityFactory.TryGetAbility(AbilityKey.BlindingLight, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.Sneak, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.Whip, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.SnakeBossLongRange, out var ability);
                        ability.costActionPoint = false;
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        AbilityFactory.TryGetAbility(AbilityKey.MagicMissile, out var ability);
                        ability.costActionPoint = false;
                    }
                }
                else if (nextLevel == 3) // new abilities at level 3.
                {
                    if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                    {
                        piece.TryAddAbilityToInventory(AbilityKey.SpawnRandomLamp);
                        piece.AddGold(0); // this only updates their inventory, but its still helpful.

                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.EnemyJavelin,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 2,
                        });
                        piece.AddGold(0);

                        piece.TryAddAbilityToInventory(AbilityKey.WeakeningShout);
                        piece.AddGold(0);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroBard)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.TeleportLamp,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 2,
                        });

                        piece.TryAddAbilityToInventory(AbilityKey.BlindingLight);
                        piece.TryAddAbilityToInventory(AbilityKey.PoisonBomb);
                        piece.AddGold(0);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Zap,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 2,
                        });

                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.TurretHealProjectile,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 3,
                        });
                        piece.AddGold(0);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.EnemyFireball,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });

                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.DiseasedBite,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });

                        piece.TryAddAbilityToInventory(AbilityKey.BoobyTrap);
                        piece.AddGold(0);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.TornadoCharge,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });

                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.WaterDive,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 3,
                        });

                        piece.TryAddAbilityToInventory(AbilityKey.EnemyFrostball);
                        piece.TryAddAbilityToInventory(AbilityKey.BeastWhisperer);
                        piece.AddGold(0);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.TurretHighDamageProjectile,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 3,
                        });

                        piece.TryAddAbilityToInventory(AbilityKey.Implode);
                        piece.AddGold(0);

                        // Your soul is forfeit.
                        piece.effectSink.AddStatusEffect(EffectStateType.Berserk, 2); // added damage and movement.
                        piece.effectSink.AddStatusEffect(EffectStateType.Corruption, 2); // just the look.
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.EnemyFireball,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 2,
                        });

                        piece.TryAddAbilityToInventory(AbilityKey.MinionCharge);
                        piece.AddGold(0);

                        AbilityFactory.TryGetAbility(AbilityKey.MinionCharge, out var ability);
                        ability.costActionPoint = false;
                    }
                }
                else if (nextLevel == 4) // extra stats and increased down counter at level 4.
                {
                    // Give Overcharge cards.
                    if (!piece.inventory.HasAbility(AbilityKey.Overcharge)) // check if the have it already.
                    {
                        // track the use of group boost. give the card and 1 counter.
                        piece.TryAddAbilityToInventory(AbilityKey.Overcharge);
                        piece.DisableEffectState(EffectStateType.Wet);
                        piece.AddGold(0);

                        if (_heroesLogDisplayOn)
                        {
                            HouseRulesEssentialsBase.LogDebug($"InnateCounterDirections = {piece.GetStat(Stats.Type.InnateCounterDirections)} for {piece.boardPieceId}");
                        }
                    }

                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 2);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 2);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, piece.GetStat(Stats.Type.DownedCounter) - 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedTimer, piece.GetStat(Stats.Type.DownedTimer) + 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Speed, piece.GetStat(Stats.Type.Speed) + 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, piece.GetStatMax(Stats.Type.Speed) + 1);
                }
                else if (nextLevel == 5) // boost to health, magic, and strength at 5th level.
                {
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 2);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 2);

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) + 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) + 1);

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) + 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) + 1);

                    if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                    {
                        piece.TryAddAbilityToInventory(AbilityKey.MarkOfVerga);
                        piece.AddGold(0);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroBard)
                    {
                        piece.TryAddAbilityToInventory(AbilityKey.MissileSwarm);
                        piece.AddGold(0);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                    {
                        piece.TryAddAbilityToInventory(AbilityKey.Rejuvenation);
                        piece.AddGold(0);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        piece.TryAddAbilityToInventory(AbilityKey.HeavensFury);
                        piece.AddGold(0);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                    {
                        piece.TryAddAbilityToInventory(AbilityKey.Teleportation);
                        piece.TryAddAbilityToInventory(AbilityKey.Regroup);
                        piece.AddGold(0);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                    {
                        piece.TryAddAbilityToInventory(AbilityKey.DeathFlurry);
                        piece.AddGold(0);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        piece.TryAddAbilityToInventory(AbilityKey.DeathBeam);
                        piece.AddGold(0);
                    }
                }
                else if (nextLevel == 6) // 3rd tier abilities and stats.
                {
                    if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.PlayerLeap,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 3,
                        });
                        piece.TryAddAbilityToInventory(AbilityKey.GrapplingSmash);
                        piece.TryAddAbilityToInventory(AbilityKey.GrapplingTotem);
                        piece.AddGold(0);

                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) + 1);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) + 1);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroBard)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.ScrollElectricity,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 4,
                        });
                        piece.TryAddAbilityToInventory(AbilityKey.FretsOfFire);
                        piece.AddGold(0);

                        piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) + 1);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) + 1);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                    {
                        piece.TryAddAbilityToInventory(AbilityKey.Charge);
                        piece.TryAddAbilityToInventory(AbilityKey.BottleOfLye);
                        piece.AddGold(0);

                        piece.effectSink.TrySetStatMaxValue(Stats.Type.TemporaryArmor, piece.GetMagicArmor() + 8);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.TemporaryArmor, piece.GetMagicArmor() + 8);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Implode,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 5,
                        });
                        piece.TryAddAbilityToInventory(AbilityKey.Blink);
                        piece.AddGold(0);

                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Speed, piece.GetStat(Stats.Type.Speed) + 1);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, piece.GetStatMax(Stats.Type.Speed) + 1);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Freeze,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 4,
                        });
                        piece.TryAddAbilityToInventory(AbilityKey.Shockwave);
                        piece.TryAddAbilityToInventory(AbilityKey.BeastWhisperer);
                        piece.AddGold(0);

                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 2);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 2);
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) + 1);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) + 1);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Electricity, // Red Mother Cy
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 3,
                        });
                        piece.TryAddAbilityToInventory(AbilityKey.ImplosionExplosionRain);
                        piece.AddGold(0);

                        piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) + 1);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) + 1);
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.HuntersMark,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 3,
                        });
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.EnemyFrostball,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 2,
                        });
                        piece.AddGold(0);

                        piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) + 1);
                        piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) + 1);
                    }
                }
                else if (nextLevel == 7) // increase downed counter and movement at level 7.
                {
                    // Give Overcharge cards.
                    if (!piece.inventory.HasAbility(AbilityKey.Overcharge)) // check if the have it already.
                    {
                        // track the use of group boost. give the card and 1 counter.
                        piece.TryAddAbilityToInventory(AbilityKey.Overcharge);
                        piece.DisableEffectState(EffectStateType.Wet);
                        piece.AddGold(0);

                        if (_heroesLogDisplayOn)
                        {
                            HouseRulesEssentialsBase.LogDebug($"InnateCounterDirections = {piece.GetStat(Stats.Type.InnateCounterDirections)} for {piece.boardPieceId}");
                        }
                    }

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, piece.GetStat(Stats.Type.DownedCounter) - 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedTimer, piece.GetStat(Stats.Type.DownedTimer) + 1);

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Speed, piece.GetStat(Stats.Type.Speed) + 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, piece.GetStatMax(Stats.Type.Speed) + 1);

                    // Let's try to increase their Cana's stats, but only for this warlock that leveled up.
                    // var gameContext = Traverse.Create(SerializableEventQueue.GetInstance()).Property<GameContext>("gameContext").Value;
                    PieceAI pieceAI = piece.pieceAI; // grab the current piece's AI, which can be Arly or Cana.
                    Piece[] listMinions = gameContext.pieceAndTurnController.GetPieceByType(PieceType.HasMinionPowder); // Get a list of all Cana's in the game.
                    int minionListLength = listMinions.GetLength(0);

                    for (int temp = 0; temp < minionListLength; temp++)
                    {
                        if (_heroesLogDisplayOn)
                        {
                            HouseRulesEssentialsBase.LogDebug($"Trying to match Warlock ID = {piece.networkID} and Minion ID = {listMinions[temp].networkID}, slot {temp} {listMinions[temp].boardPieceId}");
                            // HouseRulesEssentialsBase.LogDebug($"Minion PieceIDs = slot {temp}: {listMinions[temp].boardPieceId}");
                        }

                        // if the AI is there, and the piece's associated piece is the current iteration of the list of Canas:
                        if (pieceAI != null && pieceAI.memory.TryGetAssociatedPiece(gameContext.pieceAndTurnController, out listMinions[temp]))
                        {
                            // pieceOwner should be the warlock that leveled up.
                            if (listMinions[temp].boardPieceId == BoardPieceId.WarlockMinion && listMinions[temp].networkID == piece.networkID)
                            {
                                listMinions[temp].effectSink.TrySetStatMaxValue(Stats.Type.Health, listMinions[temp].GetMaxHealth() + 3);
                                listMinions[temp].effectSink.TrySetStatBaseValue(Stats.Type.Health, listMinions[temp].GetHealth() + 3);

                                listMinions[temp].effectSink.TrySetStatBaseValue(Stats.Type.Strength, listMinions[temp].GetStat(Stats.Type.Strength) + 2);
                                listMinions[temp].effectSink.TrySetStatMaxValue(Stats.Type.Strength, listMinions[temp].GetStatMax(Stats.Type.Strength) + 2);
                                listMinions[temp].effectSink.TrySetStatBaseValue(Stats.Type.Speed, listMinions[temp].GetStat(Stats.Type.Speed) + 1);
                                listMinions[temp].effectSink.TrySetStatMaxValue(Stats.Type.Speed, listMinions[temp].GetStatMax(Stats.Type.Speed) + 1);

                                HouseRulesEssentialsBase.LogDebug($"Match found! Leveled up Cana ({listMinions[temp].networkID}) for Warlock ({piece.networkID}). Setting counter to 0.");

                                // reset the counter variable.
                                piece.SetStatBaseValue(Stats.Type.InnateCounterDamage, 0);
                            }
                        }
                    }
                }
                else if (nextLevel == 8) // powerful ability at level 8.
                {
                    int randAbil = RandomProvider.GetThreadRandom().Next(7); // 0 to 7
                    if (randAbil == 6 && _dropchest) // allow only one drop chest
                    {
                        randAbil = RandomProvider.GetThreadRandom().Next(6);
                    }

                    if (randAbil == 0)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Petrify,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 5,
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
                            replenishCooldown = 5,
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
                            replenishCooldown = 5,
                        });
                    }
                    else if (randAbil == 3)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.DeathBeam,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 5,
                        });
                    }
                    else if (randAbil == 4)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.HeavensFury,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 5,
                        });
                    }
                    else if (randAbil == 5)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.CoinFlip,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 1, // zero Action Points. Misses too much, so refresh every turn.
                        });
                    }
                    else if (randAbil == 6)
                    {
                        _dropchest = true;
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.DropChest,
                            flags = (Inventory.ItemFlag)1,
                            originalOwner = -1,
                            replenishCooldown = 5,
                        });
                    }

                    piece.AddGold(0);
                }
                else if (nextLevel == 9) // boost to magic and strength at 9th level.
                {
                    // Give Overcharge cards.
                    if (!piece.inventory.HasAbility(AbilityKey.Overcharge)) // check if the have it already.
                    {
                        // track the use of group boost. give the card and 1 counter.
                        piece.TryAddAbilityToInventory(AbilityKey.Overcharge);
                        piece.DisableEffectState(EffectStateType.Wet);
                        piece.AddGold(0);

                        if (_heroesLogDisplayOn)
                        {
                            HouseRulesEssentialsBase.LogDebug($"InnateCounterDirections = {piece.GetStat(Stats.Type.InnateCounterDirections)} for {piece.boardPieceId}");
                        }
                    }

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) + 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) + 1);

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) + 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) + 1);
                }
                else if (nextLevel == 10) // increase their AP at levels 10 and higher.
                {
                    piece.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP + 1);
                }
                else if (nextLevel == 11 || nextLevel == 13)
                {
                    if (piece.HasEffectState(EffectStateType.Invulnerable1))
                    {
                        piece.DisableEffectState(EffectStateType.Invulnerable1);
                    }

                    piece.EnableEffectState(EffectStateType.Invulnerable1);
                }
                else if (nextLevel == 12 || nextLevel == 15 || nextLevel == 17)
                {
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 2);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 2);
                }
                else if (nextLevel == 13 || nextLevel == 16 || nextLevel == 18)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) + 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) + 1);

                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) + 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) + 1);
                }
                else if (nextLevel == 14 || nextLevel == 19)
                {
                    // Give Overcharge cards.
                    if (!piece.inventory.HasAbility(AbilityKey.Overcharge)) // check if the have it already.
                    {
                        // track the use of group boost. give the card and 1 counter.
                        piece.TryAddAbilityToInventory(AbilityKey.Overcharge);
                        piece.DisableEffectState(EffectStateType.Wet);
                        piece.AddGold(0);

                        if (_heroesLogDisplayOn)
                        {
                            HouseRulesEssentialsBase.LogDebug($"InnateCounterDirections = {piece.GetStat(Stats.Type.InnateCounterDirections)} for {piece.boardPieceId}");
                        }
                    }
                }
                else if (nextLevel > 19)
                {
                    if (piece.HasEffectState(EffectStateType.Invulnerable3))
                    {
                        piece.DisableEffectState(EffectStateType.Invulnerable3);
                    }

                    piece.EnableEffectState(EffectStateType.Invulnerable3);
                }
            }
        }

        private static bool Inventory_RestoreReplenishables_Prefix(ref bool __result, Piece piece)
        {
            if (!_isActivated)
            {
                return true;
            }

            // using CritChance to track levels.
            int level = piece.GetStatMax(Stats.Type.CritChance);

            // Warlock minion has increased stats at level 7 and higher.
            // tracking Cana increased stats based on the Warlock owner.
            if (piece.boardPieceId == BoardPieceId.WarlockMinion)
            {
                HouseRulesEssentialsBase.LogDebug("Cana lives...");
                // Let's try to increase their Cana's stats, but only for this warlock that leveled up.
                var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
                Piece piece2;
                PieceAI pieceAI = piece.pieceAI;
                if (pieceAI == null)
                {
                    HouseRulesEssentialsBase.LogDebug("No pieceAI found in gameContext!");
                    return true;
                }
                else if (pieceAI.memory.TryGetAssociatedPiece(gameContext.pieceAndTurnController, out piece2))
                {
                    HouseRulesEssentialsBase.LogDebug("pieceAI found.");
                    if (piece2.GetStatMax(Stats.Type.CritChance) > 6)
                    {
                        HouseRulesEssentialsBase.LogDebug("Warlock is level 7+ and Cana needs buff!");
                        if (piece.GetStat(Stats.Type.InnateCounterDamage) < 1)
                        {
                            HouseRulesEssentialsBase.LogDebug("CounterDamage not found...");
                            piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() + 3);
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.Health, piece.GetHealth() + 3);
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) + 2);
                            piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) + 2);
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.Speed, piece.GetStat(Stats.Type.Speed) + 1);
                            piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, piece.GetStatMax(Stats.Type.Speed) + 1);
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamage, 1);
                            HouseRulesEssentialsBase.LogDebug($"Match found! Leveled up Cana ({piece2.networkID}) for Warlock ({piece.networkID}).");
                        }
                        else
                        {
                            HouseRulesEssentialsBase.LogDebug($"CounterDamage is already set to {piece.GetStat(Stats.Type.InnateCounterDamage)}!");
                        }
                    }
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

                /* PieceAI pieceAI = piece.pieceAI; // grab the current piece's AI, which can be Arly or Cana.
                Piece[] listMinions = gameContext.pieceAndTurnController.GetPieceByType(PieceType.HasMinionPowder); // Get a list of all Cana's in the game.
                int minionListLength = listMinions.GetLength(0);

                for (int temp = 0; temp < minionListLength; temp++)
                {
                    if (_heroesLogDisplayOn)
                    {
                        HouseRulesEssentialsBase.LogDebug($"Trying to match Warlock ID = {piece.networkID} and Minion ID = {listMinions[temp].networkID}, slot {temp} {listMinions[temp].boardPieceId}");
                        // HouseRulesEssentialsBase.LogDebug($"Minion PieceIDs = slot {temp}: {listMinions[temp].boardPieceId}");
                    }

                    // if the AI is there, and the piece's associated piece is the current iteration of the list of Canas:
                    if (pieceAI != null && pieceAI.memory.TryGetAssociatedPiece(gameContext.pieceAndTurnController, out listMinions[temp]))
                    {
                        // pieceOwner should be the warlock that leveled up.
                        if (listMinions[temp].boardPieceId == BoardPieceId.WarlockMinion && listMinions[temp].networkID == piece.networkID)
                        {
                            listMinions[temp].effectSink.TrySetStatMaxValue(Stats.Type.Health, listMinions[temp].GetMaxHealth() + 3);
                            listMinions[temp].effectSink.TrySetStatBaseValue(Stats.Type.Health, listMinions[temp].GetHealth() + 3);

                            listMinions[temp].effectSink.TrySetStatBaseValue(Stats.Type.Strength, listMinions[temp].GetStat(Stats.Type.Strength) + 2);
                            listMinions[temp].effectSink.TrySetStatMaxValue(Stats.Type.Strength, listMinions[temp].GetStatMax(Stats.Type.Strength) + 2);
                            listMinions[temp].effectSink.TrySetStatBaseValue(Stats.Type.Speed, listMinions[temp].GetStat(Stats.Type.Speed) + 1);
                            listMinions[temp].effectSink.TrySetStatMaxValue(Stats.Type.Speed, listMinions[temp].GetStatMax(Stats.Type.Speed) + 1);

                            HouseRulesEssentialsBase.LogDebug($"Match found! Leveled up Cana ({listMinions[temp].networkID}) for Warlock ({piece.networkID}). Setting counter to 0.");
                        }
                    }
                }*/
            }

            // at levels 10 and higher they get an extra AP each turn, but we must keep giving it to them.
            if (level > 9)
            {
                piece.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                piece.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP + 1);
            }

            // at levels 8 and higher they heal at the start of their turn.
            if (level > 8 && piece.GetHealth() > 0 && piece.GetHealth() < piece.GetMaxHealth())
            {
                if (!piece.HasEffectState(EffectStateType.Downed) && !piece.HasEffectState(EffectStateType.Diseased) && !piece.HasEffectState(EffectStateType.Petrified))
                {
                    piece.effectSink.Heal(1);
                    piece.DisableEffectState(EffectStateType.Heal);
                    piece.EnableEffectState(EffectStateType.Heal, 1);
                }
            }

            // sets ability action cost for lower levels.
            if (level < 2)
            {
                if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.TauntingScream, out var ability); // howl of the ancients
                    ability.costActionPoint = true;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBard)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.CourageShanty, out var ability);
                    ability.costActionPoint = true;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.BlindingLight, out var ability);
                    ability.costActionPoint = true;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.Sneak, out var ability);
                    ability.costActionPoint = true;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.SnakeBossLongRange, out var ability); // serpents blast
                    ability.costActionPoint = true;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.Whip, out var ability); // Root whip
                    ability.costActionPoint = true;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.MagicMissile, out var ability); // master's call
                    ability.costActionPoint = true;
                }
            }
            else
            {
                if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.TauntingScream, out var ability);
                    ability.costActionPoint = false;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBard)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.CourageShanty, out var ability);
                    ability.costActionPoint = false;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.BlindingLight, out var ability);
                    ability.costActionPoint = false;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.Sneak, out var ability);
                    ability.costActionPoint = false;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.SnakeBossLongRange, out var ability);
                    ability.costActionPoint = false;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.Whip, out var ability);
                    ability.costActionPoint = false;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.MagicMissile, out var ability);
                    ability.costActionPoint = false;
                }
            }

            return false;
        }
    }
}
