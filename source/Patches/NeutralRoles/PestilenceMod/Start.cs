using System;
using HarmonyLib;
using TownOfUs.Roles;

namespace TownOfUs.NeutralRoles.PestilenceMod
{
    [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__29), nameof(IntroCutscene._CoBegin_d__29.MoveNext))]
    public static class Start
    {
        public static void Postfix(IntroCutscene._CoBegin_d__29 __instance)
        {
            foreach (var role in Role.GetRoles(RoleEnum.Pestilence))
            {
                var pestilence = (Pestilence)role;
                pestilence.LastKill = DateTime.UtcNow;
                pestilence.LastKill = pestilence.LastKill.AddSeconds(CustomGameOptions.InitialCooldowns - CustomGameOptions.PestKillCd);
            }
        }
    }
}