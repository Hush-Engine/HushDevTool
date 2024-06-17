using System;
using Cocona;
using HushDevTool.Utils;
using Avalonia.Controls;
namespace HushDevTool.Controllers;

public class FileManagementController
{
	private readonly List<FileDialogFilter> FileFilters = new List<FileDialogFilter>
    {
		new FileDialogFilter
		{
			Extensions = {"cpp", "hpp", "cs", "cmake", "ps1", "py", "sh"}
		}
    };


    public FileManagementController()
	{
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
			var saveFileDialog = new SaveFileDialog
			{
				Title = "Create a new templated file",
				Directory = "./",
				Filters = FileFilters
			};
			
			string? resultPath = await saveFileDialog.ShowAsync(null);

			HushAssertions.AssertNotNull(resultPath, "❌ File creation cancelled by the user!");

            filePath = resultPath!;
		}
		HushAssertions.AssertTrue(filePath != string.Empty, "❌ Received empty path, you must set a file name!");
		if (string.IsNullOrEmpty(brief))
		{

		}
	}
}

