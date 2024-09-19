﻿using SailwindModdingHelper;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace sailadex
{
    internal static class Names
    {
        public static string[] lagoonFish = { "swamp snapper", "blue bubbler", "fire fish" };

        public static List<string> fishNames = OceanFishes.instance.GetPrivateField<GameObject[]>("fishPrefabs")
            .Select(fish => fish.name)
            .ToList();

        public static string[] totalFishBadgeNames =
        {
            "caught50",
            "caught250",
            "caught500",
            "caughtAll"
        };

        public static string[] alankhPorts =
        {
            "Gold Rock City",
            "Al'Nilem",
            "Neverdin",
            "Albacore Town",
            "Alchemist's Island",
            "Al'Ankh Academy",
            "Oasis",
        };
        public static string[] emeraldPorts =
        {
            "Dragon Cliffs",
            "Sanctuary",
            "Crab Beach",
            "New Port",
            "Sage Hills",
            "Serpent Isle",
        };
        public static string[] mediPorts =
        {
            "Fort Aestrin",
            "Sunspire",
            "Mount Malefic",
            "Siren Song",
            "Eastwind",
            "Happy Bay",
            "Chronos",
        };
        public static string[] lagoonPorts =
        {
            "Kicia Bay",
            "Fire Fish Town",
            "On'na",
            "Sen'na"
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

        public static string[] capitals = 
        { 
            "grc", "dc", "fa", "kb"
        };

        public static string[] portBadgeNames = 
        {
            "alankhBadge",
            "emeraldBadge",
            "mediBadge",
            "lagoonBadge",
            "allPortsBadge"
        };

        public static string[] floatStatNames =
        {
            "CargoMass",
            "UnderwayTime",
            "MilesSailed"
        };

        public static string[] intStatNames =
        {
            "UnderwayDay",
            "PortsVisited",
            "MissionsCompleted",
            "DrinksTaken",
            "FoodsEaten",
            "TimesSmoked",
            "TimesSlept",
            "StormsWeathered",
            "FlotsamEncounters",
            "SeaLifeEncounters"
        };

        public static string[] transitNames =
        {
            "GrcDc",
            "GrcFa",
            "GrcKb",
            "DcGrc",
            "DcFa",
            "DcKb",
            "FaGrc",
            "FaDc",
            "FaKb",
            "KbGrc",
            "KbDc",
            "KbFa"
        };

    }
}
