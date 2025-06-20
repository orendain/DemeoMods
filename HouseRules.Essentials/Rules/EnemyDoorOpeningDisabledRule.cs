﻿namespace HouseRules.Essentials.Rules
{
    using Boardgame.BoardEntities;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class EnemyDoorOpeningDisabledRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Enemy door opening ability is disabled";

        private static bool _isActivated;

        public EnemyDoorOpeningDisabledRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(Context context) => _isActivated = true;

        protected override void OnDeactivate(Context context) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                prefix: new HarmonyMethod(
                    typeof(EnemyDoorOpeningDisabledRule),
                    nameof(CreatePiece_CanOpenDoor_Prefix)));
        }

        private static void CreatePiece_CanOpenDoor_Prefix(PieceConfigData config)
        {
            if (!_isActivated)
            {
                return;
            }

            config.CanOpenDoor = false;
        }
    }
}
