using System.Diagnostics;

namespace advent_of_code_2023;
class day_07: _base {  
  public static void run () {  
    var data = get_data_live();

    run_tests(data);
    var first = calc(data, is_v2:false);
    var second = calc(data, is_v2:true);
    Console.WriteLine($"{nameof(day_07)}\n\t1st part:\t{first}\n\t2nd part:\t{second}");
  }

  // test hand ranking
  public static void run_tests(string data) {    
    Debug.Assert(get_rank("AAAAA") == rank_type.five_of_a_kind);
    Debug.Assert(get_rank("QQQQA") == rank_type.four_of_a_kind);
    Debug.Assert(get_rank("TT555") == rank_type.full_house);
    Debug.Assert(get_rank("333TK") == rank_type.three_of_a_kind);
    Debug.Assert(get_rank("KKJJT") == rank_type.two_pair);
    Debug.Assert(get_rank("KK654") == rank_type.one_pair);
    Debug.Assert(get_rank("98765") == rank_type.high_card);

    Debug.Assert(new hand("J2345", int.MinValue, is_v2:false).rank == rank_type.high_card);
    Debug.Assert(new hand("JJ234", int.MinValue, is_v2:false).rank == rank_type.one_pair);
    Debug.Assert(new hand("JJ224", int.MinValue, is_v2:false).rank == rank_type.two_pair);
    Debug.Assert(new hand("J3222", int.MinValue, is_v2:false).rank == rank_type.three_of_a_kind);
    Debug.Assert(new hand("JJ222", int.MinValue, is_v2:false).rank == rank_type.full_house);
    Debug.Assert(new hand("J2222", int.MinValue, is_v2:false).rank == rank_type.four_of_a_kind);
    Debug.Assert(new hand("22222", int.MinValue, is_v2:false).rank == rank_type.five_of_a_kind);

    Debug.Assert(new hand("K2345", int.MinValue, is_v2:true).rank == rank_type.high_card);
    Debug.Assert(new hand("KK234", int.MinValue, is_v2:true).rank == rank_type.one_pair);
    Debug.Assert(new hand("KK224", int.MinValue, is_v2:true).rank == rank_type.two_pair);
    Debug.Assert(new hand("K3222", int.MinValue, is_v2:true).rank == rank_type.three_of_a_kind);
    Debug.Assert(new hand("KK222", int.MinValue, is_v2:true).rank == rank_type.full_house);
    Debug.Assert(new hand("K2222", int.MinValue, is_v2:true).rank == rank_type.four_of_a_kind);
    Debug.Assert(new hand("22222", int.MinValue, is_v2:true).rank == rank_type.five_of_a_kind);

    Debug.Assert(new hand("23456", int.MinValue, is_v2:true).rank == rank_type.high_card);
    Debug.Assert(new hand("J2345", int.MinValue, is_v2:true).rank == rank_type.one_pair);
    Debug.Assert(new hand("J4545", int.MinValue, is_v2:true).rank == rank_type.full_house);
    Debug.Assert(new hand("J2355", int.MinValue, is_v2:true).rank == rank_type.three_of_a_kind);
    Debug.Assert(new hand("J2555", int.MinValue, is_v2:true).rank == rank_type.four_of_a_kind);
    Debug.Assert(new hand("J5555", int.MinValue, is_v2:true).rank == rank_type.five_of_a_kind);
  }

  private static long calc(string data, bool is_v2) {
    var hands = get_values(data, is_v2);
    hands.Sort();

    var total = (long) 0;
    for (int i = 0; i < hands.Count; i++) {
      total += (i+1) * hands[i].bid;
    }
    return total;
  }

  private static int card_value(hand hand, int card_index, bool is_v2) {
    char card = !is_v2 ? 
      hand.cards[card_index]: 
      hand.cards_with_wild_devalued[card_index];
    
    return card switch {
      'T' => 10, 'J' => 11, 'Q' => 12,
      'K' => 13, 'A' => 14, _ => int.Parse(card.ToString())};  
  }

  private static List<hand> get_values(string data, bool is_v2) =>
    new List<hand>(data
      .Split("\n")
      .Select(x => x.Split(" "))
      .Select(x => new hand(x[0], int.Parse(x[1]), is_v2)));
  
  public class hand: IComparable<hand> {
    public string cards { get; }
    public string cards_after_wild { get; }
    public string cards_with_wild_devalued { get; }
    public int bid { get; }
    private bool v2 { get; }
    public rank_type rank { get => get_rank(cards_after_wild); }
    public hand(string c, int b, bool is_v2) {
      cards = c;
      bid = b;
      cards_after_wild = is_v2 ? apply_wild(cards) : cards;
      cards_with_wild_devalued = is_v2 ? apply_wild_devalues(cards) : cards;
      v2 = is_v2;
    }

    public override string ToString() => $"{cards} {cards_after_wild} {bid} {rank} {v2}";
    private string apply_wild_devalues(string h) => h.Replace('J', '1');

    // JBBCD
    private string apply_wild(string h) {
      if (h.Equals("JJJJJ")) h = "KKKKK";
      var wild_count = h.Where(x => x.Equals('J')).Count();
      if (wild_count > 0) {
        var most_significant = h
          .GroupBy(c => c)
          .Where(x => !x.Key.Equals('J'))
          .ToDictionary(g => g.Key, g => g.Count())
          .OrderByDescending(x => x.Value)
          .First();
        
          h = h.Replace('J', most_significant.Key);
        }
      return h;
    }

    private int Compare(hand? a, hand? b) {
      if (a == null || b == null) return 0;
      if (a.rank > b.rank) {
        return 1;
      } else if (b.rank > a.rank) {
        return -1;
      } else {
        for (int i = 0; i < a.cards.Length; i++) {
          if (a.cards[i] == b.cards[i]) {
            continue;
          } else if (card_value(a, i, v2) > card_value(b, i, v2)) {
            return 1;
          } else {
            return -1;
          }
        }
      }
      return 0;
    }

    int IComparable<hand>.CompareTo(hand? other) {
      return Compare(this, other);            
    }
  }

  public enum rank_type {
    high_card, one_pair, two_pair, three_of_a_kind, full_house, four_of_a_kind, five_of_a_kind
  }

  // five of a kind, four of a kind, full house, three of a kind, two pair, one pair, high card
  public static rank_type get_rank(string hand) {
    var cards = hand.ToCharArray();
    Debug.Assert(cards.Length == 5, "hand should have 5 cards");
    var groups = cards.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
  
    if (groups.Count() == 1) {
      return rank_type.five_of_a_kind;
    } else if (groups.ContainsValue(4)) {
      return rank_type.four_of_a_kind;
    } else if (groups.ContainsValue(3) && groups.ContainsValue(2)) {
      return rank_type.full_house;
    } else if (groups.ContainsValue(3)) {
      return rank_type.three_of_a_kind;
    } else if (groups.Count(x => x.Value.Equals(2)) == 2) {
      return rank_type.two_pair;
    } else if (groups.ContainsValue(2)) {
      return rank_type.one_pair;
    } else {
      return rank_type.high_card;
    }
  }
}