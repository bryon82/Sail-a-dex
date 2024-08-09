using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace sailadex
{
    public class PortsVisitedUI : MonoBehaviour
    {
        public static PortsVisitedUI instance;
        public Dictionary<string, bool> visitedPorts;
        public TextMesh[] portNameTMs;
        public TextMesh[] portVisitedTMs;

        private void Awake()
        {
            instance = this;
            visitedPorts = new Dictionary<string, bool>();

            string[] names = {
                //Al'Ankh
                "Gold Rock City",
                "Al'Nilem",
                "Neverdin",
                "Albacore Town",
                "Alchemist's Island",
                "Al'Ankh Academy",
                "Oasis",
                //Emerald Archipelago
                "Dragon Cliffs",
                "Sanctuary",
                "Crab Beach",
                "New Port",
                "Sage Hills",
                "Serpent Isle",
                //Aestrin
                "Fort Aestrin",
                "Sunspire",
                "Mount Malefic",
                "Siren Song",
                "Eastwind",
                "Happy Bay",
                "Chronos",
                //Fire Fish Lagoon
                "Kicia Bay",
                "Fire Fish Town",
                "On'na",
                "Sen'na" 
            };
            
            foreach (string port in names)
            {
                visitedPorts.Add(port, false);
            }
        }

        public void RegisterVisit(string portName)
        {
            if (visitedPorts.ContainsKey(portName))
            {
                visitedPorts[portName] = true;
                //Debug.Log("Visited: " + portName);
            }            
        }

        public void UpdateTexts()
        {
            int i = 0;
            foreach (KeyValuePair<string, bool> port in visitedPorts)
            {
                if (Plugin.portNamesHidden.Value)
                    portNameTMs[i].text = port.Value ? port.Key : "???";
                else
                    portNameTMs[i].text = port.Key;
                portVisitedTMs[i].text = port.Value ? "✓" : "✗";
                //Debug.Log(port.Key + " " + port.Value);
                i++;
            }
        }        
    }
}
