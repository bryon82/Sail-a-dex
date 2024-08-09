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

        private void Awake()
        {
            instance = this;
            caughtFish = new Dictionary<string, int>();
            var oceanFishPrefabs = Traverse.Create(OceanFishes.instance).Field("fishPrefabs").GetValue<GameObject[]>();

            foreach (GameObject fish in oceanFishPrefabs)
            {
                caughtFish.Add(fish.name, 0);
            }
        }

        public void RegisterCatch(string fishName)
        {
            caughtFish[fishName]++;
        }

        public void UpdateTexts()
        {
            int i = 0;
            foreach (KeyValuePair<string, int> fish in caughtFish)
            {
                fishNameTMs[i].text = fish.Value > 0 ? fish.Key.Substring(3, fish.Key.IndexOf("(") - 4) : "???";
                caughtCountTMs[i].text = fish.Value.ToString();
                //Debug.Log(fish.Key + " " + fish.Value);
                i++;
            }
        }
    }
}
