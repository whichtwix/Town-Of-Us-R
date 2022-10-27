using Hazel;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using Object = UnityEngine.Object;
using Reactor.Networking.Extensions;

namespace TownOfUs.Roles.Modifiers
{
    public class Disperser : Modifier
    {
        public KillButton DisperseButton;

        public bool ButtonUsed;
        public Disperser(PlayerControl player) : base(player)
        {
            Name = "Disperser";
            TaskText = () => "Separate the Crew";
            Color = Patches.Colors.Impostor;
            ModifierType = ModifierEnum.Disperser;
        }

        public void Disperse()
        {
            Dictionary<byte, Vector2> coordinates = GenerateDisperseCoordinates();

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Disperse,
                SendOption.Reliable,
                -1);
            writer.Write((byte)coordinates.Count);
            foreach ((byte key, Vector2 value) in coordinates)
            {
                writer.Write(key);
                writer.Write(value);
            }
            AmongUsClient.Instance.FinishRpcImmediately(writer);

            DispersePlayersToCoordinates(coordinates);
        }

        public static void DispersePlayersToCoordinates(Dictionary<byte, Vector2> coordinates)
        {
            if (coordinates.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
            {
                Coroutines.Start(Utils.FlashCoroutine(Palette.ImpostorRed));
                if (Minigame.Instance)
                {
                    try
                    {
                        Minigame.Instance.Close();
                    }
                    catch
                    {

                    }
                }

                if (PlayerControl.LocalPlayer.inVent)
                {
                    PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(Vent.currentVent.Id);
                    PlayerControl.LocalPlayer.MyPhysics.ExitAllVents();
                }
            }


            foreach ((byte key, Vector2 value) in coordinates)
            {
                PlayerControl player = Utils.PlayerById(key);
                player.transform.position = value;
            }
        }

        private Dictionary<byte, Vector2> GenerateDisperseCoordinates()
        {
            List<PlayerControl> targets = PlayerControl.AllPlayerControls.ToArray().Where(player => !player.Data.IsDead && !player.Data.Disconnected).ToList();

            HashSet<Vent> vents = Object.FindObjectsOfType<Vent>().ToHashSet();

            Dictionary<byte, Vector2> coordinates = new Dictionary<byte, Vector2>(targets.Count);
            foreach (PlayerControl target in targets)
            {
                Vent vent = vents.Random();

                Vector3 destination = SendPlayerToVent(vent);
                coordinates.Add(target.PlayerId, destination);
            }
            return coordinates;
        }

        public static Vector3 SendPlayerToVent(Vent vent)
        {
            Vector2 size = vent.GetComponent<BoxCollider2D>().size;
            Vector3 destination = vent.transform.position;
            destination.y += 0.3636f;
            return destination;
        }
    }
}