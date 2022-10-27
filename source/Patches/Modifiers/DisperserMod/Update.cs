using HarmonyLib;
using TownOfUs.Roles.Modifiers;
using UnityEngine;
using TMPro;

namespace TownOfUs.Modifiers.DisperserMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class Update
    {
        public static Sprite DisperseButton => TownOfUs.DisperseSprite;

        public static void Postfix(HudManager __instance)
        {
            UpdateButtonButton(__instance);
        }

        private static void UpdateButtonButton(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1) return;
            if (PlayerControl.LocalPlayer == null) return;
            if (PlayerControl.LocalPlayer.Data == null) return;
            if (!PlayerControl.LocalPlayer.Is(ModifierEnum.Disperser)) return;

            var data = PlayerControl.LocalPlayer.Data;
            var isDead = data.IsDead;


            var role = Modifier.GetModifier<Disperser>(PlayerControl.LocalPlayer);

            if (role.DisperseButton == null)
            {
                role.DisperseButton = Object.Instantiate(__instance.KillButton, __instance.transform.parent);
                role.DisperseButton.GetComponentsInChildren<TextMeshPro>()[0].text = "";
                role.DisperseButton.graphic.enabled = true;
                role.DisperseButton.graphic.sprite = DisperseButton;
            }

            role.DisperseButton.graphic.sprite = DisperseButton;


            role.DisperseButton.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsDead && !MeetingHud.Instance);

            role.DisperseButton.SetCoolDown(0f, 1f);
            var renderer = role.DisperseButton.graphic;

            var position1 = __instance.UseButton.transform.position;
            role.DisperseButton.transform.position = new Vector3(
                Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).x + 0.75f, position1.y,
                position1.z);

            if (!role.ButtonUsed)
            {
                renderer.color = Palette.EnabledColor;
                renderer.material.SetFloat("_Desat", 0f);
                return;
            }

            renderer.color = Palette.DisabledClear;
            renderer.material.SetFloat("_Desat", 1f);
        }
    }
}