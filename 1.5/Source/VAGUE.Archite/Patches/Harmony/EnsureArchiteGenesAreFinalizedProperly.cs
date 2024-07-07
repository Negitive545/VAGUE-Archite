using HarmonyLib;
using Verse;
using VREAndroids;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace VAGUEArchite
{
    [HarmonyPatch(typeof(Building_AndroidBehavioristStation), "FinishAndroidProject")] 
    public class EnsureArchiteGenesAreFinalizedProperly
    {
        [HarmonyPrefix]
        public static bool Prefix(Building_AndroidBehavioristStation __instance, CustomXenotype ___curAndroidProject)
        {
            List<Gene> Xenogenes = __instance.Occupant.genes.Xenogenes.ToList();
            foreach (Gene gene in Xenogenes)
            {
                if (DefDatabase<ArchiteAndroidGeneDef>.GetNamed(gene.def.defName, errorOnFail:false) != null)
                {
                    __instance.Occupant.genes.RemoveGene(gene);
                }
            }
            return true;
        }
    }
}
