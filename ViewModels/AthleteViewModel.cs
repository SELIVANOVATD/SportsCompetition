using CommunityToolkit.Mvvm.ComponentModel;
using SportsCompetition.Models;

namespace SportsCompetition.ViewModels
{
    public partial class AthleteViewModel : ViewModelBase
    {
        private readonly Athlete _athlete;

        public string Name => _athlete.Name;
        public int SkillLevel => _athlete.SkillLevel;
        public int Stamina => _athlete.Stamina;
        public bool IsInjured => _athlete.IsInjured;
        public bool IsCompeting => _athlete.IsCompeting;
        public int Wins => _athlete.Wins;
        
        [ObservableProperty]
        private double _positionX;
        
        [ObservableProperty]
        private double _positionY;

        public AthleteViewModel(Athlete athlete)
        {
            _athlete = athlete;
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            PositionX = _athlete.PositionX;
            PositionY = _athlete.PositionY;
        }
    }
}