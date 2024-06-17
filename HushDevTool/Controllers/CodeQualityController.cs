using System;
using HushDevTool.Models.CodeQuality;

namespace HushDevTool;

public class CodeQualityController
{
	private readonly CodeAnalyzerService m_service;

    public CodeQualityController(CodeAnalyzerService service)
	{
		this.m_service = service;
	}

	public void Format()
	{

	}

	public void Tidy()
	{

	}

    /// <summary>
    /// <para>Returns a comprehensive list of all the outstanding stuff</para>
    /// ("TODO, FIXME, REFACTOR" comments and general code stability results)
    /// </summary>
    public async Task Analyze(string metric)
	{
		//Parse the metric to the enum
		ICollection<CodeReference> references = await this.m_service.GetMetricsInSource("/Users/leo.gonzalez/Documents/Projects/Personal/Hush-Engine");
		foreach (var item in references)
		{
			Console.WriteLine(item);
		}
	}

}

