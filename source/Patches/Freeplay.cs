using System;
using System.Linq;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using TownOfUs.Patches;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;

namespace TownOfUs
{
    [HarmonyPatch]

    public class Freeplay
    {
        public static readonly Dictionary<Type, string> Roles = new()
        {
            { typeof(Snitch), $"{Colors.Snitch.ToTextColor()}Role: Snitch\nFinish tasks to reveal any impostors and perhaps nuetrals that are present\nTasks:" },
            { typeof(Medium), $"{Colors.Medium.ToTextColor()}Role: Medium\nYou can see ghosts and follow them to bodies or killers\nTasks:" },
            { typeof(Sheriff), $"{Colors.Sheriff.ToTextColor()}Role: Sheriff\nYou can kill imposters but attempting a kill on a crew will kill you.\nTasks:" },
            { typeof(Mystic), $"{Colors.Mystic.ToTextColor()}Role: Mystic\nA arrow points to the body when it dies.\nTasks:" },
            { typeof(Spy), $"{Colors.Spy.ToTextColor()}Role: Spy\nSee colors on admin.\nTasks:" },
            { typeof(Tracker), $"{Colors.Tracker.ToTextColor()}Role: Tracker\nArrows of their colors point to tracked players.\nTasks:" },
            { typeof(Altruist), $"{Colors.Altruist.ToTextColor()}Role: Altruist\nYour ability kills you but revives another.\nTasks:" },
            { typeof(Medic), $"{Colors.Medic.ToTextColor()}Role: Medic\nGet info about a killer when you report bodes and shield 1 person.\nTasks:" },
            { typeof(Swapper), $"{Colors.Swapper.ToTextColor()}Role: Swapper\nSwap 2 player's votes in meetings.\nTasks:" },
            { typeof(Seer), $"{Colors.Seer.ToTextColor()}Role: Seer\nReveal player's alignment to yourself.\nTasks:" },
            { typeof(Engineer), $"{Colors.Engineer.ToTextColor()}Role: Engineer\nvent and fix 1 sabotage per round or game.\nTasks:" },
            { typeof(Mayor), $"{Colors.Mayor.ToTextColor()}Role: Mayor\nsave up your votes through meetings and vot multiple times later.\nTasks:" },
            { typeof(Amnesiac), $"{Colors.Amnesiac.ToTextColor()}Role: Amnesiac\nRemember bodies to take their role\nTasks:" },
            { typeof(Plaguebearer), $"{Colors.Plaguebearer.ToTextColor()}Role: Plaguebearer\nInfect everyone to turn to pestilance and then kill them all\nFake Tasks:" },
            { typeof(Arsonist), $"{Colors.Arsonist.ToTextColor()}Role: Arsonist\nDouse multiple people and ignite them.\nFake Tasks:" },
            { typeof(Werewolf), $"{Colors.Werewolf.ToTextColor()}Role: Werewolf\nGo on a rampage to unlock imposter abilities with a low kill cooldown.\nFake Tasks:" },
            { typeof(Grenadier), $"{Colors.Impostor.ToTextColor()}Role: Grenadier\nFlash grenade others to blind them temporarily.\nFake Tasks:" },
            { typeof(Morphling), $"{Colors.Impostor.ToTextColor()}Role: Morphling\nSample someone each round and pose as them for a time.\nFake Tasks:" },
            { typeof(Swooper), $"{Colors.Impostor.ToTextColor()}Role: Swooper\nTurn invisible and kill unseen.\nFake Tasks:" },
            { typeof(Poisoner), $"{Colors.Impostor.ToTextColor()}Role: Poisoner\nYour kills have a delay and \nmeetings cause a instant kill on currently poisoned.\nFake Tasks:" },
            { typeof(Blackmailer), $"{Colors.Impostor.ToTextColor()}Role: Blackmailer\nstop a person from talking during the next meeting.\nFake Tasks:" },
            { typeof(Janitor), $"{Colors.Impostor.ToTextColor()}Role: Janitor\nErase bodies off the map and dead indicators on vitals.\nFake Tasks:" },
            { typeof(Miner), $"{Colors.Impostor.ToTextColor()}Role: Miner\nCreate a connected network of vents anywhere.\nFake Tasks: "},
            { typeof(Undertaker), $"{Colors.Impostor.ToTextColor()}Role: Undertaker\ndrag and drop dead bodies to places they wont be found.\nFake Tasks:" },
            { typeof(Escapist), $"{Colors.Impostor.ToTextColor()}Role: Escapist\nmark a place on the map every round and teleport there when you want.\nFake Tasks:" }
        };

