using System.Collections.Generic;
using AmongUs.GameOptions;
using UnityEngine;
using Hazel;

using TownOfHost.Roles.Core;

using static TownOfHost.Modules.MeetingVoteManager;
using static TownOfHost.Translator;

namespace TownOfHost.Roles.Crewmate;
public sealed class FortuneTeller : RoleBase
{
    public static readonly SimpleRoleInfo RoleInfo =
        SimpleRoleInfo.Create(
            typeof(FortuneTeller),
            player => new FortuneTeller(player),
            CustomRoles.FortuneTeller,
            () => RoleTypes.Crewmate,
            CustomRoleTypes.Crewmate,
            28000,
            SetupOptionItem,
            "fo",
            "#6b3ec3",
            introSound: () => GetIntroSound(RoleTypes.Scientist)
        );
    public FortuneTeller(PlayerControl player)
    : base(
        RoleInfo,
        player
    )
    {
        Max = OptionMaximum.GetFloat();
        Divination.Clear();
        count = 0;
        uranaimode = false;
        Votemode = (VoteMode)OptionVoteMode.GetValue();
        rolename = Optionrolename.GetBool();
        srole = OptionRole.GetBool();
    }

    private static OptionItem OptionMaximum;
    private static OptionItem OptionVoteMode;
    private static OptionItem Optionrolename;
    private static OptionItem OptionRole;
    public float Max;
    public VoteMode Votemode;
    public bool rolename;
    public bool srole;
    int count;
    bool uranaimode;
    List<byte> Divination = new();

    enum Option
    {
        Maximum,
        Votemode,
        rolename, //占った相手の名前の上に占い結果を表示するかの設定
        tRole //占い時役職を表示するか、陣営を表示するかの設定
    }
    public enum VoteMode
    {
        uvote,
        SelfVote,
    }

    private static void SetupOptionItem()
    {
        OptionMaximum = FloatOptionItem.Create(RoleInfo, 10, Option.Maximum, new(1f, 99f, 1f), 1f, false)
            .SetValueFormat(OptionFormat.Times);
        OptionVoteMode = StringOptionItem.Create(RoleInfo, 11, Option.Votemode, EnumHelper.GetAllNames<VoteMode>(), 0, false);
        Optionrolename = BooleanOptionItem.Create(RoleInfo, 12, Option.rolename, true, false);
        OptionRole = BooleanOptionItem.Create(RoleInfo, 13, Option.tRole, true, false);
    }

    private void SendRPC(byte targetid)
    {
        using var sender = CreateSender(CustomRPC.SetFTc);
        sender.Writer.Write(count);
        using var sender2 = CreateSender(CustomRPC.SetFTtarget);
        sender2.Writer.Write(targetid);
    }
    public override void ReceiveRPC(MessageReader reader, CustomRPC rpcType)
    {
        if (rpcType == CustomRPC.SetFTc)
        {
            count = reader.ReadInt32();
        }
        else if (rpcType == CustomRPC.SetFTtarget)
        {
            Divination.Add(reader.ReadByte());
        }
        else return;
    }
    public override string GetSuffix(PlayerControl seer, PlayerControl seen = null, bool isForMeeting = false)
    {
        seen ??= seer;
        if (Divination.Contains(seen.PlayerId) && rolename)
        {
            if (srole)
                return $"<color={Utils.GetRoleColorCode(seen.GetCustomRole())}>" + GetString(seen.GetCustomRole().ToString());
            else return GetString(seen.GetCustomRole().GetCustomRoleTypes().ToString());
        }
        return "";
    }
    public override string GetProgressText(bool comms = false) => Utils.ColorString(Max <= count ? Color.gray : Color.cyan, $"({Max - count})");
    public override bool CheckVoteAsVoter(byte votedForId, PlayerControl voter)
    {
        if (Max > count && voter.PlayerId == Player.PlayerId)
        {
            if (Votemode == VoteMode.uvote)
            {
                if (Player.PlayerId == votedForId || votedForId == Skip) return true;
                Uranai(votedForId);
                return false;
            }
            else
            {
                if (!uranaimode && votedForId == Player.PlayerId)
                {
                    uranaimode = true;
                    Utils.SendMessage("占いモードになりました！\n占いたいプレイヤーに投票する\nスキップでキャンセル、\nもう一度自投票することで自身に票が入る", Player.PlayerId);
                    return false;
                }
                if (uranaimode)
                {
                    if (votedForId == Player.PlayerId)
                    {
                        uranaimode = false;
                        return true;
                    }
                    if (votedForId == Skip)
                    {
                        uranaimode = false;
                        Utils.SendMessage("占いをキャンセルしました", Player.PlayerId);
                        return false;
                    }
                    if (votedForId != Skip && votedForId != Player.PlayerId)
                    {
                        Uranai(votedForId);
                        uranaimode = false;
                    }
                    return false;
                }
            }
        }
        return true;
    }
    public void Uranai(byte votedForId)
    {
        count++;
        SendRPC(votedForId);
        Divination.Add(votedForId);
        Logger.Info($"Player: {Player.name},Target: {votedForId}, count: {count}", "FortuneTeller");
        var role = Utils.GetPlayerById(votedForId).GetCustomRole();
        var s = "";
        if (role.IsCrewmate()) s = "です！";
        else s = "です...";
        Utils.SendMessage(Utils.GetPlayerById(votedForId).name + "さんを占いました。\n結果は.." + (srole ? GetString($"{role}") : GetString($"{role.GetCustomRoleTypes()}")) + s + $"\n\n残り{Max - count}回占うことができます", Player.PlayerId);
    }
}