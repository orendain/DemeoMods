namespace HouseRules.Configuration
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    internal static class VersionChecker
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
                return IsLessThan(BuildVersion.Version, latestReleaseVersion);
            }
            catch (Exception e)
            {
                ConfigurationMod.Logger.Warning($"Failed to determine if an update is available: {e}");
                return false;
            }
        }

        /// <summary>
        /// Finds the latest HouseRules release version, returning the empty string if one can not be found.
        /// </summary>
        private static async Task<string> FindLatestReleaseVersion()
        {
            ConfigurationMod.Logger.Msg("Checking for latest HouseRules release.");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "HouseRules");

            var responseString = await client.GetStringAsync("https://api.github.com/repos/orendain/DemeoMods/releases");
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

                ConfigurationMod.Logger.Msg($"Found latest HouseRules release: {version}");
                return version;
            }

            throw new InvalidOperationException("Failed to find valid HouseRules release.");
        }

        /// <summary>
        /// Extracts a version from a standard HouseRules tag name.
        /// </summary>
        private static bool TryParseVersion(string tag, out string version)
        {
            if (!tag.EndsWith("-houserules"))
            {
                version = string.Empty;
                return false;
            }

            version = tag.Substring(1).Replace("-houserules", string.Empty);
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

        private static bool IsLessThan(int[] parts, int[] otherParts)
        {
            if (parts.Length == 0 && otherParts.Length == 0)
            {
                return false;
            }

            if (parts.Length == 0)
            {
                return 0 < otherParts[0];
            }

            if (otherParts.Length == 0)
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
