namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class EnemyHealthScaledRule : Rule, IConfigWritable<float>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Enemy health is adjusted";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.NewPieceModified;

        private static float _globalMultiplier;
        private static bool _isActivated;

        private readonly float _multiplier;

        public EnemyHealthScaledRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalMultiplier = _multiplier;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                prefix: new HarmonyMethod(
                    typeof(EnemyHealthScaledRule),
                    nameof(CreatePiece_StartHealth_Prefix)));
        }

        private static void CreatePiece_StartHealth_Prefix(PieceConfigData config)
        {
            if (!_isActivated)
            {
                return;
            }

            if (config.CriticalHitDamage > 0)
            {
                return;
            }

            if (config.HasPieceType(PieceType.Player) || config.HasPieceType(PieceType.Bot) || config.HasPieceType(PieceType.Prop) || config.HasPieceType(PieceType.Lure) || !config.HasPieceType(PieceType.Creature))
            {
                return;
            }

            config.CriticalHitDamage = config.StartHealth;
            config.StartHealth = (int)(config.StartHealth * _globalMultiplier);
        }
    }
}
