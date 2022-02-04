namespace RulesAPI
{
    public interface IPatchable
    {
        /// <summary>
        /// Patches the game with the given patcher.
        /// </summary>
        /// <remarks>
        /// Rules must check the property <c>IsActivated</c> in the patched method to ensure it makes
        /// modifications only when it is activated.
        /// </remarks>
        // private static void Patch(HarmonyLib.Harmony harmony);
    }
}
