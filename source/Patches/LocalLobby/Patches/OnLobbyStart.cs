using HarmonyLib;

namespace TownOfUs.LocalGame
{
    [HarmonyPatch]

    public sealed class OnLobbyStart
    {
        [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
        [HarmonyPostfix]

        public static void Postfix()
        {
            if (!InstanceControl.LocalGame) return;

            if (InstanceControl.Respawn && InstanceControl.clients.Count != 0)
            {
                int count = InstanceControl.clients.Count;
                InstanceControl.clients.Clear();
                InstanceControl.PlayerIdClientId.Clear();
                for (int i = 0; i < count; i++)
                {
                    Utils.CreatePlayerInstance("Robot");
                }
            }
        }
    }
}