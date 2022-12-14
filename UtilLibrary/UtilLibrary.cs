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

    private static long Gfc(long a, long b) {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public static long Lcm(long a, long b) {
        return (a / Gfc(a, b)) * b;
    }
    
}
