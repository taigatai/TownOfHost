using AmongUs.GameOptions;
using UnityEngine;
using Hazel;

using TownOfHost.Roles.Core;
using TownOfHost.Roles.Core.Interfaces;

using static TownOfHost.Patches.MovingPlatformBehaviourPatch;
using TownOfHost.Modules;

namespace TownOfHost.Roles.Impostor;
public sealed class TeleportKiller : RoleBase, IImpostor
{
    public static readonly SimpleRoleInfo RoleInfo =
        SimpleRoleInfo.Create(
            typeof(TeleportKiller),
            player => new TeleportKiller(player),
            CustomRoles.TeleportKiller,
            () => RoleTypes.Shapeshifter,
            CustomRoleTypes.Impostor,
            60016,
            SetupOptionItem,
            "tk"
        );
    public TeleportKiller(PlayerControl player)
    : base(
        RoleInfo,
        player
    )
    {
        KillCooldown = OptionKillCoolDown.GetFloat();
        Cooldown = OptionCoolDown.GetFloat();
        Ventgaaa = Optionventgaaa.GetBool();
        Maximum = OptionmMaximum.GetFloat();
        Duration = OptionDuration.GetFloat();
        //LeaveSkin = OptionLeaveSkin.GetBool();
        TpKillCooldownReset = OptionTpKillCooldownReset.GetBool();
        usecount = 0;
    }
    enum OptionName
    {
        KillCooldown,
        Cooldown,
        Ventgaaa,
        Maximum,
        Duration,
        //LeaveSkin,
        TpKillCooldownReset
    }
    static OptionItem OptionKillCoolDown;
    static OptionItem OptionCoolDown;
    static OptionItem Optionventgaaa;
    static OptionItem OptionmMaximum;
    static OptionItem OptionDuration;
    //static OptionItem OptionLeaveSkin;
    static OptionItem OptionTpKillCooldownReset;
    static float KillCooldown;
    static float Cooldown;
    static bool Ventgaaa;
    static float Maximum;
    static int usecount;
    static float Duration;
    //static bool LeaveSkin;
    static bool TpKillCooldownReset;
    private static void SetupOptionItem()
    {
        OptionKillCoolDown = FloatOptionItem.Create(RoleInfo, 10, OptionName.KillCooldown, new(2.5f, 180f, 2.5f), 30f, false)
            .SetValueFormat(OptionFormat.Seconds);
        OptionCoolDown = FloatOptionItem.Create(RoleInfo, 11, OptionName.Cooldown, new(2.5f, 180f, 2.5f), 30f, false)
            .SetValueFormat(OptionFormat.Seconds);
        Optionventgaaa = BooleanOptionItem.Create(RoleInfo, 12, OptionName.Ventgaaa, false, false);
        OptionmMaximum = FloatOptionItem.Create(RoleInfo, 13, OptionName.Maximum, new(0f, 999, 1f), 0f, false)
            .SetValueFormat(OptionFormat.Times);
        OptionDuration = FloatOptionItem.Create(RoleInfo, 14, OptionName.Duration, new(0f, 15, 1f), 5f, false)
            .SetValueFormat(OptionFormat.Seconds);
        //OptionLeaveSkin = BooleanOptionItem.Create(RoleInfo, 15, OptionName.LeaveSkin, false, false);
        OptionTpKillCooldownReset = BooleanOptionItem.Create(RoleInfo, 15, OptionName.TpKillCooldownReset, false, false);
    }

    public bool CanBeLastImpostor { get; } = false;
    private void SendRPC()
    {
        using var sender = CreateSender(CustomRPC.SetTKc);
        sender.Writer.Write(usecount);
    }
    public override void ReceiveRPC(MessageReader reader, CustomRPC rpcType)
    {
        if (rpcType != CustomRPC.SetTKc) return;

        usecount = reader.ReadInt32();
    }
    public override void OnShapeshift(PlayerControl target)
    {
        if (!AmongUsClient.Instance.AmHost || Player.PlayerId == target.PlayerId || !target.IsAlive() || (usecount >= Maximum && Maximum != 0)) return;
        usecount++;
        SendRPC();
        Logger.Info($"Player: {Player.name},Target: {target.name}, count: {usecount}", "TeleportKiller");
        _ = new LateTask(() =>
        {
            var p = Player.transform.position;
            RandomSpawn.TP(Player.NetTransform, target.transform.position);
            if ((!target.inVent && Ventgaaa) || !target.MyPhysics.Animations.IsPlayingAnyLadderAnimation() || !target.UseMovingPlatform())
            {
                RandomSpawn.TP(target.NetTransform, p);
            }
            _ = new LateTask(() =>
            {
                if (target.inVent && Ventgaaa)
                {
                    PlayerState.GetByPlayerId(Player.PlayerId).DeathReason = CustomDeathReason.Kill;
                    Player.RpcMurderPlayer(Player, true);
                }
                else
                {
                    if (target.GetCustomRole().IsImpostor() || target.MyPhysics.Animations.IsPlayingAnyLadderAnimation() || target.UseMovingPlatform()) return;
                    PlayerState.GetByPlayerId(target.PlayerId).DeathReason = CustomDeathReason.Kill;
                    target.RpcMurderPlayer(target, true);
                    if (TpKillCooldownReset) Player.SetKillCooldown(KillCooldown);
                }
            }, 0.5f, "TeleportKiller-2");
        }, 1.5f, "TeleportKiller-1");
    }

    public override string GetProgressText(bool comms = false) => Maximum == 0 ? "" : Utils.ColorString(Maximum >= usecount ? Color.red : Color.gray, $"({Maximum - usecount})");

    public float CalculateKillCooldown() => KillCooldown;

    public override void ApplyGameOptions(IGameOptions opt)
    {
        AURoleOptions.ShapeshifterCooldown = Cooldown;
        //AURoleOptions.ShapeshifterLeaveSkin = LeaveSkin;
        AURoleOptions.ShapeshifterDuration = Duration;
        AURoleOptions.KillCooldown = KillCooldown;
    }
}