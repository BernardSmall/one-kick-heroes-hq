using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneKickHeroesApp.Domain
{
    public static class RankRules
    {
        // S: 81–100, A: 61–80, B: 41–60, C: 0–40
        public static Rank FromScore(double s)
        {
            if (s >= 81) return Rank.S;
            if (s >= 61) return Rank.A;
            if (s >= 41) return Rank.B;
            return Rank.C;
        }

        public static ThreatLevel ThreatFor(Rank r)
        {
            switch (r)
            {
                case Rank.S: return ThreatLevel.FinalsWeek;
                case Rank.A: return ThreatLevel.MidtermMadness;
                case Rank.B: return ThreatLevel.GroupProjectGoneWrong;
                default: return ThreatLevel.PopQuiz;
            }
        }
    }
}
