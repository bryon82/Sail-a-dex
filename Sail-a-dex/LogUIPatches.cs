using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SailwindModdingHelper;

namespace sailadex
{
    internal class LogUIPatches
    {
        private static GameObject bookmarkFishCaught;
        private static GameObject bookmarkPortsVisited;
        private static GameObject fishCaughtUI;
        private static GameObject portsVisitedUI;
        public const MissionListMode fishCaught = (MissionListMode)5;
        public const MissionListMode portsVisited = (MissionListMode)6;        

        [HarmonyPatch(typeof(MissionListUI))]
        private class MissionListUIPatches
        {

            [HarmonyPostfix]
            [HarmonyPatch("HideUI")]
            public static void HideUIPatches(MissionListUI __instance)
            {
                if (Plugin.fishCaughtUIEnabled.Value)
                    fishCaughtUI.SetActive(value: false);
                if (Plugin.portsVisitedUIEnabled.Value)
                    portsVisitedUI.SetActive(value: false);
            }

            [HarmonyPostfix]
            [HarmonyPatch("SwitchMode")]
            public static void SwitchModePatches(MissionListMode mode)
            {
                if (Plugin.fishCaughtUIEnabled.Value)
                    fishCaughtUI.SetActive(value: false);
                if (Plugin.portsVisitedUIEnabled.Value)
                    portsVisitedUI.SetActive(value: false);
                switch (mode)
                {               
                    case fishCaught:
                        fishCaughtUI.SetActive(value: true);
                        fishCaughtUI.GetComponent<FishCaughtUI>().UpdateTexts();
                        break;
                    case portsVisited:
                        portsVisitedUI.SetActive(value: true);
                        portsVisitedUI.GetComponent<PortsVisitedUI>().UpdateTexts();
                        break;
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("Start")]
            public static void StartPatch(MissionListUI __instance, GameObject ___modeButtons, GameObject ___reputationUI)
            {
                if (Plugin.fishCaughtUIEnabled.Value)                
                    MakeFishCaughtUI(___modeButtons, ___reputationUI);
                
                if (Plugin.portsVisitedUIEnabled.Value)                
                    MakePortsVisitedUI(___modeButtons, ___reputationUI);                
            }

            private static void MakeFishCaughtUI(GameObject modeButtons, GameObject repUI)
            {
                GameObject bookmarkReceipts = modeButtons.transform.GetChild(9).gameObject;
                bookmarkFishCaught = GameObject.Instantiate(bookmarkReceipts);
                bookmarkFishCaught.transform.parent = modeButtons.transform;
                bookmarkFishCaught.transform.localPosition = new Vector3(-0.27f, bookmarkReceipts.transform.localPosition[1], bookmarkReceipts.transform.localPosition[2]);
                bookmarkFishCaught.transform.localRotation = bookmarkReceipts.transform.localRotation;
                bookmarkFishCaught.transform.localScale = bookmarkReceipts.transform.localScale;
                bookmarkFishCaught.name = "bookmark fish caught";
                bookmarkFishCaught.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "fish caught";
                GPButtonLogMode gPButtonLogMode = bookmarkFishCaught.GetComponent<GPButtonLogMode>();
                Traverse.Create(gPButtonLogMode).Field("mode").SetValue(fishCaught);
                UnityEngine.Object.Destroy(bookmarkFishCaught.GetComponent<cakeslice.Outline>());

                fishCaughtUI = GameObject.Instantiate(repUI);
                UnityEngine.Object.Destroy(fishCaughtUI.GetComponent<ReputationUI>());
                fishCaughtUI.transform.parent = repUI.transform.parent;
                fishCaughtUI.transform.localPosition = repUI.transform.localPosition;
                fishCaughtUI.transform.localRotation = repUI.transform.localRotation;
                fishCaughtUI.transform.localScale = repUI.transform.localScale;
                fishCaughtUI.name = "fish caught ui";
                UnityEngine.Object.Destroy(fishCaughtUI.transform.GetChild(2).gameObject);
                UnityEngine.Object.Destroy(fishCaughtUI.transform.GetChild(1).gameObject);
                fishCaughtUI.AddComponent<FishCaughtUI>();

                var oceanFishPrefabs = Traverse.Create(OceanFishes.instance).Field("fishPrefabs").GetValue<GameObject[]>();
                GameObject fishCaughtTextGO = fishCaughtUI.transform.GetChild(0).gameObject;
                fishCaughtTextGO.GetComponent<TextMesh>().font = fishCaughtTextGO.transform.GetChild(1).GetComponent<TextMesh>().font;
                fishCaughtTextGO.GetComponent<MeshRenderer>().material = fishCaughtTextGO.transform.GetChild(1).GetComponent<MeshRenderer>().material;
                fishCaughtTextGO.transform.GetChild(1).gameObject.name = "caught count";
                fishCaughtTextGO.transform.GetChild(1).localPosition = new Vector3(70f, 0f, fishCaughtTextGO.transform.GetChild(1).localPosition[2]);
                TextMesh[] caughtCountTexts = new TextMesh[oceanFishPrefabs.Length];
                TextMesh[] fishnameTexts = new TextMesh[oceanFishPrefabs.Length];

                for (int i = 0; i < oceanFishPrefabs.Length; i++)
                {
                    GameObject newFishCaughtTextGO = GameObject.Instantiate(fishCaughtTextGO);
                    UnityEngine.Object.Destroy(newFishCaughtTextGO.transform.GetChild(4).gameObject);
                    UnityEngine.Object.Destroy(newFishCaughtTextGO.transform.GetChild(3).gameObject);
                    UnityEngine.Object.Destroy(newFishCaughtTextGO.transform.GetChild(2).gameObject);
                    UnityEngine.Object.Destroy(newFishCaughtTextGO.transform.GetChild(0).gameObject);
                    newFishCaughtTextGO.name = oceanFishPrefabs[i].name;
                    newFishCaughtTextGO.transform.parent = fishCaughtTextGO.transform.parent;
                    newFishCaughtTextGO.transform.localPosition = new Vector3(0.75f, fishCaughtTextGO.transform.localPosition[1] - 0.04f * i, fishCaughtTextGO.transform.localPosition[2]);
                    newFishCaughtTextGO.transform.localRotation = fishCaughtTextGO.transform.localRotation;
                    newFishCaughtTextGO.transform.localScale = fishCaughtTextGO.transform.localScale;
                    fishnameTexts[i] = newFishCaughtTextGO.GetComponent<TextMesh>();
                    caughtCountTexts[i] = newFishCaughtTextGO.transform.GetChild(1).GetComponent<TextMesh>();
                }
                UnityEngine.Object.Destroy(fishCaughtTextGO);
                fishCaughtUI.GetComponent<FishCaughtUI>().fishNameTMs = fishnameTexts;
                fishCaughtUI.GetComponent<FishCaughtUI>().caughtCountTMs = caughtCountTexts;
            }

            private static void MakePortsVisitedUI(GameObject modeButtons, GameObject repUI)
            {
                GameObject bookmarkReceipts = modeButtons.transform.GetChild(9).gameObject;
                bookmarkPortsVisited = GameObject.Instantiate(bookmarkReceipts);
                bookmarkPortsVisited.transform.parent = modeButtons.transform;
                bookmarkPortsVisited.transform.localPosition = new Vector3(-0.397f, 0.0028f, bookmarkReceipts.transform.localPosition[2]); 
                bookmarkPortsVisited.transform.localRotation = bookmarkReceipts.transform.localRotation;
                bookmarkPortsVisited.transform.localScale = bookmarkReceipts.transform.localScale;
                bookmarkPortsVisited.name = "bookmark ports visited";
                bookmarkPortsVisited.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "ports visited";
                GPButtonLogMode gPButtonLogMode = bookmarkPortsVisited.GetComponent<GPButtonLogMode>();
                Traverse.Create(gPButtonLogMode).Field("mode").SetValue(portsVisited);
                UnityEngine.Object.Destroy(bookmarkPortsVisited.GetComponent<cakeslice.Outline>());

                portsVisitedUI = GameObject.Instantiate(repUI);
                UnityEngine.Object.Destroy(portsVisitedUI.GetComponent<ReputationUI>());
                portsVisitedUI.transform.parent = repUI.transform.parent;
                portsVisitedUI.transform.localPosition = repUI.transform.localPosition;
                portsVisitedUI.transform.localRotation = repUI.transform.localRotation;
                portsVisitedUI.transform.localScale = repUI.transform.localScale;
                portsVisitedUI.name = "ports visited ui";
                portsVisitedUI.AddComponent<PortsVisitedUI>();

                portsVisitedUI.transform.GetChild(0).localPosition = new Vector3(0.75f, 0.22f, portsVisitedUI.transform.GetChild(0).localPosition[2]);
                portsVisitedUI.transform.GetChild(1).localPosition = new Vector3(0.75f, 0.01f, portsVisitedUI.transform.GetChild(1).localPosition[2]);
                portsVisitedUI.transform.GetChild(2).localPosition = new Vector3(0.02f, 0.22f, portsVisitedUI.transform.GetChild(2).localPosition[2]);

                GameObject lagoon = GameObject.Instantiate(portsVisitedUI.transform.GetChild(1).gameObject);
                lagoon.transform.parent = portsVisitedUI.transform;
                lagoon.transform.localPosition = new Vector3(0.02f, 0.01f, portsVisitedUI.transform.GetChild(1).localPosition[2]);
                lagoon.transform.localRotation = portsVisitedUI.transform.GetChild(1).localRotation;
                lagoon.transform.localScale = portsVisitedUI.transform.GetChild(1).localScale;
                lagoon.name = "lagoon";
                lagoon.GetComponent<TextMesh>().text = "Fire Fish Lagoon";

                TextMesh[] portNameTMs = new TextMesh[24];
                TextMesh[] portVisitedTMs = new TextMesh[24];
                int portVisitedIndex = 0;
                int[] numPorts = { 7, 6, 7, 4 };

                for (int r = 0; r < 4; r++)
                {
                    Transform portsVisitedGO = portsVisitedUI.transform.GetChild(r);
                    UnityEngine.Object.Destroy(portsVisitedGO.GetChild(1).gameObject);
                    UnityEngine.Object.Destroy(portsVisitedGO.GetChild(2).gameObject);
                    UnityEngine.Object.Destroy(portsVisitedGO.GetChild(4).gameObject);
                    GameObject portNameTMTemplate = portsVisitedGO.GetChild(0).gameObject;
                    GameObject portVisitedTMTemplate = portsVisitedGO.GetChild(3).gameObject;
                    portNameTMTemplate.name = "port1";
                    portNameTMTemplate.transform.localPosition = new Vector3(5f, -8f, -0.1f);
                    portVisitedTMTemplate.name = "visited port1";
                    portVisitedTMTemplate.transform.localPosition = new Vector3(70f, -9f, 0f);
                    portNameTMs[portVisitedIndex] = portNameTMTemplate.GetComponent<TextMesh>();
                    portVisitedTMs[portVisitedIndex] = portVisitedTMTemplate.GetComponent<TextMesh>();
                    portVisitedIndex++;

                    for (int i = 1; i < numPorts[r]; i++)
                    {
                        GameObject portNameTM = GameObject.Instantiate(portNameTMTemplate);
                        portNameTM.transform.parent = portNameTMTemplate.transform.parent;
                        portNameTM.transform.localPosition = new Vector3(portNameTMTemplate.transform.localPosition[0], portNameTMTemplate.transform.localPosition[1] - 7.5f * i, portNameTMTemplate.transform.localPosition[2]);
                        portNameTM.transform.localRotation = portNameTMTemplate.transform.localRotation;
                        portNameTM.transform.localScale = portNameTMTemplate.transform.localScale;
                        portNameTM.name = "port" + (i + 1);

                        GameObject portVisitedTM = GameObject.Instantiate(portVisitedTMTemplate);
                        portVisitedTM.transform.parent = portVisitedTMTemplate.transform.parent;
                        portVisitedTM.transform.localPosition = new Vector3(portVisitedTMTemplate.transform.localPosition[0], portVisitedTMTemplate.transform.localPosition[1] - 7.5f * i, portVisitedTMTemplate.transform.localPosition[2]);
                        portVisitedTM.transform.localRotation = portVisitedTMTemplate.transform.localRotation;
                        portVisitedTM.transform.localScale = portVisitedTMTemplate.transform.localScale;
                        portVisitedTM.name = "visited port" + (i + 1);

                        portNameTMs[portVisitedIndex] = portNameTM.GetComponent<TextMesh>();
                        portVisitedTMs[portVisitedIndex] = portVisitedTM.GetComponent<TextMesh>();
                        portVisitedIndex++;
                    }
                }
                portsVisitedUI.GetComponent<PortsVisitedUI>().portNameTMs = portNameTMs;
                portsVisitedUI.GetComponent<PortsVisitedUI>().portVisitedTMs = portVisitedTMs;
            }
        }

