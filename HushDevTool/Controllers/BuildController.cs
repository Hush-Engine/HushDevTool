using System;
using System.Diagnostics;
using Cocona;
using HushDevTool.Services;
using System.Text;
using System.Text.RegularExpressions;
using HushDevTool.Utils;

namespace HushDevTool.Controllers;

public class BuildController
{
	private readonly IBuildService m_buildService;

	public BuildController(IBuildService buildService)
	{
		this.m_buildService = buildService;
	}

	[Command("configure", Description ="Configures the project using CMake")]
	public void Configure(
		[Option('t', Description = "Build type (Debug, Release, RelWithDebInfo, MinSizeRel)")]
		string buildType = "Release",
		[Option('b', Description = "Build directory")]
		string buildDir = "build"
	)
	{
		try
		{
			this.m_buildService.Configure(buildType, buildDir);
		}
		catch(Exception err)
		{
			Logger.Error(err);
		}
	}

	public void Build(
		[Option('b', Description = "Build directory")]
		string buildDir = "build"
	)
	{

	}
}

