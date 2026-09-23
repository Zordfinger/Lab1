using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

class Writer
{
    private string FileName;
    private int CurrentOperationNumber = 1;
    private string Separator = "-------------------------------------------";
    public Writer(string FileName)
    {
        this.FileName = FileName;
        using(StreamWriter sw = new StreamWriter(FileName, false))
        {
            sw.WriteLine("Gordeyuk Ivan");
            sw.WriteLine("Genetis search");
            sw.WriteLine(Separator);
        }
    }

    public void Search(List<GeneticData> data, string sequence)
    {
        StringBuilder result = new StringBuilder(); 
        result.AppendLine(CurrentOperationNumber.ToString("D3") + " search " + sequence);
        this.CurrentOperationNumber++;
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

        result.AppendLine(Separator);

        using (StreamWriter sw = new StreamWriter(FileName, true)) { sw.Write(result.ToString()); }
    }

    public void Diff(List<GeneticData> data, string protein1, string protein2)
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine(CurrentOperationNumber.ToString("D3") + " diff " + protein1 + " " + protein2);
        this.CurrentOperationNumber++;

        bool FindFirst = data.Exists(data => data.protein.Equals(protein1));
        bool FindSecond = data.Exists(data => data.protein.Equals(protein2));

        result.AppendLine("amino-acids difference:");

        if (!FindFirst || !FindSecond)
        {
            result.Append("MISSING:");
            if (!FindFirst)
                result.AppendLine(protein1);
            if (!FindSecond)
                result.AppendLine(protein2);
            result.AppendLine(Separator);
            
            using(StreamWriter sw = new StreamWriter(FileName, true)) { sw.Write(result.ToString()); }
            return;
        }

        string sequence1 = data.FirstOrDefault(data => data.protein.Equals(protein1)).amino_acids;
        string sequence2 = data.FirstOrDefault(data => data.protein.Equals(protein2)).amino_acids;

        int difference = 0;

        int minLength = Math.Min(sequence1.Length, sequence2.Length);
        for (int i = 0; i < minLength; i++)
        {
            if (sequence1[i] != sequence2[i])
            difference++;
        }

        difference += Math.Abs(sequence1.Length - sequence2.Length);

        result.AppendLine(difference.ToString());
        result.AppendLine(Separator);
        using (StreamWriter sw = new StreamWriter(FileName, true)) { sw.Write(result.ToString()); }
    }

    public void Mode(List<GeneticData> data, string protein)
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine(CurrentOperationNumber.ToString("D3") + " mode " + protein);
        this.CurrentOperationNumber++;

        GeneticData record = data.FirstOrDefault(data => data.protein.Equals(protein)); //todo func to find geneticdata by protein name

        result.AppendLine("amino-acid occurs:");

        Dictionary<char, int> acids = new Dictionary<char, int>();

        foreach (char acid in record.amino_acids)
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
        result.AppendLine(Separator);
        using (StreamWriter sw = new StreamWriter(FileName, true)) { sw.Write(result.ToString()); }
    }
}