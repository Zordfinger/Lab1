class Programm
{
    static void Main(string[] args)
    {
        string path = "C:\\C#\\Lab1\\Lab1\\";
        FileWorker reader1 = new(path + "sequences.0.txt", path + "commands.0.txt", path + "genedata.0.txt");
        FileWorker reader2 = new(path + "sequences.1.txt", path + "commands.1.txt", path + "genedata.1.txt");
        FileWorker reader3 = new(path + "sequences.2.txt", path + "commands.2.txt", path + "genedata.2.txt");

        reader1.Write();
        reader2.Write();
        reader3.Write();
    }
}