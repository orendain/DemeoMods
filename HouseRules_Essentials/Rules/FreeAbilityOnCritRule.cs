namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class FreeAbilityOnCritRule : Rule, IConfigWritable<Dictionary<BoardPieceId, AbilityKey>>, IPatchable,
        IMultiplayerSafe
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

            if (!source.IsPlayer())
            {
                return;
            }

            if (diceResult != Dice.Outcome.Crit)
            {
                return;
            }

            if (!_globalAdjustments.ContainsKey(source.boardPieceId))
            {
                return;
            }

            if (source.boardPieceId == BoardPieceId.HeroBard)
            {
                if (source.HasEffectState(EffectStateType.Fearless))
                {
                    source.EnableEffectState(EffectStateType.Fearless);
                }
                else if (source.HasEffectState(EffectStateType.Heroic))
                {
                    source.DisableEffectState(EffectStateType.Heroic);
                    source.EnableEffectState(EffectStateType.Fearless);
                }
                else if (source.HasEffectState(EffectStateType.Courageous))
                {
                    source.DisableEffectState(EffectStateType.Courageous);
                    source.EnableEffectState(EffectStateType.Heroic);
                }
                else
                {
                    source.EnableEffectState(EffectStateType.Courageous);
                }
            }

            source.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
            if (source.boardPieceId == BoardPieceId.HeroSorcerer && source.effectSink.HasEffectState(EffectStateType.Overcharge) && currentAP > 0)
            {
                source.TryAddAbilityToInventory(_globalAdjustments[source.boardPieceId], showTooltip: true, isReplenishable: false);
            }

            if (currentAP > 0)
            {
                return;
            }

            if (source.boardPieceId == BoardPieceId.HeroWarlock)
            {
                source.EnableEffectState(EffectStateType.SpellPower);
            }
            else if (source.boardPieceId == BoardPieceId.HeroSorcerer)
            {
                source.DisableEffectState(EffectStateType.Wet);
                source.EnableEffectState(EffectStateType.Overcharge);
            }
            else if (source.boardPieceId == BoardPieceId.HeroHunter)
            {
                Inventory.Item value;
                bool hasPower = false;
                for (int i = 0; i < source.inventory.Items.Count; i++)
                {
                    value = source.inventory.Items[i];
                    if (value.abilityKey == AbilityKey.EnemyFrostball)
                    {
                        hasPower = true;
                        break;
                    }
                }

                if (hasPower)
                {
                    if (!source.HasEffectState(EffectStateType.FireImmunity))
                    {
                        source.EnableEffectState(EffectStateType.FireImmunity);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, 6);
                    }
                    else
                    {
                        source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.FireImmunity + 6));
                    }
                }
                else
                {
                    source.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.EnemyFrostball,
                        flags = 1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    source.AddGold(0);
                    if (!source.HasEffectState(EffectStateType.FireImmunity))
                    {
                        source.EnableEffectState(EffectStateType.FireImmunity);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, 6);
                    }
                    else
                    {
                        source.effectSink.SetStatusEffectDuration(EffectStateType.FireImmunity, source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.FireImmunity + 6));
                    }
                }
            }

            source.TryAddAbilityToInventory(_globalAdjustments[source.boardPieceId], showTooltip: true, isReplenishable: false);
        }
    }
}
