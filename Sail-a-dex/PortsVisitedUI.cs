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
        public Dictionary<string, bool> portBadges;
        public Dictionary<string, GameObject> portBadgeGOs;

        private void Awake()
        {
            instance = this;
            visitedPorts = new Dictionary<string, bool>();
            portBadges = new Dictionary<string, bool>();
            portBadgeGOs = new Dictionary<string, GameObject>();
            
            foreach (string port in Names.portNames)
            {
                visitedPorts.Add(port, false);
            }

            foreach (string portBadge in Names.portBadgeNames) 
            { 
                portBadges.Add(portBadge, false);
            }

        }

        public void RegisterVisit(string portName)
        {
            if (visitedPorts.ContainsKey(portName))
            {
                if (!visitedPorts[portName])
                {                    
                    visitedPorts[portName] = true;
                    CheckBadges();
                }                
                //Debug.Log("Visited: " + portName);
            }            
        }

        public void UpdatePage()
        {
            UpdateTexts();
            UpdateBadges();
        }

        private void UpdateTexts()
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
        
        public void CheckBadges()
        {
            var allPorts = true;

            // Al'Ankh badge
            var port = true;
            for (int i = 0; i < 7; i++)
            {
                if (!visitedPorts[Names.portNames[i]])
                {
                    allPorts = false;
                    port = false;
                }               
            }
            portBadges["alankhBadge"] = port;

            // Emerald Archpelago badge
            port = true;
            for (int i = 7; i < 13; i++)
            {
                if (!visitedPorts[Names.portNames[i]])
                {
                    allPorts = false;
                    port = false;
                }
            }
            portBadges["emeraldBadge"] = port;

            // Aestrin badge
            port = true;
            for (int i = 13; i < 20; i++)
            {
                if (!visitedPorts[Names.portNames[i]])
                {
                    allPorts = false;
                    port = false;
                }
            }
            portBadges["mediBadge"] = port;

            // Fire Fish Lagoon badge
            port = true;
            for (int i = 20; i < 24; i++)
            {
                if (!visitedPorts[Names.portNames[i]])
                {
                    allPorts = false;
                    port = false;
                }
            }
            portBadges["lagoonBadge"] = port;

            portBadges["allPortsBadge"] = allPorts;
        }

        public void UpdateBadges()
        {
            foreach (KeyValuePair<string, bool> badge in portBadges)
            {            
                portBadgeGOs[badge.Key].SetActive(badge.Value);                
            }
        }
    }
}
