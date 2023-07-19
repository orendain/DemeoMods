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
        public override string Description => "Enemy attack damage is adjusted";

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
                postfix: new HarmonyMethod(
                    typeof(EnemyAttackScaledRule),
                    nameof(CreatePiece_AttackDamage_Postfix)));
        }

        private static void CreatePiece_AttackDamage_Postfix(ref Piece __result, PieceConfigData config)
        {
            if (!_isActivated)
            {
                return;
            }

            if (config.AttackDamage < 1 || config.PowerIndex > 40)
            {
                return;
            }

            if (config.HasPieceType(PieceType.Player) || config.HasPieceType(PieceType.Bot) || config.HasPieceType(PieceType.ExplodingLamp) || !config.HasPieceType(PieceType.Creature))
            {
                return;
            }

            int range = 0;
            if (HR.SelectedRuleset.Name.Contains("(PROGRESSIVE") || HR.SelectedRuleset.Name.Equals("TEST GAME"))
            {
                var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
                if (gameContext.levelManager.GetLevelSequence().CurrentLevelIndex == 1)
                {
                    range = Random.Range(0, 2);
                }
                else if (gameContext.levelManager.GetLevelSequence().CurrentLevelIndex == 3)
                {
                    range = Random.Range(1, 3);
                }
                else if (gameContext.levelManager.GetLevelSequence().CurrentLevelIsLastLevel)
                {
                    if (config.HasPieceType(PieceType.Boss))
                    {
                        range = 5;
                    }
                    else
                    {
                        range = Random.Range(2, 5);
                    }
                }
            }
            else if (HR.SelectedRuleset.Name.Contains("Revolutions"))
            {
                if (config.HasPieceType(PieceType.Boss))
                {
                    range = 3;
                }
                else if (config.AttackDamage < 5)
                {
                    if (Random.Range(1, 101) < 51)
                    {
                        range = Random.Range(0, 2);
                    }
                }
                else if (config.AttackDamage < 7)
                {
                    if (Random.Range(1, 101) < 26)
                    {
                        range = Random.Range(0, 3);
                    }
                }
            }

            int newAttackDamage = (int)(config.AttackDamage * _globalMultiplier) + range;
            __result.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, newAttackDamage);
        }
    }
}
