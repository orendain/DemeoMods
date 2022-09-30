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
        private const int HangoutsSceneIndex = 43;

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
            return MotherbrainGlobalVars.IsRunningOnDesktop;
        }

        public static bool IsInHangouts()
        {
            return SceneManager.GetActiveScene().buildIndex == HangoutsSceneIndex;
        }
    }
}
