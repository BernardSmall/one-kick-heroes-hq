using System;

namespace OneKickHeroesApp.Data
{
    public class Hero
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public string Power { get; }
        public double Score { get; }
        public Rank Rank { get; }
        public ThreatLevel Threat { get; }

        public Hero(int id, string name, int age, string power, double score, Rank rank, ThreatLevel threat)
        {
            Id = id; Name = name; Age = age; Power = power; Score = score; Rank = rank; Threat = threat;
        }

        public Hero With(int? id = null, string name = null, int? age = null, string power = null, double? score = null, Rank? rank = null, ThreatLevel? threat = null)
            => new Hero(id ?? Id, name ?? Name, age ?? Age, power ?? Power, score ?? Score, rank ?? Rank, threat ?? Threat);
    }
}


