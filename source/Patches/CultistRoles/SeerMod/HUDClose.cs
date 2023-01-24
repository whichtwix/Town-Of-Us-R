using System;
using HarmonyLib;
using TownOfUs.Roles;
using TownOfUs.Roles.Cultist;
using Object = UnityEngine.Object;

namespace TownOfUs.CultistRoles.SeerMod
{
    [HarmonyPatch(typeof(Object), nameof(Object.Destroy), typeof(Object))]
    public static class HUDClose
    {
        public static void Postfix(Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            foreach (var role in Role.GetRoles(RoleEnum.CultistSeer))
            {
                var seer = (CultistSeer) role;
                seer.LastInvestigated = DateTime.UtcNow;
            }
        }
    }
}