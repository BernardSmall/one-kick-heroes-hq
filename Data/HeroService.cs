using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace OneKickHeroesApp.Data
{
    /// <summary>
    /// Provides read/write access to superheroes stored in Data/superheroes.txt
    /// </summary>
    public class HeroService
    {
        private readonly string dataDirectory;
        private readonly string heroesPath;

        public HeroService(string baseDirectory = null)
        {
            dataDirectory = Path.Combine(baseDirectory ?? AppDomain.CurrentDomain.BaseDirectory, "Data");
            heroesPath = Path.Combine(dataDirectory, "superheroes.txt");
        }

        /// <summary>
        /// Reads all heroes from the data file. Returns empty list if file missing.
        /// </summary>
        public List<Hero> GetAllHeroes()
        {
            var result = new List<Hero>();
            if (!File.Exists(heroesPath)) return result;

            using (var reader = new StreamReader(heroesPath))
            {
                string line;
                bool isFirst = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirst)
                    {
                        // skip header line
                        isFirst = false;
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var hero = TryParseHero(line);
                    if (hero != null) result.Add(hero);
                }
            }

            return result;
        }

        private Hero TryParseHero(string line)
        {
            try
            {
                // Accept both "|" and spaced variants like "  |  "
                var parts = line.Split(new[] { '|' }, StringSplitOptions.None)
                                 .Select(p => (p ?? string.Empty).Trim())
                                 .ToArray();
                // Expected: Id|Name|Age|Power|Score|Rank|ThreatLevel
                if (parts.Length < 6) return null;

                int id; if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out id)) return null;
                string name = parts[1];
                int age; if (!int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out age)) return null;
                string power = parts[3];
                double score; if (!double.TryParse(parts[4], NumberStyles.Float, CultureInfo.InvariantCulture, out score)) return null;
                string rankStr = parts.Length >= 6 ? parts[5] : "";
                string threatStr = parts.Length >= 7 ? parts[6] : "";

                Rank rank;
                switch ((rankStr ?? string.Empty).Trim().ToUpperInvariant())
                {
                    case "S": rank = Rank.S; break;
                    case "A": rank = Rank.A; break;
                    case "B": rank = Rank.B; break;
                    default: rank = Rank.C; break;
                }

                ThreatLevel threat;
                switch ((threatStr ?? string.Empty).Trim())
                {
                    case nameof(ThreatLevel.FinalsWeek): threat = ThreatLevel.FinalsWeek; break;
                    case nameof(ThreatLevel.MidtermMadness): threat = ThreatLevel.MidtermMadness; break;
                    case nameof(ThreatLevel.GroupProjectGoneWrong): threat = ThreatLevel.GroupProjectGoneWrong; break;
                    default: threat = ThreatLevel.PopQuiz; break;
                }

                return new Hero(id, name, age, power, score, rank, threat);
            }
            catch
            {
                return null;
            }
        }
    }
}


