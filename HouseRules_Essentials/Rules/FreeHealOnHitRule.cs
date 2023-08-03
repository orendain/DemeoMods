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

    public sealed class FreeHealOnHitRule : Rule, IConfigWritable<List<BoardPieceId>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Some Heroes can restore some health by attacking";

        private static List<BoardPieceId> _globalAdjustments;
        private static bool _isActivated;

        private readonly List<BoardPieceId> _adjustments;

        public FreeHealOnHitRule(List<BoardPieceId> adjustments)
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
                    typeof(FreeHealOnHitRule),
                    nameof(Ability_GenerateAttackDamage_Postfix)));
        }

        private static void Ability_GenerateAttackDamage_Postfix(Piece source, Piece mainTarget, Dice.Outcome diceResult)
        {
            if (!_isActivated)
            {
                return;
            }

            if (diceResult != Dice.Outcome.Hit)
            {
                return;
            }

            if (!source.IsPlayer())
            {
                return;
            }

            if (source.GetHealth() == source.GetMaxHealth())
            {
                return;
            }

            int addHeal = 0;
            if (source.GetStat(Stats.Type.InnateCounterDamageExtraDamage) == 69 || HR.SelectedRuleset.Name.Contains("Revolutions"))
            {
                int chance = Random.Range(1, 101);
                if (_globalAdjustments.Contains(source.boardPieceId))
                {
                    int chance2 = Random.Range(1, 101);
                    if (source.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        if (chance2 > 50)
                        {
                            addHeal++;
                        }
                    }
                    else if (source.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        if (mainTarget == null)
                        {
                            return;
                        }

                        if (mainTarget.HasEffectState(EffectStateType.ExposeEnergy))
                        {
                            addHeal++;
                        }
                    }

                    if (chance > 98)
                    {
                        addHeal++;
                    }
                }
                else if (chance > 98)
                {
                    addHeal++;
                }
            }
            else
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
