using Cocona;
using HushDevTool;
using HushDevTool.Controllers;
using Microsoft.Extensions.DependencyInjection;

Cocona.Builder.CoconaAppBuilder appBuilder = CoconaApp.CreateBuilder();

//Add services
appBuilder.Services.AddSingleton<ICodeAnalyzerService, CodeAnalyzerService>();

CoconaApp app = appBuilder.Build();

app.AddCommands<CodeQualityController>();
app.AddCommands<BuildController>();
app.AddCommands<FileManagementController>();

app.Run();