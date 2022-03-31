namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.LevelLoading;
    using Boardgame.Networking;
    using Data.GameData;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class LevelSequenceOverriddenRule : Rule, IConfigWritable<List<string>>, IMultiplayerSafe
    {
        public override string Description => "LevelSequence is overridden";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.NewPieceModified | SyncableTrigger.StatusEffectDataModified;

        private readonly List<string> _adjustments;
        private List<string> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelSequenceOverriddenRule"/> class.
        /// </summary>
        /// <param name="adjustments">List of strings of LevelNames.</param>
        public LevelSequenceOverriddenRule(List<string> adjustments)
        {
            _adjustments = adjustments;
            _originals = new List<string>();
        }

        public List<string> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            _originals = ReplaceExistingProperties(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            ReplaceExistingProperties(_originals);
        }

        /// <summary>
        /// Replaces LevelSequence levels with predefined list.
        /// </summary>
        /// <returns>List of previous LevelSequnece levels that are now replaced.</returns>
        private static List<string> ReplaceExistingProperties(List<string> replacements)
        {
            var levelManager = Resources.FindObjectsOfTypeAll<LevelManager>().First();
            var levelSequence = Traverse.Create(levelManager).Field<LevelSequence>("levelSequence").Value;
            var originals = Traverse.Create(levelSequence).Field<string[]>("levels").Value;
            var newlist = new List<string>
            {
                originals[0], // We need the Entrance to match the current one or the game will crash when moving to the next level.
            };
            newlist.AddRange(replacements); // Append user supplied levels
            Traverse.Create(levelSequence).Field<string[]>("levels").Value = newlist.ToArray();
            return originals.ToList().GetRange(1, 5);
        }
    }
}
