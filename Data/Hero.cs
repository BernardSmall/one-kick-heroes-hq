using System;
using System.Globalization;

namespace OneKickHeroesApp.Data
{
    /// <summary>
    /// Represents a superhero with all their attributes and calculated properties
    /// </summary>
    public class Hero
    {
        #region Private Fields
        private int _id;
        private string _name;
        private int _age;
        private string _power;
        private double _score;
        #endregion

        #region Properties
        /// <summary>
        /// Unique identifier for the hero (4-digit integer)
        /// </summary>
        public int Id
        {
            get => _id;
            private set
            {
                if (value < 1000 || value > 9999)
                    throw new ArgumentException("Hero ID must be a 4-digit number (1000-9999).", nameof(Id));
                _id = value;
            }
        }

        /// <summary>
        /// Name of the superhero
        /// </summary>
        public string Name
        {
            get => _name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Hero name cannot be empty.", nameof(Name));
                _name = value.Trim();
            }
        }

        /// <summary>
        /// Age of the superhero (10-100)
        /// </summary>
        public int Age
        {
            get => _age;
            private set
            {
                if (value < 10 || value > 100)
                    throw new ArgumentException("Hero age must be between 10 and 100.", nameof(Age));
                _age = value;
            }
        }

        /// <summary>
        /// Superpower of the hero
        /// </summary>
        public string Power
        {
            get => _power;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Hero power cannot be empty.", nameof(Power));
                _power = value.Trim();
            }
        }

        /// <summary>
        /// Hero exam score (0-100)
        /// </summary>
        public double Score
        {
            get => _score;
            private set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Hero exam score must be between 0 and 100.", nameof(Score));
                _score = value;
            }
        }

        /// <summary>
        /// Calculated rank based on exam score
        /// </summary>
        public string Rank => CalculateRank();

        /// <summary>
        /// Calculated threat level based on exam score
        /// </summary>
        public string ThreatLevel => CalculateThreatLevel();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Hero class
        /// </summary>
        /// <param name="id">Unique hero identifier</param>
        /// <param name="name">Hero name</param>
        /// <param name="age">Hero age (10-100)</param>
        /// <param name="power">Hero superpower</param>
        /// <param name="score">Hero exam score (0-100)</param>
        public Hero(int id, string name, int age, string power, double score)
        {
            Id = id;
            Name = name;
            Age = age;
            Power = power;
            Score = score;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates a Hero object from a pipe-separated string
        /// </summary>
        /// <param name="dataLine">Pipe-separated data line</param>
        /// <returns>Hero object</returns>
        public static Hero FromString(string dataLine)
        {
            if (string.IsNullOrWhiteSpace(dataLine))
                throw new ArgumentException("Data line cannot be empty.", nameof(dataLine));

            var parts = dataLine.Split('|');
            // Accept 6 or 7 columns: Id|Name|Age|Power|Score|Rank|[ThreatLevel]
            if (parts.Length < 6)
                throw new ArgumentException("Invalid data format.", nameof(dataLine));

            // Parse required fields with trimming
            int id; if (!int.TryParse((parts[0] ?? "").Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out id))
                throw new ArgumentException("Invalid Hero ID format.", nameof(dataLine));

            string name = (parts[1] ?? "").Trim();

            int age; if (!int.TryParse((parts[2] ?? "").Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out age))
                throw new ArgumentException("Invalid age format.", nameof(dataLine));

            string power = (parts[3] ?? "").Trim();

            // Score may use comma or dot; normalize to dot, try invariant then current culture
            string rawScore = (parts[4] ?? "").Trim();
            string normalized = rawScore.Replace(',', '.');
            double score;
            if (!double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out score))
            {
                if (!double.TryParse(rawScore, NumberStyles.Float, CultureInfo.CurrentCulture, out score))
                    throw new ArgumentException("Invalid score format.", nameof(dataLine));
            }

            return new Hero(id, name, age, power, score);
        }

        /// <summary>
        /// Converts the Hero object to a pipe-separated string
        /// </summary>
        /// <returns>Pipe-separated string representation</returns>
        public override string ToString()
        {
            return $"{Id}  |  {Name}  |  {Age}  |  {Power}  |  {Score:F1}  |  {Rank}  |  {ThreatLevel}";
        }

        /// <summary>
        /// Creates a copy of the Hero with updated values
        /// </summary>
        /// <param name="id">New ID (optional)</param>
        /// <param name="name">New name (optional)</param>
        /// <param name="age">New age (optional)</param>
        /// <param name="power">New power (optional)</param>
        /// <param name="score">New score (optional)</param>
        /// <returns>New Hero instance with updated values</returns>
        public Hero With(int? id = null, string name = null, int? age = null, string power = null, double? score = null)
        {
            return new Hero(
                id ?? Id,
                name ?? Name,
                age ?? Age,
                power ?? Power,
                score ?? Score
            );
        }

        /// <summary>
        /// Validates if the hero data is complete and valid
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValid()
        {
            try
            {
                // This will throw exceptions if any property is invalid
                var _ = Id;
                var __ = Name;
                var ___ = Age;
                var ____ = Power;
                var _____ = Score;
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Calculates the rank based on exam score
        /// </summary>
        /// <returns>Rank string (S, A, B, C)</returns>
        private string CalculateRank()
        {
            switch ((int)_score)
            {
                case int n when n >= 81:
                    return "S";
                case int n when n >= 61:
                    return "A";
                case int n when n >= 41:
                    return "B";
                case int n when n >= 0:
                    return "C";
                default:
                    throw new ArgumentException("Invalid score for rank calculation.");
            }
        }

        /// <summary>
        /// Calculates the threat level based on exam score
        /// </summary>
        /// <returns>Threat level string</returns>
        private string CalculateThreatLevel()
        {
            switch ((int)_score)
            {
                case int n when n >= 81:
                    return "FinalsWeek";
                case int n when n >= 61:
                    return "MidtermMadness";
                case int n when n >= 41:
                    return "GroupProjectGoneWrong";
                case int n when n >= 0:
                    return "PopQuiz";
                default:
                    throw new ArgumentException("Invalid score for threat level calculation.");
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Determines whether the specified object is equal to the current Hero
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj is Hero other)
            {
                return Id == other.Id && 
                       Name == other.Name && 
                       Age == other.Age && 
                       Power == other.Power && 
                       Math.Abs(Score - other.Score) < 0.001;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for the Hero
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + (Name?.GetHashCode() ?? 0);
                hash = hash * 23 + Age.GetHashCode();
                hash = hash * 23 + (Power?.GetHashCode() ?? 0);
                hash = hash * 23 + Score.GetHashCode();
                return hash;
            }
        }
        #endregion
    }
}
