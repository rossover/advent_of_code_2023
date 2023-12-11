namespace advent_of_code_2023;
class day_08: _base {  
  public static void run () {  
    var first = calc_part_1(get_data_live());
    var second = calc_part_2(get_data_live());

    Console.WriteLine($"{nameof(day_08)}\n\t1st part:\t{first}\n\t2nd part:\t{second}");
  }

  private static int calc_part_1(string data) {
    // first line of directions is L for left and R for right in a sequence
    // e.g. LLRLR means left, left, right, left, right
    var instructions = new instructions(data.Split(Environment.NewLine).First());

    // nodes are formatted as: BQV = (HFG, GDR)
    var nodes = get_nodes(data);

    // navigate the nodes
    var path = navigate_nodes(instructions, nodes, "AAA");
    return path.Count;
  }

  private static long calc_part_2(string data) {
    // first line of directions is L for left and R for right in a sequence
    // e.g. LLRLR means left, left, right, left, right
    var instructions = new instructions(data.Split(Environment.NewLine).First());

    // nodes are formatted as: BQV = (HFG, GDR)
    var nodes = get_nodes(data);
    var sequences = new List<long>();
    var current_nodes = nodes.Where(x => x.Key.EndsWith("A")).Select(x => x.Key).ToArray();

    foreach (var start in current_nodes) {
      var x = navigate_nodes(instructions, nodes, start);
      sequences.Add(x.Count);
    }

    return greatest_common_divisor(sequences.ToArray());
  }

  private static Dictionary<string, node> get_nodes(string data) {
    // nodes are formatted as: BQV = (HFG, GDR)
    var nodes = new Dictionary<string, node>();
    foreach (var node in data.Split(Environment.NewLine).Skip(2)) {
      nodes.Add(node[..3], new node() {left = node[7..10], right = node[12..15]});
    }
    return nodes;
  }

  private static List<string> navigate_nodes (instructions ins, Dictionary<string, node> nodes, string start_node) {
    var path = new List<string>();
    var next = start_node;
    do {
      var direction = ins.next();
      next = direction == direction.left ? nodes[next].left : nodes[next].right;
      path.Add(next);
    } while (!next.EndsWith("Z"));
    return path;
  }

  private static long greatest_common_divisor(long num1, long num2) => 
    num2 == 0 ? num1 : greatest_common_divisor(num2, num1 % num2);
    
  private static long greatest_common_divisor(long[] numbers) => 
    numbers.Aggregate((S, val) => S * val / greatest_common_divisor(S, val));

  #region " data structures "
    private enum direction { left, right }

    private class node {
      public string left {get; set;} = "";
      public string right {get; set;} = "";
    }

    private class instructions {
      private char[] directions {get; set;}
      private int position {get; set;}

      public instructions(string value) {
        directions = value.ToCharArray();;
        position = 0;
      }

      public direction next() {
        // we have a list of instructions like LLRRLRRR and we need to loop through this and then repeat the pattern indefinitely
        if (position >= directions.Length) position = 0;
        var d = directions[position];
        position++;
        return d == 'L' ? direction.left : direction.right;
      } 
    }
  #endregion
}