using System.Text.Json;
Console.WriteLine("Hello, World!");
var testData = JsonSerializer.Deserialize<JSON2ClassConsole.SettingsJson.testData>(System.IO.File.ReadAllText("testData.json"));
ArgumentNullException.ThrowIfNull(testData);
Console.WriteLine(testData.DictData.Number_2);