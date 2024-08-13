using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace sailadex
{
    public class FishCaughtUI : MonoBehaviour
    {
        public static FishCaughtUI instance;
        public Dictionary<string, int> caughtFish;
        public TextMesh[] fishNameTMs;
        public TextMesh[] caughtCountTMs;
        public Dictionary<string, bool> fishBadges;
        public Dictionary<string, GameObject> fishBadgeGOs;

        private void Awake()
        {
            instance = this;
            caughtFish = new Dictionary<string, int>();
            fishBadges = new Dictionary<string, bool>();
            fishBadgeGOs = new Dictionary<string, GameObject>();

            foreach (string fishName in Names.fishNames)
            {
                caughtFish.Add(fishName, 0);

                var shortFishName = ShortFishName(fishName);
                fishBadges.Add(shortFishName + "25", false);
                fishBadges.Add(shortFishName + "50", false);
                fishBadges.Add(shortFishName + "100", false);
            }

            foreach (string badgeName in Names.totalFishBadgeNames) 
            {
                fishBadges.Add(badgeName, false);
            }
        }

        public static string ShortFishName(string name)
        {
            return name.Substring(3, name.IndexOf("(") - 4);
        }

        public void RegisterCatch(string fishName)
        {
            caughtFish[fishName]++;
            CheckBadges(fishName);
            //Debug.Log("Caught: " + fishName);
        }

        public void UpdatePage()
        {
            UpdateTexts();
            UpdateBadges();
        }

        private void UpdateTexts()
        {
            int i = 0;
            int catchSum = 0;
            foreach (KeyValuePair<string, int> fish in caughtFish)
            {
                if (Plugin.fishNamesHidden.Value)
                    fishNameTMs[i].text = fish.Value > 0 ? ShortFishName(fish.Key) : "???";
                else
                    fishNameTMs[i].text = ShortFishName(fish.Key);
                caughtCountTMs[i].text = fish.Value.ToString();
                catchSum += fish.Value;
                //Debug.Log(fish.Key + " " + fish.Value);
                i++;
            }
            fishNameTMs[fishNameTMs.Length - 1].text = "Total";
            caughtCountTMs[caughtCountTMs.Length - 1].text = "" + catchSum;
        }

        public void CheckBadges(string fishName)
        {
            CheckIndividualFishBadges(fishName);
            CheckAllFishBadges();            
        }

        public void CheckIndividualFishBadges(string fishName)
        {
            var shortFishName = ShortFishName(fishName);
            fishBadges[shortFishName + "25"] = caughtFish[fishName] >= 25;
            fishBadges[shortFishName + "50"] = caughtFish[fishName] >= 50;
            fishBadges[shortFishName + "100"] = caughtFish[fishName] >= 100;
        }

        public void CheckAllFishBadges()
        {
            var allFish = true;
            var catchSum = 0;
            foreach (KeyValuePair<string, int> fish in caughtFish)
            {
                catchSum += fish.Value;
                if (fish.Value == 0)
                    allFish = false;
            }

            fishBadges[Names.totalFishBadgeNames[0]] = catchSum >= 50;
            fishBadges[Names.totalFishBadgeNames[1]] = catchSum >= 250;
            fishBadges[Names.totalFishBadgeNames[2]] = catchSum >= 500;
            fishBadges[Names.totalFishBadgeNames[3]] = allFish;
        }

        private void UpdateBadges()
        {
            foreach (KeyValuePair<string, bool> badge in fishBadges)
            {
                fishBadgeGOs[badge.Key].SetActive(badge.Value);
            }
        }
    }
}
