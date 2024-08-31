using SailwindModdingHelper;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace sailadex
{
    public class StatsUI : MonoBehaviour
    {
        public static StatsUI instance;
        public Dictionary<string, float> floatStats;
        public Dictionary<string, int> intStats;
        public Dictionary<string, bool[]> boolArrayStats;
        public Dictionary<string, TextMesh> statTMs;
        private Vector3 lastPosition;
        private string lastPortVisited;

        private void Awake()
        {
            instance = this;
            floatStats = new Dictionary<string, float>();
            intStats = new Dictionary<string, int>();
            boolArrayStats = new Dictionary<string, bool[]>();
            statTMs = new Dictionary<string, TextMesh>();
            lastPosition = new Vector3();
            lastPortVisited = "";

            foreach (string stat in Names.floatStatNames)
            {
                floatStats.Add(stat, 0f);
                floatStats.Add("current" + stat, 0f);
                floatStats.Add("record" + stat, 0f);
            }
            
            foreach (string stat in Names.intStatNames)
            {
                intStats.Add(stat, 0);
                intStats.Add("current" + stat, 0);
                intStats.Add("record" + stat, 0);
            }            

            foreach (string transit in Names.transitNames)
            {
                floatStats.Add("last" + transit + "TransitTime", 0f);
                intStats.Add("last" + transit + "TransitDay", 0);
                floatStats.Add("record" + transit + "TransitTime", 0f);
                intStats.Add("record" + transit + "TransitDay", 0);
            }

            foreach (string capital in Names.capitals)
            {
                floatStats.Add(capital + "UnderwayTime", 0f);
                intStats.Add(capital + "UnderwayDay", 0);
                boolArrayStats.Add(capital + "Transit", new bool[4]);
            }
        }

        public void RegisterCurrentMass()
        {
            if (GameState.currentBoat?.parent == null && GameState.lastBoat == null) return;

            var boatGameObject = GameState.currentBoat != null ? GameState.currentBoat.parent.gameObject : GameState.lastBoat.gameObject;  
            floatStats["currentCargoMass"] = boatGameObject
                .GetComponent<BoatMass>()
                .GetPrivateField<List<ItemRigidbody>>("itemsOnBoat")
                .Where(item => item.GetShipItem().GetComponent<Good>() != null
                    && (item.GetShipItem().GetComponent<Good>().sizeDescription.Contains("crate")
                    || item.GetShipItem().GetComponent<Good>().sizeDescription.Contains("package")
                    || item.GetShipItem().GetComponent<Good>().sizeDescription.Contains("barrel")))
                .Sum(item => item.GetBody().mass);
        }

        public void RegisterUnderway(string islandName)
        {
            if (islandName == null || islandName == "")
                return;

            floatStats["UnderwayTime"] = Sun.sun.globalTime;
            intStats["UnderwayDay"] = GameState.day;

            if (islandName.Contains("1 A")) {
                floatStats["grcUnderwayTime"] = Sun.sun.globalTime;
                intStats["grcUnderwayDay"] = GameState.day;
                for (int i = 0; i < 4; i++)
                {
                    boolArrayStats["grcTransit"][i] = false;
                }                
                return;
            }
            if (islandName.Contains("9 E"))
            {
                floatStats["dcUnderwayTime"] = Sun.sun.globalTime;
                intStats["dcUnderwayDay"] = GameState.day;
                for (int i = 0; i < 4; i++)
                {
                    boolArrayStats["dcTransit"][i] = false;
                }
                return;
            }
            if (islandName.Contains("15 M"))
            {
                floatStats["faUnderwayTime"] = Sun.sun.globalTime;
                intStats["faUnderwayDay"] = GameState.day;
                for (int i = 0; i < 4; i++)
                {
                    boolArrayStats["faTransit"][i] = false;
                }
                return;
            }
            if (islandName.Contains("27 Lagoon"))
            {
                floatStats["kbUnderwayTime"] = Sun.sun.globalTime;
                intStats["kbUnderwayDay"] = GameState.day;
                for (int i = 0; i < 4; i++)
                {
                    boolArrayStats["kbTransit"][i] = false;
                }
            }
        }

        public void RegisterMoored(string islandName)
        {
            if (islandName == null || islandName == "")
                return;

            if (intStats["currentUnderwayDay"] > intStats["recordUnderwayDay"] 
                || (intStats["currentUnderwayDay"] == intStats["recordUnderwayDay"]
                && floatStats["currentUnderwayTime"] > floatStats["recordUnderwayTime"]))
            {
                intStats["recordUnderwayDay"] = intStats["currentUnderwayDay"];
                floatStats["recordUnderwayTime"] = floatStats["currentUnderwayTime"];
            }

            floatStats["UnderwayTime"] = 0f;
            intStats["UnderwayDay"] = 0;

            // fastest transit
            if (islandName.Contains("1 A"))
            {               
                if (!boolArrayStats["dcTransit"][0] && (floatStats["dcUnderwayTime"] > 0f || intStats["dcUnderwayDay"] > 0))
                    CheckTransitTime("dc", "DcGrc", 0);
                if (!boolArrayStats["faTransit"][0] && (floatStats["faUnderwayTime"] > 0f || intStats["faUnderwayDay"] > 0))
                    CheckTransitTime("fa", "FaGrc", 0);
                if (!boolArrayStats["kbTransit"][0] && (floatStats["kbUnderwayTime"] > 0f || intStats["kbUnderwayDay"] > 0))
                    CheckTransitTime("kb", "KbGrc", 0);
                return;
            }
            if (islandName.Contains("9 E"))
            {   
                if (!boolArrayStats["grcTransit"][1] && (floatStats["grcUnderwayTime"] > 0f || intStats["grcUnderwayDay"] > 0))
                    CheckTransitTime("grc", "GrcDc", 1);
                if (!boolArrayStats["faTransit"][1] && (floatStats["faUnderwayTime"] > 0f || intStats["faUnderwayDay"] > 0))
                    CheckTransitTime("fa", "FaDc", 1);
                if (!boolArrayStats["kbTransit"][1] && (floatStats["kbUnderwayTime"] > 0f || intStats["kbUnderwayDay"] > 0))
                    CheckTransitTime("kb", "KbDc", 1);
                return;
            }
            if (islandName.Contains("15 M"))
            {
                if (!boolArrayStats["grcTransit"][2] && (floatStats["grcUnderwayTime"] > 0f || intStats["grcUnderwayDay"] > 0))
                    CheckTransitTime("grc", "GrcFa", 2);
                if (!boolArrayStats["dcTransit"][2] && (floatStats["dcUnderwayTime"] > 0f || intStats["dcUnderwayDay"] > 0))
                    CheckTransitTime("dc", "DcFa", 2);
                if (!boolArrayStats["kbTransit"][2] && (floatStats["kbUnderwayTime"] > 0f || intStats["kbUnderwayDay"] > 0))
                    CheckTransitTime("kb", "KbFa", 2);
                return;
            }
            if (islandName.Contains("27 Lagoon"))
            {
                if (!boolArrayStats["grcTransit"][3] && (floatStats["grcUnderwayTime"] > 0f || intStats["grcUnderwayDay"] > 0))
                    CheckTransitTime("grc", "GrcKb", 3);
                if (!boolArrayStats["dcTransit"][3] && (floatStats["dcUnderwayTime"] > 0f || intStats["dcUnderwayDay"] > 0))
                    CheckTransitTime("dc", "DcKb", 3);
                if (!boolArrayStats["faTransit"][3] && (floatStats["faUnderwayTime"] > 0f || intStats["faUnderwayDay"] > 0))
                    CheckTransitTime("fa", "FaKb", 3);
            }            
        }

        public void CheckTransitTime(string underwayKey, string transitCode, int destInt)
        {
            var transitDay = GameState.day - intStats[underwayKey + "UnderwayDay"];
            var transitTime = Sun.sun.globalTime - floatStats[underwayKey + "UnderwayTime"];
            intStats["last" + transitCode + "TransitDay"] = transitDay;
            floatStats["last" + transitCode + "TransitTime"] = transitTime;


            if (transitTime < 0f)
            {
                transitTime += 24f;
                transitDay--;
            }

            if ((intStats["record" + transitCode + "TransitDay"] == 0
                && floatStats["record" + transitCode + "TransitTime"] == 0f)
                || intStats["record" + transitCode + "TransitDay"] > transitDay
                || (intStats["record" + transitCode + "TransitDay"] == transitDay
                && floatStats["record" + transitCode + "TransitTime"] > transitTime))
            {
                intStats["record" + transitCode + "TransitDay"] = transitDay;
                floatStats["record" + transitCode + "TransitTime"] = transitTime;
                if (Plugin.notificationsEnabled.Value)
                    NotificationUiQueue.instance.QueueNotification($"Fastest {AddTo(transitCode)} time");
            }
            boolArrayStats[underwayKey + "Transit"][destInt] = true;
        }

        public void IncrementIntStat(string statName)
        {
            intStats["current" + statName]++;
        }

        public void UpdatePage()
        {
            UpdateStats();
            UpdateTexts();
            //UpdateBadges();
        }

        private void UpdateStats()
        {
            if (floatStats["recordCargoMass"] < floatStats["currentCargoMass"] && GameState.distanceToLand > 300f)
            {
                floatStats["recordCargoMass"] = floatStats["currentCargoMass"];
            }

            if (intStats["UnderwayDay"] > 0 || floatStats["UnderwayTime"] > 0f)
            {
                intStats["currentUnderwayDay"] = GameState.day - intStats["UnderwayDay"];
                floatStats["currentUnderwayTime"] = Sun.sun.globalTime - floatStats["UnderwayTime"];
            }
            if (floatStats["currentUnderwayTime"] < 0f)
            {
                floatStats["currentUnderwayTime"] += 24f;
                intStats["currentUnderwayDay"]--;
            }
        }

        private void UpdateTexts()
        {
            foreach (string stat in Names.floatStatNames)
            {                
                switch (stat)
                {
                    case "UnderwayTime":
                        statTMs[stat].text = AddSpace(stat);
                        statTMs["currentUnderwayTime"].text = UnderwayText(intStats["currentUnderwayDay"], floatStats["currentUnderwayTime"]);
                        statTMs["recordUnderwayTime"].text = UnderwayText(intStats["recordUnderwayDay"], floatStats["recordUnderwayTime"]);
                        break;
                    case "CargoMass":
                        statTMs[stat].text = AddSpace(stat);
                        statTMs["currentCargoMass"].text = floatStats["currentCargoMass"] == 0f ? "-" : $"{floatStats["currentCargoMass"]:#,##0.#} lbs";
                        statTMs["recordCargoMass"].text = floatStats["recordCargoMass"] == 0f ? "-" : $"{floatStats["recordCargoMass"]:#,##0.#} lbs";
                        break;
                    case "MilesSailed":
                        statTMs[stat].text = AddSpace(stat);
                        if (Plugin.realTimeMilesSailed.Value)
                            statTMs["currentMilesSailed"].text = $"{floatStats["currentMilesSailed"]:#,##0.#}";
                        else
                            statTMs["currentMilesSailed"].text = $"{floatStats["MilesSailed"]:#,##0.#}";
                        break;
                    default:
                        statTMs[stat].text = AddSpace(stat);
                        statTMs["current" + stat].text = floatStats["current" + stat].ToString();
                        statTMs["record" + stat].text = floatStats["record" + stat].ToString();
                        break;
                }
            }

            foreach (string stat in Names.intStatNames)
            {                
                switch (stat)
                {
                    case "UnderwayDay":
                        break;
                    default:
                        statTMs[stat].text = AddSpace(stat);
                        statTMs["current" + stat].text = $"{intStats["current" + stat]:#,##0}";
                        break;
                }
            }

            foreach (string transit in Names.transitNames)
            {
                statTMs[transit].text = AddTo(transit);
                statTMs["last" + transit].text = UnderwayText(intStats["last" + transit + "TransitDay"], floatStats["last" + transit + "TransitTime"]);
                statTMs["record" + transit].text = UnderwayText(intStats["record" + transit + "TransitDay"], floatStats["record" + transit + "TransitTime"]);
            }
        }

        private string UnderwayText(int underwayDay, float underwayTime)
        {
            if (underwayDay == 0 && underwayTime == 0f)
                return "-";
            if (underwayDay > 0)
            {
                var dayText = underwayDay == 1 ? "Day" : "Days";
                return $"{underwayDay} {dayText} {underwayTime:0.0} Hours";
            }
            else
            {
                return $"{underwayTime:0.0} Hours";
            }
        }

        private string AddSpace(string name)
        {
            return Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");
        }

        private string AddTo(string name)
        {
            var temp = AddSpace(name);
            return temp.ToUpper().Insert(temp.IndexOf(' '), " to");
        }

        public void PlayerTeleported()
        {
            foreach (string capital in Names.capitals)
            {
                int j = 0;
                for (int i = 0; i < 4; i++) 
                {
                    if (i != j)
                        boolArrayStats[capital + "Transit"][i] = true;
                    j++;
                }
            }
        }

        public void TrackDistance()
        {
            var currentPosition = new Vector3(GameState.currentBoat.position.x, 0f, GameState.currentBoat.position.z);
            if (Mathf.Abs(lastPosition.x) < 0.1f || Mathf.Abs(lastPosition.z) < 0.1f)
            {
                Plugin.logger.LogDebug($"World Shift: x {lastPosition.x} z: {lastPosition.z}");
                lastPosition = currentPosition;                
                return;
            }

            floatStats["currentMilesSailed"] += Vector3.Distance(lastPosition, currentPosition) / 300f;
            lastPosition = currentPosition;
        }

        public void UpdateMilesText()
        {
            floatStats["MilesSailed"] = floatStats["currentMilesSailed"]; 
        }

        public void IncrementPortVisited(string port)
        {
            if (lastPortVisited == port) return;
            IncrementIntStat("PortsVisited");
            lastPortVisited = port;
        }
    }
}