using PottyTag.Common;
using PottyTag.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class GenderSelect : Page
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

        public GenderSelect()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            Window.Current.SizeChanged += Current_SizeChanged;
            UpdateForOrientation();
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            UpdateForOrientation();
        }

        private void UpdateForOrientation()
        {
            var bounds = Window.Current.Bounds;

            if (bounds.Height > bounds.Width)
            {
                PortraitTopContainer.Visibility = Visibility.Visible;
                PortraitBottomContainer.Visibility = Visibility.Visible;
                LandscapeContainer.Visibility = Visibility.Collapsed;
            }
            else
            {
                PortraitTopContainer.Visibility = Visibility.Collapsed;
                PortraitBottomContainer.Visibility = Visibility.Collapsed;
                LandscapeContainer.Visibility = Visibility.Visible;
            }
        }

        void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            Debug.WriteLine("[GenderSelect] SaveState");
        }

        void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            Debug.WriteLine("[GenderSelect] LoadState");
        }

        private void MaleButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("[GenderSelect] MaleButton_Click");
            SetGenderAndContinue("m");
        }

        private void FemaleButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("[GenderSelect] FemaleButton_Click");
            SetGenderAndContinue("f");
        }

        private void SetGenderAndContinue(string gender)
        {
            Debug.WriteLine("[GenderSelect] SetGenderAndContinue: " + gender);
            Settings.Current.Gender = gender;
            PopAudio.Play();
            Frame.Navigate(typeof(Status));
        }
    }
}
