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

        [SettingPropertyDropdown("{=ifJye6ml10N}Apply Troop Multiplier", Order = 1, RequireRestart = false, HintText = "{=ifJye6ml11N}Choose to which party the training will be apply to.")]
        public Dropdown<string> PartyToTrain { get; set; } = new Dropdown<string>(new string[]
        {
                    "{=ifJye6ml12N}Player",
                    "{=ifJye6ml13N}Allies (Player, Companions, Kingdom)",
                    "{=ifJye6ml14N}All"
        }, selectedIndex: 0);


        [SettingPropertyFloatingInteger("{=ifJye6ml15N}Troop XP Multiplier", 0.1f, 10f, HintText = "{=ifJye6ml16N}(Default 3) The higher this is, the more impact leadership will have on training.", Order = 1, RequireRestart = false)]
        [SettingPropertyGroup("{=ifJye6ml17N}Global Multiplier")]
        public float TroopXPMultiplier { get; set; } = 3f;

        [SettingPropertyFloatingInteger("{=ifJye6ml21N}Level Difference Multiplier", 0.1f, 20f, HintText = "{=ifJye6ml19N}(Default 10) The higher this is, the more impact level difference will have on training.", Order = 2, RequireRestart = false)]
        [SettingPropertyGroup("{=ifJye6ml17N}Global Multiplier")] 
        public float LevelDifferenceMultiplier { get; set; } = 10f;

        [SettingPropertyFloatingInteger("{=ifJye6ml15N}Troop XP Multiplier", 0.1f, 10f, HintText = "{=ifJye6ml20N}(Default 3) The higher this is, the more impact leadership will have on training for the player party.", Order = 1, RequireRestart = false)]
        [SettingPropertyGroup("{=ifJye6ml18N}Player Multiplier")]
        public float PlayerTroopXPMultiplier { get; set; } = 3f;

        [SettingPropertyFloatingInteger("{=ifJye6ml21N}Level Difference Multiplier", 0.1f, 20f, HintText = "{=ifJye6ml22N}(Default 10) The higher this is, the more impact level difference will have on training for the player party.", Order = 2, RequireRestart = false)]
        [SettingPropertyGroup("{=ifJye6ml18N}Player Multiplier")]
        public float PlayerLevelDifferenceMultiplier { get; set; } = 10f;

    }
}
