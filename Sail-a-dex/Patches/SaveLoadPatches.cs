using HarmonyLib;
using SailwindModdingHelper;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace sailadex
{
    internal class SaveLoadPatches
    {
        [HarmonyPatch(typeof(SaveLoadManager))]
        private class SaveLoadManagerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("SaveModData")]
            public static void DoSaveGamePatch()
            {
                var saveContainer = new LogUIPatches.RaddudeSaveContainer();

                if (Plugin.fishCaughtUIEnabled.Value)
                {
                    saveContainer.caughtFish = FishCaughtUI.instance.caughtFish.ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value);

                    saveContainer.fishBadges = FishCaughtUI.instance.fishBadges.ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value);
                }


                if (Plugin.portsVisitedUIEnabled.Value)
                {
                    saveContainer.visitedPorts = PortsVisitedUI.instance.visitedPorts.ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value);

                    saveContainer.portBadges = PortsVisitedUI.instance.portBadges.ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value);
                }

                if (Plugin.statsUIEnabled.Value)
                {
                    saveContainer.floatStats = StatsUI.instance.floatStats.ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value);

                    saveContainer.intStats = StatsUI.instance.intStats.ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value);

                    saveContainer.boolArrayStats = StatsUI.instance.boolArrayStats.ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value);
                }

                ModSave.Save(Plugin.instance.Info, saveContainer);
            }

            [HarmonyPatch("LoadModData")]
            [HarmonyPostfix]
            public static void LoadModDataPatch()
            {
                if (!ModSave.Load(Plugin.instance.Info, out LogUIPatches.RaddudeSaveContainer saveContainer))
                {                    
                    Plugin.logger.LogWarning("Save file loading failed. File is either corrupt or does not exist. If this is the first time loading this save with this mod, this is normal.");
                    return;                                        
                }
                if (Plugin.fishCaughtUIEnabled.Value)
                {
                    if (saveContainer.caughtFish != null)
                    {
                        LoadDictionary(saveContainer.caughtFish, FishCaughtUI.instance.caughtFish);
                    }

                    if (saveContainer.fishBadges != null)
                    {
                        LoadDictionary(saveContainer.fishBadges, FishCaughtUI.instance.fishBadges);
                    }

                    // for 1.0.0 saves that didn't have badges yet
                    var oceanFishPrefabs = OceanFishes.instance.GetPrivateField<GameObject[]>("fishPrefabs");
                    foreach (GameObject fish in oceanFishPrefabs)
                        FishCaughtUI.instance.CheckIndividualFishBadges(fish.name);
                    FishCaughtUI.instance.CheckAllFishBadges();
                }

                if (Plugin.portsVisitedUIEnabled.Value)
                {
                    if (saveContainer.visitedPorts != null)
                    {
                        LoadDictionary(saveContainer.visitedPorts, PortsVisitedUI.instance.visitedPorts);
                    }

                    if (saveContainer.portBadges != null)
                    {
                        LoadDictionary(saveContainer.portBadges, PortsVisitedUI.instance.portBadges);
                    }

                    // for 1.0.0 saves that didn't have badges yet
                    PortsVisitedUI.instance.CheckBadges();
                }

                if (Plugin.statsUIEnabled.Value)
                {
                    if (saveContainer.floatStats != null)
                    {
                        LoadDictionary(saveContainer.floatStats, StatsUI.instance.floatStats);
                    }

                    if (saveContainer.intStats != null)
                    {
                        LoadDictionary(saveContainer.intStats, StatsUI.instance.intStats);
                    }

                    if (saveContainer.boolArrayStats != null)
                    {
                        foreach (KeyValuePair<string, bool[]> item in saveContainer.boolArrayStats)
                        {
                            if (StatsUI.instance.boolArrayStats.ContainsKey(item.Key))
                            {
                                StatsUI.instance.boolArrayStats[item.Key] = (bool[])item.Value.Clone();
                                continue;
                            }
                            Plugin.logger.LogWarning($"LoadData: {item.Key} not found in game");
                        }
                    }
                }
            }  
            
            public static void LoadDictionary<T>(Dictionary<string, T> saveDict, Dictionary<string, T> gameDict)
            {
                foreach (KeyValuePair<string, T> item in saveDict)
                {
                    if (gameDict.ContainsKey(item.Key))
                    {
                        gameDict[item.Key] = item.Value;
                        continue;
                    }
                    Plugin.logger.LogWarning($"LoadData: {item.Key} not found in game");
                }                
            }
        }
    }
}
