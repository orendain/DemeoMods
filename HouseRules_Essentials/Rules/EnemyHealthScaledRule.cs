namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
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
                postfix: new HarmonyMethod(
                    typeof(EnemyHealthScaledRule),
                    nameof(CreatePiece_StartHealth_Postfix)));
        }

        private static void CreatePiece_StartHealth_Postfix(ref Piece __result, PieceConfigData config)
        {
            if (!_isActivated)
            {
                return;
            }

            if (config.HasPieceType(PieceType.Player) || config.HasPieceType(PieceType.Bot) || config.HasPieceType(PieceType.Prop) || config.HasPieceType(PieceType.Lure) || !config.HasPieceType(PieceType.Creature))
            {
                return;
            }

            float range = 1f;
            var ruleSet = HR.SelectedRuleset.Name;
            if (ruleSet.Contains("(LEGENDARY"))
            {
                if (config.StartHealth < 5 || config.PowerIndex > 40)
                {
                    return;
                }

                range = Random.Range(1.2f, 1.4f);
            }
            else if (ruleSet.Contains("(HARD"))
            {
                if (config.StartHealth < 5 || config.PowerIndex > 40)
                {
                    return;
                }

                range = Random.Range(1f, 1.3f);
            }
            else if (ruleSet.Contains("(EASY"))
            {
                if (config.StartHealth < 5 || config.PowerIndex > 40)
                {
                    return;
                }

                range = Random.Range(0.75f, 1f);
            }
            else if (ruleSet.Contains("(PROGRESSIVE"))
            {
                if (config.PowerIndex > 40)
                {
                    return;
                }

                var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
                if (gameContext.levelManager.GetLevelSequence().CurrentLevelIndex == 1)
                {
                    range = Random.Range(1.0f, 1.2f);
                }
                else if (gameContext.levelManager.GetLevelSequence().CurrentLevelIndex == 2)
                {
                    range = Random.Range(1.2f, 1.4f);
                }
                else
                {
                    range = Random.Range(1.4f, 1.6f);
                }
            }
            else if (ruleSet.Contains("Demeo Revolutions"))
            {
                if (config.StartHealth < 5 || config.PowerIndex > 40)
                {
                    return;
                }

                range = Random.Range(0.85f, 1.2f);
            }

            int newStartHealth = (int)(config.StartHealth * _globalMultiplier * range);
            __result.effectSink.TrySetStatMaxValue(Stats.Type.Health, newStartHealth);
            __result.effectSink.TrySetStatBaseValue(Stats.Type.Health, newStartHealth);
        }
    }
}
