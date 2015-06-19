using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PottyTag.Model
{
    class BathroomStatus
    {
        public int NumMales;
        public int NumFemales;
        public bool IsOneDisabled;
        public bool IsTwoDisabled;

        [JsonConstructor]
        public BathroomStatus(int m_population, int f_population, bool left_toilet, bool right_toilet)
        {
            NumMales = m_population;
            NumFemales = f_population;
            IsOneDisabled = !left_toilet;
            IsTwoDisabled = !right_toilet;
        }
    }
}
