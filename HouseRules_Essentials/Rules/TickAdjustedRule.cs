namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class TickAdjustedRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe, IDisableOnReconnect
    {
        public override string Description => "Ignore tick for Energy Potion and maybe others if we add them here.";

        private static bool _isActivated;

        public TickAdjustedRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext)
        {
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(StatusEffect), "Tick"),
                prefix: new HarmonyMethod(
                    typeof(TickAdjustedRule),
                    nameof(StatsusEffect_Tick_Prefix)));
        }

        private static void StatsusEffect_Tick_Prefix(ref StatusEffect __instance)
        {
            if (!_isActivated)
            {
                return;
            }

            if (__instance.effectStateType == EffectStateType.ExtraEnergy)
            {
                var pieceId = Traverse.Create(__instance).Field<int>("sourcePieceId").Value;
                var pieceAndTurnController = Traverse.Create(__instance).Field<PieceAndTurnController>("pieceAndTurnController").Value;
                Piece piece = pieceAndTurnController.GetPiece(pieceId);
                if (piece == null)
                {
                    return;
                }

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
                        if (piece.HasEffectState(EffectStateType.Wet) && piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.Wet) < 3)
                        {
                            for (int i = 0; i < piece.inventory.Items.Count; i++)
                            {
                                value = piece.inventory.Items[i];
                                if (value.abilityKey == AbilityKey.Zap)
                                {
                                    hasChanged = true;
                                    howMany -= 1;
                                    if (howMany < 1)
                                    {
                                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                                        piece.DisableEffectState(EffectStateType.ExtraEnergy);
                                        piece.inventory.Items.Remove(value);
                                    }

                                    break;
                                }
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
                            if (value.abilityKey == AbilityKey.FretsOfFire)
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
    }
}
