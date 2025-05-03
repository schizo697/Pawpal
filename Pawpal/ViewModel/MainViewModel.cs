using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Diagnostics;
using Pawpal.Models;
using Pawpal.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Pawpal.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {

        private readonly IFeedingTimeService _feedingTimeService;
        private readonly NotifyService _notifyService;
        private bool _isAlertShown;
        private IDispatcherTimer _alertTimer;
        public MainViewModel(IFeedingTimeService feedingTimeService, NotifyService notifyService)
        {
            _feedingTimeService = feedingTimeService;
            _notifyService = notifyService;
            Items = new ObservableCollection<string>();
            LoadFeedingTimesCommand.ExecuteAsync(null);
            StartAlertMonitoring();
        }
        [ObservableProperty]
        ObservableCollection<string> items;

        [ObservableProperty]
        TimeSpan selectedTime;

        [ObservableProperty]
        private string _statusText = "Monitoring...";

        [ObservableProperty]
        private Color _statusColor = Colors.Gray;

        [RelayCommand]
        async Task LoadFeedingTimes()
        {
            var feedingTime = await _feedingTimeService.GetAllFeedingTimeAsync();
            Items.Clear();
            foreach (var item in feedingTime)
            {
                Items.Add(item.FeedingTime);
            }
        }

        private void StartAlertMonitoring()
        {
            _alertTimer = Application.Current.Dispatcher.CreateTimer();
            _alertTimer.Interval = TimeSpan.FromSeconds(5);
            _alertTimer.Tick += (s, e) =>
            {
                Task.Run(async () => await CheckAlertStatus());
            };
            _alertTimer.Start();
        }

        private async Task CheckAlertStatus()
        {
            try
            {
                var status = await _notifyService.GetAlertStatusAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (status == "LOW_FOOD")
                    {
                        StatusText = "LOW FOOD! Refill needed!";
                        StatusColor = Colors.Red;
                        _ = ShowToast("Food is low! Refill needed!");
                    }
                    else
                    {
                        StatusText = "Monitoring...";
                        StatusColor = Colors.Gray;
                    }
                });
            }
            catch (Exception ex)
            {
                var toast = Toast.Make($"Alert check failed: {ex.Message}", CommunityToolkit.Maui.Core.ToastDuration.Short, 16);
                await toast.Show();
            }
        }



        [RelayCommand]
        async Task Add()
        {
            var timeString = SelectedTime.ToString();

            if (Items.Contains(timeString))
            {
                var toast = Toast.Make("This time is already added!", CommunityToolkit.Maui.Core.ToastDuration.Short, 16);
                await toast.Show();
                return;
            }

            var newEntry = new FeedingTimeEntry
            {
                FeedingTime = timeString
            };

            try
            {
                await _feedingTimeService.AddFeedingTimeAsync(newEntry);
                await LoadFeedingTimesCommand.ExecuteAsync(null);
                SelectedTime = TimeSpan.Zero;
            } catch (Exception ex)
            {
                var toast = Toast.Make($"Error adding time: {ex.Message}", CommunityToolkit.Maui.Core.ToastDuration.Short, 16);
                await toast.Show();
            }
        }

        [RelayCommand]
        async Task Delete(string timeString)
        {
            try
            {
                var allEntries = await _feedingTimeService.GetAllFeedingTimeAsync();
                var entryToDelete = allEntries.FirstOrDefault(e => e.FeedingTime == timeString);

                if (entryToDelete != null)
                {
                    await _feedingTimeService.DeleteFeedingTimeAsync(entryToDelete.Id);
                    await LoadFeedingTimesCommand.ExecuteAsync(null);
                }
            } catch (Exception ex)
            {
                var toast = Toast.Make($"Error deleting item: {ex.Message}", CommunityToolkit.Maui.Core.ToastDuration.Short, 16);
                await toast.Show();
            }
        }

        private async Task ShowToast(string message)
        {
            await Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short, 16).Show();
        }

    }
}
