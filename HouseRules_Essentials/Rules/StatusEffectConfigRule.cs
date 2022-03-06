namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using DataKeys;
    using global::Types;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class StatusEffectConfigRule : Rule, IConfigWritable<List<StatusEffectData>>, IMultiplayerSafe
    {
        public override string Description => "StatusEffects config is Overridden";

        private readonly List<StatusEffectData> _adjustments;
        private List<StatusEffectData> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectConfigRule"/> class.
        /// </summary>
        /// <param name="adjustments">Accepts a List of replacement StatusEffectsData
        /// which will replace exsting values</param>
        public StatusEffectConfigRule(List<StatusEffectData> adjustments)
        {
            _adjustments = adjustments;
            _originals = new List<StatusEffectData>();
        }

        public List<StatusEffectData> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            _originals = UpdateStatusEffectConfig(_adjustments);
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            UpdateStatusEffectConfig(_originals);
        }

        private static List<StatusEffectData> UpdateStatusEffectConfig(List<StatusEffectData> adjustments)
        {
            var effectsConfig = Traverse.Create(typeof(StatusEffectsConfig)).Field<StatusEffectData[]>("effectsConfig").Value;
            var previousConfigs = new List<StatusEffectData>();
            var changedEffectStateTypes = new List<EffectStateType>();
            foreach (var statusEffect in adjustments)
            {
                changedEffectStateTypes.Add(statusEffect.effectStateType);
            }

            for (int i = 0; i < effectsConfig.Length; i++)
            {
                if (changedEffectStateTypes.Contains(effectsConfig[i].effectStateType))
                {
                    previousConfigs.Add(effectsConfig[i]);
                    for (int j = 0; j < adjustments.Count; j++)
                    {
                        if (adjustments[j].effectStateType == effectsConfig[i].effectStateType)
                        {
                            effectsConfig[i] = adjustments[j];
                        }
                    }
                }
            }

            return previousConfigs;
        }
    }
}
