namespace advent_of_code_2023;
class day_02: _base {
    private static int max_red = 12, max_green = 13, max_blue = 14;
    class hand {public int blue, red, green;}
    class game {public List<hand> hands = new List<hand>();}

    public static void run(){        
        var games = get_data_live().Split("\n").Select((x, i) => new KeyValuePair<int,game> (i, parse_game(x))).ToList();        
        var valid_games = games.Where(
            x => x.Value.hands.All(
                h => h.blue <= max_blue && h.red <= max_red && h.green <= max_green));

        Console.WriteLine(nameof(day_02));

        var sum_index = valid_games.Sum(x => x.Key+1);
        Console.WriteLine($"\t1st part: \t{sum_index}");

        var powers = games.Select(x => get_power(get_max_hand(x.Value)));
        Console.WriteLine($"\t2nd part: \t{powers.Sum()}");
    }

    private static int get_power(hand hand) => 
        hand.blue * hand.red * hand.green;

    private static hand get_max_hand(game game) => 
        new hand() { 
            blue = game.hands.Max(x => x.blue),
            red = game.hands.Max(x => x.red),  
            green = game.hands.Max(x => x.green)
        };

    private static game parse_game(string s) {
        
        var hands = new List<hand>();
        var inputs = s.Split(";").Select(x => x.Split(","));

        foreach (var input in inputs) {
            var hand = new hand();
            
            foreach (var r in input.Select(x => x.Trim())) {
                var roll = r;
                if (roll.StartsWith("Game")) roll = roll[(roll.IndexOf(':') + 1)..];

                var dice = roll.Trim().Split(" ")[1].ToLower();
                var value = roll.Trim().Split(" ")[0];

                switch (dice) {
                    case "blue":
                        hand.blue = int.Parse(value);
                        break;
                    case "red":
                        hand.red = int.Parse(value);
                        break;
                    case "green":
                        hand.green = int.Parse(value);
                        break;
                    default:
                        throw new Exception($"unknown dice: {dice}");
                }
            }
            hands.Add(hand);
        }

        return new game() { hands = hands };
    }
}
