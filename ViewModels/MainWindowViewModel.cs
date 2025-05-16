using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportsCompetition.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SportsCompetition.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<CompetitionViewModel> _competitions = new();

        [ObservableProperty]
        private ObservableCollection<Athlete> _allAthletes = new();

        [ObservableProperty]
        private ObservableCollection<IDoctor> _doctors = new();

        [ObservableProperty]
        private ObservableCollection<string> _logs = new();

        [ObservableProperty]
        private string _newAthleteName = "Athlete";

        [ObservableProperty]
        private string _newCompetitionName = "Competition";

        [ObservableProperty]
        private string _newDoctorName = "Dr. Smith";

        public MainWindowViewModel()
        {
            // Initialize with some sample data
            AddDoctorCommand.Execute(null);
            AddAthleteCommand.Execute(null);
            AddCompetitionCommand.Execute(null);
        }


        [RelayCommand]
        private void AddAthlete()
        {
            var athlete = new Athlete($"{NewAthleteName} {AllAthletes.Count + 1}", new Random().Next(1, 10));
            athlete.Injured += (sender, e) => 
            {
                var injuredAthlete = (Athlete)sender!;
                var message = $"{injuredAthlete.Name} получил травму с тяжестью {e.Severity}!";
                Logs.Add(message);
                Console.WriteLine(message);

                // Находим доступного врача
                var doctor = Doctors.FirstOrDefault();
                if (doctor != null)
                {
                    var treatmentMsg = $"{doctor.Name} лечит {injuredAthlete.Name}";
                    Logs.Add(treatmentMsg);
                    Console.WriteLine(treatmentMsg);
                    
                    doctor.TreatAthlete(injuredAthlete);
                    
                    var healedMsg = $"{injuredAthlete.Name} вылечен и готов к соревнованиям!";
                    Logs.Add(healedMsg);
                    Console.WriteLine(healedMsg);
                }
            };

            athlete.WonCompetition += (sender, _) => 
            {
                var winningAthlete = (Athlete)sender!;
                var message = $"{winningAthlete.Name} победил в соревновании!";
                Logs.Add(message);
                Console.WriteLine(message);
            };

            AllAthletes.Add(athlete);
            Console.WriteLine($"Добавлен новый атлет: {athlete.Name}");
        }

        [RelayCommand]
        private void AddCompetition()
        {
            var competition = new Competition($"{NewCompetitionName} {Competitions.Count + 1}");
            var competitionVm = new CompetitionViewModel(competition);
            
            competition.AthleteWon += (sender, athlete) => 
            {
                
            };
            
            competition.CompetitionLog += (sender, message) => 
            {
                Logs.Add(message);
                Console.WriteLine(message);
            };
            
            Competitions.Add(competitionVm);
            Console.WriteLine($"Добавлено новое соревнование: {competition.Name}");
        }

        [RelayCommand]
        private void AddDoctor()
        {
            var doctor = new SportsDoctor($"{NewDoctorName} {Doctors.Count + 1}");
            Doctors.Add(doctor);
            Console.WriteLine($"Добавлен новый врач: {doctor.Name}");
        }

        [RelayCommand]
        private async Task StartCompetition(CompetitionViewModel competitionVm)
        {
            if (competitionVm == null || competitionVm.IsRunning) return;

            // Добавляем случайных атлетов в соревнование
            competitionVm.UpdateAthletes();
            var competition = competitionVm.Competition;

            competition.Athletes.Clear();
            var random = new Random();
            var athletes = AllAthletes
                .Where(a => !a.IsInjured)
                .OrderBy(_ => random.Next())
                .Take(3)
                .ToList();

            foreach (var athlete in athletes)
            {
                competition.Athletes.Add(athlete);
            }

            Console.WriteLine($"Соревнование '{competition.Name}' началось с участниками: " +
                string.Join(", ", competition.Athletes.Select(a => a.Name)));

            await competition.StartAsync();
            competitionVm.UpdateAthletes();
        }
       [RelayCommand]
        private void ClearLogs()
        {
            Logs.Clear();
            Console.WriteLine("Журнал событий очищен");
        }
    }
}