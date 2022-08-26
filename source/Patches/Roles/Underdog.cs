using TownOfUs.ImpostorRoles.UnderdogMod;

namespace TownOfUs.Roles
{
    public class Underdog : Role
    {
        public Underdog(PlayerControl player) : base(player)
        {
            Name = "Underdog";
            ImpostorText = () => "Use Your Comeback Power To Win";
            TaskText = () => PerformKill.LastImp() ? "You have a shortened kill cooldown!" : "You have a long kill cooldown until your teammate(s) die";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Underdog;
            AddToRoleHistory(RoleType);
            Faction = Faction.Impostors;
        }

        public float MaxTimer() => PerformKill.LastImp() ? PlayerControl.GameOptions.KillCooldown - CustomGameOptions.UnderdogKillBonus : (PerformKill.IncreasedKC() ? PlayerControl.GameOptions.KillCooldown : PlayerControl.GameOptions.KillCooldown + CustomGameOptions.UnderdogKillBonus);

        public void SetKillTimer()
        {
            Player.SetKillTimer(MaxTimer());
        }
    }
}
