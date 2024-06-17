using System;
using HushDevTool.Models;
using HushDevTool.Models.CodeQuality;
using HushDevTool.Utils;
using HushDevTool.Extensions;

namespace HushDevTool;

public class CodeAnalyzerService : ICodeAnalyzerService
{
    private const int MAX_FILES_PER_THREAD = 6;
    private const string EXCLUDED_PATHS = "third_party";

    public CodeAnalyzerService()
    {
    }

    public async Task<ICollection<CodeReference>> GetMetricsInSource(string rootFolderPath, CodeMetrics metric = CodeMetrics.All)
    {
        //Start from the root, get all the files with the included formats
        string[] filesToSearch = Directory.GetFiles(rootFolderPath, "*.?pp", SearchOption.AllDirectories);
        //Get them in equal batches for 4 threads
        int filesPerThread = filesToSearch.Length / MAX_FILES_PER_THREAD;
        if (filesPerThread < 1)
        {
            //Single threaded operation
            return await GetMetricForFiles(filesToSearch, metric);
        }
        //Multithreaded fun
        int batchSize = (int)Math.Ceiling((double)filesToSearch.Length / MAX_FILES_PER_THREAD);
        var tasks = new List<Task<ICollection<CodeReference>>>();

        for (int i = 0; i < MAX_FILES_PER_THREAD; i++)
        {
            var batch = filesToSearch.Skip(i * batchSize).Take(batchSize).ToArray();
            if (batch.Length > 0)
            {
                tasks.Add(GetMetricForFiles(batch, metric));
            }
        }

        var results = await Task.WhenAll(tasks);
        //Append the found code references onto shared memory
        return results.SelectMany(r => r).ToList();
    }

    private async Task<ICollection<CodeReference>> GetMetricForFiles(string[] filesToSearch, CodeMetrics metric)
    {
        var allReferences = new List<CodeReference>();
        //Read all files in the array
        for (int i = 0; i < filesToSearch.Length; i++)
        {
            if (filesToSearch[i].Contains(EXCLUDED_PATHS)) continue;
            await ProcessSingleFile(filesToSearch[i], allReferences, metric);
        }
        //Check if they contain any metric
        //Append that to the collection
        return allReferences;
    }

    private async Task ProcessSingleFile(string file, IList<CodeReference> referenceList, CodeMetrics metric)
    {
        string[] lines = await File.ReadAllLinesAsync(file);
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].TrimStart().ToUpper();
            //Has flag for comment based, but it's not a comment
            if (metric.HasCompositeFlagUInt32(CodeMetrics.CommentBasedMetric) && !line.StartsWith("//"))
            {
                continue;
            }
            //Otherwise, we found a comment, let's see if it fits a code metric
            bool validReference = TryGetCodeReference(file, i, line, out CodeReference codeRef);
            if (!validReference) continue;
            referenceList.Add(codeRef);
        }
    }

    private bool TryGetCodeReference(string file, int index, string line, out CodeReference codeRef)
    {
        //TODO: Refactor
        bool found = false;
        codeRef = new CodeReference
        {
            File = file,
            Line = index + 1,
            LineContent = line
        };
        if (line.Contains("TODO"))
        {
            found = true;
            codeRef.Metric |= CodeMetrics.Todo;
        }
        if (line.Contains("FIXME"))
        {
            found = true;
            codeRef.Metric |= CodeMetrics.FixMe;
        }
        if (line.Contains("REFACTOR"))
        {
            found = true;
            codeRef.Metric |= CodeMetrics.Refactor;
        }
        return found;
    }

}

