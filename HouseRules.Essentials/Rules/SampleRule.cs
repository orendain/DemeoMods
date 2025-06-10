namespace HouseRules.Essentials.Rules
{
    using HarmonyLib;
    using HouseRules.Core.Types;

    /// <summary>
    /// Represents a modular gameplay modification.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     It is recommended to use passive voice for naming rule classes, and to use the suffix <c>Rule</c>.
    ///     </para>
    ///     <para>
    ///     For example: Use <c>BallistaActionPointsAdjustedRule</c> instead of <c>AdjustBallistaActionPoints</c>.
    ///     </para>
    /// </remarks>
    public sealed class SampleRule : Rule, IPatchable
    {
        /// <summary>
        /// Gets the description of the rule.
        /// </summary>
        public override string Description => "Sample rule";

        /// <summary>
        /// Called when the rule is activated.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     Rules should use this as an indication that they may begin to take effect.
        ///     </para>
        ///     <para>
        ///     This will be called at some point after the rule is selected to be used, but before a game begins
        ///     to be created.
        ///     </para>
        /// </remarks>
        protected override void OnActivate(Context context)
        {
        }

        /// <summary>
        /// Called when the rule is deactivated.
        /// </summary>
        /// <remarks>
        /// This method should undo any persisting changes made by the rule up until this point.
        /// </remarks>
        protected override void OnDeactivate(Context context)
        {
        }

        /// <summary>
        /// Called before a game is created.
        /// </summary>
        protected override void OnPreGameCreated(Context context)
        {
        }

        /// <summary>
        /// Called after a game is created.
        /// </summary>
        /// <remarks>
        /// Note that even though the game is created, the level/POIs/enemies may not yet fully be loaded/spawned.
        /// </remarks>
        protected override void OnPostGameCreated(Context context)
        {
        }

        /// <summary>
        /// Patches the game with the given patcher.
        /// </summary>
        /// <remarks>
        /// Rules should use a static flag (e.g., <c>_isActivated</c>) in the patched method to
        /// ensure it makes modifications only when it is activated.
        /// </remarks>
        private static void Patch(Harmony harmony)
        {
        }
    }
}
