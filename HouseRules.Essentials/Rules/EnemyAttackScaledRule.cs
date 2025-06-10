namespace HouseRules.Essentials.Rules
{
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class EnemyAttackScaledRule : Rule, IConfigWritable<float>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Enemy attack damage is adjusted";

        private static float _globalMultiplier;
        private static bool _isActivated;

        private readonly float _multiplier;

        public EnemyAttackScaledRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnActivate(Context context)
        {
            _globalMultiplier = _multiplier;
            _isActivated = true;
        }

        protected override void OnDeactivate(Context context) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                prefix: new HarmonyMethod(
                    typeof(EnemyAttackScaledRule),
                    nameof(CreatePiece_AttackDamage_Prefix)));
        }

        private static void CreatePiece_AttackDamage_Prefix(PieceConfigData config)
        {
            if (!_isActivated)
            {
                return;
            }

            if (config.CriticalHitDamageOLD > 0)
            {
                return;
            }

            if (config.HasPieceType(PieceType.Player) || config.HasPieceType(PieceType.Bot) || config.HasPieceType(PieceType.ExplodingLamp) || !config.HasPieceType(PieceType.Creature))
            {
                return;
            }

            config.CriticalHitDamageOLD = config.AttackDamage;
            config.AttackDamage = (int)(config.AttackDamage * _globalMultiplier);
        }
    }
}
