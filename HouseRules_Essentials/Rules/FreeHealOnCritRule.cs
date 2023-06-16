namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class FreeHealOnCritRule : Rule, IConfigWritable<List<BoardPieceId>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Critical Hit restores health.";

        private static List<BoardPieceId> _globalAdjustments;
        private static bool _isActivated;

        private readonly List<BoardPieceId> _adjustments;

        public FreeHealOnCritRule(List<BoardPieceId> adjustments)
        {
            _adjustments = adjustments;
        }

        public List<BoardPieceId> GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Ability), "GenerateAttackDamage"),
                postfix: new HarmonyMethod(
                    typeof(FreeHealOnCritRule),
                    nameof(Ability_GenerateAttackDamage_Postfix)));
        }

        private static void Ability_GenerateAttackDamage_Postfix(Piece source, Piece mainTarget, Dice.Outcome diceResult)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!source.IsPlayer())
            {
                return;
            }

            if (diceResult != Dice.Outcome.Crit)
            {
                return;
            }

            int addHeal = 0;
            if (source.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69 || HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                int chance = Random.Range(1, 101);
                int chance2 = Random.Range(1, 101);
                var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
                if (gameContext.levelManager.GetLevelSequence().CurrentLevelIsLastLevel)
                {
                    addHeal++;
                }
                else if (source.GetStat(Stats.Type.BonusCorruptionDamage) > 3)
                {
                    source.inventory.AddGold(30);
                }
                else if (source.GetStat(Stats.Type.BonusCorruptionDamage) == 0)
                {
                    source.inventory.AddGold(10);
                }

                if (_globalAdjustments.Contains(source.boardPieceId))
                {
                    if (source.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        if (chance > 98 && chance2 > 50)
                        {
                            addHeal += 3;
                        }
                        else if (chance2 > 50)
                        {
                            addHeal += 2;
                        }
                        else
                        {
                            addHeal++;
                        }
                    }
                    else if (source.boardPieceId == BoardPieceId.HeroBard)
                    {
                        if (chance > 98)
                        {
                            addHeal += 2;
                        }
                        else
                        {
                            addHeal++;
                        }
                    }
                    else if (source.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        if (mainTarget != null && mainTarget.HasEffectState(EffectStateType.ExposeEnergy))
                        {
                            addHeal++;
                        }
                    }
                    else
                    {
                        if (chance > 98)
                        {
                            addHeal += 2;
                        }
                        else
                        {
                            addHeal++;
                        }
                    }
                }
                else
                {
                    if (chance > 98)
                    {
                        addHeal++;
                    }
                }
            }
            else if (_globalAdjustments.Contains(source.boardPieceId))
            {
                addHeal++;
            }

            if (addHeal > 0)
            {
                source.effectSink.Heal(addHeal);
                source.DisableEffectState(EffectStateType.Heal);
                source.EnableEffectState(EffectStateType.Heal, 1);
            }
        }
    }
}
