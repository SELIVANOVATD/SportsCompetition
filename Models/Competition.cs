using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SportsCompetition.Models
{
    public class Competition
    {
        public event EventHandler<Athlete>? AthleteWon;
        public event EventHandler<string>? CompetitionLog;
        
        public string Name { get; }
        public ObservableCollection<Athlete> Athletes { get; } = new();
        public bool IsRunning { get; private set; }
        public Athlete? Winner { get; private set; }

        public Competition(string name)
        {
            Name = name;
        }

public async Task StartAsync()
{
    if (IsRunning || Athletes.Count == 0) return;

    IsRunning = true;
    Winner = null;
    OnCompetitionLog("Соревнование началось");

    var cts = new System.Threading.CancellationTokenSource();
    var tasks = Athletes.Select(athlete => athlete.CompeteAsync(cts.Token)).ToList();

    try
    {
        while (tasks.Count > 0 && Winner == null)
        {
            var completedTask = await Task.WhenAny(tasks);
            tasks.Remove(completedTask);

            Winner = Athletes.FirstOrDefault(a => a.Wins > 0);
            
            if (Winner != null)
            {
                cts.Cancel();
                var winMsg = $"{Winner.Name} побеждает в соревновании '{Name}'! (Навык: {Winner.SkillLevel}, Выносливость: {Winner.Stamina})";
                OnAthleteWon(Winner);
                OnCompetitionLog(winMsg);
                Console.WriteLine(winMsg);
            }
        }

        if (Winner == null)
        {
            var endMsg = $"Соревнование '{Name}' завершено без победителя. Все участники получили травмы.";
            OnCompetitionLog(endMsg);
            Console.WriteLine(endMsg);
        }
    }
    finally
    {
        
    IsRunning = false;
    OnCompetitionLog("Соревнование завершено");
    }
}

        protected virtual void OnAthleteWon(Athlete athlete)
        {
            AthleteWon?.Invoke(this, athlete);
        }

        protected virtual void OnCompetitionLog(string message)
        {
            CompetitionLog?.Invoke(this, message);
        }
    }
}