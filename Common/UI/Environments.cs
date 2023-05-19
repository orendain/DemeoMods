namespace Common.UI
{
    using MelonLoader;
    using UnityEngine.SceneManagement;

    public enum Environment
    {
        Hangouts,
        NonVr,
        Vr,
    }

    public static class Environments
    {
        private const string DemeoPCEditionString = "Demeo PC Edition";
        private const int SteamHangoutsSceneIndex = 45;
        private const int OculusHangoutsSceneIndex = 43;

        public static Environment CurrentEnvironment()
        {
            if (IsPcEdition())
            {
                return Environment.NonVr;
            }

            if (IsInHangouts())
            {
                return Environment.Hangouts;
            }

            return Environment.Vr;
        }

        public static bool IsPcEdition()
        {
            return MotherbrainGlobalVars.IsRunningOnNonVRPlatform;
        }

        public static bool IsInHangouts()
        {
            return SceneManager.GetActiveScene().buildIndex == SteamHangoutsSceneIndex
                || SceneManager.GetActiveScene().buildIndex == OculusHangoutsSceneIndex;
        }
    }
}
