using System;
using System.Collections.Generic;

class CommandHandlerTests
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

        Check("Search finds sequence", SearchFindsSequence);
        Check("Search returns NOT FOUND", SearchReturnsNotFound);
        Check("Diff counts different amino acids", DiffCountsDifference);
        Check("Diff counts different lengths", DiffCountsDifferentLengths);
        Check("Diff reports missing protein", DiffReportsMissingProtein);
        Check("Mode finds most frequent amino acid", ModeFindsMostFrequent);
        Check("Mode uses alphabet when frequency is equal", ModeUsesAlphabet);

        Console.WriteLine();
        Console.WriteLine($"Passed: {passed}; Failed: {failed}");

        return failed == 0 ? 0 : 1;
    }

    private static void Contains(string text, string expected)
    {
        if (!text.Contains(expected))
        {
            throw new Exception($"Expected text to contain: [{expected}]");
        }
    }

    private static void SearchFindsSequence()
    {
        List<GeneticData> data = new List<GeneticData>
        {
            new GeneticData("Protein1", "Human", "ACDEFG"),
            new GeneticData("Protein2", "Mouse", "GGACDE"),
            new GeneticData("Protein3", "Cat", "TTTT")
        };

        string result = CommandHandler.Search(data, "ACD");

        Contains(result, "Protein1");
        Contains(result, "Protein2");
    }

    private static void SearchReturnsNotFound()
    {
        List<GeneticData> data = new List<GeneticData>
        {
            new GeneticData("Protein1", "Human", "ACDEFG"),
            new GeneticData("Protein2", "Mouse", "GGACDE")
        };

        string result = CommandHandler.Search(data, "AVD");

        Contains(result, "NOT FOUND");
    }

    private static void DiffCountsDifference()
    {
        List<GeneticData> data = new List<GeneticData>
        {
            new GeneticData("Protein1", "Human", "ACDE"),
            new GeneticData("Protein2", "Mouse", "ACDF")
        };

        string result = CommandHandler.Diff(data,"Protein1","Protein2");

        Contains(result, "1");
    }

    private static void DiffCountsDifferentLengths()
    {
        List<GeneticData> data = new List<GeneticData>
        {
            new GeneticData("Protein1", "Human", "ACDE"),
            new GeneticData("Protein2", "Mouse", "ACDEFG")
        };

        string result = CommandHandler.Diff(data,"Protein1","Protein2");

        Contains(result, "2");
    }

    private static void DiffReportsMissingProtein()
    {
        List<GeneticData> data = new List<GeneticData>
        {
            new GeneticData("Protein1", "Human", "ACDE")
        };

        string result = CommandHandler.Diff(data,"Protein1","Unknown");

        Contains(result, "MISSING:Unknown");
    }

    private static void ModeFindsMostFrequent()
    {
        List<GeneticData> data = new List<GeneticData>
        {
            new GeneticData("Protein1", "Human", "ACCCAA")
        };

        string result = CommandHandler.Mode(data,"Protein1");

        Contains(result, "A 3");
    }

    private static void ModeUsesAlphabet()
    {
        List<GeneticData> data = new List<GeneticData>
        {
            new GeneticData("Protein1", "Human", "CCAA")
        };

        string result = CommandHandler.Mode(data,"Protein1");

        Contains(result, "A 2");
    }
}