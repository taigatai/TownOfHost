using HarmonyLib;
using UnityEngine;

namespace TownOfHost
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class Zoom
    {
        public static int size = (int)HudManager.Instance.UICamera.orthographicSize;
        public static void Postfix()
        {
            if ((GameStates.IsFreePlay && Main.UseZoom.Value) || (GameStates.IsInGame && !PlayerControl.LocalPlayer.IsAlive() && PlayerControl.LocalPlayer.CanMove & GameStates.IsInTask && Main.UseZoom.Value))
            {
                if (Input.mouseScrollDelta.y < 0) size += (int)1.5;
                if (Input.mouseScrollDelta.y > 0 && size > 1.5) size -= (int)1.5;
                HudManager.Instance.UICamera.orthographicSize = size;
                Camera.main.orthographicSize = size;
            }
            else
            {
                HudManager.Instance.UICamera.orthographicSize = 3.0f;
                Camera.main.orthographicSize = 3.0f;
                HudManager.Instance.Chat.transform.localScale = Vector3.one; //チャットの位置を戻す 参考: TOH_Y https://github.com/Yumenopai/TownOfHost_Y/blob/main/Modules/Zoom.cs
                if (GameStates.IsMeeting) MeetingHud.Instance.transform.localScale = Vector3.one;
            }
        }
    }
}