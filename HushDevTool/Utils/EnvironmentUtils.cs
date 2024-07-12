using System;
namespace HushDevTool.Utils;

public static class EnvironmentUtils
{
    public static string s_EnvironmentPath { get; private set; }

    public static string s_RunningExecutablePath { get; private set; }

    public const string HUSH_ROOT_NAME = "HUSH_ROOT";

    public static bool IsHushRootSet()
    {
        string? env = Environment.GetEnvironmentVariable(HUSH_ROOT_NAME);
        return !string.IsNullOrEmpty(env);
    }

    public static string ChooseHushRoot()
    {
        string resultPath;
        bool success = FileDialogNative.ShowOpenDirectoryDialog(
            "Choose the root folder of the HushEngine",
            out resultPath
        );
        if (!success)
        {
            Logger.Error("Could not get the root environment from the file dialog, please try again!");
        }
#if WIN32
        Environment.SetEnvironmentVariable(HUSH_ROOT_NAME, resultPath, EnvironmentVariableTarget.Machine);
#else
        //FIXME: This only sets the current process' env variables
        Environment.SetEnvironmentVariable(HUSH_ROOT_NAME, resultPath);
#endif
        s_EnvironmentPath = resultPath;
        return s_EnvironmentPath;
    }
}

