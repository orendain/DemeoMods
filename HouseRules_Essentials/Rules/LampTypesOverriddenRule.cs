namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.AIDirector;
    using Boardgame.Board;
    using Boardgame.BoardEntities;
    using Boardgame.Data;
    using Boardgame.LayerCake;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using Utils;

    public sealed class LampTypesOverriddenRule : Rule, IConfigWritable<List<BoardPieceId>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Lamp types are overridden.";

        private static List<BoardPieceId> _globalAdjustments;
        private static bool _isActivated;

        private readonly List<BoardPieceId> _adjustments;

        public LampTypesOverriddenRule(List<BoardPieceId> adjustments)
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
                original: AccessTools.Method(typeof(ZoneSpawner), "GetLampTypes"),
                prefix: new HarmonyMethod(
                    typeof(MonsterDeckOverriddenRule),
                    nameof(ZoneSpawner_GetLampTypes_Prefix)));
        }

        private static bool ZoneSpawner_GetLampTypes_Prefix(ref BoardPieceId[] __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            __result = _globalAdjustments.ToArray();

            return false; // We returned an user-adjusted config.
        }
    }
}
