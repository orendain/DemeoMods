namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class EnemyDoorOpeningDisabledRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Enemies can't open doors";

        private static bool _isActivated;

        public EnemyDoorOpeningDisabledRule(bool value)
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
