using HarmonyLib;
using Verse;
using RimWorld;
using Verse.AI;
using VREAndroids;
using System.Collections.Generic;
using VAGUEArchite;
using System.Linq;

namespace VAGUEArchite
{
    //[HarmonyPatch(typeof(WorkGiver_ModifyAndroid), "JobOnThing")]
    //public class WorkGiverPatch_MakeBehaviorStationRequireItems
    //{
    //    [HarmonyPostfix]
    //    public static void Postfix(WorkGiver_ModifyAndroid __instance, Job __result, Pawn pawn, Thing thing, bool forced = false)
    //    {
    //        var station = (Building_AndroidBehavioristStation)thing;

    //        if (__result == null || station.requiredItems() is null)
    //        {
    //            return;
    //        }

    //        var chosenItems = new List<ThingCount>();
    //        var requiredItems = station.requiredItems();

    //        IEnumerable<IngredientCount> RequiredIngredients()
    //        {
    //            var ingCountList = new List<IngredientCount>();
    //            foreach (var item in requiredItems)
    //            {
    //                ingCountList.Add(new ThingDefCountClass(item.ThingDef, item.Count).ToIngredientCount());
    //            }
    //            return ingCountList;
    //        }

    //        var reqIngredients = RequiredIngredients().ToList();

    //        if (!AndroidCreationUtility.TryFindBestFixedIngredients(reqIngredients, pawn, station, chosenItems))
    //            JobFailReason.Is("VREA.MissingMaterials".Translate(string.Join(", ", reqIngredients.Select(x => x.ToString()))));

    //        else if (chosenItems.Any(x => !pawn.CanReserveAndReach(x.Thing, PathEndMode.ClosestTouch, Danger.Some)))
    //            JobFailReason.Is("VREA.MissingMaterials".Translate(string.Join(", ", reqIngredients.Select(x => x.ToString()))));

    //        else
    //        {
    //            if (station.curAndroidProject != null && station.currentWorkAmountDone > 0)
    //            {
    //                var resumeJob = JobMaker.MakeJob(VREA_DefOf.VREA_ModifyAndroid);
    //                resumeJob.targetA = station;
    //                __result = resumeJob;
    //                return;
    //            }


    //            var newJob = JobMaker.MakeJob(VREA_DefOf.VREA_ModifyAndroid);
    //            newJob.targetA = station;
    //            newJob.targetQueueB = new List<LocalTargetInfo>(chosenItems.Count);
    //            newJob.countQueue = new List<int>(chosenItems.Count);

    //            for (var i = 0; i < chosenItems.Count; i++)
    //            {
    //                newJob.targetQueueB.Add(chosenItems[i].Thing);
    //                newJob.countQueue.Add(chosenItems[i].Count);
    //            }
    //            __result = newJob;
    //        }
    //    }
    //}

    [HarmonyPatch(typeof(Window_AndroidModification), "AcceptInner")]
    public class WindowPatch_MakeBehaviorStationRequireItems
    {
        [HarmonyPostfix]
        public static void BehaviorWindow_AddItems(Window_AndroidModification __instance, List<ThingDefCount> ___requiredItems)
        {
            foreach (var gene in __instance.SelectedGenes)
            {
                if (gene is ArchiteAndroidGeneDef architeGene && !__instance.android.genes.HasActiveGene(gene))
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

                            return;
                        }

                        ThingDefCount TDCount = new ThingDefCount(TDCountClass.thingDef, TDCountClass.count);

                        if (___requiredItems is null)
                        {
                            ___requiredItems = new List<ThingDefCount>();
                        }

                        ___requiredItems?.Add(TDCount);
                    }
                }
            }
            __instance.station.requiredItems() = ___requiredItems;
        }
    }
}
