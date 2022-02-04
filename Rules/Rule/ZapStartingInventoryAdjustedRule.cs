namespace Rules.Rule
{
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;

    public sealed class ZapStartingInventoryAdjustedRule : RulesAPI.Rule, RulesAPI.IPatchable
    {
        public override string Description => "Zap starting inventory is adjusted";

        private static bool _isActivated;

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Piece), "CreatePiece"),
                transpiler: new HarmonyMethod(typeof(ZapStartingInventoryAdjustedRule), nameof(Piece_CreatePiece_Transpiler)));
        }

        private static IEnumerable<CodeInstruction> Piece_CreatePiece_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var subsequentInstructionsToNop = 0;
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_I4_S && instruction.operand.ToString().Contains("26"))
                {
                    subsequentInstructionsToNop = 3;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ZapStartingInventoryAdjustedRule), nameof(AddZapCardsToInventory), new[] { typeof(Piece) }));
                    yield return instruction;
                    continue;
                }

                if (subsequentInstructionsToNop > 0)
                {
                    --subsequentInstructionsToNop;
                    yield return new CodeInstruction(OpCodes.Nop);
                    continue;
                }

                yield return instruction;
            }
        }

        private static void AddZapCardsToInventory(Piece piece)
        {
            if (!_isActivated)
            {
                piece.TryAddAbilityToInventory(AbilityKey.Zap, isReplenishable: true);
                return;
            }

            piece.TryAddAbilityToInventory(AbilityKey.Zap, isReplenishable: true);
            piece.TryAddAbilityToInventory(AbilityKey.Zap, isReplenishable: true);
        }
    }
}
