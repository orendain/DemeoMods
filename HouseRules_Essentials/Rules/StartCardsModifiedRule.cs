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

            // Remove Frost Arrow if used
            if (piece.boardPieceId == BoardPieceId.HeroHunter)
            {
                if (piece.inventory.HasAbility(AbilityKey.EnemyFrostball))
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.EnemyFrostball)
                        {
                            if (value.IsReplenishing)
                            {
                                piece.inventory.Items.Remove(value);
                                piece.AddGold(0);
                                if (piece.HasEffectState(EffectStateType.FireImmunity))
                                {
                                    int fireImm = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.FireImmunity);
                                    if (fireImm > 5)
                                    {
                                        piece.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, fireImm - 5);
                                    }
                                    else
                                    {
                                        piece.DisableEffectState(EffectStateType.FireImmunity);
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }

            // Energy Potion effects per class
            if (piece.HasEffectState(EffectStateType.ExtraEnergy))
            {
                bool hasPower = false;
                if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.LeapHeavy)
                        {
                            hasPower = true;
                            if (!value.IsReplenishing)
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy) + 1);
                            }

                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.LeapHeavy,
                            flags = 1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.SpawnRandomLamp)
                        {
                            hasPower = true;
                            if (!value.IsReplenishing)
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy) + 1);
                            }

                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.SpawnRandomLamp,
                            flags = 1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBard)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.PVPBlink)
                        {
                            hasPower = true;
                            if (!value.IsReplenishing)
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy) + 1);
                            }

                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.PVPBlink,
                            flags = 1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.SpellPowerPotion)
                        {
                            hasPower = true;
                            if (!value.IsReplenishing)
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy) + 1);
                            }

                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.SpellPowerPotion,
                            flags = 1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.FretsOfFire)
                        {
                            hasPower = true;
                            if (!value.IsReplenishing)
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy) + 1);
                            }

                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.FretsOfFire,
                            flags = 1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.Weaken)
                        {
                            hasPower = true;
                            if (!value.IsReplenishing)
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy) + 1);
                            }

                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.Weaken,
                            flags = 1,
                            originalOwner = -1,
                            replenishCooldown = 1,
                        });
                        piece.AddGold(0);
                    }
                }
            }
            else
            {
                if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.LeapHeavy)
                        {
                            piece.inventory.Items.Remove(value);
                            piece.AddGold(0);
                            break;
                        }
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.SpawnRandomLamp)
                        {
                            piece.inventory.Items.Remove(value);
                            piece.AddGold(0);
                            break;
                        }
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBard)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.PVPBlink)
                        {
                            piece.inventory.Items.Remove(value);
                            piece.AddGold(0);
                            break;
                        }
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.SpellPowerPotion)
                        {
                            piece.inventory.Items.Remove(value);
                            piece.AddGold(0);
                            break;
                        }
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.FretsOfFire)
                        {
                            piece.inventory.Items.Remove(value);
                            piece.AddGold(0);
                            break;
                        }
                    }
                }
                else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.Weaken)
                        {
                            piece.inventory.Items.Remove(value);
                            piece.AddGold(0);
                            break;
                        }
                    }
                }
            }

            __result = false;
            for (int i = 0; i < piece.inventory.Items.Count; i++)
            {
                value = piece.inventory.Items[i];

                if (value.IsReplenishing)
                {
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
                            value.flags &= -3; // unsets isReplenishing (bit1 ) allowing card to be used again.
                            piece.inventory.Items[i] = value;
                            __result = true;
                            // Force inventory sync to clients
                            piece.AddGold(0);
                        }
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
            Traverse.Create(spawnSettings).Property<bool>("SetInventory").Value = true;
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
                int flags = 0;
                if (card.ReplenishFrequency > 0)
                {
                    Traverse.Create(inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                    flags = 1;
                }

                // Only add Torch in Rat King adventure
                if (MotherbrainGlobalVars.CurrentConfig != GameConfigType.Sewers && card.Card == AbilityKey.Torch)
                {
                    continue;
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
