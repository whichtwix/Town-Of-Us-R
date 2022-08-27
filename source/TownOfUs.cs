using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using Reactor.Extensions;
using TownOfUs.CustomOption;
using TownOfUs.Patches;
using TownOfUs.Patches.CustomHats;
using TownOfUs.RainbowMod;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnhollowerBaseLib.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using InnerNet;
using TMPro;

namespace TownOfUs
{
    [BepInPlugin(Id, "Town Of Us", VersionString)]
    [BepInDependency(ReactorPlugin.Id)]
    [BepInDependency(SubmergedCompatibility.SUBMERGED_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class TownOfUs : BasePlugin
    {
        public const string Id = "com.slushiegoose.townofus";
        public const string VersionString = "3.3.1";
        public static System.Version Version = System.Version.Parse(VersionString);
        
        public static Sprite JanitorClean;
        public static Sprite EngineerFix;
        public static Sprite SwapperSwitch;
        public static Sprite SwapperSwitchDisabled;
        public static Sprite Footprint;
        public static Sprite Rewind;
        public static Sprite NormalKill;
        public static Sprite MedicSprite;
        public static Sprite SeerSprite;
        public static Sprite SampleSprite;
        public static Sprite MorphSprite;
        public static Sprite Arrow;
        public static Sprite MineSprite;
        public static Sprite SwoopSprite;
        public static Sprite DouseSprite;
        public static Sprite IgniteSprite;
        public static Sprite ReviveSprite;
        public static Sprite ButtonSprite;
        public static Sprite CycleBackSprite;
        public static Sprite CycleForwardSprite;
        public static Sprite GuessSprite;
        public static Sprite DragSprite;
        public static Sprite DropSprite;
        public static Sprite FlashSprite;
        public static Sprite AlertSprite;
        public static Sprite RememberSprite;
        public static Sprite TrackSprite;
        public static Sprite PoisonSprite;
        public static Sprite PoisonedSprite;
        public static Sprite TransportSprite;
        public static Sprite MediateSprite;
        public static Sprite VestSprite;
        public static Sprite ProtectSprite;
        public static Sprite BlackmailSprite;
        public static Sprite BlackmailLetterSprite;
        public static Sprite BlackmailOverlaySprite;
        public static Sprite LighterSprite;
        public static Sprite DarkerSprite;
        public static Sprite InfectSprite;
        public static Sprite RampageSprite;
        public static Sprite TrapSprite;
        public static Sprite ExamineSprite;
        public static Sprite HackSprite;
        public static Sprite MimicSprite;
        public static Sprite LockSprite;

        public static Sprite SettingsButtonSprite;
        public static Sprite CrewSettingsButtonSprite;
        public static Sprite NeutralSettingsButtonSprite;
        public static Sprite ImposterSettingsButtonSprite;
        public static Sprite ModifierSettingsButtonSprite;
        public static Sprite ToUBanner;
        public static Sprite UpdateTOUButton;
        public static Sprite UpdateSubmergedButton;

        public static Sprite HorseEnabledImage;
        public static Sprite HorseDisabledImage;
        public static Vector3 ButtonPosition { get; private set; } = new Vector3(2.6f, 0.7f, -9f);

        private static DLoadImage _iCallLoadImage;


        private Harmony _harmony;

        public ConfigEntry<string> Ip { get; set; }

        public ConfigEntry<ushort> Port { get; set; }

        public DebuggerComponent Component { get; private set; }

        public override void Load()
        {
            System.Console.WriteLine("000.000.000.000/000000000000000000");

            _harmony = new Harmony("com.slushiegoose.townofus");

            Generate.GenerateAll();

            JanitorClean = CreateSprite("TownOfUs.Resources.Janitor.png");
            EngineerFix = CreateSprite("TownOfUs.Resources.Engineer.png");
            SwapperSwitch = CreateSprite("TownOfUs.Resources.SwapperSwitch.png");
            SwapperSwitchDisabled = CreateSprite("TownOfUs.Resources.SwapperSwitchDisabled.png");
            Footprint = CreateSprite("TownOfUs.Resources.Footprint.png");
            Rewind = CreateSprite("TownOfUs.Resources.Rewind.png");
            NormalKill = CreateSprite("TownOfUs.Resources.NormalKill.png");
            MedicSprite = CreateSprite("TownOfUs.Resources.Medic.png");
            SeerSprite = CreateSprite("TownOfUs.Resources.Seer.png");
            SampleSprite = CreateSprite("TownOfUs.Resources.Sample.png");
            MorphSprite = CreateSprite("TownOfUs.Resources.Morph.png");
            Arrow = CreateSprite("TownOfUs.Resources.Arrow.png");
            MineSprite = CreateSprite("TownOfUs.Resources.Mine.png");
            SwoopSprite = CreateSprite("TownOfUs.Resources.Swoop.png");
            DouseSprite = CreateSprite("TownOfUs.Resources.Douse.png");
            IgniteSprite = CreateSprite("TownOfUs.Resources.Ignite.png");
            ReviveSprite = CreateSprite("TownOfUs.Resources.Revive.png");
            ButtonSprite = CreateSprite("TownOfUs.Resources.Button.png");
            DragSprite = CreateSprite("TownOfUs.Resources.Drag.png");
            DropSprite = CreateSprite("TownOfUs.Resources.Drop.png");
            CycleBackSprite = CreateSprite("TownOfUs.Resources.CycleBack.png");
            CycleForwardSprite = CreateSprite("TownOfUs.Resources.CycleForward.png");
            GuessSprite = CreateSprite("TownOfUs.Resources.Guess.png");
            FlashSprite = CreateSprite("TownOfUs.Resources.Flash.png");
            AlertSprite = CreateSprite("TownOfUs.Resources.Alert.png");
            RememberSprite = CreateSprite("TownOfUs.Resources.Remember.png");
            TrackSprite = CreateSprite("TownOfUs.Resources.Track.png");
            PoisonSprite = CreateSprite("TownOfUs.Resources.Poison.png");
            PoisonedSprite = CreateSprite("TownOfUs.Resources.Poisoned.png");
            TransportSprite = CreateSprite("TownOfUs.Resources.Transport.png");
            MediateSprite = CreateSprite("TownOfUs.Resources.Mediate.png");
            VestSprite = CreateSprite("TownOfUs.Resources.Vest.png");
            ProtectSprite = CreateSprite("TownOfUs.Resources.Protect.png");
            BlackmailSprite = CreateSprite("TownOfUs.Resources.Blackmail.png");
            BlackmailLetterSprite = CreateSprite("TownOfUs.Resources.BlackmailLetter.png");
            BlackmailOverlaySprite = CreateSprite("TownOfUs.Resources.BlackmailOverlay.png");
            LighterSprite = CreateSprite("TownOfUs.Resources.Lighter.png");
            DarkerSprite = CreateSprite("TownOfUs.Resources.Darker.png");
            InfectSprite = CreateSprite("TownOfUs.Resources.Infect.png");
            RampageSprite = CreateSprite("TownOfUs.Resources.Rampage.png");
            TrapSprite = CreateSprite("TownOfUs.Resources.Trap.png");
            ExamineSprite = CreateSprite("TownOfUs.Resources.Examine.png");
            HackSprite = CreateSprite("TownOfUs.Resources.Hack.png");
            MimicSprite = CreateSprite("TownOfUs.Resources.Mimic.png");
            LockSprite = CreateSprite("TownOfUs.Resources.Lock.png");

            SettingsButtonSprite = CreateSprite("TownOfUs.Resources.SettingsButton.png");
            CrewSettingsButtonSprite = CreateSprite("TownOfUs.Resources.Crewmate.png");
            NeutralSettingsButtonSprite = CreateSprite("TownOfUs.Resources.Neutral.png");
            ImposterSettingsButtonSprite = CreateSprite("TownOfUs.Resources.Impostor.png");
            ModifierSettingsButtonSprite = CreateSprite("TownOfUs.Resources.Modifiers.png");
            ToUBanner = CreateSprite("TownOfUs.Resources.TownOfUsBanner.png");
            UpdateTOUButton = CreateSprite("TownOfUs.Resources.UpdateToUButton.png");
            UpdateSubmergedButton = CreateSprite("TownOfUs.Resources.UpdateSubmergedButton.png");

            HorseEnabledImage = CreateSprite("TownOfUs.Resources.HorseOn.png");
            HorseDisabledImage = CreateSprite("TownOfUs.Resources.HorseOff.png");

            PalettePatch.Load();
            ClassInjector.RegisterTypeInIl2Cpp<RainbowBehaviour>();

            // RegisterInIl2CppAttribute.Register();

            Ip = Config.Bind("Custom", "Ipv4 or Hostname", "127.0.0.1");
            Port = Config.Bind("Custom", "Port", (ushort) 22023);
            var defaultRegions = ServerManager.DefaultRegions.ToList();
            var ip = Ip.Value;
            if (Uri.CheckHostName(Ip.Value).ToString() == "Dns")
                foreach (var address in Dns.GetHostAddresses(Ip.Value))
                {
                    if (address.AddressFamily != AddressFamily.InterNetwork)
                        continue;
                    ip = address.ToString();
                    break;
                }

            ServerManager.DefaultRegions = defaultRegions.ToArray();
            
            _harmony.PatchAll();
            SubmergedCompatibility.Initialize();

            Component = this.AddComponent<DebuggerComponent>();
        }

        public static Sprite CreateSprite(string name)
        {
            var pixelsPerUnit = 100f;
            var pivot = new Vector2(0.5f, 0.5f);

            var assembly = Assembly.GetExecutingAssembly();
            var tex = GUIExtensions.CreateEmptyTexture();
            var imageStream = assembly.GetManifestResourceStream(name);
            var img = imageStream.ReadFully();
            LoadImage(tex, img, true);
            tex.DontDestroy();
            var sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, pixelsPerUnit);
            sprite.DontDestroy();
            return sprite;
        }

        public static void LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
        {
            _iCallLoadImage ??= IL2CPP.ResolveICall<DLoadImage>("UnityEngine.ImageConversion::LoadImage");
            var il2CPPArray = (Il2CppStructArray<byte>) data;
            _iCallLoadImage.Invoke(tex.Pointer, il2CPPArray.Pointer, markNonReadable);
        }

        private delegate bool DLoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
    
        public class DebuggerComponent : MonoBehaviour
        {
            [HideFromIl2Cpp]
            
            public static void increment(ref int variable, int max)
            {
                if (variable == max) variable = -1;
                variable++;
            }
            static int roleindex;
            static int modifierindex;

            [HideFromIl2Cpp]
            public DragWindow TestWindow { get; }

            public DebuggerComponent(IntPtr ptr) : base(ptr)
            {
                TestWindow = new DragWindow(new Rect(20, 20, 0, 0), "Debugger", () =>
                {
                    GUILayout.Label("Name: " + SaveManager.PlayerName);
                    
                    if (TutorialManager.InstanceExists && PlayerControl.LocalPlayer)
                    {
                        var data = PlayerControl.LocalPlayer.Data;

                        var player = PlayerControl.LocalPlayer;
                        
                        var newIsImpostor = GUILayout.Toggle(data.Role.IsImpostor, "Is Impostor");
                        if (data.Role.IsImpostor != newIsImpostor)
                        {
                            PlayerControl.LocalPlayer.RpcSetRole(newIsImpostor ? RoleTypes.Impostor : RoleTypes.Crewmate);
                        }

                        if (GUILayout.Button("Spawn a dummy"))
                        {
                            var playerControl = Instantiate(TutorialManager.Instance.PlayerPrefab);
                            var i = playerControl.PlayerId = (byte) GameData.Instance.GetAvailableId();
                            GameData.Instance.AddPlayer(playerControl);
                            AmongUsClient.Instance.Spawn(playerControl, -2, SpawnFlags.None);
                            playerControl.transform.position = PlayerControl.LocalPlayer.transform.position;
                            playerControl.GetComponent<DummyBehaviour>().enabled = true;
                            playerControl.NetTransform.enabled = false;
                            playerControl.SetName($"{TranslationController.Instance.GetString(StringNames.Dummy, Array.Empty<Il2CppSystem.Object>())} {i}");
                            var color = (byte) (i % Palette.PlayerColors.Length);
                            playerControl.SetColor(color);
                            playerControl.SetHat(HatManager.Instance.allHats[i % HatManager.Instance.allHats.Count].ProdId, playerControl.Data.DefaultOutfit.ColorId);
                            playerControl.SetPet(HatManager.Instance.allPets[i % HatManager.Instance.allPets.Count].ProdId);
                            playerControl.SetSkin(HatManager.Instance.allSkins[i % HatManager.Instance.allSkins.Count].ProdId, color);
                            GameData.Instance.RpcSetTasks(playerControl.PlayerId, new Il2CppStructArray<byte>(0));
                        }
                         if (GUILayout.Button("Force game end"))
                        {
                            ShipStatus.Instance.enabled = false;
                            ShipStatus.RpcEndGame(GameOverReason.ImpostorDisconnect, false);
                        }

                        if (GUILayout.Button("Call a meeting"))
                        {
                            PlayerControl.LocalPlayer.CmdReportDeadBody(null);
                        }
                        if (GUILayout.Button("role"))
                        {
                            GameObject.FindObjectsOfType<KillButton>().ToList().ForEach((x) => x.transform.GetComponentsInChildren<TextMeshPro>().ToList().ForEach(x => 
                            { if (x.alignment == TMPro.TextAlignmentOptions.Right) GameObject.Destroy(x); }));
                            DestroyableSingleton<HudManager>.Instance.KillButton.gameObject.SetActive(false);
                            player.myTasks.RemoveAt(0);
                            Role.RoleDictionary.Remove(player.PlayerId);
                            var tasktext = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                            tasktext.transform.SetParent(player.transform, false);
                            switch(roleindex)
                           {
                            case 0:
                                var Sheriff = new Sheriff(player);
                                tasktext.Text = $"{Sheriff.ColorString}Role: Sheriff\nYou can kill imposters but attempting a kill on a crew will kill you.\nTasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 1:
                                DestroyableSingleton<HudManager>._instance.KillButton.buttonLabelText.text = null;
                                var medium = new Medium(player);
                                tasktext.Text = $"{medium.ColorString}Role: Medium\nYou can see ghosts and follow them to bodies or killers\nTasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 2:
                                var mystic = new Mystic(player);
                                tasktext.Text = $"{mystic.ColorString}Role: Mystic\nA arrow points to the body when it dies.\nTasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 3:
                                var spy = new Spy(player);
                                tasktext.Text = $"{spy.ColorString}Role: Spy\nSee colors on admin.\nTasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 4:
                                DestroyableSingleton<HudManager>._instance.KillButton.buttonLabelText.text = null;
                                var tracker = new Tracker(player);
                                tasktext.Text = $"{tracker.ColorString}Role: Tracker\nArrows of their colors point to tracked players.\nTasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 5:
                                DestroyableSingleton<HudManager>._instance.KillButton.buttonLabelText.text = null;
                                var altruist = new Altruist(player);
                                tasktext.Text = $"{altruist.ColorString}Role: Altruist\nYour ability kills you but revives another.\nTasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 6:
                                DestroyableSingleton<HudManager>._instance.KillButton.buttonLabelText.text = null;
                                var medic = new Medic(player);
                                tasktext.Text = $"{medic.ColorString}Role: Medic\nGet info about a killer when you report bodes and shield 1 person.\nTasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 7:
                                var swapper = new Swapper(player);
                                tasktext.Text = $"{swapper.ColorString}Role: Swapper\nSwap 2 player's votes in meetings.\nTasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 8:
                                DestroyableSingleton<HudManager>._instance.KillButton.buttonLabelText.text = null;
                                var timelord = new TimeLord(player);
                                tasktext.Text = $"{timelord.ColorString}Role: Timelord\nRewind to retrace you and other player's steps.\nTasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 9:
                                DestroyableSingleton<HudManager>._instance.KillButton.buttonLabelText.text = null;
                                var engineer = new Engineer(player);
                                tasktext.Text = $"{engineer.ColorString}Role: Engineer\nvent and fix 1 sabotage per round or game.\nTasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 10:
                                var mayor = new Mayor(player);
                                tasktext.Text = $"{mayor.ColorString}Role: Mayor\nsave up your votes through meetings and vot multiple times later.\nTasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 11:
                                DestroyableSingleton<HudManager>._instance.KillButton.buttonLabelText.text = null;
                                var detective = new Detective(player);
                                tasktext.Text = $"{detective.ColorString}Role: Detective\nReport bodies to know the killer role or faction and\nexamine the living to know if they have killed recently\nTasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 12:
                                DestroyableSingleton<HudManager>._instance.KillButton.buttonLabelText.text = null;
                                var plague = new Plaguebearer(player);
                                tasktext.Text = $"{plague.ColorString}Role: Plaguebearer\nInfect everyone to turn to pestilance and then kill them all\nFake Tasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 13:
                                DestroyableSingleton<HudManager>._instance.KillButton.buttonLabelText.text = null;
                                var arsonist = new Arsonist(player);
                                tasktext.Text = $"{arsonist.ColorString}Role: Arsonist\nDouse everyone to ignite and kill everyone together.\nFake Tasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 14:
                                GameObject.FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                var werewolf = new Werewolf(player);
                                tasktext.Text = $"{werewolf.ColorString}Role: Werewolf\nGo on a rampage to unlock imposter abilities with a low kill cooldown.\nFake Tasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 15:
                                GameObject.FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                var grenadier = new Grenadier(player);
                                tasktext.Text = $"{grenadier.ColorString}Role: Grenadier\nFlash grenade others to blind them temporarily.\nFake Tasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 16:
                                GameObject.FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                var morphling = new Morphling(player);
                                tasktext.Text = $"{morphling.ColorString}Role: Morphling\nSample someone each round and pose as them for a time.\nFake Tasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 17:
                                GameObject.FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                var swooper = new Swooper(player);
                                tasktext.Text = $"{swooper.ColorString}Role: Swooper\nTurn invisible and kill unseen.\nFake Tasks:";
                                player.myTasks.Insert(0, tasktext);
                                break; 
                            case 18:
                                GameObject.FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                var poisoner = new Poisoner(player);
                                tasktext.Text = $"{poisoner.ColorString}Role: Poisoner\nYour kills have a delay and \nmeetings cause a instant kill on currently poisoned.\nFake Tasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 19:
                                GameObject.FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                var blackmailer = new Blackmailer(player);
                                tasktext.Text = $"{blackmailer.ColorString}Role: Blackmailer\nstop a person from talking during the next meeting.\nFake Tasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 20:
                                GameObject.FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                var janitor = new Janitor(player);
                                tasktext.Text = $"{janitor.ColorString}Role: Janitor\nErase bodies off the map and dead indicators on vitals.\nFake Tasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 21:
                                GameObject.FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                var miner = new Miner(player);
                                tasktext.Text = $"{miner.ColorString}Role: Miner\nCreate a connected network of vents anywhere.\nFake Tasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 22:
                                GameObject.FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                var undertaker = new Undertaker(player);
                                tasktext.Text = $"{undertaker.ColorString}Role: Undertaker\ndrag and drop dead bodies to places they wont be found.\nFake Tasks:";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            case 23:
                                GameObject.FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                tasktext.Text = "<#FFFF00>Resetting buttons for next loop, click again";
                                player.myTasks.Insert(0, tasktext);
                                break;
                            }
                            increment(ref roleindex, 23);    
                        }
                        if (GUILayout.Button("Modifier"))
                        {
                            DestroyableSingleton<HudManager>.Instance.KillButton.gameObject.SetActive(false);
                            player.myTasks.RemoveAt(1);
                            Modifier.ModifierDictionary.Remove(player.PlayerId);
                            var tasktext = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                            tasktext.transform.SetParent(player.transform, false);
                            switch(modifierindex)
                            {
                                case 0:
                                    var buttonbarry = new ButtonBarry(player);
                                    tasktext.Text = $"{buttonbarry.ColorString}Modifier: Button barry\n You can button from anywhere and during a sabotage";
                                    player.myTasks.Insert(1, tasktext);
                                    break;
                                case 1:
                                    GameObject.FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                    var blind = new Blind(player);
                                    tasktext.Text = $"{blind.ColorString}Modifier: Blind\n Your report button doesnt light up near a body";
                                    player.myTasks.Insert(1, tasktext);
                                    break;
                                case 2:
                                    var flash = new Flash(player);
                                    tasktext.Text = $"{flash.ColorString}Modifier: Flash\n Your faster than everyone else";
                                    player.myTasks.Insert(1, tasktext);
                                    break;
                                 case 3:
                                    var giant = new Giant(player);
                                    tasktext.Text = $"{giant.ColorString}Modifier: Giant\n Your huge and have a bigger dead body. You might be slower";
                                    player.myTasks.Insert(1, tasktext);
                                    break;
                                 case 4:
                                    var tiebreaker = new Tiebreaker(player);
                                    tasktext.Text = $"{tiebreaker.ColorString}Modifier: Tiebreaker\n your vote counts as 2 if there is a tie you helped to create";
                                    player.myTasks.Insert(1, tasktext);
                                    break;
                                 case 5:
                                    var torch = new Torch(player);
                                    tasktext.Text = $"{torch.ColorString}Modifier: Torch\n You can see when the lights are off\ntoggle imp and sab lights and revert to a crew to test";
                                    player.myTasks.Insert(1, tasktext);
                                    break;
                            }
                            increment(ref modifierindex, 5);
                        }
                    }  if (PlayerControl.LocalPlayer)
                       {
                        var position = PlayerControl.LocalPlayer.transform.position;
                        GUILayout.Label($"x: {position.x}");
                        GUILayout.Label($"y: {position.y}");
                    }
                })
                {
                    Enabled = false
                };
            }

            private void Update()
            {
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    TestWindow.Enabled = !TestWindow.Enabled;
                }
            }

            private void OnGUI()
            {
                TestWindow.OnGUI();
            }
        }
    }
}
