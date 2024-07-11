using System;
using HushDevTool.Services;

namespace HushDevTool.Utils;

public class AppleDialogDriver : IDialogDriver
{
    private ICommandLineService m_commandLineService;
    private const int USER_CANCELLED_RC = 1;

    private const string SAVE_AS_CMD = "-e \"POSIX path of (choose file name with prompt \\\"Please enter a file name:\\\")\"";
    private const string OPEN_FILE_CMD = "-e \"POSIX path of (choose file name with prompt \\\"Please enter a file name:\\\")\"";

    public AppleDialogDriver()
    {
        this.m_commandLineService = new CommandLineService();
    }

    public string? OpenFileDialog(string windowTitle)
    {
        string outputFromCmd;
        int rc = this.m_commandLineService.RunWithOutput(
            "osascript",
            OPEN_FILE_CMD,
            out outputFromCmd
        );
        if (rc == USER_CANCELLED_RC)
        {
            return null;
        }
        return outputFromCmd;
    }

    public string? NewFileDialog(string windowTitle)
    {
        string outputFromCmd;
        int rc = this.m_commandLineService.RunWithOutput(
            "osascript",
            SAVE_AS_CMD,
            out outputFromCmd
        );
        if (rc == USER_CANCELLED_RC)
        {
            return null;
        }
        return outputFromCmd;
    }
}
