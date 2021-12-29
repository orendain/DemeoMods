namespace Common
{
    using Common.Patches;
    using Common.States;
    using MelonLoader;

    internal class CommonModule
    {
        public static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("Common");

        public static GameContextState GameContextState { get; } = GameContextState.NewInstance();

        // Ensure constructor is called exactly once.
        private static CommonModule _instance = new CommonModule();

        private CommonModule()
        {
            var harmony = new HarmonyLib.Harmony("com.orendain.demeomods.common");
            GameContextPatcher.Patch(harmony);
        }
    }
}
