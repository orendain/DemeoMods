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

        private static void Ability_GenerateAttackDamage_Postfix(Piece source, Dice.Outcome diceResult)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!source.IsPlayer())
            {
                return;
            }

            if (source.HasEffectState(EffectStateType.PlayerPanic))
            {
                return;
            }

            if (diceResult != Dice.Outcome.Crit)
            {
                return;
            }

            int chance = Random.Range(1, 101);
            int chance2 = Random.Range(1, 101);

            if (_globalAdjustments.Contains(source.boardPieceId))
            {
                if (source.boardPieceId == BoardPieceId.HeroRogue)
                {
                    if (chance > 98 && chance2 > 50)
                    {
                        source.effectSink.Heal(3);
                        source.AnimateWobble();
                    }
                    else if (chance2 > 50)
                    {
                        source.effectSink.Heal(2);
                        source.AnimateWobble();
                    }
                    else
                    {
                        source.effectSink.Heal(1);
                        source.AnimateWobble();
                    }
                }
                else if (source.boardPieceId == BoardPieceId.HeroBard)
                {
                    if (chance > 98 && chance2 > 66)
                    {
                        source.effectSink.Heal(3);
                        source.AnimateWobble();
                    }
                    else if (chance2 > 66)
                    {
                        source.effectSink.Heal(2);
                        source.AnimateWobble();
                    }
                    else
                    {
                        source.effectSink.Heal(1);
                        source.AnimateWobble();
                    }
                }
                else
                {
                    if (chance > 98)
                    {
                        source.effectSink.Heal(2);
                        source.AnimateWobble();
                    }
                    else
                    {
                        source.effectSink.Heal(1);
                        source.AnimateWobble();
                    }
                }
            }
        }
    }
}
