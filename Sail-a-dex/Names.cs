﻿using SailwindModdingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace sailadex
{
    internal class Names
    {
        public static string[] fishNames = OceanFishes.instance.GetPrivateField<GameObject[]>("fishPrefabs")
            .Select(fish => fish.name)
            .ToArray();

        public static string[] totalFishBadgeNames =
        {
            "caught50",
            "caught250",
            "caught500",
            "caughtAll"
        };

        public static string[] portNames =
        {
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
            //Aestrin(medi)
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

        public static string[] portBadgeNames = 
        {
            "alankhBadge",
            "emeraldBadge",
            "mediBadge",
            "lagoonBadge",
            "allPortsBadge"
        };

    }
}
