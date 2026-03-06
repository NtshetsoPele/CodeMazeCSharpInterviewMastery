
Console.WriteLine("Hot() - first call");
// Tier 0 compilation
Hot();

Console.WriteLine("Hot() - hot loop");
// Higher tiers compilation
for (int i = 0; i < 1_000_000; i++)
{
    Hot();
}

static void Hot()
{
    Console.WriteLine("Hello from Hot!");
}