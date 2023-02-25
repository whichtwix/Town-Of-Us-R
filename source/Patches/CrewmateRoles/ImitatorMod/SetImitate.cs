using System.Linq;
using HarmonyLib;
using Hazel;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.ImitatorMod
{
    [HarmonyPatch(typeof(MeetingHud))]
    public class SetImitate
    {
        public static PlayerVoteArea Imitate;

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
        public static class VotingComplete
        {
            public static void Postfix(MeetingHud __instance)
            {
                if (Imitate == null) return;

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Imitator))
                {
                    var imitator = Role.GetRole<Imitator>(PlayerControl.LocalPlayer);
                    foreach (var button in imitator.Buttons.Where(button => button != null)) button.SetActive(false);

                    foreach (var player in PlayerControl.AllPlayerControls)
                    {
                        if (player.PlayerId == Imitate.TargetPlayerId) 
                        { 
                            imitator.ImitatePlayer = player;
                        }
                    }

                    if (Imitate == null)
                    {
                        var writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.Imitate, SendOption.Reliable, -1);
                        writer2.Write(imitator.Player.PlayerId);
                        writer2.Write(sbyte.MaxValue);
                        AmongUsClient.Instance.FinishRpcImmediately(writer2);
                        return;
                    }

                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.Imitate, SendOption.Reliable, -1);
                    writer.Write(imitator.Player.PlayerId);
                    writer.Write(imitator.ImitatePlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        public static class MeetingHud_Start
        {
            public static void Postfix(MeetingHud __instance)
            {
                Imitate = null;
            }
        }
    }
}