namespace RulesAPI
{
    public interface IConfigurableRule
    {
        /// <summary>
        /// Gets a string that can represents the rule's configuration.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     The implementation of this method must guarantee that when the returned string is passed into the
        ///     corresponding <c>FromConfigString</c>, it will produce a rule identical to the one that
        ///     <c>ToConfigString</c> was called on.
        ///     </para>
        ///     <para>
        ///     For portability purposes, it is recommended that the configuration string keep to UTF-8 encoding.
        ///     </para>
        /// </remarks>
        /// <returns>a string that represents the rule's configuration.</returns>
        string ToConfigString();

        /// <summary>
        /// Returns an instantiated rule initialized with the specified configuration string.
        /// </summary>
        // public static <the-rule-type> FromConfigString(string configString);
    }
}
