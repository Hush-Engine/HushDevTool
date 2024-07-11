using Cocona;
using HushDevTool;
using HushDevTool.Controllers;
using HushDevTool.Services;
using HushDevTool.Utils;
using Microsoft.Extensions.DependencyInjection;

Cocona.Builder.CoconaAppBuilder appBuilder = CoconaApp.CreateBuilder();

//Add services
appBuilder.Services.AddSingleton<ICodeAnalyzerService, CodeAnalyzerService>();
appBuilder.Services.AddSingleton<ICommandLineService, CommandLineService>();
appBuilder.Services.AddSingleton<IBuildService, BuildService>();


CoconaApp app = appBuilder.Build();

app.AddCommands<CodeQualityController>();
app.AddCommands<BuildController>();
app.AddCommands<FileManagementController>();

app.Run();