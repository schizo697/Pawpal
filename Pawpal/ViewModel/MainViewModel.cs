using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Pawpal.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            Items = new ObservableCollection<string>();
        }

        [ObservableProperty]
        ObservableCollection<string> items;

        [ObservableProperty]
        TimeSpan selectedTime;

        [RelayCommand]
        async Task Add()
        {
            if (!Items.Contains(SelectedTime.ToString()))
            {
                Items.Add(SelectedTime.ToString());
            }
            else
            {
                var toast = Toast.Make("This time is already added!", CommunityToolkit.Maui.Core.ToastDuration.Short, 16);
                await toast.Show();
            }

                SelectedTime = TimeSpan.Zero;
        }

        [RelayCommand]
        void Delete(string item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
            }
        }
    }
}
