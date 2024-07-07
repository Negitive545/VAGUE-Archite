using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using VREAndroids;

namespace VAGUEArchite
{
    [HarmonyPatch(typeof(Window_AndroidCreation), "OnGenesChanged")]
    public class AddReqItemsToCreationStation
    {
        [HarmonyPostfix]
        public static void AddItems(Window_AndroidCreation __instance, List<ThingDefCount> ___requiredItems, List<GeneDef> ___selectedGenes)
        {
            foreach (var gene in ___selectedGenes)
            {
                if (gene is ArchiteAndroidGeneDef architeGene)
                {
                    foreach (ThingDefCountClass TDCountClass in architeGene.requiredItems)
                    {
                        int existingIndex = -1;
                        if (___requiredItems != null)
                        {
                            existingIndex = ___requiredItems.FindIndex(x => x.ThingDef == TDCountClass.thingDef);
                        }
                        if (existingIndex >= 0)
                        {
                            int newCount = ___requiredItems[existingIndex].Count + TDCountClass.count;
                            ___requiredItems[existingIndex] = ___requiredItems[existingIndex].WithCount(newCount);
                        } else {
                            ThingDefCount TDCount = new ThingDefCount(TDCountClass.thingDef, TDCountClass.count);

                            ___requiredItems.Add(TDCount);
                        }
                    }
                }
            }
        }
    }
}