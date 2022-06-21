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

    public sealed class FreeAbilityOnCritRule : Rule, IConfigWritable<Dictionary<BoardPieceId, AbilityKey>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Critical Hit gives free card.";

        private static Dictionary<BoardPieceId, AbilityKey> _globalAdjustments;
        private static bool _isActivated;

        private readonly Dictionary<BoardPieceId, AbilityKey> _adjustments;

        public FreeAbilityOnCritRule(Dictionary<BoardPieceId, AbilityKey> adjustments)
        {
            _adjustments = adjustments;
        }

        public Dictionary<BoardPieceId, AbilityKey> GetConfigObject() => _adjustments;

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
                    typeof(FreeAbilityOnCritRule),
                    nameof(Ability_GenerateAttackDamage_Postfix)));
        }

        private static void Ability_GenerateAttackDamage_Postfix(Piece source, Dice.Outcome diceResult)
        {
            if (!_isActivated)
            {
                return;
            }

            MelonLoader.MelonLogger.Msg("Free Ability called");
            if (diceResult == Dice.Outcome.Crit)
            {
                if (source.IsPlayer() && _globalAdjustments.ContainsKey(source.boardPieceId))
                {
                    source.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                    if (source.boardPieceId == BoardPieceId.HeroRogue)
                    {
                        if (currentAP < 1)
                        {
                            source.inventory.AddGold(10);
                            source.TryAddAbilityToInventory(_globalAdjustments[source.boardPieceId], showTooltip: true, isReplenishable: false);
                        }
                        else
                        {
                            int money = Random.Range(11, 21);
                            source.inventory.AddGold(money);
                        }
                    }
                    else
                    {
                        if (currentAP < 1)
                        {
                            source.inventory.AddGold(10);
                            source.TryAddAbilityToInventory(_globalAdjustments[source.boardPieceId], showTooltip: true, isReplenishable: false);
                        }
                        else
                        {
                            int money = Random.Range(11, 21);
                            source.inventory.AddGold(money);
                        }
                    }

                    HR.ScheduleBoardSync();
                }
            }

            return;
        }
    }
}
