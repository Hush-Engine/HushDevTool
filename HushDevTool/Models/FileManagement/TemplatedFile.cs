using System;
using HushDevTool.Utils;

namespace HushDevTool.Models.FileManagement;

public class TemplatedFile
{
	public string FileName { get; private set; }

	public string AbsolutePath { get; private set; }

	private string m_templatedFilePath;

	private string m_author;

	private string m_brief;

	//Extension?

	public TemplatedFile(string path, string templateFileName, string authorName, string brief)
	{
		this.AbsolutePath = path;
		this.FileName = path.Replace(EnvironmentUtils.s_EnvironmentPath, string.Empty);
		this.m_templatedFilePath = templateFileName;
		this.m_author = authorName;
		this.m_brief = brief;
	}

	public void Save()
	{
        string templateContent = File.ReadAllText(this.m_templatedFilePath);
        string dateString = DateTime.Now.ToString("yyyy-MM-dd");

        const string FILENAME_REPLACE = "{{filename}}";
        const string AUTHOR_REPLACE = "{{author}}";
        const string DATE_REPLACE = "{{date}}";
        const string BRIEF_REPLACE = "{{brief}}";

		Dictionary<string, string> matchAndReplaceDict = new Dictionary<string, string>
		{
			{ FILENAME_REPLACE, this.FileName},
			{ AUTHOR_REPLACE, this.m_author },
			{ DATE_REPLACE, dateString },
			{ BRIEF_REPLACE, this.m_brief }
		};

		//TODO: See if we can use StringBuilder
		foreach (var entry in matchAndReplaceDict)
		{
			templateContent = templateContent.Replace(entry.Key, entry.Value);
		}

		File.WriteAllText(this.AbsolutePath, templateContent);
    }

}

