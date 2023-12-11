using System.Diagnostics;

namespace advent_of_code_2023;
class day_06: _base {  
  public static void run () 
  {  
    var first = calc_part_1(get_data());
    var second = calc_part_2(get_data());

    Console.WriteLine($"{nameof(day_06)}\n\t1st part:\t{first}\n\t2nd part:\t{second}");
  }

  private static string[] get_data() {
    // expected format:
    //Time:      7  15   30
    //Distance:  9  40  200
    var data = get_data_live().Split("\n");
    
    Debug.Assert(data.Length == 2, "input data should have 2 lines");
    Debug.Assert(data[0].StartsWith("Time:"), "1st line should start with 'Time:'");
    Debug.Assert(data[1].StartsWith("Distance:"), "2nd line should start with 'Distance:'");

    return data;
  }

  private static int calc_part_1(string[] data) {  
    var times = parse_part_1_number_line(data[0]);
    var distance = parse_part_1_number_line(data[1]);

    Debug.Assert(times.Count == distance.Count, "number of times and distances should be equal");

    return times.Select((time, idx) => new {time, idx})
      .Select(x => calculate_winning_charge_times(distance[x.idx], x.time).Count)
      .Aggregate((a, x) => a * x);
  }
  
  private static List<int> parse_part_1_number_line(string line) => line
      .Substring(line.IndexOf(':') + 1)
      .Split(" ")
      .Where(x => !string.IsNullOrEmpty(x.Trim()))
      .Select(x => int.Parse(x))
      .ToList();

  private static long parse_part_2_number_line(string line) => 
    long.Parse(line.Substring(line.IndexOf(':') + 1).Replace(" ", ""));

  private static List<long> calculate_winning_charge_times(long distance, long time_to_beat) {
    var times = new List<long>();
    for (var i = 1; i < time_to_beat - 1; i++) {
      var calc = i + (distance / i);
      if (calc < time_to_beat) times.Add(i);
    }
    return times;
  }

  private static int calc_part_2(string[] data) {
    var time = parse_part_2_number_line(data[0]);
    var distance = parse_part_2_number_line(data[1]);
    return calculate_winning_charge_times(distance, time).Count;
  }
}