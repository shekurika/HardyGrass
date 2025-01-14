﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.TerrainFeatures;
using Location = xTile.Dimensions.Location;
using Harmony;

namespace HardyGrass
{
    [HarmonyPatch(typeof(GameLocation))]
    [HarmonyPatch("growWeedGrass", new Type[] { typeof(int) })]
    public class GameLocation_growWeedGrass_Patch
    {
        static bool Prefix(GameLocation __instance, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                for (int j = __instance.terrainFeatures.Count() - 1; j >= 0; j--)
                {
                    KeyValuePair<Vector2, TerrainFeature> kvp = __instance.terrainFeatures.Pairs.ElementAt(j);
                    if (kvp.Value is Grass grass && (int)grass.numberOfWeeds > 0)
                    {
                        bool isQuick = ModEntry.GrassIsQuick(grass);
                        if ((int)grass.numberOfWeeds < 4)
                        {
                            // This is to mimic vanilla behavior of the "spread" having a chance to grow again if the grass patch isn't full.
                            if (!ModEntry.config.simplifyGrassGrowth)
                            {
                                grass.numberOfWeeds.Value = Utility.Clamp((int)grass.numberOfWeeds + ModEntry.CalculateTuftsToAdd(isQuick, ModEntry.GrowthType.SpreadConsolidate), 0, 4);
                            }

                            return false;
                        }

                        int X = (int)kvp.Key.X;
                        int Y = (int)kvp.Key.Y;

                        if (!__instance.isTileOnMap(kvp.Key))
                        {
                            return false;
                        }

                        if (!__instance.isTileOccupied(kvp.Key + new Vector2(-1f, 0f)) && __instance.isTileLocationOpenIgnoreFrontLayers(new Location(X - 1, Y)) && __instance.doesTileHaveProperty(X - 1, Y, "Diggable", "Back") != null && __instance.doesTileHaveProperty(X - 1, Y, "NoSpawn", "Back") == null)
                        {
                            int tuftsToAdd = ModEntry.CalculateTuftsToAdd(isQuick, ModEntry.GrowthType.Spread);
                            if (tuftsToAdd > 0)
                            {
                                Grass newGrass = new Grass((byte)grass.grassType, tuftsToAdd);
                                if (isQuick)
                                {
                                    newGrass.modData.Add(ModEntry.IsQuickModDataKey, ModEntry.IsQuickModDataValue);
                                }
                                __instance.terrainFeatures.Add(kvp.Key + new Vector2(-1f, 0f), newGrass);
                            }
                        }
                        if (!__instance.isTileOccupied(kvp.Key + new Vector2(1f, 0f)) && __instance.isTileLocationOpenIgnoreFrontLayers(new Location(X + 1, Y)) && __instance.doesTileHaveProperty(X + 1, Y, "Diggable", "Back") != null && __instance.doesTileHaveProperty(X + 1, Y, "NoSpawn", "Back") == null)
                        {
                            int tuftsToAdd = ModEntry.CalculateTuftsToAdd(isQuick, ModEntry.GrowthType.Spread);
                            if (tuftsToAdd > 0)
                            {
                                Grass newGrass = new Grass((byte)grass.grassType, tuftsToAdd);
                                if (isQuick)
                                {
                                    newGrass.modData.Add(ModEntry.IsQuickModDataKey, ModEntry.IsQuickModDataValue);
                                }
                                __instance.terrainFeatures.Add(kvp.Key + new Vector2(1f, 0f), newGrass);
                            }
                        }
                        if (!__instance.isTileOccupied(kvp.Key + new Vector2(0f, 1f)) && __instance.isTileLocationOpenIgnoreFrontLayers(new Location(X, Y + 1)) && __instance.doesTileHaveProperty(X, Y + 1, "Diggable", "Back") != null && __instance.doesTileHaveProperty(X, Y + 1, "NoSpawn", "Back") == null)
                        {
                            int tuftsToAdd = ModEntry.CalculateTuftsToAdd(isQuick, ModEntry.GrowthType.Spread);
                            if (tuftsToAdd > 0)
                            {
                                Grass newGrass = new Grass((byte)grass.grassType, tuftsToAdd);
                                if (isQuick)
                                {
                                    newGrass.modData.Add(ModEntry.IsQuickModDataKey, ModEntry.IsQuickModDataValue);
                                }
                                __instance.terrainFeatures.Add(kvp.Key + new Vector2(0f, 1f), newGrass);
                            }
                        }
                        if (!__instance.isTileOccupied(kvp.Key + new Vector2(0f, -1f)) && __instance.isTileLocationOpenIgnoreFrontLayers(new Location(X, Y - 1)) && __instance.doesTileHaveProperty(X, Y - 1, "Diggable", "Back") != null && __instance.doesTileHaveProperty(X, Y - 1, "NoSpawn", "Back") == null)
                        {
                            int tuftsToAdd = ModEntry.CalculateTuftsToAdd(isQuick, ModEntry.GrowthType.Spread);
                            if (tuftsToAdd > 0)
                            {
                                Grass newGrass = new Grass((byte)grass.grassType, tuftsToAdd);
                                if (isQuick)
                                {
                                    newGrass.modData.Add(ModEntry.IsQuickModDataKey, ModEntry.IsQuickModDataValue);
                                }
                                __instance.terrainFeatures.Add(kvp.Key + new Vector2(0f, -1f), newGrass);
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}