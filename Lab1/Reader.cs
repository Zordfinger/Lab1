using System.IO;
using System.Runtime.InteropServices.Marshalling;

class Reader
{
    List<GenericData> work(string FilePath)
    {
        List<GenericData> data = new List<GenericData>();
        try
        {
            using (StreamReader reader = new(FilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] datas = line.Split("\t");
                    data.Add(new GenericData(datas[0], datas[1], datas[2]));
                }
            }
        }
        catch (IOException ex) { 
            System.Console.WriteLine(ex.ToString());
            return null;
        }
        return data;
    }
}