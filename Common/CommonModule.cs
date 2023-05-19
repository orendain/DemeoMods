namespace Common
{
    using Bowser.Legacy;
    using HarmonyLib;
    using MelonLoader;

    internal static class CommonModule
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("Common");

        internal static BowserButtonHandler HangoutsButtonHandler { get; set; }

        /// <summary>
        /// Initialize the module. This should be called during the dependant module's OnInitializeMelon().
        /// </summary>
        public static void Initialize(Harmony harmony)
        {
            Patcher.Patch(harmony);
        }
    }
}
