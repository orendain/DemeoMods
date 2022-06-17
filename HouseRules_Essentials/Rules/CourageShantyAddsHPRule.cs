namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.GameplayEffects;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CourageShantyAddsHPRule : Rule, IConfigWritable<int>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Courage Shanty also adds HP.";

        private static int _globalAdjustments;
        private static bool _isActivated;

        private readonly int _adjustments;

        public CourageShantyAddsHPRule(int adjustments)
        {
            _adjustments = adjustments;
        }

        public int GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(StrengthenCourage), "UpdateEffectsOnTarget"),
                postfix: new HarmonyMethod(
                    typeof(CourageShantyAddsHPRule),
                    nameof(StrengthenCourager_UpdateEffectsOnTarget_Postfix)));
        }

        private static void StrengthenCourager_UpdateEffectsOnTarget_Postfix(Target target)
        {
            if (_isActivated)
            {
                target.piece.effectSink.Heal(_globalAdjustments);
                HR.ScheduleBoardSync();
            }
        }
    }
}
