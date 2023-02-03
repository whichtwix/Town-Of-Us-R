using HarmonyLib;
using Hazel;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.SurvivorMod
{
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.RpcEndGame))]
    public class EndGame
    {
        public static bool Prefix(GameManager __instance, [HarmonyArgument(0)] GameOverReason reason)
        {
            if (reason != GameOverReason.HumansByVote && reason != GameOverReason.HumansByTask)
            {
                foreach (var role in Role.AllRoles)
                    if (role.RoleType == RoleEnum.Survivor && !role.Player.Data.IsDead && !role.Player.Data.Disconnected)
                    {
                        ((Survivor)role).AliveImpWin();

                        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.SurvivorImpWin,
                            SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        return true;
                    }
            }
            else
            {
                foreach (var role in Role.AllRoles)
                    if (role.RoleType == RoleEnum.Survivor && (role.Player.Data.IsDead || role.Player.Data.Disconnected))
                    {
                        ((Survivor)role).DeadCrewWin();

                        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.SurvivorCrewWin,
                            SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        return true;
                    }
            }
            return true;
        }
    }
}