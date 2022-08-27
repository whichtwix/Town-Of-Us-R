using HarmonyLib;
using UnityEngine;
using Hazel;

namespace TownOfUs
{
    [HarmonyPatch(typeof(RoomTracker), nameof(RoomTracker.FixedUpdate))]
    public class killself
    {
        [HarmonyPostfix]
        public static void postfix()
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
                killed.Revive();
                killed.moveable = true;
                killed.gameObject.GetComponent<CustomNetworkTransform>().enabled = true;
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 
                    (byte)CustomRPC.Revive, SendOption.Reliable, -1);
                writer.Write(killed.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
            DestroyableSingleton<FollowerCamera>.Instance.transform.position = PlayerControl.LocalPlayer.transform.position;
            // camera studders while moving after suiciding, havent found solution
        }
    }
}