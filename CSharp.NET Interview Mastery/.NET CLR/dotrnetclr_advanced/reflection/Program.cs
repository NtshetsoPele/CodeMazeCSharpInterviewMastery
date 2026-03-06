using System.Reflection;
using System.Diagnostics;

MiniLoader.Run(
    assemblyPath: "System.Collections.Generic.dll",
    typeFullName: "MyLibrary.MyClass",
    methodName: "MyMethod",
    warmup: 1_000,
    timed: 10_000
);

public static class MiniLoader
{
    // Load assembly path, list public static methods on a specified type,
    // then invoke a method by name with N warmup calls and M timed calls.
    public static void Run(string assemblyPath, string typeFullName, string methodName, int warmup, int timed)
    {
        var assembly = Assembly.LoadFrom(assemblyPath);
        var type = assembly.GetType(typeFullName);
        if (type == null)
        {
            Console.WriteLine($"Type {typeFullName} not found in assembly {assemblyPath}.");
            return;
        }

        var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
        if (method == null)
        {
            Console.WriteLine($"Method {methodName} not found in type {typeFullName}.");
            return;
        }

        for (int i = 0; i < warmup; i++)
        {
            method.Invoke(obj: null, parameters: null);
        }

        var stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < timed; i++)
        {
            method.Invoke(obj: null, parameters: null);
        }
        stopwatch.Stop();

        Console.WriteLine($"Executed {timed} calls in {stopwatch.ElapsedMilliseconds} ms.");
    }
}