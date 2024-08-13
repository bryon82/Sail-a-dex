﻿using HarmonyLib;
using SailwindModdingHelper;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace sailadex
{
    internal class AssetsLoader
    {
        public static Dictionary<string, Material> materials;
        public static Dictionary<string, Texture2D> textures;

        public static void Start()
        {
            materials = new Dictionary<string, Material>();
            textures = new Dictionary<string, Texture2D>();
        }

        public static void LoadFishBadges()
        {
            var fishBadgesPath = Path.Combine(Extensions.GetFolderLocation(Plugin.instance.Info), "assets", "badges", "fish");
            int[] amountNums = { 25, 50, 100 };
            Texture2D tempTexture;
            string fishBadgeName;

            foreach (string fishName in Names.fishNames)
            {
                for (int i = 0; i < 3; i++)
                {
                    //Debug.Log("Start " + FishCaughtUI.ShortFishName(fish.name) + amountNums[i] + " load");
                    fishBadgeName = FishCaughtUI.ShortFishName(fishName) + amountNums[i];
                    tempTexture = LoadTexture(Path.Combine(fishBadgesPath, fishBadgeName + ".png"));
                    //Debug.Log("Texture loaded, adding material");
                    textures.Add(fishBadgeName, tempTexture);
                    materials.Add(fishBadgeName, CreateMaterial(tempTexture));
                }
            }

            foreach (string caughtBadge in Names.totalFishBadgeNames)
            {                
                //Debug.Log("Start " + caughtBadge + " load");
                fishBadgeName = caughtBadge;
                tempTexture = LoadTexture(Path.Combine(fishBadgesPath, fishBadgeName + ".png"));
                //Debug.Log("Texture loaded, adding material");
                textures.Add(fishBadgeName, tempTexture);
                materials.Add(fishBadgeName, CreateMaterial(tempTexture));                
            }

            Plugin.logger.LogInfo("Fishing badges loaded.");
        }

        public static void LoadPortBadges()
        {
            var portBadgesPath = Path.Combine(Extensions.GetFolderLocation(Plugin.instance.Info), "assets", "badges", "ports");
            Texture2D tempTexture;

            foreach (string pbName in Names.portBadgeNames)
            {
                tempTexture = LoadTexture(Path.Combine(portBadgesPath, pbName + ".png"));
                textures.Add(pbName, tempTexture);
                materials.Add(pbName, CreateMaterial(tempTexture));                
            }
            Plugin.logger.LogInfo("Port badges loaded.");
        }

        private static Texture2D LoadTexture(string path)
        {
            byte[] array = File.Exists(path) ? File.ReadAllBytes(path) : null;
            Texture2D texture2D = new Texture2D(1, 1);
            if (array == null)
            {
                Plugin.logger.LogError("Failed to load " + path);
                return texture2D;                
            }
            ImageConversion.LoadImage(texture2D, array);
            return texture2D;
        }

        private static Material CreateMaterial(Texture2D tex)
        {
            Material material = new Material(Shader.Find("Standard"))
            {
                renderQueue = 2001,
                mainTexture = tex
            };
            material.EnableKeyword("_ALPHATEST_ON");
            material.SetShaderPassEnabled("ShadowCaster", false);
            return material;
        }
    }
}
