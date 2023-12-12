using System.Runtime.CompilerServices;
namespace advent_of_code_2023;
abstract class _base {
    public static string get_data_test([CallerMemberName] string caller = "", [CallerFilePath] string file = "") =>
        File.ReadAllText($"./data/{Path.GetFileNameWithoutExtension(file)}_test.txt");   
    
    public static string get_data_live([CallerMemberName] string caller = "", [CallerFilePath] string file = "") {
        var path = $"./data/{Path.GetFileNameWithoutExtension(file)}.txt";  
        var data = File.ReadAllText(path);
        return data;
    }    
}