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
        public int PlayerTroopXPMultiplier { get; set; } = 3;
        public int PlayerLevelDifferenceMultiplier { get; set; } = 10;
        public int TroopXPMultiplier { get; set; } = 3;
        public int LevelDifferenceMultiplier { get; set; } = 10;

        public Dropdown<string> PartyToTrain { get; set; }
    }
}
