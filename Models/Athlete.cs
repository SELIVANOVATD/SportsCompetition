using System;
using System.Threading;
using System.Threading.Tasks;

namespace SportsCompetition.Models
{
    public class Athlete
    {
        public event EventHandler<InjuryEventArgs>? Injured;
        public event EventHandler? WonCompetition;
        
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public int SkillLevel { get; }
        public int Stamina { get; private set; } = 100;
        public bool IsInjured { get; private set; }
        public bool IsCompeting { get; private set; }
        public int Wins { get; private set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }

        private readonly Random _random = new();
        private CancellationTokenSource? _competitionCts;

        public Athlete(string name, int skillLevel)
        {
            Name = name;
            SkillLevel = skillLevel;
        }

public async Task CompeteAsync(CancellationToken cancellationToken)
{
    if (IsCompeting || IsInjured) return;

    IsCompeting = true;
    _competitionCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    try
    {
        while (!_competitionCts.Token.IsCancellationRequested)
        {
            await Task.Delay(100, _competitionCts.Token);
            
            // Логика движения и проверки травм
            if (_random.Next(100) < 5)
            {
                var severity = _random.Next(1, 11);
                OnInjured(new InjuryEventArgs(severity));
                break;
            }

            Stamina = Math.Max(0, Stamina - _random.Next(1, 4));
            
            if (Stamina > 50 && _random.Next(100) < SkillLevel)
            {
                Wins++;
                OnWonCompetition();
                break;
            }
        }
    }
    catch (TaskCanceledException)
    {
        // Соревнование было отменено
    }
    finally
    {
        IsCompeting = false;
        _competitionCts?.Dispose();
        _competitionCts = null;
    }
}

        public void StopCompeting()
        {
            _competitionCts?.Cancel();
        }

        public void Heal()
        {
            IsInjured = false;
            Stamina = 100;
        }

        protected virtual void OnInjured(InjuryEventArgs e)
        {
            IsInjured = true;
            Injured?.Invoke(this, e);
        }

        protected virtual void OnWonCompetition()
        {
            Wins++;
            WonCompetition?.Invoke(this, EventArgs.Empty);
        }
    }

    public class InjuryEventArgs : EventArgs
    {
        public int Severity { get; }

        public InjuryEventArgs(int severity)
        {
            Severity = severity;
        }
    }
}