using PottyTag.Common;
using PottyTag.Model;
using PottyTag.Network;
using PottyTag.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace PottyTag.View
{
    public sealed partial class Status : Page
    {
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        private NavigationHelper navigationHelper;

        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(
            "ViewModel", typeof(StatusViewModel),
            typeof(Status), null
        );
        
        public StatusViewModel ViewModel
        {
            get { return (StatusViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion

        private DispatcherTimer _timer;
        private int _audioCounter = 0;

        public Status()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            this.ViewModel = new StatusViewModel();

            Debug.WriteLine("[Status] Constructor");
        }

        void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            Debug.WriteLine("[Status] SaveState");
            _timer.Stop();
            _timer = null;
        }

        async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            Debug.WriteLine("[Status] LoadState");
            UpdateStatus();

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 5, 0);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, object e)
        {
            Debug.WriteLine("[Status] DispatcherTimer_Tick()");
            UpdateStatus();
        }

        private async void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("[Status] CheckInButton_Click");
            var success = await API.Current.CheckIn();
            PopAudio.Play();
            await UpdateStatus();
        }

        private async void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("[Status] CheckOutButton_Click");
            var success = await API.Current.CheckOut();
            FlushAudio.Play();
            await UpdateStatus();

            await Task.Delay(2000);

            /*int numSounds = 2;
            
            if (_audioCounter == 0)
            {*/
                OhMyAudio.Play();
            /*}
            else if (_audioCounter == 1)
            {
                NiceWork.Play();
            }*/

            /*_audioCounter++;
            
            if (_audioCounter >= numSounds)
            {
                _audioCounter = 0;
            }*/
        }

        private void Reset()
        {
            ViewModel.IsMaleOneOccupied = false;
            ViewModel.IsMaleTwoOccupied = false;
            ViewModel.IsFemaleOneOccupied = false;
            ViewModel.IsFemaleTwoOccupied = false;
            ViewModel.IsToiletOneDisabled = false;
            ViewModel.IsToiletTwoDisabled = false;
            ViewModel.IsToiletOneOpen = true;
            ViewModel.IsToiletTwoOpen = true;
        }

        private async Task UpdateStatus()
        {
            var status = await API.Current.GetStatus();

            if (status == null)
            {
                Debug.WriteLine("[Status] Failed to get status");
                return;
            }
            
            Debug.WriteLine("[Status] m: " + status.NumMales + " f: " + status.NumFemales);

            // CheckIn / Checkout Buttons
            if (Settings.Current.LastUpdate != null)
            {
                var lastUpdate = (DateTime)Settings.Current.LastUpdate;
                TimeSpan diff = new TimeSpan(DateTime.Now.Ticks - lastUpdate.Ticks);
                ViewModel.IsCheckedIn = diff.TotalMinutes < 5;
            }
            else
            {
                ViewModel.IsCheckedIn = false;
            }

            // Toilet buttons
            // Reset to defaults
            Reset();

            if (status.IsOneDisabled && status.IsTwoDisabled)
            {
                ViewModel.IsToiletOneDisabled = true;
                ViewModel.IsToiletTwoDisabled = true;
                ViewModel.IsToiletOneOpen = false;
                ViewModel.IsToiletTwoOpen = false;
            }
            else
            {
                ViewModel.IsToiletOneDisabled = status.IsOneDisabled;
                ViewModel.IsToiletTwoDisabled = status.IsTwoDisabled;
                
                if (ViewModel.IsToiletOneDisabled)
                {
                    if (status.NumMales > 0)
                    {
                        ViewModel.IsMaleTwoOccupied = true;
                    }
                    else if (status.NumFemales > 0)
                    {
                        ViewModel.IsFemaleTwoOccupied = true;
                    }
                }
                else if (ViewModel.IsToiletTwoDisabled)
                {
                    if (status.NumMales > 0)
                    {
                        ViewModel.IsMaleOneOccupied = true;
                    }
                    else if (status.NumFemales > 0)
                    {
                        ViewModel.IsFemaleOneOccupied = true;
                    }
                }
                else
                {
                    if (status.NumMales > 0 && status.NumFemales > 0)
                    {
                        // Somebody forgot to check out
                        ViewModel.IsMaleOneOccupied = true;
                        ViewModel.IsFemaleTwoOccupied = true;
                    }
                    else if (status.NumMales >= 2)
                    {
                        ViewModel.IsMaleOneOccupied = true;
                        ViewModel.IsMaleTwoOccupied = true;
                    }
                    else if (status.NumFemales >= 1)
                    {
                        // One female counts as occupying both stalls
                        ViewModel.IsFemaleOneOccupied = true;
                        ViewModel.IsFemaleTwoOccupied = true;
                    }
                    else if (status.NumMales == 1)
                    {
                        ViewModel.IsMaleOneOccupied = true;
                    }
                }
            }
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        ///
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void GenderSelectButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GenderSelect));
        }

        private void EnabledToiletOneButton_Click(object sender, RoutedEventArgs e)
        {
            API.Current.DisableToilet(true);
            UpdateStatus();
            PopAudio.Play();
        }

        private void EnabledToiletTwoButton_Click(object sender, RoutedEventArgs e)
        {
            API.Current.DisableToilet(false);
            UpdateStatus();
            PopAudio.Play();
        }
        
        private void DisabledToiletOneButton_Click(object sender, RoutedEventArgs e)
        {
            API.Current.EnableToilet(true);
            UpdateStatus();
            PopAudio.Play();
        }

        private void DisabledToiletTwoButton_Click(object sender, RoutedEventArgs e)
        {
            API.Current.EnableToilet(false);
            UpdateStatus();
            PopAudio.Play();
        }
    }
}
