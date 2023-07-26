namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class RevolutionsRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Revolutions style game is enabled";

        private static bool _isActivated;

        public RevolutionsRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                postfix: new HarmonyMethod(
                    typeof(RevolutionsRule),
                    nameof(CreatePiece_Revolutions_Postfix)));
        }

        private static void CreatePiece_Revolutions_Postfix(ref Piece __result)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!__result.IsPlayer())
            {
                var ruleSet = HR.SelectedRuleset.Name;
                var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
                if (__result.boardPieceId == BoardPieceId.FireElemental || __result.boardPieceId == BoardPieceId.ServantOfAlfaragh)
                {
                    __result.effectSink.AddStatusEffect(EffectStateType.FireImmunity, 99);
                }
                else if (__result.boardPieceId == BoardPieceId.Tornado || __result.boardPieceId == BoardPieceId.GasLamp)
                {
                    __result.effectSink.AddStatusEffect(EffectStateType.Overcharge, 99);
                }
                else if (__result.boardPieceId.ToString().Contains("SummoningRift"))
                {
                    __result.effectSink.AddStatusEffect(EffectStateType.Corruption, 99);
                }
                else if (ruleSet.Contains("(PROGRESSIVE") || ruleSet.Equals("TEST GAME") || ruleSet.Contains("(LEGENDARY"))
                {
                    if (gameContext.levelManager.GetLevelSequence().CurrentLevelIndex == 3)
                    {
                        if (__result.boardPieceId == BoardPieceId.ReptileMutantWizard || __result.boardPieceId == BoardPieceId.TheUnseen)
                        {
                            __result.effectSink.AddStatusEffect(EffectStateType.MagicShield, 99);
                        }
                        else if (__result.boardPieceId.ToString().Contains("Goblin") || __result.boardPieceId.ToString().Contains("Elven"))
                        {
                            __result.effectSink.AddStatusEffect(EffectStateType.Courageous, 99);
                        }
                    }
                    else if (gameContext.levelManager.GetLevelSequence().CurrentLevelIndex == 5)
                    {
                        if (__result.boardPieceId == BoardPieceId.ReptileMutantWizard || __result.boardPieceId == BoardPieceId.TheUnseen)
                        {
                            __result.effectSink.AddStatusEffect(EffectStateType.MagicShield, 99);
                        }
                        else if (__result.boardPieceId.ToString().Contains("The"))
                        {
                            __result.effectSink.AddStatusEffect(EffectStateType.Courageous, 99);
                        }
                        else if (__result.boardPieceId.ToString().Contains("Goblin") || (__result.boardPieceId != BoardPieceId.ElvenQueen && __result.boardPieceId.ToString().Contains("Elven")))
                        {
                            __result.effectSink.AddStatusEffect(EffectStateType.Heroic, 99);
                        }
                        else if (__result.boardPieceId.ToString().Contains("Druid"))
                        {
                            __result.effectSink.AddStatusEffect(EffectStateType.Recovery, 99);
                        }
                    }
                }

                return;
            }

            __result.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDamageExtraDamage, 69);
        }
    }
}
