using System;
using System.Globalization;

namespace OneKickHeroesApp.Data
{
    /// <summary>
    /// Rich hero model with validation, computed rank/threat, parsing and formatting helpers.
    /// </summary>
    public class Hero
    {
        // Backing fields to enforce invariants
        private int _id;
        private string _name;
        private int _age;
        private string _power;
        private double _score;

        public int Id
        {
            get { return _id; }
            private set
            {
                if (value < 1000 || value > 9999) throw new ArgumentException("Hero ID must be a 4-digit number (1000-9999).", nameof(Id));
                _id = value;
            }
        }

        public string Name
        {
            get { return _name; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Hero name cannot be empty.", nameof(Name));
                _name = value.Trim();
            }
        }

        public int Age
        {
            get { return _age; }
            private set
            {
                if (value < 10 || value > 100) throw new ArgumentException("Hero age must be between 10 and 100.", nameof(Age));
                _age = value;
            }
        }

        public string Power
        {
            get { return _power; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Hero power cannot be empty.", nameof(Power));
                _power = value.Trim();
            }
        }

        public double Score
        {
            get { return _score; }
            private set
            {
                if (value < 0 || value > 100) throw new ArgumentException("Hero exam score must be between 0 and 100.", nameof(Score));
                _score = value;
            }
        }

        /// <summary>
        /// Rank is derived from Score using 81/61/41/0 split.
        /// </summary>
        public Rank Rank { get { return CalculateRank(_score); } }

        /// <summary>
        /// Threat level derived from rank.
        /// </summary>
        public ThreatLevel Threat { get { return CalculateThreat(Rank); } }

        /// <summary>
        /// Preferred constructor: compute rank/threat from score.
        /// </summary>
        public Hero(int id, string name, int age, string power, double score)
        {
            Id = id; Name = name; Age = age; Power = power; Score = score;
        }

        /// <summary>
        /// Compatibility constructor: incoming rank/threat are ignored; Rank/Threat are computed from score.
        /// Kept to preserve call sites that still pass these values.
        /// </summary>
        public Hero(int id, string name, int age, string power, double score, Rank /*rank*/ _, ThreatLevel /*threat*/ __)
        {
            Id = id; Name = name; Age = age; Power = power; Score = score;
        }

        public Hero With(int? id = null, string name = null, int? age = null, string power = null, double? score = null)
        {
            return new Hero(id ?? Id, name ?? Name, age ?? Age, power ?? Power, score ?? Score);
        }

        public bool IsValid()
        {
            try
            {
                var _ = Id; var __ = Name; var ___ = Age; var ____ = Power; var _____ = Score; // accessors validate
                return true;
            }
            catch { return false; }
        }

        public override string ToString()
        {
            // Pipe-separated format used by storage
            return string.Format(CultureInfo.InvariantCulture,
                "{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                Id, Name, Age, Power, Score, RankToCode(Rank), Threat.ToString());
        }

        public static Hero FromString(string dataLine)
        {
            if (string.IsNullOrWhiteSpace(dataLine)) throw new ArgumentException("Data line cannot be empty.", nameof(dataLine));
            var parts = dataLine.Split('|');
            if (parts.Length < 5) throw new ArgumentException("Invalid data format.", nameof(dataLine));

            int id; if (!int.TryParse(parts[0].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out id)) throw new ArgumentException("Invalid ID.");
            string name = (parts[1] ?? "").Trim();
            int age; if (!int.TryParse(parts[2].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out age)) throw new ArgumentException("Invalid age.");
            string power = (parts[3] ?? "").Trim();
            double score; if (!double.TryParse(parts[4].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out score)) throw new ArgumentException("Invalid score.");

            // Construct from validated fields; rank/threat computed
            return new Hero(id, name, age, power, score);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Hero; if (other == null) return false;
            return Id == other.Id && Name == other.Name && Age == other.Age && Power == other.Power && Math.Abs(Score - other.Score) < 0.000001;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + (Name == null ? 0 : Name.GetHashCode());
                hash = hash * 23 + Age.GetHashCode();
                hash = hash * 23 + (Power == null ? 0 : Power.GetHashCode());
                hash = hash * 23 + Score.GetHashCode();
                return hash;
            }
        }

        private static Rank CalculateRank(double s)
        {
            if (s >= 81) return Rank.S;
            if (s >= 61) return Rank.A;
            if (s >= 41) return Rank.B;
            return Rank.C;
        }

        private static ThreatLevel CalculateThreat(Rank r)
        {
            switch (r)
            {
                case Rank.S: return ThreatLevel.FinalsWeek;
                case Rank.A: return ThreatLevel.MidtermMadness;
                case Rank.B: return ThreatLevel.GroupProjectGoneWrong;
                default: return ThreatLevel.PopQuiz;
            }
        }

        private static string RankToCode(Rank r)
        {
            switch (r)
            {
                case Rank.S: return "S";
                case Rank.A: return "A";
                case Rank.B: return "B";
                default: return "C";
            }
        }
    }
}


