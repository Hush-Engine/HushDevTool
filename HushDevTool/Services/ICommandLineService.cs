using System;
namespace HushDevTool.Services;

public interface ICommandLineService
{
    int Run(string cmd, string args);

    Task<int> RunAsync(string cmd, string args);

    int RunWithOutput(string cmd, string args, out string cmdOutput);
}

