using System;
namespace HushDevTool.Services;

public interface IBuildService
{
    void Configure(string buildType, string buildDir);

    void Build();
}

