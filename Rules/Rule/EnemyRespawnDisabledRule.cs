﻿namespace Rules.Rule
{
    using Boardgame.AIDirector;
    using HarmonyLib;

    public sealed class EnemyRespawnDisabledRule : RulesAPI.Rule
    {
        public override string Description => "Enemy respawns are disabled";

        private static bool _isActivated;

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        private static void OnPatch(Harmony harmony)
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