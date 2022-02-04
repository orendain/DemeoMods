namespace Rules.Rule
{
    using Boardgame.AIDirector;
    using HarmonyLib;
    using MelonLoader.TinyJSON;

    public sealed class EnemyRespawnDisabledRule : RulesAPI.Rule, RulesAPI.IConfigWritable, RulesAPI.IPatchable
    {
        public override string Description => "Enemy respawns are disabled";

        private static bool _isActivated;

        public static EnemyRespawnDisabledRule FromConfigString(string configString)
        {
            return new EnemyRespawnDisabledRule();
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
                original: AccessTools.Method(typeof(AIDirectorController2), "DynamicSpawning"),
                prefix: new HarmonyMethod(
                    typeof(EnemyRespawnDisabledRule),
                    nameof(AIDirectorController2_DynamicSpawning_Prefix)));
        }

        private static bool AIDirectorController2_DynamicSpawning_Prefix()
        {
            if (!_isActivated)
            {
                return true;
            }

            return false;
        }
    }
}
