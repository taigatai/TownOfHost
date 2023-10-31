using HarmonyLib;

namespace TownOfHost.Patches;

[HarmonyPatch(typeof(SabotageButton), nameof(SabotageButton.DoClick))]
public static class SabotageButtonDoClickPatch
{
    public static bool Prefix()
    {
        if (!PlayerControl.LocalPlayer.inVent && GameManager.Instance.SabotagesEnabled())
        {
            DestroyableSingleton<HudManager>.Instance.ToggleMapVisible(new MapOptions
            {
                Mode = MapOptions.Modes.Sabotage
            });
        }

        return false;
    }
}
/*[HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
public static class KillButtonDoClickPatch
{
    public static void Prefix()
    {
        var players = PlayerControl.LocalPlayer.GetPlayersInAbilityRangeSorted(false);
        PlayerControl closest = players.Count <= 0 ? null : players[0];
        if (!GameStates.IsInTask || !PlayerControl.LocalPlayer.CanUseKillButton() || closest == null
            || PlayerControl.LocalPlayer.Data.IsDead || HudManager._instance.KillButton.isCoolingDown) return;
        PlayerControl.LocalPlayer.CheckMurder(closest); //一時的な修正
    }
}*/
