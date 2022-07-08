using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch(typeof(DummyBehaviour), nameof(DummyBehaviour.Update))]
    public class Freeplaychanges 
    {
        [HarmonyPrefix]
        public static bool prefix()
        {
            
            return false;
        }
    
        [HarmonyPostfix]

        public static void postfix(DummyBehaviour __instance)
        {
            if (Input.GetKeyDown(KeyCode.F2)) __instance.StartCoroutine_Auto(__instance.DoVote());
        
        }
    }
}