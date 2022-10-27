using HarmonyLib;
using TownOfUs.Roles.Modifiers;
using UnityEngine;

namespace TownOfUs.Modifiers.UnderdogMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetKillTimer))]
    public static class PatchKillTimer
    {
        public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] float time)
        {
            var modifier = Modifier.GetModifier(__instance);
            if (modifier?.ModifierType != ModifierEnum.Underdog) return true;
            var maxTimer = ((Underdog)modifier).MaxTimer();
            __instance.killTimer = Mathf.Clamp(time, 0, maxTimer);
            HudManager.Instance.KillButton.SetCoolDown(__instance.killTimer, maxTimer);
            return false;
        }
    }
}
