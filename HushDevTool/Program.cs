//TODO: Cocona integration, or a possibly new in-house thing??
using System;
using HushDevTool;

class Program
{
	static async Task Main(string[] args) {
		CodeAnalyzerService service = new CodeAnalyzerService();
		CodeQualityController controller = new CodeQualityController(service);
		await controller.Analyze(string.Empty);

	}
}
