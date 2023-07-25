using HarmonyLib;
using UnityEngine;

namespace TownOfUs
{
    [HarmonyPatch(typeof(RoomTracker), nameof(RoomTracker.FixedUpdate))]
    public class Killself
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            /*System.Random chance = new System.Random();
            
            int num = chance.Next(0,1000);

            if (num == 999)
            {
                Utils.RpcMurderPlayer(PlayerControl.LocalPlayer, PlayerControl.LocalPlayer);

            } */
            if (Input.GetKeyDown(KeyCode.F9)) 
            {
                var killed = PlayerControl.LocalPlayer;
                Utils.RpcMurderPlayer(killed, killed);
            }
        }
    }
}