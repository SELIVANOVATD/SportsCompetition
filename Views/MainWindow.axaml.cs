using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using SportsCompetition.ViewModels;

namespace SportsCompetition.Views
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _updateTimer;

        public MainWindow()
        {
            InitializeComponent();
            
            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };
            _updateTimer.Tick += UpdatePositions;
            _updateTimer.Start();
        }

        private void UpdatePositions(object? sender, EventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                foreach (var competitionVm in vm.Competitions)
                {
                    foreach (var athleteVm in competitionVm.Athletes)
                    {
                        athleteVm.UpdatePosition();
                    }
                }
            }
        }

        private void StartCompetition_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && 
                button.DataContext is CompetitionViewModel competitionVm &&
                DataContext is MainWindowViewModel mainViewModel)
            {
                mainViewModel.StartCompetitionCommand.Execute(competitionVm);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _updateTimer.Stop();
            base.OnClosed(e);
        }
    }
}