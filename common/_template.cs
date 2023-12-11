namespace advent_of_code_2023;
class _template: _base {  
  public static void run () 
  {  
    var data = get_data_live();
    var first = calc_part_1(data);
    var second = calc_part_2(data);

    Console.WriteLine($"{nameof(_template)}\n\t1st part:\t{first}\n\t2nd part:\t{second}");
  }

  public static int calc_part_1(string data) {
    return default(int);
  }

  public static int calc_part_2(string data) {
    return default(int);
  }
}