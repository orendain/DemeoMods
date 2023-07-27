namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class EnemyDoorOpeningEnabledRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "All enemies can open doors";

        private static bool _isActivated;

        public EnemyDoorOpeningEnabledRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                prefix: new HarmonyMethod(
                    typeof(EnemyDoorOpeningEnabledRule),
                    nameof(CreatePiece_CanOpenDoor_Prefix)));
        }

        private static void CreatePiece_CanOpenDoor_Prefix(PieceConfigData config)
        {
            if (!_isActivated)
            {
                return;
            }

            config.CanOpenDoor = true;
        }
    }
}
