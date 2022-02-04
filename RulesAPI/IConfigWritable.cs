namespace RulesAPI
{
    public interface IConfigWritable
    {
        /// <summary>
        /// Gets a string that can represents the rule in configuration.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     The implementation of this method must guarantee that when the returned string is passed into a
        ///     corresponding <c>FromConfigString</c>, it will produce a rule identical to the one that
        ///     <c>ToConfigString</c> was called on.
        ///     </para>
        ///     <para>
        ///     For portability purposes, it is recommended that the configuration string keep to UTF-8 encoding.
        ///     </para>
        /// </remarks>
        /// <returns>A string representing the instance of this rule.</returns>
        string ToConfigString();

        /// <summary>
        /// Returns a rule initialized with the specified configuration string.
        /// </summary>
        // public static RulesAPI.Rule FromConfigString(string configString);
    }
}
