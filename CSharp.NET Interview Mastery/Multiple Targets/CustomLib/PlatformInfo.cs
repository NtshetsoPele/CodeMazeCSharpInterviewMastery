namespace CustomLib;

public static class PlatformInfo
{
#if NET8_0
    public static string GetRuntimeLabel() => "Running on .NET 8.0";
#elif NET9_0
    public static string GetRuntimeLabel() => "Running on .NET 9.0";
#elif NET10_0
    public static string GetRuntimeLabel() => "Running on .NET 10.0";
#else
    public static string GetRuntimeLabel() => "Unknown .NET version";
#endif
}