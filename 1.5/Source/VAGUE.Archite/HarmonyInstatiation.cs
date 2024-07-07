using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;

namespace VAGUEArchite
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            Log.Message("VAGUE - Archite instantiating.");
            var harmonyInstance = new Harmony("com.VAGUE_Archite");
            harmonyInstance.PatchAll();
        }
    }
}
