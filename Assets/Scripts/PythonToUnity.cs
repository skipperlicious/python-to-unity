using System;
using System.Diagnostics;
using System.Threading.Tasks;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;

public class PythonToUnity : MonoBehaviour
{
    // Public static variables to store the Python output data
    public static string PythonOutputLog { get; private set; }      // Complete log of all output
    public static string PythonErrorLog { get; private set; }       // Complete log of all errors
    public static string LatestPythonOutputData { get; private set; }  // Latest output data
    public static string LatestPythonErrorData { get; private set; }   // Latest error data

    // This method is called when the script instance is being loaded
    void Start()
    {
        // Initialize the output and error data
        PythonOutputLog = string.Empty;
        PythonErrorLog = string.Empty;
        LatestPythonOutputData = string.Empty;
        LatestPythonErrorData = string.Empty;

        // Run the Python script asynchronously without blocking Unity
        RunPythonScriptAsync();
    }

    // Asynchronous method to run the Python script
    async void RunPythonScriptAsync()
    {
        // Run the Python script on a separate thread to avoid blocking the main thread
        await Task.Run(() => RunPythonScript());
    }

    // Method to run the Python script
    void RunPythonScript()
    {
        // Create a new process to run the Python script
        Process process = new Process();

        // Use Application.dataPath to get the path to the Assets folder, then move up one directory to reach the project root
        string projectPath = Application.dataPath + "/../";  // Path to your Unity project directory
        string batchFilePath = System.IO.Path.Combine(projectPath, "Assets/Scripts/run_python.bat");  // Combine the project path with the batch file name

        process.StartInfo.FileName = batchFilePath;  // Path to your .bat file
        process.StartInfo.RedirectStandardOutput = true;  // Redirect standard output to capture the script's output
        process.StartInfo.RedirectStandardError = true;   // Redirect standard error to capture any errors
        process.StartInfo.UseShellExecute = false;  // Allows capturing output
        process.StartInfo.CreateNoWindow = true;    // Run the process without creating a new window

        // Event handler for capturing standard output in real-time
        process.OutputDataReceived += (sender, args) => {
            if (!string.IsNullOrEmpty(args.Data))
            {
                // Enqueue the output to be logged on the main thread
                UnityMainThreadDispatcher.Instance().Enqueue(() => {
                    LatestPythonOutputData = args.Data;  // Store the latest output
                    PythonOutputLog += args.Data + "\n";  // Append the output to the full log
                    UnityEngine.Debug.Log("Latest Python Output: " + LatestPythonOutputData);
                });
            }
        };

        // Event handler for capturing standard error in real-time
        process.ErrorDataReceived += (sender, args) => {
            if (!string.IsNullOrEmpty(args.Data))
            {
                // Enqueue the error to be logged on the main thread
                UnityMainThreadDispatcher.Instance().Enqueue(() => {
                    LatestPythonErrorData = args.Data;  // Store the latest error
                    PythonErrorLog += args.Data + "\n";  // Append the error to the full log
                    UnityEngine.Debug.LogError("Latest Python Error: " + LatestPythonErrorData);
                });
            }
        };

        // Start the process and begin reading output
        process.Start();
        process.BeginOutputReadLine();  // Begin reading standard output
        process.BeginErrorReadLine();   // Begin reading standard error

        // Wait for the process to exit
        process.WaitForExit();
        process.Close();  // Close the process to free resources
    }
}
