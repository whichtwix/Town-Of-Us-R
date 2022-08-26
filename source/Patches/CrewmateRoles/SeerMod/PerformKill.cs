using System;
using HarmonyLib;
using Hazel;
using TownOfUs.Roles;
using UnityEngine;
using TownOfUs.CrewmateRoles.MedicMod;

namespace TownOfUs.CrewmateRoles.SeerMod
{
    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    public class PerformKill
    {
        public static bool Prefix(KillButton __instance)
        {
            if (__instance != DestroyableSingleton<HudManager>.Instance.KillButton) return true;
            var flag = PlayerControl.LocalPlayer.Is(RoleEnum.Seer);
            if (!flag) return true;
            var role = Role.GetRole<Seer>(PlayerControl.LocalPlayer);
            if (!PlayerControl.LocalPlayer.CanMove || role.ClosestPlayer == null) return false;
            var flag2 = role.SeerTimer() == 0f;
            if (!flag2) return false;
            if (!__instance.enabled) return false;
            var maxDistance = GameOptionsData.KillDistances[PlayerControl.GameOptions.KillDistance];
            if (Vector2.Distance(role.ClosestPlayer.GetTruePosition(),
                PlayerControl.LocalPlayer.GetTruePosition()) > maxDistance) return false;
            if (role.ClosestPlayer == null) return false;
            var playerId = role.ClosestPlayer.PlayerId;
            if (role.ClosestPlayer.IsInfected() || role.Player.IsInfected())
            {
                foreach (var pb in Role.GetRoles(RoleEnum.Plaguebearer)) ((Plaguebearer)pb).RpcSpreadInfection(role.ClosestPlayer, role.Player);
            }
            if (role.ClosestPlayer.IsOnAlert() || role.ClosestPlayer.Is(RoleEnum.Pestilence))
            {
                if (role.Player.IsShielded())
                {
                    var writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.AttemptSound, SendOption.Reliable, -1);
                    writer2.Write(PlayerControl.LocalPlayer.GetMedic().Player.PlayerId);
                    writer2.Write(PlayerControl.LocalPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer2);

                    System.Console.WriteLine(CustomGameOptions.ShieldBreaks + "- shield break");
                    if (CustomGameOptions.ShieldBreaks)
                        role.LastInvestigated = DateTime.UtcNow;
                    StopKill.BreakShield(PlayerControl.LocalPlayer.GetMedic().Player.PlayerId, PlayerControl.LocalPlayer.PlayerId, CustomGameOptions.ShieldBreaks);
                    return false;
                }
                else if (!role.Player.IsProtected())
                {
                    Utils.RpcMurderPlayer(role.ClosestPlayer, PlayerControl.LocalPlayer);
                    return false;
                }
                role.LastInvestigated = DateTime.UtcNow;

                return false;
            }

            role.Investigated.Add(role.ClosestPlayer.PlayerId);
            role.LastInvestigated = DateTime.UtcNow;
            return false;
        }
    }
}
