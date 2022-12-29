using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using AmongUs.GameOptions;
using HarmonyLib;

namespace TownOfUs
{
    [HarmonyPatch]

    internal sealed class Hauntpatch
    {
        [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.AssignRoleOnDeath))]
        [HarmonyPrefix]

        public static bool Stop_postmortem()
        {
            return false;
        }

        [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]

        public static void Postfix()
        {
            CheckForHaunterOrPhantom();
        }

        [HarmonyPatch(typeof(AirshipExileController), nameof(AirshipExileController.WrapUpAndSpawn))]
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]

        public static void PostfixAirship()
        {
            CheckForHaunterOrPhantom();
        }

        [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.SetFilterText))]
        [HarmonyPrefix]

        public static bool Prefix(HauntMenuMinigame __instance)
        {
            var role = Role.GetRole(__instance.HauntTarget);
            var modifier = Modifier.GetModifier(__instance.HauntTarget);
            string Isassassin = __instance.HauntTarget.Is(AbilityEnum.Assassin) ? "- Assassin" : string.Empty;

            __instance.FilterText.text = modifier != null ? $"{role.Name} - {modifier.Name} {Isassassin}"
                                                          : $"{role.Name} {Isassassin}";
            return false;
        }

        public static void CheckForHaunterOrPhantom()
        {
            var player = PlayerControl.LocalPlayer;
            if (!player.Data.IsDead) return;

            if (player.Data.IsDead &&
               (!player.Is(RoleEnum.Phantom) || (player.Is(RoleEnum.Phantom) && Role.GetRole<Phantom>(player).Caught)) &&
               (!player.Is(RoleEnum.Haunter) || (player.Is(RoleEnum.Haunter) && Role.GetRole<Haunter>(player).Caught)))
            {
                RoleTypes ghostrole = player.Data.Role.TeamType == RoleTeamTypes.Impostor ? RoleTypes.ImpostorGhost
                                                                                          : RoleTypes.CrewmateGhost;
                player.SetRole(ghostrole);
            }
        }
    }
}