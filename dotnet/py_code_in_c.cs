using System.Diagnostics;

public void RunPythonScript(string inputFilePath, string outputFilePath)
{
    // Path to python.exe
    string pythonPath = @"C:\Python39\python.exe"; // Adjust based on Python installation

    // Path to your Python script
    string scriptPath = @"C:\Scripts\generate_excel.py";

    // Create a process to run the script
    var processStartInfo = new ProcessStartInfo
    {
        FileName = pythonPath,
        Arguments = $"{scriptPath} {inputFilePath} {outputFilePath}",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    using var process = new Process { StartInfo = processStartInfo };

    try
    {
        process.Start();

        // Read the output (optional)
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        // Handle errors
        if (process.ExitCode != 0)
        {
            Console.WriteLine($"Error: {error}");
            throw new Exception($"Python script failed with exit code {process.ExitCode}");
        }

        Console.WriteLine($"Output: {output}");
        Console.WriteLine("Python script executed successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error running Python script: {ex.Message}");
    }
}
