namespace Rules.Rule
{
    using Boardgame;
    using HarmonyLib;
    using MelonLoader.TinyJSON;

    public sealed class EnemyDoorOpeningDisabledRule : RulesAPI.Rule, RulesAPI.IConfigWritable, RulesAPI.IPatchable
    {
        public override string Description => "Enemy door opening ability is disabled";

        private static bool _isActivated;

        public static EnemyDoorOpeningDisabledRule FromConfigString(string configString)
        {
            return new EnemyDoorOpeningDisabledRule();
        }

        public string ToConfigString()
        {
            return JSON.Dump(string.Empty, EncodeOptions.NoTypeHints);
        }

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
