using System.Diagnostics;

namespace advent_of_code_2023;
class day_05: _base {  
  public static void run () 
  {  
    var data = get_data_live();
    var first = calc_part_1(data);
    var second = calc_part_2(data);

    Console.WriteLine($"{nameof(day_05)}\n\t1st part:\t{first}\n\t2nd part:\t{second}");
  }

  public static long calc_part_1(string data) {
    var almanac = new almanac(data, is_part_2: false);
    return almanac.seeds.Min(x => almanac.get_location(x.start));
  }

  public static long calc_part_2(string data) {
    var almanac = new almanac(data, is_part_2: true);
    var winning_seed = default(long);; 
    var winning_location = default(long);

    var n = 0;
    do {
      var tmp = almanac.get_seed(n);
      if (almanac.seeds.Any(x => tmp >= x.start && tmp <= x.end)) {
        winning_seed = tmp;
        winning_location = n;
        break;
      }
      n++;
    } while (true);

    return winning_location;
  }

  private class almanac {
    public seed[] seeds { get; set; }
    public map seed_to_soil { get; set; }
    public map soil_to_fertilizer { get; set; }
    public map fertilizer_to_water { get; set; }
    public map water_to_light { get; set; }
    public map light_to_temperature { get; set; }
    public map temperature_to_humidity { get; set; }
    public map humidity_to_location { get; set; }

    public almanac(string data, bool is_part_2) {
      Debug.Assert(data.StartsWith("seeds: "));
      var lines = data.Split(Environment.NewLine);

      // read the first line that looks like this:
      // seeds: 79 14 55 13
      seeds = get_seeds(lines[0], is_part_2);

      // read each of the map sections that look like this: 20 30 4
      // where 20 is the destination start, 30 is the source start, and 4 is the range length
      seed_to_soil = process_map_section("seed-to-soil map", lines);
      soil_to_fertilizer = process_map_section("soil-to-fertilizer", lines);
      fertilizer_to_water = process_map_section("fertilizer-to-water", lines);
      water_to_light = process_map_section("water-to-light", lines);
      light_to_temperature = process_map_section("light-to-temperature", lines);
      temperature_to_humidity = process_map_section("temperature-to-humidity", lines);
      humidity_to_location = process_map_section("humidity-to-location", lines);      
    }

    // use the maps to go all the way from a seed to a location
    public long get_location(long seed) {
      return humidity_to_location
        .get_value(temperature_to_humidity
        .get_value(light_to_temperature
        .get_value(water_to_light
        .get_value(fertilizer_to_water
        .get_value(soil_to_fertilizer
        .get_value(seed_to_soil
        .get_value(seed)))))));
    }

    // use the maps to go all the way from a location back to a seed
    public long get_seed(long location) {
      return seed_to_soil
        .get_key(soil_to_fertilizer
        .get_key(fertilizer_to_water
        .get_key(water_to_light
        .get_key(light_to_temperature
        .get_key(temperature_to_humidity
        .get_key(humidity_to_location
        .get_key(location)))))));
    }
  }

  // seed section is formatted as: seeds: 79 14 55 13
  // in v1, this is 4 separate seeds
  // in v2, this is 2 seeds, each with a range (e.g. 79 14 55 13 becomes 79-92 55-67)
  private static seed[] get_seeds(string line, bool is_part_2) {
    var raw = line.Split(" ").Skip(1).Select(long.Parse).ToArray(); 
    if (!is_part_2) {
      return raw.Select(x => new seed() { start = x, end = x }).ToArray();
    }

    var final = new List<seed>();
    for (long i = 0; i < raw.Length; i+=2) {
      final.Add(new seed() { start = raw[i], end = raw[i] + raw[i+1] - 1 });
    }
    return final.ToArray();
  }

  // map section is formatted as:
  // seed-to-soil map
  // 20 30 4
  // 24 34 2
  private static map process_map_section(string section, string[] lines) {
    var map = new map();
    var section_found = false;
    for (int i = 0; i < lines.Length; i++) {
      if (lines[i].StartsWith(section)) {
        section_found = true;
        i += 1;
        while (i < lines.Length && lines[i].Length > 0 && char.IsDigit(lines[i][0])) {
          map.add(new range(lines[i]));
          i += 1;
        }
        break;
      }
      if (section_found) break;
    }
    if (!section_found) throw new Exception($"section {section} not found");
    return map;
  }

  private class seed {
    public long start { get; set; }
    public long end { get; set; }  
  }

  private class range {
    public long destination_start { get; set; }
    public long source_start { get; set; }
    public long range_length { get; set; }

    // data should be in format of: 20 30 4
    // where 20 is the destination start, 30 is the source start, and 4 is the range length
    public range(string data) {
      var parts = data.Split(' ').Select(long.Parse).ToArray();
      destination_start = parts[0];
      source_start = parts[1];  
      range_length = parts[2];
    }
  }

  private class map {
    private List<range> ranges {get; set;}

    public map() {
      ranges = new List<range>();
    }

    public void add(range r) {
      ranges.Add(r);
    }

    public long get_key(long value) {
      // if the value is in the range of one of the ranges, then return the mapped key
      foreach (var r in ranges) {
        if (value >= r.destination_start && value < r.destination_start + r.range_length) {
          return r.source_start + (value - r.destination_start);
        }
      }
      // wasn't found, so it is not mapped, and just return the value
      return value;
    }

    public long get_value(long key) {
      // if the key is in the range of one of the ranges, then return the mapped value
      foreach (var r in ranges) {
        if (key >= r.source_start && key < r.source_start + r.range_length) {
          return r.destination_start + (key - r.source_start);
        }
      }
      // wasn't found, so it is not mapped, and just return the key
      return key;
    }
  }
}