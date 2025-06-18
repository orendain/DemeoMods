namespace Common.UI
{
    using UnityEngine.SceneManagement;

    public static class Environments
    {
        private const int SteamVRHangoutsSceneIndex = 45;
        private const int RiftHangoutsSceneIndex = 43;

        public static bool IsInHangouts()
        {
            if (!MotherbrainGlobalVars.IsRunningOnVRPlatform)
            {
                return false;
            }

            return SceneManager.GetActiveScene().buildIndex == SteamVRHangoutsSceneIndex
                || SceneManager.GetActiveScene().buildIndex == RiftHangoutsSceneIndex;
        }
    }
}
