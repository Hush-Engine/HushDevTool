namespace HushDevTool.Models;

[Flags]
public enum CodeMetrics : uint
{
    None = 0u,
    Todo = 0b00000001u,
    FixMe = 0b00000010u,
    Refactor = 0b00000100u,
    Warnings = 0b00001000u,
    Smells = 0b00010000u,
    CommentBasedMetric = Todo | FixMe | Refactor,
    All = uint.MaxValue
}