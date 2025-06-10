namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using global::Types;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class StatusEffectConfigRule : Rule, IConfigWritable<List<StatusEffectData>>, IMultiplayerSafe
    {
        public override string Description => "Some buffs/debuffs have their duration & effects adjusted";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.StatusEffectDataModified;

        private readonly List<StatusEffectData> _adjustments;
        private List<StatusEffectData> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectConfigRule"/> class.
        /// </summary>
        /// <param name="adjustments">Accepts a List of replacement StatusEffectsData
        /// which will replace existing values.</param>
        public StatusEffectConfigRule(List<StatusEffectData> adjustments)
        {
            _adjustments = adjustments;
            _originals = new List<StatusEffectData>();
        }

        public List<StatusEffectData> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(Context context)
        {
            _originals = UpdateStatusEffectConfig(_adjustments);
        }

        protected override void OnDeactivate(Context context)
        {
            UpdateStatusEffectConfig(_originals);
        }

        private static List<StatusEffectData> UpdateStatusEffectConfig(List<StatusEffectData> adjustments)
        {
            var effectsConfigs = Traverse.Create(typeof(StatusEffectsConfig)).Field<StatusEffectData[]>("effectsConfig").Value;
            var previousConfigs = new List<StatusEffectData>();
            for (var i = 0; i < effectsConfigs.Length; i++)
            {
                var matchingIndex = adjustments.FindIndex(a => a.effectStateType == effectsConfigs[i].effectStateType);
                if (matchingIndex < 0)
                {
                    continue;
                }

                previousConfigs.Add(effectsConfigs[i]);
                effectsConfigs[i] = adjustments[matchingIndex];
            }

            return previousConfigs;
        }
    }
}
