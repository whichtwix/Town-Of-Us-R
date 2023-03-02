using HarmonyLib;
using UnityEngine;

namespace TownOfUs.LocalGame
{
    [HarmonyPatch]

    public sealed class Keyboard_Joystick
    {
        private static int controllingFigure;

        [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
        [HarmonyPostfix]

        public static void Postfix()
        {
            if (!InstanceControl.LocalGame) return;

            if (Input.GetKeyDown(KeyCode.F5))
            {
                controllingFigure = PlayerControl.LocalPlayer.PlayerId;
                if (PlayerControl.AllPlayerControls.Count == 15 && !Input.GetKeyDown(KeyCode.F6)) return; //press f6 and f5 to bypass limit
                Utils.CleanUpLoad();
                Utils.CreatePlayerInstance("Robot");
            }

            if (Input.GetKeyDown(KeyCode.F9))
            {
                controllingFigure++;
                controllingFigure = Mathf.Clamp(controllingFigure, 0, PlayerControl.AllPlayerControls.Count - 1);
                InstanceControl.SwitchTo((byte)controllingFigure);
            }

            if (Input.GetKeyDown(KeyCode.F10))
            {
                controllingFigure--;
                controllingFigure = Mathf.Clamp(controllingFigure, 0, PlayerControl.AllPlayerControls.Count - 1);
                InstanceControl.SwitchTo((byte)controllingFigure);
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                InstanceControl.Respawn = !InstanceControl.Respawn;
            }

            if (Input.GetKeyDown(KeyCode.F11))
            {
                Utils.RemoveAllPlayers();
            }
        }
    }
}
