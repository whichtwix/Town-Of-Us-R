using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.DetectiveMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public class MeetingStart
    {
        public static void Postfix(MeetingHud __instance)
        {
            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Detective)) return;
            if (!CustomGameOptions.ExamineReportOn) return;
            var detectiveRole = Role.GetRole<Detective>(PlayerControl.LocalPlayer);
            if (detectiveRole.LastExaminedPlayer != null)
            {
                var playerResults = BodyReport.PlayerReportFeedback(detectiveRole.LastExaminedPlayer);

                if (!string.IsNullOrWhiteSpace(playerResults)) DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, playerResults);
            }
        }
    }
}
