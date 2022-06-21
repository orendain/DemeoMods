namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class FixStatusEffectsRule : Rule, IConfigWritable<List<BoardPieceId>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Fix status effects not updating to client.";

        private static List<BoardPieceId> _globalAdjustments;
        private static bool _isActivated;

        private readonly List<BoardPieceId> _adjustments;

        public FixStatusEffectsRule(List<BoardPieceId> adjustments)
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
                    typeof(FixStatusEffectsRule),
                    nameof(Ability_GenerateAttackDamage_Postfix)));
        }

        private static void Ability_GenerateAttackDamage_Postfix(Piece source, Piece mainTarget)
        {
            if (!_isActivated)
            {
                return;
            }

            MelonLoader.MelonLogger.Msg("Fix Effects called");
            if (!source.IsPlayer() && !mainTarget.IsPlayer())
            {
                return;
            }

            source.effectSink.SyncStatusEffectsToController();
            mainTarget.effectSink.SyncStatusEffectsToController();
            HR.ScheduleBoardSync();

            /*if (mainTarget.IsPlayer())
            {
                if (mainTarget.characterClass == CharacterClass.Guardian)
                {
                    mainTarget.effectSink.AddStatusEffect(EffectStateType.Weaken, 1);
                    mainTarget.effectSink.RemoveStatusEffect(EffectStateType.Weaken);
                    HR.ScheduleBoardSync();
                }
                else if (mainTarget.characterClass == CharacterClass.Sorcerer)
                {

                    mainTarget.effectSink.AddStatusEffect(EffectStateType.Frozen, 1);
                    mainTarget.effectSink.RemoveStatusEffect(EffectStateType.Frozen);
                    HR.ScheduleBoardSync();
                }
                else if (mainTarget.characterClass == CharacterClass.Hunter)
                {
                    mainTarget.effectSink.AddStatusEffect(EffectStateType.Petrified, 1);
                    mainTarget.effectSink.RemoveStatusEffect(EffectStateType.Petrified);
                    HR.ScheduleBoardSync();
                }
                else if (mainTarget.characterClass == CharacterClass.Bard)
                {
                    mainTarget.effectSink.AddStatusEffect(EffectStateType.Diseased, 1);
                    mainTarget.effectSink.RemoveStatusEffect(EffectStateType.Diseased);
                    HR.ScheduleBoardSync();
                }
                else if (mainTarget.characterClass == CharacterClass.Assassin)
                {
                    mainTarget.effectSink.AddStatusEffect(EffectStateType.Tangled, 1);
                    mainTarget.effectSink.RemoveStatusEffect(EffectStateType.Tangled);
                    HR.ScheduleBoardSync();
                }
            }
            else if (source.IsPlayer())
            {
                if (source.characterClass == CharacterClass.Guardian)
                {
                    source.effectSink.AddStatusEffect(EffectStateType.Weaken, 1);
                    source.effectSink.RemoveStatusEffect(EffectStateType.Weaken);
                    HR.ScheduleBoardSync();
                }
                else if (source.characterClass == CharacterClass.Sorcerer)
                {
                    source.effectSink.AddStatusEffect(EffectStateType.Frozen, 1);
                    source.effectSink.RemoveStatusEffect(EffectStateType.Frozen);
                    HR.ScheduleBoardSync();
                }
                else if (source.characterClass == CharacterClass.Hunter)
                {
                    source.effectSink.AddStatusEffect(EffectStateType.Petrified, 1);
                    source.effectSink.RemoveStatusEffect(EffectStateType.Petrified);
                    HR.ScheduleBoardSync();
                }
                else if (source.characterClass == CharacterClass.Bard)
                {
                    source.effectSink.AddStatusEffect(EffectStateType.Diseased, 1);
                    source.effectSink.RemoveStatusEffect(EffectStateType.Diseased);
                    HR.ScheduleBoardSync();
                }
                else if (source.characterClass == CharacterClass.Assassin)
                {
                    source.effectSink.AddStatusEffect(EffectStateType.Tangled, 1);
                    source.effectSink.RemoveStatusEffect(EffectStateType.Tangled);
                    if (source.effectSink.HasEffectState(EffectStateType.Stealthed))
                    {
                        int currentST = source.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.Stealthed);
                        source.effectSink.RemoveStatusEffect(EffectStateType.Stealthed);
                        source.effectSink.AddStatusEffect(EffectStateType.Stealthed, currentST);
                        source.EnableEffectState(EffectStateType.Stealthed);
                        source.effectSink.SetStatusEffectDuration(EffectStateType.Stealthed, currentST);
                    }

                    HR.ScheduleBoardSync();
                }
            }*/

            return;
        }
    }
}
