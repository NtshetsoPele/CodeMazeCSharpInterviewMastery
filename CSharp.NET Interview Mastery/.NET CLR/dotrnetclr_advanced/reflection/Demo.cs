using System.Reflection;
using AdditionalProject;

namespace reflection;

public static class Demo
{
    // Example target assembly & type.
    // Compile this file into a separate dll.
    public static int Square(int x) => x * x;

    public static void Main()
    {
        var path = Assembly.GetExecutingAssembly().Location;
        var typeName = typeof(Demo).FullName!;
        MiniLoader.Run(path, typeName, nameof(Square), warmup: 10_000, timed: 100_000);
    }
}