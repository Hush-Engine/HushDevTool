using System;
using Cocona;
using HushDevTool.Models.CodeQuality;
using HushDevTool.Utils;

namespace HushDevTool.Controllers;

public class CodeQualityController
{
	private readonly ICodeAnalyzerService m_analyzerService;

    public CodeQualityController(ICodeAnalyzerService analyzerService)
	{
		this.m_analyzerService = analyzerService;
	}

	[Command("format", Description = "Runs clang-format on the project")]
	public void Format()
	{

	}

	[Command("tidy", Description ="Runs clang-tidy on the project")]
	public void Tidy()
	{

	}

	/// <summary>
	/// <para>Returns a comprehensive list of all the outstanding stuff</para>
	/// ("TODO, FIXME, REFACTOR" comments and general code stability results)
	/// </summary>
	[Command("analyze", Description = "Returns a comprehensive list of code references for all the outstanding TODO, FIXME and REFACTOR comments")]
    public async Task Analyze(string metric)
	{
		//Parse the metric to the enum
		ICollection<CodeReference> references = await this.m_analyzerService.GetMetricsInSource("/Users/leo.gonzalez/Documents/Projects/Personal/Hush-Engine");
		foreach (var item in references)
		{
			Logger.Info(item.ToString());
		}
	}

}

