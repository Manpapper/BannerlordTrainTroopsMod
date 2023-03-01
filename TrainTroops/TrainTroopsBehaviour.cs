using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TrainTroops
{
    class MobilePartyDailyTickBehaviour : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new System.Action<MobileParty>(this.train));
        }

        private void train(MobileParty party)
        {
            //Whatever the case we can only add xp to Lord Party and Caravans
            //For exemple Caravan don't have a leader in their party (so no Leadership skill)
            if (!party.IsLordParty && !party.IsCaravan)
                return;


            //Main Party XP Only
            if (TrainTroopsSettings.Instance.PartyToTrain.SelectedIndex == (int) PartyToTrain.Player && party.IsMainParty)
            {
                if (!party.IsMainParty)
                    return;

                addXp(party, party.LeaderHero);                
            }

            //Allies Only
            if (TrainTroopsSettings.Instance.PartyToTrain.SelectedIndex == (int)PartyToTrain.Allies)
            {            
                //Caravans don't have Leader so we have to check Owner for skill
                if (party.IsCaravan == true && party.Owner != null)
                {
                    //Caravans own by the player
                    if(party.Owner == Hero.MainHero)
                    {
                        addXp(party, party.Owner);
                        return;
                    }

                    //Caravans own by Allies
                    if(party.Owner.CurrentSettlement != null && party.Owner.CurrentSettlement.OwnerClan != null && party.Owner.CurrentSettlement.OwnerClan.Kingdom == Hero.MainHero.Clan.Kingdom)
                    {
                        addXp(party, party.Owner);
                        return;
                    }
                }

                //Check if leader is an "Ally" of Player
                if (party.IsLordParty == true && party.LeaderHero != null)
                {
                    //Companion or Player
                    if(party.LeaderHero.Clan == Hero.MainHero.Clan)
                    {
                        addXp(party, party.LeaderHero);
                        return;
                    }

                    //Allied Party
                    if(party.LeaderHero.Clan != null && party.LeaderHero.Clan.Kingdom == Hero.MainHero.Clan.Kingdom)
                    {
                        addXp(party, party.LeaderHero);
                        return;
                    }
                }                    
            }

            //Every Party
            if (TrainTroopsSettings.Instance.PartyToTrain.SelectedIndex == (int)PartyToTrain.All)
            {
                if (party.IsCaravan && party.Owner != null)
                    addXp(party, party.Owner);

                if (party.IsLordParty && party.LeaderHero != null)
                    addXp(party, party.LeaderHero);
            }
        }

        private void addXp(MobileParty party, Hero hero)
        {
            //In case of hero being null
            if (hero == null)
                return;

            int troopXPMultiplier = TrainTroopsSettings.Instance.TroopXPMultiplier;
            int LevelDifferenceMultiplier = TrainTroopsSettings.Instance.LevelDifferenceMultiplier;
            if (party.IsMainParty)
            {
                troopXPMultiplier = TrainTroopsSettings.Instance.PlayerTroopXPMultiplier;
                LevelDifferenceMultiplier = TrainTroopsSettings.Instance.PlayerLevelDifferenceMultiplier;
            }

            int leaderLeadership = hero.GetSkillValue(DefaultSkills.Leadership);

            int totalXPEarned = 0;
            Dictionary<string, int> troopsReadyToUpgrade = new Dictionary<string, int>();
            for (int i = 0; i < party.MemberRoster.Count; i++)
            {
                //IMPORTANT: bear in mind we only get a COPY. So after any changes to the troop, info will be inconsistent.
                TroopRosterElement troop = party.MemberRoster.GetElementCopyAtIndex(i);
                //Only gain XP if character LVL is lower than the leader's LVL AND the troop can be upgraded
                if (troop.Character.Level < hero.Level && !troop.Character.UpgradeTargets.IsEmpty())
                {
                    int lvlDifference = hero.Level - troop.Character.Level;

                    //Get the least xp this troop needs to lvl up (seems it could have different troops to level up to and need different xp for each one)
                    int minXPForUpgrade = troop.Character.GetUpgradeXpCost(party.Party, 0);
                    int targetIndex = 1;
                    while (targetIndex < troop.Character.UpgradeTargets.Length)
                    {
                        minXPForUpgrade = System.Math.Min(minXPForUpgrade, troop.Character.GetUpgradeXpCost(party.Party, targetIndex));
                        targetIndex++;
                    }

                    //If minXPForUpgrade = 0 we skip this iteration
                    if (minXPForUpgrade == 0)
                        continue;

                    int trainableTroopCount = troop.Number - troop.Xp / minXPForUpgrade;

                    //Perform the math
                    int xpEarned = (leaderLeadership * troopXPMultiplier + lvlDifference * LevelDifferenceMultiplier) * trainableTroopCount;
                    party.Party.MemberRoster.AddXpToTroopAtIndex(xpEarned, i);
                    int troopsReadyToUpgradeCount = (troop.Xp + xpEarned) / minXPForUpgrade;
                    //Report troops ready to upgrade
                    if (troopsReadyToUpgradeCount != 0)
                    {
                        //Will update the party button notification so that the red icon is shown (?)
                        //PlayerUpdateTracker.Current.GetPartyNotificationText();
                        //PlayerUpdateTracker.Current.UpdatePartyNotification();

                        //TODO: get the troop name, for now it only gets it in english
                        string troopName = troop.Character.ToString();
                        //Count how many troops of each type are ready to upgrade


                        //If a troop with the same name has already been counted, add it
                        if (troopsReadyToUpgrade.ContainsKey(troopName))
                        {
                            troopsReadyToUpgrade[troopName] += troopsReadyToUpgradeCount;
                        }
                        //Else add it anew
                        else
                        {
                            troopsReadyToUpgrade.Add(troopName, troopsReadyToUpgradeCount);
                        }
                    }

                    totalXPEarned += xpEarned;
                }
            }

            if(party.IsMainParty)
                InformationManager.DisplayMessage(new InformationMessage("Total training XP for the day: " + totalXPEarned + "." + getTroopsReadyToUpgradeMessage(troopsReadyToUpgrade)));

        }

        private static string getTroopsReadyToUpgradeMessage(Dictionary<string, int> troopsReadyToUpgrade)
        {
            string troopsReadyToUpgradeMessage = "";
            if (troopsReadyToUpgrade.Count != 0)
            {
                troopsReadyToUpgradeMessage += " Troops ready to upgrade: ";
                for (int i = 0; i < troopsReadyToUpgrade.Count; i++)
                {
                    troopsReadyToUpgradeMessage += troopsReadyToUpgrade.Keys.ElementAt(i) + ": " + troopsReadyToUpgrade[troopsReadyToUpgrade.Keys.ElementAt(i)];
                    if (i != troopsReadyToUpgrade.Count - 1)
                    {
                        troopsReadyToUpgradeMessage += ", ";
                    }
                }
                troopsReadyToUpgradeMessage += ".";
            }
            return troopsReadyToUpgradeMessage;
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

    }

}