        [HarmonyPatch(typeof(FishingRodFish))]
        private class FishingRodFishPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("CollectFish")]
            public static void CollectFishPatch(FishingRodFish __instance, GameObject ___currentFish)
            {
                if (Plugin.fishCaughtUIEnabled.Value)
                    fishCaughtUI.GetComponent<FishCaughtUI>().RegisterCatch(___currentFish.name);
            }
        }

        [HarmonyPatch(typeof(IslandMarketWarehouseArea))]
        private class IslandMarketWarehouseAreaPatches
        {

            [HarmonyPostfix]
            [HarmonyPatch("OnTriggerEnter")]
            public static void OnTriggerEnterPatch(IslandMarketWarehouseArea __instance, IslandMarket ___market, Collider other)
            {
                if (Plugin.portsVisitedUIEnabled.Value && other.CompareTag("Player"))                
                    portsVisitedUI.GetComponent<PortsVisitedUI>().RegisterVisit(___market.GetPortName());                             
            }
        }

        [HarmonyPatch(typeof(SaveLoadManager))]
        private class SaveLoadManagerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("SaveModData")]
            public static void DoSaveGamePatch()
            {
                var saveContainer = new RaddudeSaveContainer();

                if (Plugin.fishCaughtUIEnabled.Value)
                    saveContainer.caughtFish = fishCaughtUI.GetComponent<FishCaughtUI>().caughtFish.ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value);

                if (Plugin.portsVisitedUIEnabled.Value)
                    saveContainer.visitedPorts = portsVisitedUI.GetComponent<PortsVisitedUI>().visitedPorts.ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value);

                ModSave.Save(Plugin.instance.Info, saveContainer);
            }

            [HarmonyPatch("LoadModData")]
            [HarmonyPostfix]
            public static void LoadModDataPatch()
            {
                if (!ModSave.Load(Plugin.instance.Info, out RaddudeSaveContainer saveContainer))
                {
                    Plugin.logger.LogWarning("Raddude.sailadex: Save file loading failed. File is either corrupt or does not exist. If this is the first time loading this save with this mod, this is normal.");
                    return;
                }

                if (saveContainer.caughtFish != null && Plugin.fishCaughtUIEnabled.Value) 
                {
                    foreach(KeyValuePair<string, int> fish in saveContainer.caughtFish)
                    {
                        if (fishCaughtUI.GetComponent<FishCaughtUI>().caughtFish.ContainsKey(fish.Key))
                        {
                            fishCaughtUI.GetComponent<FishCaughtUI>().caughtFish[fish.Key] = fish.Value;
                        }                        
                    }
                }

                if (saveContainer.visitedPorts != null && Plugin.portsVisitedUIEnabled.Value)
                {
                    foreach (KeyValuePair<string, bool> port in saveContainer.visitedPorts)
                    {
                        if (portsVisitedUI.GetComponent<PortsVisitedUI>().visitedPorts.ContainsKey(port.Key))
                        {
                            portsVisitedUI.GetComponent<PortsVisitedUI>().visitedPorts[port.Key] = port.Value;
                        }
                    }
                }
            }
        }

        [Serializable]
        public class RaddudeSaveContainer
        {
            public Dictionary<string, int> caughtFish;
            public Dictionary<string, bool> visitedPorts;
        }
    }
}
