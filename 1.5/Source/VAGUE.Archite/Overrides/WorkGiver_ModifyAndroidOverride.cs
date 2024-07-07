using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Verse.AI;
using VREAndroids;

namespace VAGUEArchite
{
    public class WorkGiver_ModifyAndroid : WorkGiver_Scanner
    {
        public override PathEndMode PathEndMode => PathEndMode.InteractionCell;
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(VREA_DefOf.VREA_AndroidBehavioristStation);
        public override Danger MaxPathDanger(Pawn pawn) => Danger.Some;

        public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            if (pawn.CurJob?.def != VREA_DefOf.VREA_ModifyAndroid && thing is Building_AndroidBehavioristStation station && pawn.CanReserveAndReach(thing, PathEndMode.Touch, MaxPathDanger(pawn)))
            {
                

                IEnumerable<IngredientCount> RequiredIngredients()
                {
                    if (station.requiredItems() == null)
                    {
                        return new List<IngredientCount>();
                    }
                    var ingredientCountList = new List<IngredientCount>();
                    foreach (var data in station.requiredItems())
                    {
                        ingredientCountList.Add(new ThingDefCountClass(data.ThingDef, data.Count).ToIngredientCount());
                    }
                    return ingredientCountList;
                }


                if (!station.ReadyForModifying(pawn, out var failReason))
                    JobFailReason.Is(failReason);
                else
                {
                    if (station.currentWorkAmountDone > 0)
                    {
                        var job = JobMaker.MakeJob(VREA_DefOf.VREA_ModifyAndroid);
                        job.targetA = station;
                        return job;
                    }
                    else
                    {
                        var chosen = new List<ThingCount>();
                        var requiredIngredients = RequiredIngredients().ToList();
                        if (!AndroidCreationUtility.TryFindBestFixedIngredients(requiredIngredients, pawn, station, chosen))
                            JobFailReason.Is("VREA.MissingMaterials".Translate(string.Join(", ", requiredIngredients.Select(x => x.ToString()))));
                        else if (chosen.Any(x => !pawn.CanReserveAndReach(x.Thing, PathEndMode.ClosestTouch, MaxPathDanger(pawn))))
                            JobFailReason.Is("VREA.MissingMaterials".Translate(string.Join(", ", requiredIngredients.Select(x => x.ToString()))));
                        else
                        {
                            var job = JobMaker.MakeJob(VREA_DefOf.VREA_ModifyAndroid);
                            job.targetA = station;
                            job.targetQueueB = new List<LocalTargetInfo>(chosen.Count);
                            job.countQueue = new List<int>(chosen.Count);
                            for (var i = 0; i < chosen.Count; i++)
                            {
                                job.targetQueueB.Add(chosen[i].Thing);
                                job.countQueue.Add(chosen[i].Count);
                            }
                            return job;
                        }
                    }
                }
            }
            return null;
        }
    }
}
