namespace advent_of_code_2023;
class day_01: _base {
    public static void run () {  
      var data = get_data_live();
      var first = calc(data, with_spellings:false);
      var second = calc(data, with_spellings:true);

      Console.WriteLine($"{nameof(day_01)}\n\t1st part:\t{first}\n\t2nd part:\t{second}");
    }

    public static int calc(string value, bool with_spellings) {
      return value.Split(Environment.NewLine)
        .Sum(x => calc_single(x, with_spellings));
    }

    private static int calc_single(string value, bool with_spellings) {
      var nums = new Dictionary<string, int>() {
        {"one", 1}, {"two", 2}, {"three", 3}, {"four", 4}, {"five", 5}, {"six", 6}, {"seven", 7}, {"eight", 8}, {"nine", 9}
      };

      char? first = null, last = null;
    
      for (int i = 0; i < value.Length; i++) {
        var s = value[i..];
        if (char.IsNumber(s.First())) { 
          last = s.First();
        } else {
          if (with_spellings) {
            var g = nums.FirstOrDefault(x => s.StartsWith(x.Key));
            if (!String.IsNullOrEmpty(g.Key)) last = Char.Parse(g.Value.ToString());
          }
        }
        first ??= last;
      }

      return Int32.Parse($"{first}{last}");
    }
}