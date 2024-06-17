using HushDevTool.Models;
using HushDevTool.Models.CodeQuality;

namespace HushDevTool;

public interface ICodeAnalyzerService
{
    Task<ICollection<CodeReference>> GetMetricsInSource(string rootFolderPath, CodeMetrics metric = CodeMetrics.All);
}