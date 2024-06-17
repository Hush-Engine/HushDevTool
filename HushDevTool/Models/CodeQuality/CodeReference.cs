using System;
namespace HushDevTool.Models.CodeQuality;

public struct CodeReference
{
    public string File { get; set; }

    public int Line { get; set; }

    public int BeginCharacterPosition { get; set; }

    public string LineContent { get; set; }

    public CodeMetrics Metric { get; set; }

    public override string ToString()
    {
        return $"Found {Metric} metric in file {File}\n\tOn line {Line}:{BeginCharacterPosition}\n\t{LineContent}";
    }

}

