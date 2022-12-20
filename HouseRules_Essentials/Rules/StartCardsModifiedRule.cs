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
                                Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                piece.inventory.Items.Remove(value);
                                piece.AddGold(0);
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
                if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                {
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.ImplosionExplosionRain)
                        {
                            int howMany = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy);
                            hasPower = true;
                            if (value.IsReplenishing)
                            {
                                howMany -= 1;
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                                if (howMany < 1)
                                {
                                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                    piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                    piece.inventory.Items.Remove(value);
                                    piece.AddGold(0);
                                }
                            }
                            else
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                            }

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
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.LeapHeavy)
                        {
                            int howMany = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy);
                            hasPower = true;
                            if (value.IsReplenishing)
                            {
                                howMany -= 1;
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                                if (howMany < 1)
                                {
                                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                    piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                    piece.inventory.Items.Remove(value);
                                    piece.AddGold(0);
                                }
                            }
                            else
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                            }

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
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.ScrollElectricity)
                        {
                            int howMany = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy);
                            hasPower = true;
                            if (value.IsReplenishing)
                            {
                                howMany -= 1;
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                                if (howMany < 1)
                                {
                                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                    piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                    piece.inventory.Items.Remove(value);
                                    piece.AddGold(0);
                                }
                            }
                            else
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                            }

                            break;
                        }
                    }

                    if (!hasPower)
                    {
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                        piece.inventory.Items.Add(new Inventory.Item
                        {
                            abilityKey = AbilityKey.ScrollElectricity,
                            flags = (Inventory.ItemFlag)1,
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
                            int howMany = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy);
                            hasPower = true;
                            if (value.IsReplenishing)
                            {
                                howMany -= 1;
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                                if (howMany < 1)
                                {
                                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                    piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                    piece.inventory.Items.Remove(value);
                                    piece.AddGold(0);
                                }
                            }
                            else
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                            }

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
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.DeathBeam)
                        {
                            int howMany = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy);
                            hasPower = true;
                            if (value.IsReplenishing)
                            {
                                howMany -= 1;
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                                if (howMany < 1)
                                {
                                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                    piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                    piece.inventory.Items.Remove(value);
                                    piece.AddGold(0);
                                }
                            }
                            else
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                            }

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
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.FretsOfFire)
                        {
                            int howMany = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy);
                            hasPower = true;
                            if (value.IsReplenishing)
                            {
                                howMany -= 1;
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                                if (howMany < 1)
                                {
                                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                    piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                    piece.inventory.Items.Remove(value);
                                    piece.AddGold(0);
                                }
                            }
                            else
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                            }

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
                    for (int i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.WeakeningShout)
                        {
                            int howMany = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy);
                            hasPower = true;
                            if (value.IsReplenishing)
                            {
                                howMany -= 1;
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                                if (howMany < 1)
                                {
                                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                    piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                    piece.inventory.Items.Remove(value);
                                    piece.AddGold(0);
                                }
                            }
                            else
                            {
                                piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                            }

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
                            value.flags &= (Inventory.ItemFlag)(-3); // unsets isReplenishing (bit1 ) allowing card to be used again.
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
                if (card.ReplenishFrequency > 0)
                {
                    Traverse.Create(inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                    flags = (Inventory.ItemFlag)1;
                }

                if (HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
                {
                    // Only add Torch in Rat King adventure
                    if (MotherbrainGlobalVars.CurrentConfig != GameConfigType.Sewers && card.Card == AbilityKey.Torch)
                    {
                        continue;
                    }
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
