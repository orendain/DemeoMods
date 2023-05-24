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

            if (config.HasPieceType(PieceType.Player) || config.HasPieceType(PieceType.Bot) || config.HasPieceType(PieceType.ExplodingLamp) || !config.HasPieceType(PieceType.Creature))
            {
                return;
            }

            int range = 0;
            if (HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                if (config.PowerIndex > 40)
                {
                    return;
                }

                int high = 1;
                if (config.AttackDamage < 5)
                {
                    high = 2;
                }
                else if (config.AttackDamage < 7)
                {
                    if (Random.Range(1, 101) < 21)
                    {
                        high = 3;
                    }
                }

                range = Random.Range(0, high);
            }

            int newAttackDamage = (int)(config.AttackDamage * _globalMultiplier) + range;
            __result.effectSink.TrySetStatBaseValue(Stats.Type.AttackDamage, newAttackDamage);
        }
    }
}
