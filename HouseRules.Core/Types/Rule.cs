namespace HouseRules.Core.Types
{
    public abstract class Rule
    {
        /// <summary>
        /// Gets the description of the rule.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Called when the rule is activated.
        /// </summary>
        /// <param name="context">A HouseRules Context.</param>
        /// <remarks>
        ///     <para>
        ///     Rules should use this as an indication that they may begin to take effect.
        ///     </para>
        ///     <para>
        ///     This will be called at some point after the rule is selected to be used, but before a game begins
        ///     to be created.
        ///     </para>
        /// </remarks>
        protected internal virtual void OnActivate(Context context)
        {
        }

        /// <summary>
        /// Called when the rule is deactivated.
        /// </summary>
        /// <param name="context">A HouseRules Context.</param>
        /// <remarks>
        /// This method should undo any persisting changes made by the rule up until this point.
        /// </remarks>
        protected internal virtual void OnDeactivate(Context context)
        {
        }

        /// <summary>
        /// Called before a game is created.
        /// </summary>
        /// <param name="context">A HouseRules Context.</param>
        protected internal virtual void OnPreGameCreated(Context context)
        {
        }

        /// <summary>
        /// Called after a game is created.
        /// </summary>
        /// <param name="context">A HouseRules Context.</param>
        /// <remarks>
        /// Note that even though the game is created, the level/POIs/enemies may not yet fully be loaded/spawned.
        /// </remarks>
        protected internal virtual void OnPostGameCreated(Context context)
        {
        }

        /// <summary>
        /// Gets the syncable types that this rule modifies.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     There is no need to override this field unless the rule makes changes to one of the defined
        ///     <see cref="SyncableTrigger"/> types.
        ///     </para>
        ///     <para>
        ///     Multiple types may be separated by a vertical bar <c>|</c>.
        ///     </para>
        /// </remarks>
        /// <example><code>SyncableTrigger.PieceDataChanged</code></example>
        /// <example><code>SyncableTrigger.PieceDataChanged | SyncableTrigger.StatusEffectImmunityChanged</code></example>
        protected internal virtual SyncableTrigger ModifiedSyncables => SyncableTrigger.None;
    }
}
