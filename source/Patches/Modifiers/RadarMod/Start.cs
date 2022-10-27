using HarmonyLib;
using TownOfUs.CrewmateRoles.EngineerMod;
using TownOfUs.Patches;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnityEngine;

namespace TownOfUs.Modifiers.RadarMod
{
    [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__19), nameof(IntroCutscene._CoBegin_d__19.MoveNext))]
    public static class Start
    {
        public static Sprite Sprite => TownOfUs.Arrow;
        public static void Postfix(IntroCutscene._CoBegin_d__19 __instance)
        {
            foreach (var modifier in Modifier.GetModifiers(ModifierEnum.Radar))
            {
                if (PlayerControl.LocalPlayer.Is(ModifierEnum.Radar))
                {
                    var radar = (Radar)modifier;
                    var gameObj = new GameObject();
                    var arrow = gameObj.AddComponent<ArrowBehaviour>();
                    gameObj.transform.parent = PlayerControl.LocalPlayer.gameObject.transform;
                    var renderer = gameObj.AddComponent<SpriteRenderer>();
                    renderer.sprite = Sprite;
                    renderer.color = Colors.Radar;
                    arrow.image = renderer;
                    gameObj.layer = 5;
                    arrow.target = PlayerControl.LocalPlayer.transform.position;
                    radar.RadarArrow.Add(arrow);
                }
            }
        }
    }
}