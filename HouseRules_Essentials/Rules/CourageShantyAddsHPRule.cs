namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.GameplayEffects;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CourageShantyAddsHpRule : Rule, IConfigWritable<int>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Courage Shanty also gives a heal";

        private static int _globalAdjustments;
        private static bool _isActivated;

        private readonly int _adjustments;

        public CourageShantyAddsHpRule(int adjustments)
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
                    typeof(CourageShantyAddsHpRule),
                    nameof(StrengthenCourage_UpdateEffectsOnTarget_Postfix)));
        }

        private static void StrengthenCourage_UpdateEffectsOnTarget_Postfix(Target target)
        {
            if (!_isActivated)
            {
                return;
            }

            target.piece.effectSink.Heal(_globalAdjustments);
            HR.ScheduleBoardSync();
        }
    }
}
