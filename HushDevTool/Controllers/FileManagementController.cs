using System;
using Cocona;
using HushDevTool.Utils;
using Xamarin.Essentials;
using Plugin.FilePicker;
using HushDevTool.Services;
using HushDevTool.Models.FileManagement;

namespace HushDevTool.Controllers;

public class FileManagementController
{
	public const string TEMPLATE_FILES_KEY = "TemplateFilesDir";

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

		//Create the file based on the template
		string[] parts = filePath.Split('.');
		string extension = parts[parts.Length - 1];
		string? templatedFilesDir = EnvironmentUtils.GetLocalVariableOrDefault(TEMPLATE_FILES_KEY);
		if (templatedFilesDir == null)
		{
			Logger.Warn("Template files directory not found, press any key to set this directory");
			Console.ReadKey(true);
			bool selected = FileDialogNative.ShowOpenDirectoryDialog("Select the templates directory!", out templatedFilesDir);
			if (!selected)
			{
				Logger.Error("❌ Template files directory not set! Aborting file creation");
				return;
			}
			//Set the env variable
			EnvironmentUtils.SetLocalVariable(TEMPLATE_FILES_KEY, templatedFilesDir);
		}
		string[] allowedExtensions = GetAllowedFileExtensions(templatedFilesDir);

		if (!allowedExtensions.Contains(extension))
		{
			Logger.Error($"❌ File format \"{extension}\" is not supported! Please create a template for this file type under the {templatedFilesDir} directory");
			return;
		}

		//Actually create the file
		string templateVersion = $"{templatedFilesDir}.{extension}";
		string author = GetGitUserString();

		var fileToCreate = new TemplatedFile(
			filePath,
			templateVersion,
			author,
			brief
		);

		fileToCreate.Save();

		Logger.Success($"File {fileToCreate.FileName} created successfully!");
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

