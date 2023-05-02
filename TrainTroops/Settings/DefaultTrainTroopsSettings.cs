using MCM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTroops.Settings
{
    class DefaultTrainTroopsSettings : ISettingsProvider
    {
        public float PlayerTroopXPMultiplier { get; set; } = 3;
        public float PlayerLevelDifferenceMultiplier { get; set; } = 10;
        public float TroopXPMultiplier { get; set; } = 3;
        public float LevelDifferenceMultiplier { get; set; } = 10;

        public Dropdown<string> PartyToTrain { get; set; }
    }
}
