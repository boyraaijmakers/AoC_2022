namespace UtilLibrary;

public class AoCUtils
{
    private static string getFilePath(string s) {
        return $"C:\\Users\\boy.raaijmakers\\git\\AoC_2022\\input\\{s}.txt";
    }

    public static string readInput(string dayNum) {
        string fileName = getFilePath(dayNum);
        try {
            return System.IO.File.ReadAllText(fileName);
        } catch {
            Console.WriteLine($"Input file with path {fileName} not found!");
            return "";
        }
    }

    public static string[] tokenize(string input, string delimiter) {
        return input.Split(delimiter);
    }
}
