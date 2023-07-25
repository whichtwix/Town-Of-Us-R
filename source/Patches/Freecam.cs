using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch]
    public class Freecam
    {
        [HarmonyPatch(typeof(FollowerCamera), nameof(FollowerCamera.Update))]
        [HarmonyPostfix]

        public static void Postfix(FollowerCamera __instance)
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                __instance.Target =  __instance.Target != null ? null : PlayerControl.LocalPlayer;
            }

            if (__instance.Target)
            {
                PlayerControl.LocalPlayer.moveable = true;
                HudManager.Instance.ShadowQuad.enabled = true;
                return;
            }
            PlayerControl.LocalPlayer.moveable = false;
            HudManager.Instance.ShadowQuad.enabled = false;

            if (Input.GetKeyDown(KeyCode.W))
            {
                __instance.transform.position = new(__instance.centerPosition.x, __instance.centerPosition.y + 1);
                __instance.centerPosition = __instance.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                __instance.transform.position = new(__instance.centerPosition.x - 1, __instance.centerPosition.y);
                __instance.centerPosition = __instance.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                __instance.transform.position = new(__instance.centerPosition.x, __instance.centerPosition.y - 1);
                __instance.centerPosition = __instance.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                __instance.transform.position = new(__instance.centerPosition.x + 1, __instance.centerPosition.y);
                __instance.centerPosition = __instance.transform.position;
            }
        }
        
    }
}