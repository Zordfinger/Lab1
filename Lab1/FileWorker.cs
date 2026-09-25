using System.Data.Common;
using System.IO;
using System.Text;

class FileWorker
{
    private string DataFile;
    private string CommandFile;
    private string WriteFile;

    public FileWorker(string DataFile, string CommandFile, string WriteFile)
    {
        this.DataFile = DataFile;
        this.CommandFile = CommandFile;
        this.WriteFile = WriteFile;
    }

    private string RLEDecoding(string AminAcid)
    {
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < AminAcid.Length; i++)
        {
            if (char.IsDigit(AminAcid[i]))
            {
                int count = AminAcid[i] - '0';
                char amin = AminAcid[i + 1];

                result.Append(new string(amin, count));
                i++;
            }
            else {
                result.Append(AminAcid[i]);
            }
        }
        return result.ToString();
    }

    private List<GeneticData> DataFileRead()
    {
        List<GeneticData> data = new List<GeneticData>();
        try{
            using (StreamReader reader = new(DataFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] datas = line.Split('\t');
                    if (datas.Length != 3)
                        continue;
                    string protein = datas[0];
                    string organism = datas[1];
                    string sequence = RLEDecoding(datas[2]);
                    data.Add(new GeneticData(protein, organism, sequence));
                }
            }
        }
        catch (IOException ex) { 
            System.Console.WriteLine(ex.Message);
            return null;
        }
        return data;
    }

    public List<string[]> CommandFileRead()
    {
        List<string[]> commands = new List<string[]>();
        try
        {
            using (StreamReader reader = new(CommandFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] commandstoreturn = line.Split("\t");
                    if (!(commandstoreturn.Length <= 3 && commandstoreturn.Length > 1))
                    continue;
                    commands.Add(commandstoreturn);
                }
            }
            return commands;
        }
        catch (IOException ex) {
            System.Console.WriteLine(ex.Message);
            return commands;
        }
    }

    public void Write() {
        List<GeneticData> data = DataFileRead();
        List<string[]> commands = CommandFileRead();
        int CurrentOperationNumber = 1;
        string Separator = "-------------------------------------------";

        using (StreamWriter sw = new StreamWriter(WriteFile, false))
        {
            sw.WriteLine("Gordeyuk Ivan");
            sw.WriteLine("Genetis search");
            sw.WriteLine(Separator);

            foreach(string[] item in commands)
            {
                string command = item[0];
                switch (command) {
                    case "search": {
                            string sequence = RLEDecoding(item[1]);
                            sw.WriteLine(CurrentOperationNumber.ToString("D3") + " search " + sequence);
                            CurrentOperationNumber++;
                            sw.Write(CommandHandler.Search(data, sequence));
                            sw.WriteLine(Separator);
                        }
                        break;

                    case "diff": {
                            string protein1 = item[1];
                            string protein2 = item[2];
                            sw.WriteLine(CurrentOperationNumber.ToString("D3") + " diff " + protein1 + " " + protein2);
                            CurrentOperationNumber++;
                            sw.Write(CommandHandler.Diff(data, protein1, protein2));
                            sw.WriteLine(Separator);
                        }
                        break;

                    case "mode": {
                            string protein = item[1];
                            sw.WriteLine(CurrentOperationNumber.ToString("D3") + " mode " + protein);
                            CurrentOperationNumber++;
                            sw.Write(CommandHandler.Mode(data, protein));
                            sw.WriteLine(Separator);
                        }
                        break;

                    default: continue;
                
                }
            }
        }
    }
}