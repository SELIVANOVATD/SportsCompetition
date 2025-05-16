using CommunityToolkit.Mvvm.ComponentModel;
using SportsCompetition.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SportsCompetition.ViewModels
{
    public partial class CompetitionViewModel : ViewModelBase
    {
        public Competition Competition { get; }
        
        public string Name => Competition.Name;
        
        [ObservableProperty]
        private bool _isRunning;
        
        [ObservableProperty]
        private string _winner;
        
        public ObservableCollection<AthleteViewModel> Athletes { get; } = new();

        public CompetitionViewModel(Competition competition)
        {
            Competition = competition;
            _isRunning = competition.IsRunning;
            _winner = competition.Winner?.Name ?? "No winner yet";
            UpdateAthletes();
            
            competition.CompetitionLog += (_, _) => 
            {
                IsRunning = Competition.IsRunning;
                Winner = Competition.Winner?.Name ?? "No winner yet";
            };
        }

        public void UpdateAthletes()
        {
            // Сохраняем текущие позиции
            var currentPositions = Athletes.ToDictionary(
                avm => avm.Name,
                avm => (avm.PositionX, avm.PositionY));

            Athletes.Clear();
            
            foreach (var athlete in Competition.Athletes)
            {
                var athleteVm = new AthleteViewModel(athlete);
                
                // Восстанавливаем позиции, если атлет уже был в коллекции
                if (currentPositions.TryGetValue(athleteVm.Name, out var position))
                {
                    athleteVm.PositionX = position.PositionX;
                    athleteVm.PositionY = position.PositionY;
                }
                
                Athletes.Add(athleteVm);
            }
        }
    }
}