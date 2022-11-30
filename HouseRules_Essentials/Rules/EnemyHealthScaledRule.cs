namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.NonVR.Ui;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class EnemyHealthScaledRule : Rule, IConfigWritable<float>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Enemy health is scaled";

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

            float range;
            if (config.PieceName.Contains("HRH_"))
            {
                range = Random.Range(0.75f, 1.25f);
                config.StartHealth = (int)(config.CriticalHitDamage * _globalMultiplier * range);
                return;
            }

            if (config.HasPieceType(PieceType.Player) || config.HasPieceType(PieceType.Bot) || config.HasPieceType(PieceType.Prop) || config.HasPieceType(PieceType.Lure) || !config.HasPieceType(PieceType.Creature))
            {
                return;
            }

            if (config.StartHealth < 4 || config.PowerIndex > 40)
            {
                return;
            }

            config.PieceName = "HRH_" + config.PieceName;
            config.CriticalHitDamage = config.StartHealth;
            range = Random.Range(0.75f, 1.25f);
            config.StartHealth = (int)(config.StartHealth * _globalMultiplier * range);
        }
    }
}
