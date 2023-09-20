namespace HouseRules.Core.Types
{
    public interface IPatchable : ISingular
    {
        /// <summary>
        /// Patches the game with the given patcher.
        /// </summary>
        /// <remarks>
        /// Rules should use a static flag (e.g., <c>_isActivated</c>) in the patched method to
        /// ensure it makes modifications only when it is activated.
        /// </remarks>
        // private static void Patch(HarmonyLib.Harmony harmony);
    }
}
