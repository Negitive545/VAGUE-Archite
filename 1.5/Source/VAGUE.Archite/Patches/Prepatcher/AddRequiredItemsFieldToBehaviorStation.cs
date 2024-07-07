using Prepatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using VREAndroids;

namespace VAGUEArchite
{
    static class PrepatcherFields {
        [PrepatcherField]
        public static extern ref List<ThingDefCount> requiredItems(this Building_AndroidBehavioristStation building_AndroidBehavioristStation);
    }
}
