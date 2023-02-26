using HarmonyLib;

namespace TownOfUs.LocalGame
{
    [HarmonyPatch]

    public sealed class SameVoteAll
    {
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Confirm))]
        [HarmonyPostfix]

        public static void Postfix(MeetingHud __instance, ref byte suspectStateIdx)
        {
            if (AmongUsClient.Instance.NetworkMode != NetworkModes.LocalGame) return;

            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                __instance.CmdCastVote(player.PlayerId, suspectStateIdx);
            }
        }
    }
}
