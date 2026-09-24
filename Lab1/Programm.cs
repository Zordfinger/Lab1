class Programm
{
    static void Main(string[] args)
    {
        Reader reader1 = new("sequences.0.txt", "commands.0.txt", "genedata.0.txt");
        Reader reader2 = new("sequences.1.txt", "commands.1.txt", "genedata.1.txt");
        Reader reader3 = new("sequences.2.txt", "commands.2.txt", "genedata.2.txt");

        reader1.Manipulate();
        reader2.Manipulate();
        reader3.Manipulate();
    }
}