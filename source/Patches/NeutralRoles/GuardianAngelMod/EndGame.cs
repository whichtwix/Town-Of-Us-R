using HarmonyLib;
using Hazel;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.GuardianAngelMod
{
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.RpcEndGame))]
    public class EndGame
    {
        public static bool Prefix(GameManager __instance, [HarmonyArgument(0)] GameOverReason reason)
        {
            foreach (var role in Role.AllRoles)
                if (role.RoleType == RoleEnum.GuardianAngel && ((GuardianAngel)role).target.Is(Faction.Impostors))
                {
                    if (reason != GameOverReason.HumansByVote && reason != GameOverReason.HumansByTask)
                    {
                        ((GuardianAngel)role).ImpTargetWin();

                        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.GAImpWin,
                            SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                    }
                    else
                    {
                        ((GuardianAngel)role).ImpTargetLose();

                        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.GAImpLose,
                            SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                    }
                    return true;
                }
            return true;
        }
    }
}