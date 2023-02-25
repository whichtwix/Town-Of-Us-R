using System;
using HarmonyLib;
using Hazel;
using TownOfUs.Roles;
using AmongUs.GameOptions;

namespace TownOfUs.NeutralRoles.PlaguebearerMod
{
    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    public class PerformKill
    {
        public static bool Prefix(KillButton __instance)
        {
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Plaguebearer);
            if (!flag) return true;
            if (PlayerControl.LocalPlayer.Data.IsDead) return false;
            if (!PlayerControl.LocalPlayer.CanMove) return false;
            var role = Role.GetRole<Plaguebearer>(PlayerControl.LocalPlayer);
            if (role.InfectTimer() != 0) return false;

            if (role.ClosestPlayer == null) return false;
            if (role.InfectedPlayers.Contains(role.ClosestPlayer.PlayerId)) return false;
            var distBetweenPlayers = Utils.GetDistBetweenPlayers(PlayerControl.LocalPlayer, role.ClosestPlayer);
            var flag3 = distBetweenPlayers <
                        GameOptionsData.KillDistances[GameOptionsManager.Instance.currentNormalGameOptions.KillDistance];
            if (!flag3) return false;
            var interact = Utils.Interact(PlayerControl.LocalPlayer, role.ClosestPlayer);
            if (interact[4] == true)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.Infect, SendOption.Reliable, -1);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                writer.Write(role.ClosestPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                role.InfectedPlayers.Add(role.ClosestPlayer.PlayerId);
            }
            if (interact[0] == true)
            {
                role.LastInfected = DateTime.UtcNow;
                return false;
            }
            else if (interact[1] == true)
            {
                role.LastInfected = DateTime.UtcNow;
                role.LastInfected.AddSeconds(CustomGameOptions.ProtectKCReset - CustomGameOptions.InfectCd);
                return false;
            }
            else if (interact[3] == true) return false;
            return false;
        }
    }
}