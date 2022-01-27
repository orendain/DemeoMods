namespace RulesAPI
{
    public abstract class Rule
    {
        /// <summary>
        /// Gets the description of the rule.
        /// </summary>
        public abstract string Description { get; }

        internal void Activate()
        {
            RulesAPI.Logger.Msg($"Activating rule type: {GetType()}");
            OnActivate();
        }

        internal void Deactivate()
        {
            RulesAPI.Logger.Msg($"Deactivating rule type: {GetType()}");
            OnDeactivate();
        }

        // Recommendation is using passive voice for rule names.

        /// <summary>
        /// Called when the game is patched with this rule.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This method should perform any required patching using the given patcher.
        ///     </para>
        ///     <para>
        ///         Rules should use a flag (e.g., <c>_isActivated</c>) in the patched method to ensure they make modifications only while the rule is activated.
        ///     </para>
        /// </remarks>
        // private static void OnPatch(HarmonyLib.Harmony harmony);

        /// <summary>
        /// Called when a game is started.
        /// </summary>
        /// <remarks>
        ///     This method is where one could make changes to, for example:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>A <c>PieceConfig</c>.</description>
        ///         </item>
        ///         <item>
        ///             <description>An ability's <c>(Clone)</c> object.</description>
        ///         </item>
        ///     </list>
        /// </remarks>
        // protected internal abstract void OnGameStart();

        /// <summary>
        /// Called when a game ends, including when a player leaves a game in progress.
        /// </summary>
        /// <remarks>
        /// This method should undo any persistant changes made during <see cref="OnGameStart"/>.
        /// </remarks>
        // protected internal abstract void OnGameEnd();

        /// <summary>
        /// Called when the rule is activated.
        /// </summary>
        protected abstract void OnActivate();

        /// <summary>
        /// Called when the rule is deactivated.
        /// </summary>
        /// <remarks>
        /// This method should undo any persistant changes made during <see cref="OnActivate"/>.
        /// </remarks>
        protected abstract void OnDeactivate();
    }
}
