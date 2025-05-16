namespace SportsCompetition.Models
{
    public interface IDoctor
    {
        string Name { get; }
        void TreatAthlete(Athlete athlete);
    }
}