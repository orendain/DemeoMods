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
        public override string Description => "Hit restores health.";

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

        private static void Ability_GenerateAttackDamage_Postfix(Piece source, Dice.Outcome diceResult)
        {
            if (!_isActivated)
            {
                return;
            }

            MelonLoader.MelonLogger.Msg("Free Heal/Hit called");
            if (diceResult == Dice.Outcome.Hit && source.IsPlayer())
            {
                int chance = Random.Range(1, 101);
                if (chance < 6)
                {
                    if (_globalAdjustments.Contains(source.boardPieceId))
                    {
                        source.effectSink.Heal(2);
                        source.AnimateWobble();
                    }
                    else
                    {
                        source.effectSink.Heal(1);
                        source.AnimateWobble();
                    }

                    HR.ScheduleBoardSync();
                }
                else if (_globalAdjustments.Contains(source.boardPieceId))
                {
                    source.effectSink.Heal(1);
                    HR.ScheduleBoardSync();
                }
            }

            return;
        }
    }
}
