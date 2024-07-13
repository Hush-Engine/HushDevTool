using System.Text;

namespace HushDevTool.Utils;

public static class EnvironmentUtils
{
    public static string s_EnvironmentPath { get; private set; }

    private static string s_ConfigPath => $"{s_EnvironmentPath}{DEVTOOL_CONFIG_NAME}";

    private const string ENV_PATH_KEY = "EnvironmentPath";

    private const string DEVTOOL_CONFIG_NAME = "devtool.config";

    public static bool IsHushRootSet()
    {
        if (string.IsNullOrEmpty(s_EnvironmentPath))
        {
            s_EnvironmentPath = GetLocalVariableOrDefault(ENV_PATH_KEY, $"{Environment.CurrentDirectory}{DEVTOOL_CONFIG_NAME}");
        }
        return !string.IsNullOrEmpty(s_EnvironmentPath);
    }

    public static string ChooseHushRoot()
    {
        string resultPath;
        bool success = FileDialogNative.ShowOpenDirectoryDialog(
            "Choose the root folder of the HushEngine",
            out resultPath
        );
        if (!success)
        {
            Logger.Error("Could not get the root environment from the file dialog, please try again!");
        }
        s_EnvironmentPath = resultPath;
        //Create a file with the devtool config
        SetLocalVariable(ENV_PATH_KEY, s_EnvironmentPath);
        return s_EnvironmentPath;
    }

    public static string? GetLocalVariableOrDefault(string name, string configFilePath = "")
    {
        //Open the file, find the variable and write to it or append it
        //TODO: Optimize this if needed (I don't think so tbh)
        if (configFilePath == string.Empty)
        {
            configFilePath = s_ConfigPath;
        }

        if (!File.Exists(configFilePath)) return null;

        return File.ReadAllLines(configFilePath)
            .Where(line => line.Split('=')[0].Trim() == name)
            .Select(line => line.Split('=')[1].Trim())
            .FirstOrDefault();
    }

    public static void SetLocalVariable<T>(string name, in T value)
    {
        //TODO: Use a single file pointer for all of this

        //Open the file, find the variable and write to it or append it
        using (FileStream fileStream = File.Open(s_ConfigPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            //let's do a buffer of 150 chars and a max of 14 lines of this length
            Span<byte> fileBuffer = stackalloc byte[2010]; // 16Kb
            fileStream.Read(fileBuffer);
            IEnumerable<string> rawLines = Encoding.ASCII.GetString(fileBuffer).Split('\n');
            Dictionary<string, string> lines = rawLines
                .Where(line => line[0] != '\0')
                .ToDictionary(line => line.Split('=')[0].Trim());
            lines[name] = $"{name}={value}\n";
            fileBuffer = Encoding.ASCII.GetBytes(string.Join('\n', lines.Values));
            //Rewrite the file (we can get away with this because it's honestly not that much data)
            fileStream.SetLength(0);
            fileStream.Write(fileBuffer);
        }
    }
}

