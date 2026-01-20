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
        float PlayerTroopXPMultiplier { get; set; }

        float PlayerLevelDifferenceMultiplier { get; set; }

        float TroopXPMultiplier { get; set; }

        float LevelDifferenceMultiplier { get; set; }

        Boolean DisplayLog { get; set; }

        Dropdown<string> PartyToTrain { get; set; }
    }
}
