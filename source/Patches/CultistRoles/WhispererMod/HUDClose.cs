using System;
using HarmonyLib;
using TownOfUs.Roles;
using TownOfUs.Roles.Cultist;
using Object = UnityEngine.Object;

namespace TownOfUs.CultistRoles.WhispererMod
{
    [HarmonyPatch(typeof(Object), nameof(Object.Destroy), typeof(Object))]
    public static class HUDClose
    {
        public static void Postfix(Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            if (PlayerControl.LocalPlayer.Is(RoleEnum.Whisperer))
            {
                var role = Role.GetRole<Whisperer>(PlayerControl.LocalPlayer);
                role.LastWhispered = DateTime.UtcNow;
            }
        }
    }
}