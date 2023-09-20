namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class CardLimitModifiedRule : Rule, IConfigWritable<int>, IPatchable
    {
        public override string Description => "Card hand maximum size limit is adjusted";

        private static int _globalLimit;
        private static bool _isActivated;

        private readonly int _limit;

        public CardLimitModifiedRule(int limit)
        {
            _limit = limit;
        }

        public int GetConfigObject() => _limit;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalLimit = _limit;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.PropertyGetter(typeof(Inventory), "MaxNumberOfCards"),
                postfix: new HarmonyMethod(
                    typeof(CardLimitModifiedRule),
                    nameof(Inventory_MaxNumberOfCards_Prefix)));
        }

        private static void Inventory_MaxNumberOfCards_Prefix(ref Inventory __instance, ref int __result)
        {
            if (!_isActivated)
            {
                return;
            }

            var numReplenishable = Traverse.Create(__instance).Field<int>("numberOfReplenishableCards").Value;
            __result = _globalLimit + numReplenishable;
        }
    }
}
