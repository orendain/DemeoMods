namespace RulesAPI.Essentials.Rules
{
    using Boardgame;
    using HarmonyLib;

    public sealed class EnemyDoorOpeningDisabledRule : Rule, IConfigWritable<bool>, IPatchable
    {
        public override string Description => "Enemy door opening ability is disabled";

        private static bool _isActivated;

        public EnemyDoorOpeningDisabledRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.PropertyGetter(typeof(PieceConfig), "CanOpenDoor"),
                prefix: new HarmonyMethod(
                    typeof(EnemyDoorOpeningDisabledRule),
                    nameof(PieceConfig_CanOpenDoor_Prefix)));
        }

        private static bool PieceConfig_CanOpenDoor_Prefix(ref bool __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            __result = false;
            return false;
        }
    }
}
