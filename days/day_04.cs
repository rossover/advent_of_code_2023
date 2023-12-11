namespace advent_of_code_2023;
class day_04: _base {  
  public static void run () 
  {  
    var data = get_data_live();
    var first = calc_part_1(data);
    var second = calc_part_2(data);

    Console.WriteLine($"{nameof(day_04)}\n\t1st part:\t{first}\n\t2nd part:\t{second}");
  }

  public static int calc_part_1(string data) {
    // e.g. if one match (2 ^ 0 = 1), if two matches (2 ^ 1 = 2), if three matches (2 ^ 2 = 4), etc
    return data.Split(Environment.NewLine)
      .Sum(x => (int) Math.Pow(2, calc_matches(x).Count() -1));
  }

  public static int calc_part_2(string data) {
    var matches = data.Split(Environment.NewLine)
      .Select(x => calc_matches(x).Count())
      .ToList();

    var duplicates = new List<int>();
    for (int i = 0; i < matches.Count; i++) {
      add_duplicates(duplicates, matches, i);
    }

    var r = duplicates.Count + matches.Count;
    return r;
  }

  private static void add_duplicates(List<int> duplicates, List<int> matches, int start) {
    for (int i = start + 1; i <= start + matches[start]; i++) {
      add_duplicates(duplicates, matches, i);
      duplicates.Add(i);
    }
  }

  public static int[] calc_matches(string value) {
    // example values:
    // Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
    // Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19

    // remote "Card " from the beginning      
    value = value[4..].TrimStart();

    // e.g. 1 from Card 1: 42 48 83 86 27
    // e.g. 2 from Card 2: 13 14 33 43 56
    var card_number = int.Parse(value.Split(":")[0]);

    // e.g. from Card 1: 41,48,83,86,17
    var winning_numbers = value.Split(":")[1].Split("|").First()
      .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
      .Select(x => int.Parse(x.Trim())).ToArray();

    // e.g. from Card 1: 83,86,6,31,17,9,48,53
    var your_numbers = value.Split(":")[1].Split("|").Last()
      .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
      .Select(x => int.Parse(x.Trim())).ToArray();

    // find the numbers on the card that match the winning numbers
    var matches = winning_numbers.Intersect(your_numbers);

    return matches.ToArray();
  }
}