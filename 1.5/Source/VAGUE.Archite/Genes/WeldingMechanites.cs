using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VREAndroids;
using RimWorld;
using Verse;

namespace VAGUEArchite
{
    public class WeldingMechanites : Gene
    {
        public int ticksPerHour = 2500;
        public int ticksPerSecond = 60;

        public int healAmount = 1;

        public int healInterval = 1250;

        public override void PostAdd()
        {
            base.PostAdd();
        }

        public override void Tick()
        {
            base.Tick();

            if (pawn.IsHashIntervalTick(healInterval))
            {
                var injuryList = pawn.health.hediffSet.hediffs.FindAll(x => x is Hediff_Injury);
                if (injuryList.Count > 0)
                {
                    List<Hediff> bleeders = injuryList.FindAll(x => x.Bleeding);

                    if (bleeders != null && bleeders.Count > 0) 
                    {
                        var sortedList = bleeders.OrderByDescending(x => x.BleedRate).ToList();

                        sortedList[0].Tended(1f, 1f);
                        sortedList[0].Heal(healAmount);
                        return;
                    } 
                    else
                    {
                        var sortedList = injuryList.OrderByDescending(x => x.Severity).ToList();

                        if (!sortedList[0].IsTended())
                        {
                            sortedList[0].Tended(1f, 1f);
                        }
                        sortedList[0].Heal(healAmount);
                        return;
                    }
                }
            }
        }
    }
}
