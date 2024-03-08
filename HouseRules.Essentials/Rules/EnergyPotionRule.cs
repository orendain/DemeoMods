namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class EnergyPotionRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "The Energy Potion can be activated";

        private static bool _isActivated;

        public EnergyPotionRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext)
        {
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            _isActivated = false;
        }

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(StatusEffect), "Tick"),
                prefix: new HarmonyMethod(
                    typeof(EnergyPotionRule),
                    nameof(StatsusEffect_Tick_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Inventory), "RestoreReplenishables"),
                prefix: new HarmonyMethod(
                    typeof(EnergyPotionRule),
                    nameof(Inventory_RestoreReplenishables_Prefix)));
        }

        private static void StatsusEffect_Tick_Prefix(ref StatusEffect __instance)
        {
            if (!_isActivated)
            {
                return;
            }

            var pieceId = Traverse.Create(__instance).Field<int>("sourcePieceId").Value;
            var pieceAndTurnController = Traverse.Create(__instance).Field<PieceAndTurnController>("pieceAndTurnController").Value;
            Piece piece = pieceAndTurnController.GetPiece(pieceId);
            if (piece == null)
            {
                return;
            }

            if (!piece.IsPlayer())
            {
                return;
            }

            if (__instance.effectStateType == EffectStateType.ExtraEnergy)
            {
                Inventory.Item value;
                int howMany = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.ExtraEnergy);
                bool hasChanged = false;

                // Energy Potion tick/prevention and card removal per class
                if (piece.HasEffectState(EffectStateType.ExtraEnergy))
                {
                    if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                    {
                        for (int i = 0; i < piece.inventory.Items.Count; i++)
                        {
                            value = piece.inventory.Items[i];
                            if (value.abilityKey == AbilityKey.ImplosionExplosionRain)
                            {
                                if (value.IsReplenishing)
                                {
                                    hasChanged = true;
                                    howMany -= 1;
                                    if (howMany < 1)
                                    {
                                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                        piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                        piece.inventory.Items.Remove(value);
                                    }
                                }

                                break;
                            }
                        }
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                    {
                        for (int i = 0; i < piece.inventory.Items.Count; i++)
                        {
                            value = piece.inventory.Items[i];
                            if (value.abilityKey == AbilityKey.LeapHeavy)
                            {
                                if (value.IsReplenishing)
                                {
                                    hasChanged = true;
                                    howMany -= 1;
                                    if (howMany < 1)
                                    {
                                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                        piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                        piece.inventory.Items.Remove(value);
                                    }
                                }

                                break;
                            }
                        }
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                    {
                        for (int i = 0; i < piece.inventory.Items.Count; i++)
                        {
                            value = piece.inventory.Items[i];
                            if (value.abilityKey == AbilityKey.PVPMissileSwarm)
                            {
                                if (value.IsReplenishing)
                                {
                                    hasChanged = true;
                                    howMany -= 1;
                                    if (howMany < 1)
                                    {
                                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                        piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                        piece.inventory.Items.Remove(value);
                                    }
                                }

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
                                if (value.IsReplenishing)
                                {
                                    hasChanged = true;
                                    howMany -= 1;
                                    if (howMany < 1)
                                    {
                                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                        piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                        piece.inventory.Items.Remove(value);
                                    }
                                }

                                break;
                            }
                        }
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                    {
                        for (int i = 0; i < piece.inventory.Items.Count; i++)
                        {
                            value = piece.inventory.Items[i];
                            if (value.abilityKey == AbilityKey.DeathBeam)
                            {
                                if (value.IsReplenishing)
                                {
                                    hasChanged = true;
                                    howMany -= 1;
                                    if (howMany < 1)
                                    {
                                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                        piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                        piece.inventory.Items.Remove(value);
                                    }
                                }

                                break;
                            }
                        }
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        for (int i = 0; i < piece.inventory.Items.Count; i++)
                        {
                            value = piece.inventory.Items[i];
                            if (value.abilityKey == AbilityKey.PVPFireball)
                            {
                                if (value.IsReplenishing)
                                {
                                    hasChanged = true;
                                    howMany -= 1;
                                    if (howMany < 1)
                                    {
                                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                        piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                        piece.inventory.Items.Remove(value);
                                    }
                                }

                                break;
                            }
                        }
                    }
                    else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        for (int i = 0; i < piece.inventory.Items.Count; i++)
                        {
                            value = piece.inventory.Items[i];
                            if (value.abilityKey == AbilityKey.WeakeningShout)
                            {
                                if (value.IsReplenishing)
                                {
                                    hasChanged = true;
                                    howMany -= 1;
                                    if (howMany < 1)
                                    {
                                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                        piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                        piece.inventory.Items.Remove(value);
                                    }
                                }

                                break;
                            }
                        }
                    }

                    if (howMany > 0 && !hasChanged)
                    {
                        Traverse.Create(__instance).Field<int>("durationTurnsLeft").Value = howMany + 1;
                        piece.effectSink.SetStatusEffectDuration(EffectStateType.ExtraEnergy, howMany);
                        piece.effectSink.AddStatusEffect(EffectStateType.It, 1);
                    }
                }
            }
        }

        private static bool Inventory_RestoreReplenishables_Prefix(Piece piece)
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
    }
}
