namespace RulesAPI
{
    using System;
    using HarmonyLib;

    internal static class RuleParser
    {
        internal static Rule Parse(string ruleName, string configString)
        {
            var traverse = Traverse.CreateWithType(ruleName);
            if (!traverse.TypeExists())
            {
                throw new ArgumentException($"Failed to recognize rule of type: {ruleName}");
            }

            traverse = traverse.Method("FromConfigString", paramTypes: new[] { typeof(string) }, arguments: new object[] { configString });
            if (!traverse.MethodExists())
            {
                throw new ArgumentException($"Failed to find expected FromConfigString method for rule: {ruleName}");
            }

            return traverse.GetValue<Rule>();
        }
    }
}
