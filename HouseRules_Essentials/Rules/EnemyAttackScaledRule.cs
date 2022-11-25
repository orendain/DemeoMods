namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class EnemyAttackScaledRule : Rule, IConfigWritable<float>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Enemy attack damage is scaled";

        private static float _globalMultiplier;
        private static bool _isActivated;

        private readonly float _multiplier;

        public EnemyAttackScaledRule(float multiplier)
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
                    typeof(EnemyAttackScaledRule),
                    nameof(CreatePiece_AttackDamage_Prefix)));
        }

        private static void CreatePiece_AttackDamage_Prefix(PieceConfigData config)
        {
            if (!_isActivated)
            {
                return;
            }

            if (config.PieceName.Contains("HRA_"))
            {
                return;
            }

            if (config.HasPieceType(PieceType.Player) || config.HasPieceType(PieceType.Bot) || config.HasPieceType(PieceType.Interactable) || config.PieceName.Contains("Lamp"))
            {
                return;
            }

            if (config.AttackDamage < 3 || config.PowerIndex > 40)
            {
                return;
            }

            config.PieceName = "HRA_" + config.PieceName;
            int range = Random.Range(-1, 1);
            config.AttackDamage = (int)(config.AttackDamage * _globalMultiplier) + range;
        }
    }
}