        public static readonly Dictionary<Type, string> Modifiers = new()
        {
            { typeof(ButtonBarry), $"{Colors.ButtonBarry.ToTextColor()}Modifier: Button barry\n You can button from anywhere and during a sabotage" },
            { typeof(Blind), $"{Colors.Blind.ToTextColor()}Modifier: Blind\n Your report button doesnt light up near a body" },
            { typeof(Flash), $"{Colors.Flash.ToTextColor()}Modifier: Flash\n Your faster than everyone else" },
            { typeof(Giant), $"{Colors.Giant.ToTextColor()}Modifier: Giant\n Your huge and have a bigger dead body. You might be slower" },
            { typeof(Tiebreaker), $"{Colors.Tiebreaker.ToTextColor()}Modifier: Tiebreaker\n your vote counts as 2 if there is a tie you helped to create" },
            { typeof(Torch), $"{Colors.Torch.ToTextColor()}Modifier: Torch\n You can see when the lights are off\ntoggle imp and sab lights and revert to a crew to test" },
            { typeof(Disperser), $"{Colors.Impostor.ToTextColor()}Modifier: Disperser\n teleports all players to random vents on click" },
            { typeof(Multitasker), $"{Colors.Multitasker.ToTextColor()}Modifier: Multitasker\n your tasks are transparent and you can see through while on it" }
        };

        [HarmonyPatch(typeof(DummyBehaviour), nameof(DummyBehaviour.Update))]
        [HarmonyPrefix]

        private static bool Prefix()
        {
            return !PlayerControl.LocalPlayer.Is(RoleEnum.Mayor);
        }

        [HarmonyPatch(typeof(DummyBehaviour), nameof(DummyBehaviour.Update))]
        [HarmonyPostfix]

        private static void Postfix(DummyBehaviour __instance)
        {
            if (Input.GetKeyDown(KeyCode.F2) && PlayerControl.LocalPlayer.Is(RoleEnum.Mayor)) __instance.StartCoroutine_Auto(__instance.DoVote());
        }

        [HarmonyPatch(typeof(DummyBehaviour), nameof(DummyBehaviour.Start))]
        [HarmonyPostfix]

        public static void Setdummyrole(DummyBehaviour __instance)
        {
            System.Random rand = new();
            int index = rand.Next(0, Roles.Count - 1);
            Activator.CreateInstance(Roles.Keys.ToList()[index], new object[] { __instance.myPlayer });
        }

        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Begin))]
        [HarmonyPostfix]

        private static void Setplaceholders()
        {
            if (AmongUsClient.Instance.GameMode != GameModes.FreePlay) return;

            var tasktext = new GameObject("").AddComponent<ImportantTextTask>();
            tasktext.transform.SetParent(PlayerControl.LocalPlayer.transform, false);
            tasktext.Text = "placeholder text";
            PlayerControl.LocalPlayer.myTasks.Insert(0, tasktext);
            PlayerControl.LocalPlayer.myTasks.Insert(1, tasktext);
        }

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.ExitGame))]
        [HarmonyPrefix]

        private static void Destroytasktextandrole(AmongUsClient __instance)
        {
            if (__instance.GameMode != GameModes.FreePlay) return;

            var player = PlayerControl.LocalPlayer;
            if (Role.RoleDictionary.ContainsKey(player.PlayerId)) Role.RoleDictionary.Remove(player.PlayerId);
            if (Modifier.ModifierDictionary.ContainsKey(player.PlayerId)) Modifier.ModifierDictionary.Remove(player.PlayerId);
            PlayerControl.LocalPlayer.myTasks.RemoveAt(0);
        }
    }
}