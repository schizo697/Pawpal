using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pawpal.Models;
using Pawpal.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Pawpal.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {

        private readonly IFeedingTimeService _feedingTimeService;
        public MainViewModel(IFeedingTimeService feedingTimeService)
        {
            _feedingTimeService = feedingTimeService;
            Items = new ObservableCollection<string>();
            LoadFeedingTimesCommand.ExecuteAsync(null);
        }
        [ObservableProperty]
        ObservableCollection<string> items;

        [ObservableProperty]
        TimeSpan selectedTime;

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
    }
}
