namespace Common
{
    using Bowser.Core;
    using MelonLoader;
    using UnityEngine.SceneManagement;

    internal static class CommonModule
    {
        private const string DemeoPCEditionString = "Demeo PC Edition";
        private const int HangoutsSceneIndex = 43;

        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("Common");

        internal static BowserButtonHandler HangoutsButtonHandler { get; set; }

        internal static bool IsInitialized { get; private set; }

        /// <summary>
        /// Initialize the module. This should be called during the dependant module's OnApplicationStart().
        /// </summary>
        public static void Initialize()
        {
            var harmony = new HarmonyLib.Harmony("com.orendain.demeomods.common");
            ModPatcher.Patch(harmony);
            IsInitialized = true;
        }

        public static bool IsPcEdition()
        {
            return MelonUtils.CurrentGameAttribute.Name == DemeoPCEditionString;
        }

        public static bool IsInHangouts()
        {
            return SceneManager.GetActiveScene().buildIndex == HangoutsSceneIndex;
        }
    }
}
