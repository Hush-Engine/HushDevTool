using System;
using Cocona;

namespace HushDevTool.Controllers;

public class BuildController
{
	public BuildController()
	{
	}

	[Command("configure", Description ="Configures the project using CMake")]
	public void Configure(
		[Option('t', Description = "Build type (Debug, Release, RelWithDebInfo, MinSizeRel)")]
		string buildType = "Release",
		[Option('b', Description = "Build directory")]
		string buildDir = "build"
	)
	{

	}

	public void Build(
		[Option('b', Description = "Build directory")]
		string buildDir = "build"
	)
	{

	}

}

