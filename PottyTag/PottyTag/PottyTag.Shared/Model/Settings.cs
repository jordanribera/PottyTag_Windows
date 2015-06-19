using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace PottyTag.Model
{
    class Settings
    {
        public static Settings Current = new Settings();

        public Settings()
        {
        }

        public string Gender {
            get {
                return (string)ApplicationData.Current.LocalSettings.Values["gender"];
            }

            set {
                ApplicationData.Current.LocalSettings.Values["gender"] = value;
            }
        }

        public string Id
        {
            get
            {
                return (string)ApplicationData.Current.LocalSettings.Values["id"];
            }

            set
            {
                ApplicationData.Current.LocalSettings.Values["id"] = value;

                if (!string.IsNullOrEmpty(value))
                {
                    LastUpdate = DateTime.Now;
                }
                else
                {
                    LastUpdate = null;
                }
            }
        }

        public DateTime? LastUpdate
        {
            get
            {
                var lastUpdate = ApplicationData.Current.LocalSettings.Values["lastUpdate"];

                if (lastUpdate != null)
                {
                    return new DateTime((long)lastUpdate);
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    DateTime lastUpdate = (DateTime)value;
                    ApplicationData.Current.LocalSettings.Values["lastUpdate"] = lastUpdate.Ticks;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["lastUpdate"] = null;
                }
            }
        }
    }
}
