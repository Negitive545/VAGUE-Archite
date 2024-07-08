using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using RimWorld;
using VREAndroids;

namespace VAGUEArchite.Patches.Harmony
{
    [HarmonyPatch(typeof(Hediff_AndroidReactor), "PowerEfficiencyDrainMultiplier")]
    [HarmonyPatch(MethodType.Getter)]
    public class ReactorPostAddPatch
    {
        public static void PostFix(Hediff_AndroidReactor __instance, float __result)
        {
            if (__instance.pawn.HasActiveGene(VAGUE_Archite_DefOf.VAGUE_Archite_ExpandedReactor))
            {
                __result /= 2;
            }
        }
    }
}
