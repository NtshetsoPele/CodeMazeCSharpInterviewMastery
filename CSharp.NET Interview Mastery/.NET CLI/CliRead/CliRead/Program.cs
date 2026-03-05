using System.Text.Json;

var json = await File.ReadAllTextAsync("C:/ProgramData/CodeMaze/data.json");
using var doc = JsonDocument.Parse(json);
var root = doc.RootElement;
var peeps = root.GetProperty("people").EnumerateArray().Select(p => new Person
{
    Name = p.GetProperty("name").GetString(),
    Age = p.GetProperty("age").GetInt32(),
    City = p.GetProperty("city").GetString()
}).ToList();
Console.WriteLine(peeps[0].Name);

return;

class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string City{ get; set; }
}