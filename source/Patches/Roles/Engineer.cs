using UnityEngine;

namespace TownOfUs.Roles
{
    public class Engineer : Role
    {
        public Engineer(PlayerControl player) : base(player)
        {
            Name = "Engineer";
            ImpostorText = () => "Maintain Important Systems On The Ship";
            TaskText = () => "Vent around and fix sabotages";
            Color = Patches.Colors.Engineer;
            RoleType = RoleEnum.Engineer;
            AddToRoleHistory(RoleType);
        }

        public bool UsedThisRound { get; set; } = false;
    }
}