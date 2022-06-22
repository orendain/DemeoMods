namespace Common.UI
{
    internal enum UiEnvironment
    {
        Hangouts,
        NonVr,
        Vr,
    }

    internal class UiEnvironments
    {
        internal static UiEnvironment GetCurrentEnvironment()
        {
            if (CommonModule.IsInHangouts()) {
                return UiEnvironment.Hangouts;
            }

            if (CommonModule.IsPcEdition())
            {
                return UiEnvironment.NonVr;
            }

            return UiEnvironment.Vr;
        }
    }
}
