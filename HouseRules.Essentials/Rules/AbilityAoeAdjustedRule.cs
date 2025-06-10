namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class AbilityAoeAdjustedRule : Rule, IConfigWritable<Dictionary<AbilityKey, int>>, IMultiplayerSafe
    {
        public override string Description => "Ability AOE Ranges are adjusted";

        protected override SyncableTrigger ModifiedSyncables => SyncableTrigger.StatusEffectDataModified;

        private readonly Dictionary<AbilityKey, int> _adjustments;
        private Dictionary<AbilityKey, int> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityAoeAdjustedRule"/> class.
        /// </summary>
        /// <param name="adjustments">Key-value pairs mapping the name of an ability and the AOE range
        /// added to their base.</param>
        /// <remarks>Adding '1' to a 3x3 spell will make a 5x5. Negative values will reduce AOE.</remarks>
        public AbilityAoeAdjustedRule(Dictionary<AbilityKey, int> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<AbilityKey, int>();
        }

        public Dictionary<AbilityKey, int> GetConfigObject() => _adjustments;

        protected override void OnPreGameCreated(Context context)
        {
            _originals = ReplaceAbilities(context, _adjustments);
        }

        protected override void OnDeactivate(Context context)
        {
            ReplaceAbilities(context, _originals);
        }

        private static Dictionary<AbilityKey, int> ReplaceAbilities(
            Context context,
            Dictionary<AbilityKey, int> replacements)
        {
            var originals = new Dictionary<AbilityKey, int>();
            foreach (var replacement in replacements)
            {
                var abilityPromise = context.AbilityFactory.LoadAbility(replacement.Key);
                abilityPromise.OnLoaded(ability =>
                {
                    originals[replacement.Key] = -replacement.Value;
                    var aoe = Traverse.Create(ability).Field<AreaOfEffect>("areaOfEffect").Value;
                    Traverse.Create(aoe).Field<int>("range").Value += replacement.Value; // Adjust the AOE outline when casting.
                    Traverse.Create(ability).Field<int>("areaOfEffectRange").Value += replacement.Value; // Adjust value displayed on the card.
                });
            }

            // Theoretically, there can be a race condition as there's no guarantee the promise above is fulfilled by
            // the return value is used. Realistically, there's no concern since the value isn't used until after the
            // promise has long been fulfilled.
            return originals;
        }
    }
}
