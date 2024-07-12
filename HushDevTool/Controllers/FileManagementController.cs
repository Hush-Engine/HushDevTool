using System;
using Cocona;
using HushDevTool.Utils;
using Xamarin.Essentials;
using Plugin.FilePicker;
using HushDevTool.Services;

namespace HushDevTool.Controllers;

public class FileManagementController
{
	private ICommandLineService m_commandLineService;

    public FileManagementController(ICommandLineService commandLineService)
	{
		this.m_commandLineService = commandLineService;
	}

	[Command("new-file", Description = "Adds a new file to the project.")]
	public async Task NewFile(
		[Option('f', Description ="File path")]
		string filePath = "",
		[Option('b', Description ="Brief description of the file")]
		string brief = ""
	)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			bool success = FileDialogNative.ShowNewFileDialog("New templated file", out filePath);
			if (!success)
			{
                Logger.Error("❌ Received empty path, you must set a file name!");
                return;
            }
        }
		if (string.IsNullOrEmpty(brief))
		{
			Console.Write("Brief description of the file: ");
			brief = Console.ReadLine();
		}
		string author = this.GetGitUserString();
		string dateString = DateTime.Now.ToString("yyyy-MM-dd");

		//Create the file based on the template
		string[] parts = filePath.Split('.');
		string extension = parts[parts.Length - 1];
		string scriptsDir = "";
		string[] allowedExtensions = GetAllowedFileExtensions(scriptsDir);

		if (!allowedExtensions.Contains(extension))
		{
			Logger.Error($"❌ File format \"{extension}\" is not supported! Please create a template for this file type under the {scriptsDir} directory");
			return;
		}
		
	}

	/// <summary>
	/// Gets the currently active git user and its email modifying the project
	/// </summary>
	private string GetGitUserString()
	{
		string result;
        int rc = this.m_commandLineService.RunWithOutput("git", "config user.name", out result);
		if (rc != 0)
		{
			throw new CoconaException("Git is not installed or failed to run, try reinstalling to fix this issue!");
		}
		return result;
	}

	private string[] GetAllowedFileExtensions(string templatesPath)
	{
		string[] allFiles = Directory.GetFiles(templatesPath);

		return allFiles.Select(fileName =>
		{
			string[] parts = fileName.Split('.');
			return parts[parts.Length - 1];
		}).ToArray();
	}

}

