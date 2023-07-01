namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using Revolutions;

    internal static class RevolutionsChecker
    {
        /// <summary>
        /// Returns true if an update is available, false otherwise.
        /// </summary>
        /// <remarks>
        /// Issues encountered during this process, such as unexpected version formats or internet connectivity issues,
        /// will result in this method returning false.
        /// </remarks>
        internal static async Task<bool> IsUpdateAvailable()
        {
            try
            {
                var latestReleaseVersion = await FindLatestReleaseVersion();
                return IsLessThan(RevolutionsVersion.Version, latestReleaseVersion);
            }
            catch (Exception e)
            {
                ConfigurationMod.Logger.Warning($"Failed to determine if an update is available: {e}");
                return false;
            }
        }

        /// <summary>
        /// Finds the latest Revolutions release version, returning the empty string if one can not be found.
        /// </summary>
        private static async Task<string> FindLatestReleaseVersion()
        {
            ConfigurationMod.Logger.Msg("Searching for the latest Revolutions release.");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "Demeo Revolutions");

            var responseString = await client.GetStringAsync("https://api.github.com/repos/TheGrayAlien/DemeoMods/releases");
            var responseJson = JArray.Parse(responseString);
            foreach (var obj in responseJson.Children<JObject>())
            {
                var tagName = obj["tag_name"];
                if (tagName == null)
                {
                    continue;
                }

                if (!TryParseVersion(tagName.ToString(), out var version))
                {
                    continue;
                }

                ConfigurationMod.Logger.Msg($"Found the latest Revolutions release: {version}");
                return version;
            }

            throw new InvalidOperationException("Failed to find a valid Revolutions release tag.");
        }

        /// <summary>
        /// Extracts a version from a standard Revolutions tag name.
        /// </summary>
        private static bool TryParseVersion(string tag, out string version)
        {
            if (!tag.EndsWith("-demeorevolutions"))
            {
                version = string.Empty;
                return false;
            }

            version = tag.Substring(1).Replace("-demeorevolutions", string.Empty);
            return true;
        }

        private static bool IsLessThan(string version, string otherVersion)
        {
            var versionParts = SplitVersion(version);
            var otherVersionParts = SplitVersion(otherVersion);
            return IsLessThan(versionParts, otherVersionParts);
        }

        private static int[] SplitVersion(string version)
        {
            return version.Split('.').Select(int.Parse).ToArray();
        }

        private static bool IsLessThan(IReadOnlyList<int> parts, IReadOnlyList<int> otherParts)
        {
            if (parts.Count == 0 && otherParts.Count == 0)
            {
                return false;
            }

            if (parts.Count == 0)
            {
                return otherParts[0] > 0;
            }

            if (otherParts.Count == 0)
            {
                return parts[0] < 0;
            }

            if (parts[0] > otherParts[0])
            {
                return false;
            }

            if (parts[0] < otherParts[0])
            {
                return true;
            }

            return IsLessThan(parts.Skip(1).ToArray(), otherParts.Skip(1).ToArray());
        }
    }
}
