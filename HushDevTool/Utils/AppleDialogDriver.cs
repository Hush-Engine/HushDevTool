using System;
using HushDevTool.Services;

namespace HushDevTool.Utils;

public class AppleDialogDriver : IDialogDriver
{
    private ICommandLineService m_commandLineService;
    private const int USER_CANCELLED_RC = 1;


    public AppleDialogDriver()
    {
        this.m_commandLineService = new CommandLineService();
    }

    public string? OpenFileDialog(string windowTitle)
    {
        string outputFromCmd;
        int rc = this.m_commandLineService.RunWithOutput(
            "osascript",
            OpenFileCommand(windowTitle),
            out outputFromCmd
        );
        if (rc == USER_CANCELLED_RC)
        {
            return null;
        }
        return outputFromCmd.TrimEnd('\n');
    }

    public string? NewFileDialog(string windowTitle)
    {
        string outputFromCmd;
        int rc = this.m_commandLineService.RunWithOutput(
            "osascript",
            SaveAsCommand(windowTitle),
            out outputFromCmd
        );
        if (rc == USER_CANCELLED_RC)
        {
            return null;
        }
        return outputFromCmd.TrimEnd('\n');
    }

    public string? OpenDirectoryDialog(string windowTitle)
    {
        string outputFromCmd;
        int rc = this.m_commandLineService.RunWithOutput(
            "osascript",
            OpenDirCommand(windowTitle),
            out outputFromCmd
        );
        if (rc == USER_CANCELLED_RC)
        {
            return null;
        }
        return outputFromCmd.TrimEnd('\n');
    }

    private string SaveAsCommand(string windowTitle)
    {
        return $"-e \"POSIX path of (choose file name with prompt \\\"{windowTitle}:\\\")\"";
    }

    private string OpenFileCommand(string windowTitle)
    {
        return $"-e \"POSIX path of (choose file with prompt \\\"{windowTitle}:\\\")\"";
    }

    private string OpenDirCommand(string windowTitle)
    {
        return $"-e \"POSIX path of (choose folder with prompt \\\"{windowTitle}:\\\")\"";
    }
}
