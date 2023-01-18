namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class ReviveRemovesEffectsRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Reviving removes all negative effects.";

        private static bool _isActivated;

        public ReviveRemovesEffectsRule(bool value)
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
                original: AccessTools.Method(typeof(MotherTracker), "TrackRevive"),
                postfix: new HarmonyMethod(
                    typeof(ReviveRemovesEffectsRule),
                    nameof(MotherTracker_TrackRevive_Postfix)));
        }

        private static void MotherTracker_TrackRevive_Postfix(Piece revivedPiece)
        {
            if (!_isActivated)
            {
                return;
            }

            revivedPiece.effectSink.RemoveStatusEffect(EffectStateType.Stunned);
            revivedPiece.effectSink.RemoveStatusEffect(EffectStateType.Frozen);
            revivedPiece.effectSink.RemoveStatusEffect(EffectStateType.Weaken1Turn);
            revivedPiece.effectSink.RemoveStatusEffect(EffectStateType.Weaken2Turns);
            revivedPiece.effectSink.RemoveStatusEffect(EffectStateType.Thorns);
            revivedPiece.AddGold(0);
        }
    }
}
