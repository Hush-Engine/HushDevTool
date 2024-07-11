using System;
using System.Runtime.InteropServices;

namespace HushDevTool.Utils;

public static class FileDialogNative
{
    private static IDialogDriver s_dialogDriver = ChooseDriver();

    public static bool ShowOpenDialog(string title, out string path)
    {
        path = s_dialogDriver.OpenFileDialog(title);
        return path != null;
    }

    public static bool ShowNewFileDialog(string title, out string path)
    {
        path = s_dialogDriver.OpenFileDialog(title);
        return path != null;
    }

    private static IDialogDriver ChooseDriver()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return new AppleDialogDriver();
        }
        return null;
    }

}

