using AmongUs.GameOptions;
using TownOfHost.Roles.Core;
using TownOfHost.Roles.Core.Interfaces;



namespace TownOfHost.Roles.Impostor
{
    public sealed class Tairou : RoleBase, IImpostor
    {
        public static readonly SimpleRoleInfo RoleInfo =
            SimpleRoleInfo.Create(
                typeof(Tairou),
                player => new Tairou(player),
                CustomRoles.Tairou,
                () => RoleTypes.Impostor,
                CustomRoleTypes.Impostor,
                5000,
                null,
                "t"
            );
        public Tairou(PlayerControl player)
            : base(
                RoleInfo,
                player
            )
        { }
        public override bool OnCheckMurderAsTarget(MurderInfo info) //シェリフが大狼を切ったら誤爆する処理
        {
            (var killer, var target) = info.AttemptTuple;
            if (killer.GetCustomRole() is CustomRoles.Sheriff)
            {
                killer.RpcMurderPlayer(killer);
                PlayerState.GetByPlayerId(killer.PlayerId).DeathReason = CustomDeathReason.Misfire;
                info.DoKill = false;
            }
            return false;
        }
    }
}