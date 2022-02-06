namespace RulesAPI.Essentials.Rules
{
    using Boardgame;
    using Boardgame.AIDirector;
    using HarmonyLib;

    public sealed class EnemyRespawnDisabledRule : Rule, IConfigWritable<bool>, IPatchable
    {
        public override string Description => "Enemy respawns are disabled";

        private static bool _isActivated;

        public EnemyRespawnDisabledRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

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
