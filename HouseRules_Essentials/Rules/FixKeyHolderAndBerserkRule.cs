namespace HouseRules.Essentials.Rules
{
    using System;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.Data;
    using Boardgame.GameplayEffects;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class FixKeyHolderAndBerserkRule : Rule, IConfigWritable<bool>, IPatchable,
        IMultiplayerSafe
    {
        public override string Description => "Fix Keyholder and Berserk visual.";

        private static bool _isActivated;

        public FixKeyHolderAndBerserkRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Damage), "DealDamage", parameters: new[] { typeof(Target), typeof(Damage), typeof(IntPoint2D), typeof(Target), typeof(PieceAndTurnController), typeof(BoardModel), typeof(OverkillController), typeof(bool) }),
                prefix: new HarmonyMethod(
                    typeof(FixKeyHolderAndBerserkRule),
                    nameof(Damage_DealDamage_Prefix)));
        }

        private static bool Damage_DealDamage_Prefix(Target target, Damage damage, Target attacker, IntPoint2D targetTile, PieceAndTurnController pieceAndTurnController, ref int __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            if (target.piece.IsImmuneToDamage())
            {
                return true;
            }

            if (target.piece.GetStat(Stats.Type.BarkArmor, 0) > 0)
            {
                return true;
            }

            if (target.piece.IsPlayer())
            {
                if (target.piece.characterClass == CharacterClass.Hunter && damage.HasTag(DamageTag.Fire))
                {
                    target.piece.effectSink.SubtractHealth(0);
                    return false;
                }
            }

            if (target.piece.IsBot())
            {
                if (target.piece.boardPieceId == BoardPieceId.Verochka && damage.HasTag(DamageTag.Fire))
                {
                    target.piece.effectSink.SubtractHealth(0);
                    return false;
                }
            }

            int num = (target.piece != null && target.piece.gridPos.min != targetTile) ? damage.SplashDamage : damage.Amount;
            num += damage.StrengthBonusDamage + damage.MagicBonusDamage;

            if (attacker.HasEffectState(EffectStateType.Heroic) || attacker.HasEffectState(EffectStateType.Fearless))
            {
                num++;
            }

            if (target.HasEffectState(EffectStateType.MarkOfAvalon))
            {
                num += 3;
            }

            if (target.piece.HasPieceType(PieceType.Corruption) && num >= 0 && damage.EnableCorruptionNodeBonusDamage)
            {
                num += damage.ExtraCorruptionDamage;
            }

            if (attacker.HasEffectState(EffectStateType.Frenzy) && damage.HasTag(DamageTag.PhysicalMelee))
            {
                num++;
            }

            if (attacker.HasEffectState(EffectStateType.Weaken) && damage.EnableWeakenPenalty)
            {
                if (num < 3)
                {
                    num--;
                }
                else
                {
                    num = (int)((float)num * 0.5f);
                }

                if (num < 0)
                {
                    num = 0;
                }
            }

            if (target.HasEffectState(EffectStateType.Frozen) && num > 0 && damage.EnableFrozenBonusDamage)
            {
                num += 2;
            }

            if (damage.HasTag(DamageTag.Electricity) && target.piece.HasEffectState(EffectStateType.Wet))
            {
                num = Mathf.CeilToInt((float)num * 3f);
            }

            if (damage.HasTag(DamageTag.Fire) && target.piece.HasEffectState(EffectStateType.Wet))
            {
                num /= 2;
            }

            if (damage.IsStealthed)
            {
                num += damage.SneakBonusDamage;
            }

            if (damage.IsBackStab)
            {
                num += 2;
            }

            if (attacker.HasEffectState(EffectStateType.SpellPower) && damage.EnableSpellPowerDamage)
            {
                attacker.DisableEffectState(EffectStateType.SpellPower);
                num += 3;
            }

            if (target.HasEffectState(EffectStateType.CorruptedRage) && damage.HasTag(DamageTag.Fire))
            {
                num -= num * 2;
                num = Mathf.Max(num, -damage.MaxCorruptionHeal);
            }

            if (target.pieceConfig.BerserkBelowHealth > 0f)
            {
                if (target.piece.GetHealth() - num < target.piece.GetMaxHealth() / 2)
                {
                    target.piece.effectSink.AddStatusEffect(EffectStateType.Berserk, -1);
                    target.piece.EnableEffectState(EffectStateType.Berserk, -1);
                }
            }

            if (target.piece.HasEffectState(EffectStateType.Key))
            {
                if (target.piece.GetHealth() - num < 1)
                {
                    if (target.HasEffectState(EffectStateType.Key))
                    {
                        Piece piece4 = target.piece;
                        target.piece.DisableEffectState(EffectStateType.Key);
                        if (attacker.piece != null && attacker.piece.GetHealth() > 0 && !attacker.piece.IsBot() && !attacker.piece.IsConfused())
                        {
                            Piece piece5 = attacker.piece;
                            attacker.piece.effectSink.AddStatusEffect(EffectStateType.Key, -1);
                            attacker.piece.EnableEffectState(EffectStateType.Key, -1);
                        }
                        else
                        {
                            Piece piece6 = pieceAndTurnController.GetPiece(pieceAndTurnController.CurrentPieceId, PieceAndTurnController.SearchPiece.All);
                            if (piece6 != null && piece6.IsPlayer() && piece6.GetHealth() > 0)
                            {
                                Piece piece7 = piece6;
                                Transform transform;
                                if (piece6.GetGameObject() != null)
                                {
                                    transform = piece6.GetGameObject().transform;
                                }
                                else
                                {
                                    transform = target.piece.GetGameObject().transform;
                                }

                                MotherbrainAudio.KeyObtained(transform);
                                piece6.effectSink.AddStatusEffect(EffectStateType.Key - 1);
                                piece6.EnableEffectState(EffectStateType.Key, -1);
                            }
                            else
                            {
                                Piece piece8;
                                Piece piece9;
                                if (attacker.piece != null && attacker.piece.boardPieceId == BoardPieceId.WarlockMinion && attacker.piece.pieceAI.memory.TryGetAssociatedPiece(pieceAndTurnController, out piece8) && piece8.IsPlayer() && piece8.GetHealth() > 0)
                                {
                                    piece9 = piece8;
                                }
                                else
                                {
                                    piece9 = pieceAndTurnController.FindFirstPiece((Piece p) => p.IsPlayer() && p.GetHealth() > 0, PieceAndTurnController.SearchPiece.All);
                                }

                                if (piece9 == null)
                                {
                                    piece9 = pieceAndTurnController.FindFirstPiece((Piece p) => p.IsPlayer(), PieceAndTurnController.SearchPiece.All);
                                }

                                Piece piece10 = piece9;
                                piece9.effectSink.AddStatusEffect(EffectStateType.Key, -1);
                                piece9.EnableEffectState(EffectStateType.Key, -1);
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
