using System.Diagnostics;
namespace advent_of_code_2023;
class day_11: _base {  
  public static void run () 
  {  
    var data = get_data_live();
    var first = calc_part_1(data);
    var second = calc_part_2(data);

    Console.WriteLine($"{nameof(day_11)}\n\t1st part:\t{first}\n\t2nd part:\t{second}");
  }

  public static long calc_part_1(string data) {
    return calculate(data, expand_factor: 1);
  }

  public static long calc_part_2(string data) {
    return calculate(data, expand_factor: 999999);
  }

  public static long calculate(string data, int expand_factor) {
    var matrix = parse_string_to_matrix(data);
    var empty_rows = get_empty_rows(matrix);
    var empty_cols = get_empty_columns(matrix);
    var number_positions = assign_numbers_virtually(matrix, empty_rows, empty_cols, expand_factor);

    return get_pairs(Enumerable.Range(1, number_positions.Count).ToArray())
      .Sum(x => calc_distance_between_numbers(x.Key, x.Value, number_positions));;
  }

  /// <summary>
  /// from a 2d array/matrix, return a list of row indices that are completely empty
  /// </summary>
  private static List<int> get_empty_rows(char[,] matrix) {
    var empty_rows = new List<int>();
    for (int i = 0; i <= matrix.GetUpperBound(0); i++) {
      var all_cols_empty = true;
      for (int j = 0; j <= matrix.GetUpperBound(1); j++) {
        if (matrix[i, j] != '.' ) all_cols_empty = false; 
      }
      if (all_cols_empty) empty_rows.Add(i);
    }
    return empty_rows;
  }

  /// <summary>
  /// from a 2d array/matrix, return a list of column indices that are completely empty
  /// </summary>
  private static List<int> get_empty_columns(char[,] matrix) {
    var empty_cols = new List<int>();
    for (int j = 0; j <= matrix.GetUpperBound(1); j++) {
      var all_rows_empty = true;
      for (int i = 0; i <= matrix.GetUpperBound(0); i++) {
        if (matrix[i, j] != '.') all_rows_empty = false; 
      }
      if (all_rows_empty) empty_cols.Add(j);
    }
    return empty_cols;
  }

  /// <summary>
  /// find each pair of numbers (a & b) in the matrix and calculate the distance between them
  /// </summary>
  private static long calc_distance_between_numbers (int a, int b, Dictionary<int, Tuple<int,int>> positions) {
    Debug.Assert(positions.ContainsKey(a), $"number_positions should contain {a}");
    Debug.Assert(positions.ContainsKey(b), $"number_positions should contain {b}");
    
    return calc_distance_between_coordinates(positions[a].Item1, positions[a].Item2, 
      positions[b].Item1, positions[b].Item2);;
  }

  /// <summary>
  /// given two set of coordinates in a 2d array, return the distance between them
  /// </summary>
  private static int calc_distance_between_coordinates(int x1, int y1, int x2, int y2) =>
    Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

  /// <summary>
  /// takes a string and converts it to a 2d char array
  /// </summary>
  private static char[,] parse_string_to_matrix(string data) {
    var lines = data.Split("\n");
    Debug.Assert(lines.Length > 0, "should have at least 1 line");
    Debug.Assert(lines.All(l => l.Length == lines[0].Length), "all lines should be the same length");
    
    var matrix = new char[lines.Length, lines[0].Length]; 
    for (int i = 0; i < lines.Length; i++) {
        for (int j = 0; j < lines[i].Length; j++) {
            matrix[i,j] = lines[i][j];
        }
    }

    return matrix;
  }

  /// <summary>
  /// for an array of numbers, returns a list of all possible pairs
  /// </summary>
  private static List<KeyValuePair<int, int>> get_pairs(int[] nums) {
    var pairs = new List<KeyValuePair<int, int>>();
    for (long i = 0; i < nums.Length; i++) {
      for (long j = i + 1; j < nums.Length; j++) {
        pairs.Add(new KeyValuePair<int,int>(nums[i], nums[j]));
      }
    }
    return pairs;
  }

  /// <summary>
  /// finds the coordinates of all # symbols in the matrix, numbers them sequentially starting at 1, and returns the x, y coordinates for each
  /// </summary>
  private static Dictionary<int, Tuple<int,int>> assign_numbers_virtually(char[,] matrix, List<int> empty_rows, List<int> empty_cols, int expand_factor) {
    var numbers = new Dictionary<int, Tuple<int,int>>();
    var counter = 1;

    var row_offset = 0;
    for (int r = 0; r <= matrix.GetUpperBound(0); r++) {
      if (empty_rows.Contains(r)) row_offset++;

      var col_offset = 0;
      for (int c = 0; c <= matrix.GetUpperBound(1); c++) {
        if (empty_cols.Contains(c)) col_offset++;
        if (matrix[r, c] == '#') {
          numbers.Add(counter, new Tuple<int, int>(r + (row_offset * expand_factor), c + (col_offset * expand_factor)));
          counter++;
        }
      }
    }
    return numbers;
  }
}