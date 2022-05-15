namespace TurnOrderCustomizer
{
    using MelonLoader;

    internal class TurnOrderCustomizerMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("TurnOrderCustomizer");

        internal static PieceScorer PieceScorer { get; private set; }

        public override void OnApplicationStart()
        {
            PieceScorer = PieceScorer.FromMelonConfig();

            var harmony = new HarmonyLib.Harmony("com.orendain.demeomods.turnordercustomizer");
            TurnOrderInjector.Patch(harmony);
        }
    }
}
