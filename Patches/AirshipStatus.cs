using HarmonyLib;
using UnityEngine;

using TownOfHost.Roles.Core;

namespace TownOfHost
{
    //参考元:https://github.com/yukieiji/ExtremeRoles/blob/master/ExtremeRoles/Patches/AirShipStatusPatch.cs
    [HarmonyPatch(typeof(AirshipStatus), nameof(AirshipStatus.PrespawnStep))]
    public static class AirshipStatusPrespawnStepPatch
    {
        public static bool Prefix()
        {
            if (PlayerControl.LocalPlayer.Is(CustomRoles.GM))
            {
                if (PlayerControl.LocalPlayer.transform.position == new Vector3(-25f, 40f, 0.04f) && Main.NormalOptions.MapId == 4)
                    RandomSpawn.TP(PlayerControl.LocalPlayer.NetTransform, new Vector2(0f, 0f));
                return false; // GMは湧き画面をスキップ
            }
            return true;
        }
    }
}