namespace StackVsHeap;

public static class Checksums
{
    private static void Fill(Span<byte> s)
    {
        for (var i = 0; i < s.Length; i++)
        {
            s[i] = unchecked((byte)i); // Ignore overflows (wrap around)
        }
    }

    private static int SumSpan(ReadOnlySpan<byte> s)
    {
        var acc = 0;
        foreach (var v in s)
        {
            acc += v;
        }
        return acc;
    }

    public static int SumHeap(int n)
    {
        if (n <= 0)
        {
            return 0;
        }

        var arr = new byte[n]; // heap
        Fill(arr);
        return SumSpan(arr);
    }

    public static int SumStack(int n)
    {
        if (n <= 0)
        {
            return 0;
        }

        const int maxStack = 1024;
        if (n <= maxStack)
        {
            Span<byte> b = stackalloc byte[n]; // stack
            Fill(b);
            return SumSpan(b);
        }

        var arr = new byte[n];            // heap fallback
        Fill(arr);
        return SumSpan(arr);
    }

    public static int SumCaptured(int n)
    {
        if (n <= 0)
        {
            return 0;
        }

        const int maxStack = 1024;
        // choose storage
        if (n <= maxStack)
        {
            Span<byte> b = stackalloc byte[n];
            Fill(b);
            // Capturing 'b' directly is illegal (ref struct). Capture a copy into a heap array to simulate the common pitfall.
            var copy = b.ToArray(); // allocation
            Func<int> f = () => SumSpan(copy); // closure captures 'copy' (heap)
            return f();
        }
        else
        {
            var arr = new byte[n];  // heap
            Fill(arr);
            Func<int> f = () => SumSpan(arr); // closure; captures heap array
            return f();
        }
    }
    public static int SumFixed(int n)
    {
        if (n <= 0)
        {
            return 0;
        }

        const int maxStack = 1024;
        if (n <= maxStack)
        {
            Span<byte> b = stackalloc byte[n];
            Fill(b);
            // No capture: pass span directly
            return SumSpan(b);
        }

        var arr = new byte[n];
        Fill(arr);
        return SumSpan(arr); // direct call; no closure
    }
}