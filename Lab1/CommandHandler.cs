using System.Text;

static class CommandHandler
{
    static private GeneticData? FindGenDataByProtein(List<GeneticData> data, string protein)
    {
        foreach (GeneticData gen in data)
        {
            if (gen.protein.Equals(protein))
            {
                return gen;
            }
        }
        return null;
    }

    static public string Search(List<GeneticData> data, string sequence)
    {
        StringBuilder result = new StringBuilder(); 
        result.AppendLine($"{"Organism", -20} {"Protein"}");
        bool hasfound = false;

        foreach (GeneticData record in data)
        {
            if (record.amino_acids.Contains(sequence))
            {
                result.AppendLine($"{record.organism,-20} {record.protein}");
                hasfound = true;
            }
        }

        if (!hasfound){
            result.AppendLine("NOT FOUND");
        }

        return result.ToString();
    }

    static public string Diff(List<GeneticData> data, string protein1, string protein2)
    {
        StringBuilder result = new StringBuilder();

        GeneticData? FindFirst = FindGenDataByProtein(data, protein1);
        GeneticData? FindSecond = FindGenDataByProtein(data, protein2);

        result.AppendLine("amino-acids difference:");

        if (FindFirst == null || FindSecond == null)
        {
            result.Append("MISSING:");
            if (FindFirst == null)
                result.AppendLine(protein1);
            if (FindSecond == null)
                result.AppendLine(protein2);

            return result.ToString();
        }

        string sequence1 = FindFirst.Value.amino_acids;
        string sequence2 = FindSecond.Value.amino_acids;

        int difference = 0;

        int minLength = Math.Min(sequence1.Length, sequence2.Length);
        for (int i = 0; i < minLength; i++)
        {
            if (sequence1[i] != sequence2[i])
            difference++;
        }

        difference += Math.Abs(sequence1.Length - sequence2.Length);

        result.AppendLine(difference.ToString());
        return result.ToString();
    }

    static public string Mode(List<GeneticData> data, string protein)
    {
        StringBuilder result = new StringBuilder();

        result.AppendLine("amino-acid occurs:");

        GeneticData? record = FindGenDataByProtein(data, protein);
        if (record == null)
        {
            result.AppendLine("MISSING:" + protein.Trim());
            return result.ToString();
        }

        Dictionary<char, int> acids = new Dictionary<char, int>();

        
        foreach (char acid in record.Value.amino_acids)
        {
            if (acids.ContainsKey(acid))
            {
                acids[acid]++;
            } else { acids.Add(acid, 1); }
        }

        int max = 0;
        char mostFrequent = 'Z';

        foreach(char key in acids.Keys)
        {
            if (acids[key] > max || (acids[key] == max && key < mostFrequent))
            {
                max = acids[key];
                mostFrequent = key;
            }
        }

        result.AppendLine(mostFrequent + " " + max);
        return result.ToString();
    }
}