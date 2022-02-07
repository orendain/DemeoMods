namespace HouseRules.Types
{
    /// <summary>
    /// Represents a rule whose configuration can be written.
    /// </summary>
    /// <remarks>
    /// If the implementing class defines a constructor whose exactly one parameter is of the type
    /// of this interface's argument type, that constructor may be used to instantiate a rule from configuration.
    /// </remarks>
    /// <returns>The type of this rule's configuration.</returns>
    public interface IConfigWritable<T>
    {
        /// <summary>
        /// Gets the object that represents the rule's configuration.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     To maximum compatibility, the object must be either a primitive, string or enum,
        ///     or a struct, list, dictionary or array with primitive, string or enum types.
        ///     </para>
        /// </remarks>
        /// <returns>An object representing this rule's configuration.</returns>
        T GetConfigObject();
    }
}
