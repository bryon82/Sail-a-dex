using System.Collections.Generic;
using System.Linq;
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
            if (visitedPorts.ContainsKey(portName) && !visitedPorts[portName])
            {                   
                visitedPorts[portName] = true;
                CheckBadges();
            }
            Plugin.logger.LogDebug("Visited: " + portName);
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
                i++;
            }
        }
        
        public void CheckBadges()
        {            
            var alankhBadge = Names.alankhPorts.All(p => visitedPorts[p]);
            var emeraldBadge = Names.emeraldPorts.All(p => visitedPorts[p]);
            var mediBadge = Names.mediPorts.All(p => visitedPorts[p]);
            var lagoonBadge = Names.lagoonPorts.All(p => visitedPorts[p]);

            if (!portBadges["alankhBadge"] && alankhBadge)
            {
                if (Plugin.notificationsEnabled.Value)
                    NotificationUiQueue.instance.QueueNotification("Visited all Al'Ankh ports");
                portBadges["alankhBadge"] = true;
            }
            if (!portBadges["emeraldBadge"] && emeraldBadge)
            {
                if (Plugin.notificationsEnabled.Value)
                    NotificationUiQueue.instance.QueueNotification("Visited all Emerald\nArchipelago ports");
                portBadges["emeraldBadge"] = true;
            }
            if (!portBadges["mediBadge"] && mediBadge)
            {
                if (Plugin.notificationsEnabled.Value)
                    NotificationUiQueue.instance.QueueNotification("Visited all Aestrin ports");
                portBadges["mediBadge"] = true;
            }
            if (!portBadges["lagoonBadge"] && lagoonBadge)
            {
                if (Plugin.notificationsEnabled.Value)
                    NotificationUiQueue.instance.QueueNotification("Visited all Fire\nFish Lagoon ports");
                portBadges["lagoonBadge"] = true;
            }
            if (!portBadges["allPortsBadge"] && alankhBadge && emeraldBadge && mediBadge && lagoonBadge)
            {
                if (Plugin.notificationsEnabled.Value)
                    NotificationUiQueue.instance.QueueNotification("Visited all ports");
                portBadges["allPortsBadge"] = true;
            }           
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
