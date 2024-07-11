using System;
using System.Text.RegularExpressions;
using Cocona;
using HushDevTool.Utils;

namespace HushDevTool.Services;

public class BuildService : IBuildService
{
    private readonly ICommandLineService m_commandLineService;

    private static readonly Regex VersionFindingExpression = new Regex("version\\s+(\\d+\\.\\d+\\.\\d+)");

    public BuildService(ICommandLineService commandLineService)
    {
        this.m_commandLineService = this.m_commandLineService = commandLineService;
    }

    public void Configure(string buildType, string buildDir)
    {
        Logger.Info("🛠️ Configuring project...");
        this.CheckCMakeVersion();
        if (!this.IsNinjaInstalled())
        {
            Logger.Warn("Ninja is not installed. Ninja is the recommended build system!");
        }

        Directory.CreateDirectory(buildDir);
        Directory.SetCurrentDirectory(buildDir);
    }

    public void Build()
    {
        throw new NotImplementedException();
    }

    private void CheckCMakeVersion()
    {
        const string minimumCMakeVersion = "3.25";
        string output;
        int rc = this.m_commandLineService.RunWithOutput("cmake", "--version", out output);

        if (rc != 0)
        {
            throw new CoconaException("CMake is not installed. Please install CMake and try again.");
        }
        //Take out the whitespace from the output and convert the version to a float
        Match matchFound = VersionFindingExpression.Match(output);
        string versionFromCmd = matchFound.Groups[1].Value;
        if (versionFromCmd.CompareTo(minimumCMakeVersion) < 0)
        {
            throw new CoconaException($"CMake version must be at least {minimumCMakeVersion} Please update CMake and try again.");
        }
    }

    private bool IsNinjaInstalled()
    {
        int rc = this.m_commandLineService.Run("ninja", "--version");
        return rc == 0;
    }
}

