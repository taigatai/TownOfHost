using AmongUs.GameOptions;

using TownOfHost.Roles.Core;
using TownOfHost.Roles.Core.Interfaces;
using static TownOfHost.Translator;

namespace TownOfHost.Roles.Neutral
{
    public sealed class Remotekiller : RoleBase, IKiller, ISchrodingerCatOwner
    {
        public static readonly SimpleRoleInfo RoleInfo =
            SimpleRoleInfo.Create(
                typeof(Remotekiller),
                player => new Remotekiller(player),
                CustomRoles.Remotekiller,
                () => RoleTypes.Impostor,
                CustomRoleTypes.Neutral,
                50908,
                SetupOptionItem,
                "rk",
                "#8f00ce",
                true,
                introSound: () => GetIntroSound(RoleTypes.Impostor),
                countType: CountTypes.Remotekiller,
                assignCountRule: new(1, 1, 1)
            );
        public Remotekiller(PlayerControl player)
        : base(
            RoleInfo,
            player,
            () => HasTask.False
        )
        {
            KillCooldown = RKillCooldown.GetFloat();
        }

        private static OptionItem RKillCooldown;
        private static OptionItem RKillAnimation;
        private static PlayerControl Rtarget;
        private static float KillCooldown;

        public bool CanBeLastImpostor { get; } = false;
        enum OptionName
        {
            KillAnimation
        }

        private static void SetupOptionItem()
        {
            RKillCooldown = FloatOptionItem.Create(RoleInfo, 10, GeneralOption.KillCooldown, new(2.5f, 180f, 2.5f), 30f, false)
                .SetValueFormat(OptionFormat.Seconds);
            RKillAnimation = BooleanOptionItem.Create(RoleInfo, 11, OptionName.KillAnimation, true, false);
        }
        public SchrodingerCat.TeamType SchrodingerCatChangeTo => SchrodingerCat.TeamType.Remotekiller;
        public float CalculateKillCooldown() => KillCooldown;
        public override bool OnInvokeSabotage(SystemTypes systemType) => false;

        public void OnCheckMurderAsKiller(MurderInfo info)
        {
            if (!info.CanKill) return; //キル出来ない相手には無効
            var (killer, target) = info.AttemptTuple;

            if (target.Is(CustomRoles.Bait)) return;
            if (info.IsFakeSuicide) return;
            //登録
            killer.SetKillCooldown(KillCooldown);
            Rtarget = target;
            killer.RpcGuardAndKill(target);
            info.DoKill = false;
        }
        public override void OnReportDeadBody(PlayerControl _, GameData.PlayerInfo __)
        {
            Rtarget = null;
        }
        public bool OverrideKillButtonText(out string text)
        {
            text = GetString("rkTargetButtonText");
            return true;
        }
        public override bool OnEnterVent(PlayerPhysics physics, int ventId)
        {
            var user = physics.myPlayer;
            if (Rtarget.IsAlive() && Rtarget != null)
            {
                if (RKillAnimation.GetBool())
                {
                    _ = new LateTask(() =>
                    {
                        user.RpcMurderPlayer(Rtarget, true);
                    }, 1f);
                }
                else
                {
                    Rtarget.SetRealKiller(user);
                    Rtarget.RpcMurderPlayer(Rtarget, true);
                }

                RPC.PlaySoundRPC(user.PlayerId, Sounds.KillSound);
                RPC.PlaySoundRPC(user.PlayerId, Sounds.TaskComplete);
                Logger.Info($"Remotekillerのターゲット{Rtarget.name}のキルに成功", "Remotekiller.kill");
            }
            return !RKillAnimation.GetBool();
        }
    }
}
