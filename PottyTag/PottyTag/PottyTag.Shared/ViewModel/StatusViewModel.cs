using PottyTag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PottyTag.ViewModel
{
    public class StatusViewModel : BindableBase
    {
        public bool IsMaleOneOccupied
        {
            get
            {
                return this._isMaleOneOccupied;
            }
            set
            {
                SetProperty(ref _isMaleOneOccupied, value);

                if (value)
                {
                    IsToiletOneOpen = false;
                }
            }
        }
        private bool _isMaleOneOccupied;

        public bool IsMaleTwoOccupied
        {
            get
            {
                return this._isMaleTwoOccupied;
            }
            set
            {
                SetProperty(ref _isMaleTwoOccupied, value);

                if (value)
                {
                    IsToiletTwoOpen = false;
                }
            }
        }
        private bool _isMaleTwoOccupied;

        public bool IsFemaleOneOccupied
        {
            get
            {
                return this._isFemaleOneOccupied;
            }
            set
            {
                SetProperty(ref _isFemaleOneOccupied, value);

                if (value)
                {
                    IsToiletOneOpen = false;
                }
            }
        }
        private bool _isFemaleOneOccupied;

        public bool IsFemaleTwoOccupied
        {
            get
            {
                return this._isFemaleTwoOccupied;
            }
            set
            {
                SetProperty(ref _isFemaleTwoOccupied, value);

                if (value)
                {
                    IsToiletTwoOpen = false;
                }
            }
        }
        private bool _isFemaleTwoOccupied;

        public bool IsToiletOneDisabled
        {
            get
            {
                return this._isToiletOneDisabled;
            }
            set
            {
                SetProperty(ref _isToiletOneDisabled, value);

                if (value)
                {
                    IsToiletOneOpen = false;
                }
            }
        }
        private bool _isToiletOneDisabled;

        public bool IsToiletTwoDisabled
        {
            get
            {
                return this._isToiletTwoDisabled;
            }
            set
            {
                SetProperty(ref _isToiletTwoDisabled, value);

                if (value)
                {
                    IsToiletTwoOpen = false;
                }
            }
        }
        private bool _isToiletTwoDisabled;

        public bool IsToiletOneOpen
        {
            get
            {
                return this._isToiletOneOpen;
            }
            set
            {
                SetProperty(ref _isToiletOneOpen, value);
            }
        }
        private bool _isToiletOneOpen = true;

        public bool IsToiletTwoOpen
        {
            get
            {
                return this._isToiletTwoOpen;
            }
            set
            {
                SetProperty(ref _isToiletTwoOpen, value);
            }
        }
        private bool _isToiletTwoOpen = true;

        public bool IsCheckedIn
        {
            get
            {
                return this._isCheckedIn;
            }
            set
            {
                SetProperty(ref _isCheckedIn, value);
            }
        }
        private bool _isCheckedIn = false;
    }
}
