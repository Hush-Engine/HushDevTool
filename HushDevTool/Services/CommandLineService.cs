using System;
using System.Diagnostics;

namespace HushDevTool.Services;

public class CommandLineService : ICommandLineService
{

	public CommandLineService()
	{
	}

    /// <summary>
    /// Runs a given command and returns its exit code
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public int Run(string cmd, string args)
    {
        try
        {
            using(Process? p = Process.Start(CreateProcessInfo(cmd, args)))
            {
                if (p == null) return 1;
                p.WaitForExit();
                return p.ExitCode;
            }
        }
        catch (Exception)
        {
            return 1;
        }
    }

    public Task<int> RunAsync(string cmd, string args)
    {
        throw new NotImplementedException();
    }

    public int RunWithOutput(string cmd, string args, out string cmdOutput)
    {
        cmdOutput = string.Empty;
        try
        {
            using (Process? p = Process.Start(CreateProcessInfo(cmd, args, true)))
            {
                if (p == null) return 1;
                p.WaitForExit();
                cmdOutput = p.StandardOutput.ReadToEnd().TrimEnd('\n');
                return p.ExitCode;
            }
        }
        catch(Exception)
        {
            return 1;
        }
    }

    private ProcessStartInfo CreateProcessInfo(string cmd, string args, bool redirectOutput = false)
    {
        return new ProcessStartInfo
        {
            FileName = cmd,
            Arguments = args,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = redirectOutput
        };
    }

}

