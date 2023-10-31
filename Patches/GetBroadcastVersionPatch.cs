using HarmonyLib;

namespace TownOfHost.Patches;

[HarmonyPatch(typeof(Constants), nameof(Constants.GetBroadcastVersion))]
class GetBroadcastVersionPatch
{
    static void Postfix(ref int __result)
    {
        if (GameStates.IsLocalGame || GameStates.IsFreePlay || IsCs()) return;
        //Logger.Info($"{__result}", "ConnectVersion");
        __result += 25;
        //Logger.Info($"{__result}", "ConnectVersion");

    }

    [HarmonyPatch(typeof(Constants), nameof(Constants.IsVersionModded))]
    public static class IsVersionModdedPatch
    {
        public static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
    public static bool IsCs()
    {
        if (ServerManager.Instance == null) return false;
        var sn = ServerManager.Instance.CurrentRegion.TranslateName;
        if (sn is StringNames.ServerNA or StringNames.ServerEU or StringNames.ServerAS or StringNames.ServerSA)
            return false;
        else return true;
    }
}
