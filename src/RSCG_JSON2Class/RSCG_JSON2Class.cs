using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace RSCG_JSON2Class;

[Generator]
public class RSCG_JSON2Class : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var jsonFiles = context
            .AdditionalTextsProvider
            .Where(f => f.Path.EndsWith(".json"))
            .Collect()
            ;

        var namespaceRoot = context.AnalyzerConfigOptionsProvider.Select((it, _) =>
        {
            it.GlobalOptions.TryGetValue("build_property.rootnamespace", out var value);
            
            return value??"RSCG_JSON2Class";
        });

        var data = namespaceRoot.Combine(jsonFiles);
        context.RegisterSourceOutput(data, GenerateSource);

        
    }

    private void GenerateSource(SourceProductionContext context, (string namespaceName, ImmutableArray<AdditionalText> jsonFiles) tuple)
    {
        var namespaceName = tuple.namespaceName;
        var jsonFiles = tuple.jsonFiles;
        if(jsonFiles.Length == 0)
        {
            return;
        }
        var split= new char[2] {'/','\\'};
        foreach ( var jsonFile in jsonFiles)
        {
            var nameSettings = jsonFile.Path.Split(split).Last().Split('.').First();
            var json = jsonFile.GetText()?.ToString();
            if(string.IsNullOrWhiteSpace(json ))
            {
                continue;
            }
            var g = new GeneratorFromJSON();
            var generatedCode = g.GenerateFile(json, nameSettings, namespaceName, "appsettings.txt");
            context.AddSource($"{nameSettings}.cs", generatedCode);
        }
    }
}
