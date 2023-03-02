using HarmonyLib;

namespace TownOfUs.LocalGame
{
    [HarmonyPatch]

    public sealed class OnGameStart
    {
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.CoStartGameHost))]
        [HarmonyPrefix]

        public static void Prefix(AmongUsClient __instance)
        {
            if (!InstanceControl.LocalGame) return;

            foreach (var player in __instance.allClients)
            {
                player.IsReady = true;
            }
        }
    }
}
