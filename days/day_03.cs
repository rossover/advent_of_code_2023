using System.Data;

namespace advent_of_code_2023;
class day_03: _base {
    public static void run() {

        var sum1 = calc_part_1();
        Console.WriteLine(nameof(day_03));
        Console.WriteLine($"\t1st part: \t{sum1}");

        var sum2 = calc_part_2();
        Console.WriteLine($"\t2nd part: \t{sum2}");
    }

    private static int calc_part_1() {
        var data = get_data_live().Split(Environment.NewLine).Select(x => x.ToArray()).ToArray();
        var total = new List<int>();

        for (int l = 0; l < data.Length; l++) {
            var line = data[l];
            var num = "";
            var adj = new List<char>();

            for (int c = 0; c < line.Length; c++) {
                var d = line[c];
                if (Char.IsNumber(d)) {
                    num += d;
                    adj.AddRange(get_adj_cells(data, l, c));
                    if (c < line.Length-1 && Char.IsNumber(line[c+1])) continue;

                    if (adj.Any(is_symbol)) total.Add(Int32.Parse(num));
                    adj.Clear();
                    num = "";
                }
            }
        }

        return total.Sum();
    }
    
    private static int calc_part_2() {
        var raw = get_data_live();
        var data = raw.Split(Environment.NewLine).Select(x => x.ToArray()).ToArray();
        var total = new List<int>();

        for (int row = 0; row < data.Length; row++) {
            var line = data[row];
            for (int col = 0; col < line.Length; col++) {
                var d = line[col];
                if (d.Equals('*')) {
                    var numbers = new List<int>();
                    numbers.AddRange(get_adj_full_numbers_offset_row(data, line:row, col:col, offset:-1));
                    numbers.AddRange(get_adj_full_numbers_offset_row(data, line:row, col:col, offset:0));
                    numbers.AddRange(get_adj_full_numbers_offset_row(data, line:row, col:col, offset:1));

                    // if there was exactly 2 adjacent numbers, we have a winner
                    if (numbers.Count == 2) {
                        total.Add(numbers[0] * numbers[1]);
                    }
                }
            }
        }
        return total.Sum();
    }

    private static bool is_symbol(char c) => !Char.IsDigit(c) && !c.Equals('.');

    private static bool are_cells_touching(char[][] data, int rowA, int colA, int rowB, int colB) {
        // if rows are too far away (not adjacent) they are not touching
        if (Math.Abs(rowA - rowB) > 1) return false;

        // we know they are on the same row or within one row of each other
        // so if they are in the same column or within one column of each other they are touching
        // (with bounds checking for beginning and end of row)
        return colA == colB || (colA > 0 && colB == colA-1) || (colA < data[rowA].Length && colB == colA+1);
    }

    private static List<int> get_adj_full_numbers_offset_row(char[][] data, int line, int col, int offset) {
        // offset should be -1 for previous row, 1 for next row

        // if this is the first or last row return empty list
        if (line + offset < 0) return new List<int>();
        if (line + offset > data.Length - 1) return new List<int>();

        var adj = new List<int>();
        var row = data[line + offset];
        var num = "";
        var touching = false;
        
        for (int i = 0; i < row.Length; i++) {
            // if we did find a number, save it to the number string and continue
            // also check to see if it is touching the asterisk
            if (Char.IsNumber(row[i])) {
                num += row[i];
                if (!touching) touching = are_cells_touching(data, line, col, line + offset, i);
            }
            if (!Char.IsNumber(row[i]) || i == row.Length-1) {
                // if we didn't find a number cell, save our existing number if it is touching
                if (num.Length > 0 && touching) adj.Add(Int32.Parse(num));
                num = "";
                touching = false;
            }
        }   
        return adj;
    }

    private static List<char> get_adj_cells(char[][] data, int line, int col) {
        var adj = new List<char>();

        // line above
        if (line > 0) {
            if (col > 0) adj.Add(data[line-1][col-1]);
            adj.Add(data[line-1][col]);
            if (col < data[line-1].Length-1) adj.Add(data[line-1][col+1]);
        }

        // same line
        if (col > 0) adj.Add(data[line][col-1]);
        if (col < data[line].Length-1) adj.Add(data[line][col+1]);

        // line below
        if (line < data.Length-1) {
            if (col > 0) adj.Add(data[line+1][col-1]);
            adj.Add(data[line+1][col]);
            if (col < data[line+1].Length-1) adj.Add(data[line+1][col+1]);
        }

        return adj;
    }
}