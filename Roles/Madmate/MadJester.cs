using AmongUs.GameOptions;

using TownOfHost.Roles.Core;
using TownOfHost.Roles.Core.Interfaces;

namespace TownOfHost.Roles.Madmate;
public sealed class MadJester : RoleBase, IKillFlashSeeable, IDeathReasonSeeable
{
    public static readonly SimpleRoleInfo RoleInfo =
        SimpleRoleInfo.Create(
            typeof(MadJester),
            player => new MadJester(player),
            CustomRoles.MadJester,
            () => RoleTypes.Crewmate,
            CustomRoleTypes.Madmate,
            60050,
            SetupOptionItem,
            "mje",
            introSound: () => GetIntroSound(RoleTypes.Impostor)
        );
    public MadJester(PlayerControl player)
    : base(
        RoleInfo,
        player,
        () => HasTask.ForRecompute
    )
    {
        canSeeKillFlash = Options.MadmateCanSeeKillFlash.GetBool();
        canSeeDeathReason = Options.MadmateCanSeeDeathReason.GetBool();
    }
    private static bool canSeeKillFlash;
    private static bool canSeeDeathReason;

    public bool CheckKillFlash(MurderInfo info) => canSeeKillFlash;
    public bool CheckSeeDeathReason(PlayerControl seen) => canSeeDeathReason;
    private static Options.OverrideTasksData Tasks;

    public static void SetupOptionItem()
    {
        Tasks = Options.OverrideTasksData.Create(RoleInfo, 10);
    }

    public override void OnExileWrapUp(GameData.PlayerInfo exiled, ref bool DecidedWinner)
    {
        if (!AmongUsClient.Instance.AmHost || Player.PlayerId != exiled.PlayerId) return;
        if (!IsTaskFinished) return;
        CustomWinnerHolder.ResetAndSetWinner(CustomWinner.Impostor);
        CustomWinnerHolder.WinnerIds.Add(exiled.PlayerId);
        DecidedWinner = true;

    }
    public override bool OnCompleteTask()
    {
        if (IsTaskFinished)
        {
            Player.MarkDirtySettings();
        }

        return true;
    }
}