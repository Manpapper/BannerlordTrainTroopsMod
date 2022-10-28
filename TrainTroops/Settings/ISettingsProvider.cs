using MCM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTroops.Settings
{
    interface ISettingsProvider
    {
        int PlayerTroopXPMultiplier { get; set; }

        int PlayerLevelDifferenceMultiplier { get; set; }

        int TroopXPMultiplier { get; set; }

        int LevelDifferenceMultiplier { get; set; }

        Dropdown<string> PartyToTrain { get; set; }
    }
}
