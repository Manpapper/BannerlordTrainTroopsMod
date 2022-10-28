using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using MCM.Common;

namespace TrainTroops.Settings
{
    public class MCMTrainTroopsSettings : AttributeGlobalSettings<MCMTrainTroopsSettings>, ISettingsProvider
    {
        public override string Id => nameof(MCMTrainTroopsSettings);

        public override string DisplayName => "Train Troops Settings";

        public override string FolderName => nameof(MCMTrainTroopsSettings);

        public override string FormatType => "json2";

        [SettingPropertyDropdown("Apply Troop Multiplier", Order = 1, RequireRestart = false, HintText = "Choose to which party the training will be apply to.")]
        public Dropdown<string> PartyToTrain { get; set; } = new Dropdown<string>(new string[]
        {
                    "Player",
                    "Allies (Player, Companions, Kingdom)",
                    "All"
        }, selectedIndex: 0);


        [SettingPropertyInteger("Troop XP Multiplier", 1, 10, HintText = "(Default 3) The higher this is, the more impact leadership will have on training.", Order = 1, RequireRestart = false)]
        [SettingPropertyGroup("Global Multiplier")]
        public int TroopXPMultiplier { get; set; } = 3;

        [SettingPropertyInteger("Level Difference Multiplier", 2, 20, HintText = "(Default 10) The higher this is, the more impact level difference will have on training.", Order = 2, RequireRestart = false)]
        [SettingPropertyGroup("Global Multiplier")] 
        public int LevelDifferenceMultiplier { get; set; } = 10;

        [SettingPropertyInteger("Troop XP Multiplier", 1, 10, HintText = "(Default 3) The higher this is, the more impact leadership will have on training for the player party.", Order = 1, RequireRestart = false)]
        [SettingPropertyGroup("Player Multiplier")]
        public int PlayerTroopXPMultiplier { get; set; } = 3;

        [SettingPropertyInteger("Level Difference Multiplier", 2, 20, HintText = "(Default 10) The higher this is, the more impact level difference will have on training for the player party.", Order = 2, RequireRestart = false)]
        [SettingPropertyGroup("Player Multiplier")]
        public int PlayerLevelDifferenceMultiplier { get; set; } = 10;

    }
}
