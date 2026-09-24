using System.IO;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

class Reader
{
    private string DataFile;
    private string CommandFile;
    private string WriteFile;

    public Reader(string DataFile, string CommandFile, string WriteFile)
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
                    data.Add(new GeneticData(datas[0], datas[1], RLEDecoding(datas[2])));
                }
            }
        }
        catch (IOException ex) { 
            System.Console.WriteLine(ex.Message);
            return null;
        }
        return data;
    }

    public void Manipulate()
    {
        List<GeneticData> data = DataFileRead();
        Writer writer = new(WriteFile);
        try
        {
            using (StreamReader reader = new(CommandFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] commands = line.Split("\t");
                    switch (commands[0])
                    {
                        case "search":
                            writer.Search(data, commands[1]);
                            break;
                        case "diff":
                            writer.Diff(data, commands[1], commands[2]);
                            break;
                        case "mode":
                            writer.Mode(data, commands[1]);
                            break;
                        default: break;
                    }
                }
            }
            Console.WriteLine("Данные записаны в " + WriteFile);
        }
        catch (IOException ex) {
            System.Console.WriteLine(ex.Message);
        }
    }
}