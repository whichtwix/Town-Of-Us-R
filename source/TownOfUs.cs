using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using AmongUs.Data;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using Reactor.Utilities.ImGui;
using Reactor.Utilities.Extensions;
using TownOfUs.CustomOption;
using TownOfUs.Patches;
using TownOfUs.RainbowMod;
using TownOfUs.Roles;
using TownOfUs.Roles.Modifiers;
using InnerNet;
using TMPro;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.Attributes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using UnityEngine.SceneManagement;
using TownOfUs.Extensions;


namespace TownOfUs
{
    [BepInPlugin(Id, "Town Of Us", VersionString)]
    [BepInDependency(ReactorPlugin.Id)]
    [BepInDependency(SubmergedCompatibility.SUBMERGED_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class TownOfUs : BasePlugin
    {
        public const string Id = "com.slushiegoose.townofus";
        public const string VersionString = "3.4.0";
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
        public static Sprite DisperseSprite;
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
        public static Sprite EscapeSprite;
        public static Sprite MarkSprite;
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
            DisperseSprite = CreateSprite("TownOfUs.Resources.Disperse.png");
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
            EscapeSprite = CreateSprite("TownOfUs.Resources.Recall.png");
            MarkSprite = CreateSprite("TownOfUs.Resources.Mark.png");
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

            Component = AddComponent<DebuggerComponent>();
        }

        public static Sprite CreateSprite(string name)
        {
            var pixelsPerUnit = 100f;
            var pivot = new Vector2(0.5f, 0.5f);

            var assembly = Assembly.GetExecutingAssembly();
            var tex = AmongUsExtensions.CreateEmptyTexture();
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
            private static void Increment(ref int variable, int max)
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
                    GUILayout.Label("Name: " +  DataManager.Player.Customization.Name);

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

                        if (!MeetingHud.Instance)
                        {
                            if (GUILayout.Button("Call a meeting"))
                            {
                                if (!data.IsDead)
                                {
                                    PlayerControl.LocalPlayer.CmdReportDeadBody(null);
                                }
                                else
                                {
                                    DestroyableSingleton<DummyBehaviour>.Instance.myPlayer.CmdReportDeadBody(null);
                                }
                            }
                            if (GUILayout.Button("Role"))
                            {
                                //for destroying number of uses text
                                FindObjectsOfType<KillButton>().ToList().ForEach((x) => x.transform.GetComponentsInChildren<TextMeshPro>().ToList().ForEach(x =>
                                { if (x.alignment == TextAlignmentOptions.Right) Destroy(x); }));

                                DestroyableSingleton<HudManager>.Instance.KillButton.gameObject.SetActive(false);
                                player.myTasks.RemoveAt(0);
                                Role.RoleDictionary.Remove(player.PlayerId);

                                var tasktext = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                                tasktext.transform.SetParent(player.transform, false);

                                if (roleindex == Freeplay.Roles.Count)
                                {
                                    FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                    tasktext.Text = "<#FFFF00>Resetting buttons for next loop, click again";
                                    player.myTasks.Insert(0, tasktext);
                                    Increment(ref roleindex, Freeplay.Roles.Count);
                                    return;
                                }
                                var role = Activator.CreateInstance(Freeplay.Roles.Keys.ToList()[roleindex], new object[] { player });
                                tasktext.Text = Freeplay.Roles[role.GetType()];
                                player.myTasks.Insert(0, tasktext);
                                switch (role)
                                {
                                    //setactive() or string.empty is 1 role later of the actual role being affected
                                    //crew or nuetral roles where "kill" needs to be removed from normal button
                                    case Sheriff:
                                    case Tracker:
                                    case Altruist:
                                    case Medic:
                                    case Seer:
                                    case Engineer:
                                    case Amnesiac:
                                    case Plaguebearer:
                                    case Arsonist:
                                        DestroyableSingleton<HudManager>._instance.KillButton.buttonLabelText.text = string.Empty;
                                        break;
                                    //neutral and impostor roles where ability buttons need to be removed 
                                    //to prevent stacking
                                    case Werewolf:
                                    case Grenadier:
                                    case Morphling:
                                    case Swooper:
                                    case Poisoner:
                                    case Blackmailer:
                                    case Janitor:
                                    case Miner:
                                    case Undertaker:
                                    case Escapist:
                                        FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                        break;
                                }
                                Increment(ref roleindex, Freeplay.Roles.Count);
                            }
                            if (GUILayout.Button("Modifier"))
                            {
                                DestroyableSingleton<HudManager>.Instance.KillButton.gameObject.SetActive(false);

                                player.myTasks.RemoveAt(1);
                                Modifier.ModifierDictionary.Remove(player.PlayerId);

                                var tasktext = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                                tasktext.transform.SetParent(player.transform, false);

                                if (modifierindex == Freeplay.Modifiers.Count)
                                {
                                    tasktext.Text = "<#FFFF00>Resetting buttons for next loop, click again";
                                    player.myTasks.Insert(1, tasktext);
                                    Increment(ref modifierindex, Freeplay.Modifiers.Count);
                                    return;
                                }
                                var mod = Activator.CreateInstance(Freeplay.Modifiers.Keys.ToList()[modifierindex], new object[] { player });
                                tasktext.Text = Freeplay.Modifiers[mod.GetType()];
                                player.myTasks.Insert(1, tasktext);
                                switch (mod)
                                {
                                    //setactive() is 1 modifier later of the actual modifier being afected
                                    case Blind:
                                    case Multitasker:
                                        FindObjectOfType<KillButton>().gameObject.SetActive(false);
                                        break;
                                }
                                Increment(ref modifierindex, Freeplay.Modifiers.Count);
                            }
                        }
                    }
                    if (PlayerControl.LocalPlayer)
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
