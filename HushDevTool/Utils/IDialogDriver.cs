using System;
namespace HushDevTool.Utils;

public interface IDialogDriver
{
    public string? OpenFileDialog(string windowTitle);

    public string? OpenDirectoryDialog(string windowTitle);

    public string? NewFileDialog(string windowTitle);

}

