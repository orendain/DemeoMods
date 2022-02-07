namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using Boardgame;
    using Boardgame.BoardEntities;
    using HarmonyLib;

    public sealed class CardLimitModifiedRule : Rule, IConfigWritable<int>, IPatchable
    {
        public override string Description => "Card limit is modified";

        private static int _limit;
        private static bool _isActivated;

        public CardLimitModifiedRule(int limit)
        {
            _limit = limit;
        }

        public int GetConfigObject() => _limit;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.PropertyGetter(typeof(Inventory), "MaxNumberOfCards"),
                postfix: new HarmonyMethod(
                    typeof(CardLimitModifiedRule),
                    nameof(Inventory_MaxNumberOfCards_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(CardHandView), "InitializeCardHolders"),
                transpiler: new HarmonyMethod(typeof(CardLimitModifiedRule), nameof(CardHandView_InitializeCardHolders_Transpiler)));

            // TODO(orendain): Tweak if necessary. Adjusts position and rotation of cards.
            // harmony.Patch(
            //     original: AccessTools.Method(typeof(CardHandView), "CalculateCardSlots"),
            //     transpiler: new HarmonyMethod(typeof(CardLimitModifiedRule), nameof(CardHandView_CalculateCardSlots_Transpiler)));
        }

        private static IEnumerable<CodeInstruction> CardHandView_InitializeCardHolders_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_I4_S && instruction.operand.ToString().Contains("11"))
                {
                    yield return new CodeInstruction(OpCodes.Ldc_I4_S, _limit);
                    continue;
                }

                yield return instruction;
            }
        }

        private static IEnumerable<CodeInstruction> CardHandView_CalculateCardSlots_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_R4 && instruction.operand.ToString().Contains("11"))
                {
                    yield return new CodeInstruction(OpCodes.Ldc_R4, (float)_limit);
                    continue;
                }

                yield return instruction;
            }
        }

        private static void Inventory_MaxNumberOfCards_Prefix(ref int __result)
        {
            if (!_isActivated)
            {
                return;
            }

            __result = _limit;
        }
    }
}
