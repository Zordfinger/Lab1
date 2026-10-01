using System;
using System.IO;
using System.Collections.Generic;

class FileWorkerTests
{
    public static int RunTests()
    {
        int passed = 0;
        int failed = 0;

        void Check(string name, Action test)
        {
            try
            {
                test();
                passed++;
                Console.WriteLine("PASSED: " + name);
            }
            catch (Exception ex)
            {
                failed++;
                Console.WriteLine("FAIL: " + name);
                Console.WriteLine("     " + ex.Message);
            }
        }

        Check("CommandFileRead reads commands", CommandFileReadReadsCommands);
        Check("Write creates output file", WriteCreatesFile);
        Check("Write decodes RLE sequence", WriteDecodesRLE);

        Console.WriteLine();
        Console.WriteLine($"Passed: {passed}; Failed: {failed}");

        return failed == 0 ? 0 : 1;
    }

    private static void Equal(int expected, int actual)
    {
        if (expected != actual)
        {
            throw new Exception($"Expected: {expected}, but was: {actual}");
        }
    }

    private static void Contains(string text, string expected)
    {
        if (!text.Contains(expected))
        {
            throw new Exception($"Expected text to contain: [{expected}]");
        }
    }

    private static void CommandFileReadReadsCommands()
    {
        string commandFile = "test_commands.txt";

        File.WriteAllText(commandFile,"search\tACD\nmode\tProtein1\n");

        FileWorker worker = new FileWorker("not_used.txt",commandFile,"not_used_output.txt");

        List<string[]> commands = worker.CommandFileRead();

        Equal(2, commands.Count);

        if (commands[0][0] != "search")
            throw new Exception("First command should be search");

        if (commands[1][0] != "mode")
            throw new Exception("Second command should be mode");

        File.Delete(commandFile);
    }

    private static void WriteCreatesFile()
    {
        string dataFile = "test_data.txt";
        string commandFile = "test_commands.txt";
        string outputFile = "test_output.txt";

        File.WriteAllText(dataFile,"Protein1\tHuman\tACDE\n");

        File.WriteAllText(commandFile,"search\tACD\n");

        FileWorker worker = new FileWorker(dataFile,commandFile,outputFile);

        worker.Write();

        if (!File.Exists(outputFile))
            throw new Exception("Output file was not created");

        File.Delete(dataFile);
        File.Delete(commandFile);
        File.Delete(outputFile);
    }

    private static void WriteDecodesRLE()
    {
        string dataFile = "test_data.txt";
        string commandFile = "test_commands.txt";
        string outputFile = "test_output.txt";

        File.WriteAllText(dataFile,"Protein1\tHuman\tFK3I\n");

        File.WriteAllText(commandFile,"search\tKIII\n");

        FileWorker worker = new FileWorker(dataFile,commandFile,outputFile);

        worker.Write();

        string result = File.ReadAllText(outputFile);

        Contains(result, "Protein1");

        File.Delete(dataFile);
        File.Delete(commandFile);
        File.Delete(outputFile);
    }
}
