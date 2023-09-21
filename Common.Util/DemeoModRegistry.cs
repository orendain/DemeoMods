namespace Common.Util
{
    using System.Collections.Generic;
    using Boardgame.Modding;
    using HarmonyLib;

    public static class DemeoModRegistry
    {
        /// <summary>
        /// Registers a mod so that it appears in the in-game mod list.
        /// </summary>
        /// <param name="name">The name of the mod.</param>
        /// <param name="version">The version of the mod.</param>
        /// <param name="author">The author of the mod.</param>
        /// <param name="description">The description of the mod.</param>
        /// <param name="isNetworkCompatible">Whether the mod is network compatible.</param>
        public static void RegisterMod(
            string name,
            string version,
            string author,
            string description,
            bool isNetworkCompatible)
        {
            if (ModdingAPI.ExternallyInstalledMods == null)
            {
                ModdingAPI.ExternallyInstalledMods = new List<ModdingAPI.ModInformation>();
            }

            ModdingAPI.ExternallyInstalledMods.Add(new ModdingAPI.ModInformation
            {
                name = name,
                version = version,
                author = author,
                description = description,
                isNetworkCompatible = isNetworkCompatible,
            });
        }

        /// <summary>
        /// Unregisters a mod.
        /// </summary>
        /// <param name="name">The name of the mod.</param>
        public static void UnregisterMod(string name)
        {
            ModdingAPI.ExternallyInstalledMods?.RemoveAll(m => m.name == name);
            var installedMods = Traverse.Create(typeof(ModdingAPI)).
                Field<List<ModdingAPI.ModInformation>>("installedMods").
                Value;
            installedMods?.RemoveAll(m => m.name == name);
        }
    }
}
