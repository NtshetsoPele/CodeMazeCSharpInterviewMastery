using System.Diagnostics;
using System.Reflection;

namespace AdditionalProject;

public static class MiniLoader
{
    // Load assembly path, list public static methods on a specified type,
    // then invoke a method by name with N warmup calls and M timed calls.
    public static void Run(string assemblyPath, string typeFullName, string methodName, int warmup, int timed)
    {
        if (!File.Exists(assemblyPath))
        {
            Console.WriteLine($"No assembly found using '{assemblyPath}'.");
            return;
        }

        var assembly = Assembly.LoadFrom(assemblyPath);
        var type = assembly.GetType(typeFullName);
        if (type is null)
        {
            Console.WriteLine($"Type '{typeFullName}' not found in assembly '{assemblyPath}'.");
            return;
        }

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
        foreach (var m in methods)
        {
            Console.WriteLine($" - {m.Name}({string.Join(", ", m.GetParameters().Select(p => p.ParameterType.Name))}) -> {m.ReturnType.Name}");
        }

        var target = methods.FirstOrDefault(m =>
            m.Name == methodName &&
            m.ReturnType == typeof(int) &&
            m.GetParameters() is var ps && ps.Length == 1 && ps[0].ParameterType == typeof(int));

        if (target is null)
        {
            Console.WriteLine("Method not found or signature mismatch (expected: int f(int)).");
            return;
        }

        object?[] args = new object?[] { 0 };
        int checksum = 0;
        // Warmup: trigger JIT (and potential tier-up preparations)
        for (int i = 0; i < warmup; i++)
        {
            args[0] = i;
            checksum ^= (int)target.Invoke(null, args)!;
        }
        // Timed phase
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < timed; i++)
        {
            args[0] = i;
            checksum ^= (int)target.Invoke(null, args)!;
        }
        sw.Stop();
        Console.WriteLine($"Timed calls: {timed}, Elapsed: {sw.ElapsedMilliseconds} ms, Checksum: {checksum}");
    }
}