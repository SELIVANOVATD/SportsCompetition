using System;

namespace SportsCompetition.Models
{
    public class SportsDoctor : IDoctor
    {
        public string Name { get; }

        public SportsDoctor(string name)
        {
            Name = name;
        }

        public void TreatAthlete(Athlete athlete)
        {
            Console.WriteLine($"{Name} лечит {athlete.Name}");
            athlete.Heal();
        }
    }
}