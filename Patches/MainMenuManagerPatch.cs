using System;
using HarmonyLib;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;
using AmongUs.Data;
using Assets.InnerNet;
using AmongUs.Data.Player;
using System.Collections.Generic;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Linq;
using System.Text.RegularExpressions;

namespace TownOfHost
{
    [HarmonyPatch(typeof(MainMenuManager))]
    public class MainMenuManagerPatch
    {
        private static PassiveButton template;
        private static PassiveButton discordButton;
        public static PassiveButton UpdateButton { get; private set; }
        public static PassiveButton UpdateButton2;
        public static AnnouncementPopUp updatea;
        private static PassiveButton gitHubButton;

        [HarmonyPatch(nameof(MainMenuManager.Start)), HarmonyPostfix, HarmonyPriority(Priority.Normal)]
        public static void StartPostfix(MainMenuManager __instance)
        {
            if (template == null) template = __instance.quitButton;
            if (template == null) return;
            //Discordボタンを生成
            if (discordButton == null)
            {
                discordButton = CreateButton(
                    "DiscordButton",
                    new(-1f, -1f, 1f),
                    new(88, 101, 242, byte.MaxValue),
                    new(148, 161, byte.MaxValue, byte.MaxValue),
                    () => Application.OpenURL(Main.DiscordInviteUrl),
                    "Discord");
            }
            discordButton.gameObject.SetActive(Main.ShowDiscordButton);

            // GitHubボタンを生成
            if (gitHubButton == null)
            {
                gitHubButton = CreateButton(
                    "GitHubButton",
                    new(1f, -1f, 1f),
                    new(153, 153, 153, byte.MaxValue),
                    new(209, 209, 209, byte.MaxValue),
                    () => Application.OpenURL("https://github.com/KYMario/TownOfHost-K"),
                    "GitHub");
            }

            //Updateボタンを生成
            if (UpdateButton == null)
            {
                UpdateButton = CreateButton(
                    "UpdateButton",
                    new(0f, -1.7f, 1f),
                    new(0, 202, 255, byte.MaxValue),
                    new(60, 255, 255, byte.MaxValue),
                    () =>
                    {
                        UpdateButton.gameObject.SetActive(false);
                        ModUpdater.StartUpdate(ModUpdater.downloadUrl);
                    },
                    $"{Translator.GetString("updateButton")}\n{ModUpdater.latestTitle}",
                    new(2.5f, 1f));
            }
            // アップデート(詳細)ボタンを生成
            if (UpdateButton2 == null)
            {
                UpdateButton2 = CreateButton(
                    "UpdateButton2",
                    new(1.3f, -1.9f, 1f),
                    new(153, 153, 153, byte.MaxValue),
                    new(209, 209, 209, byte.MaxValue),
                    () =>
                    {
                        if (updatea == null)
                        {
                            updatea = Object.Instantiate(__instance.announcementPopUp);
                        }
                        updatea.name = "Update Detail";
                        updatea.gameObject.SetActive(true);
                        updatea.AnnouncementListSlider.SetActive(false);
                        updatea.Title.text = "TOH-K " + ModUpdater.latestTitle;
                        updatea.AnnouncementBodyText.text = Regex.Replace(ModUpdater.body.Replace("#", "").Replace("**", ""), @"\[(.*?)\]\(.*?\)", "$1");
                        updatea.DateString.text = "Latest Release";
                        updatea.SubTitle.text = "";
                        updatea.ListScroller.gameObject.SetActive(false);
                    },
                    "▽",
                    new(0.5f, 0.5f));
            }
            UpdateButton.gameObject.SetActive(false);

#if RELEASE
            // フリープレイの無効化
            var howToPlayButton = __instance.howToPlayButton;
            var freeplayButton = howToPlayButton.transform.parent.Find("FreePlayButton");
            if (freeplayButton != null)
            {
                freeplayButton.gameObject.SetActive(false);
            }
            // フリープレイが消えるのでHowToPlayをセンタリング
            howToPlayButton.transform.SetLocalX(0);
#endif
        }

        /// <summary>TOHkロゴの子としてボタンを生成</summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="normalColor">普段のボタンの色</param>
        /// <param name="hoverColor">マウスが乗っているときのボタンの色</param>
        /// <param name="action">押したときに発火するアクション</param>
        /// <param name="label">ボタンのテキスト</param>
        /// <param name="scale">ボタンのサイズ 変更しないなら不要</param>
        private static PassiveButton CreateButton(string name, Vector3 localPosition, Color32 normalColor, Color32 hoverColor, Action action, string label, Vector2? scale = null)
        {
            var button = Object.Instantiate(template, CredentialsPatch.TohkLogo.transform);
            button.name = name;
            Object.Destroy(button.GetComponent<AspectPosition>());
            button.transform.localPosition = localPosition;

            button.OnClick = new();
            button.OnClick.AddListener(action);

            var buttonText = button.transform.Find("FontPlacer/Text_TMP").GetComponent<TMP_Text>();
            buttonText.DestroyTranslator();
            buttonText.fontSize = buttonText.fontSizeMax = buttonText.fontSizeMin = 3.5f;
            buttonText.enableWordWrapping = false;
            buttonText.text = label;
            var normalSprite = button.inactiveSprites.GetComponent<SpriteRenderer>();
            var hoverSprite = button.activeSprites.GetComponent<SpriteRenderer>();
            normalSprite.color = normalColor;
            hoverSprite.color = hoverColor;

            // ラベルをセンタリング
            var container = buttonText.transform.parent;
            Object.Destroy(container.GetComponent<AspectPosition>());
            Object.Destroy(buttonText.GetComponent<AspectPosition>());
            container.SetLocalX(0f);
            buttonText.transform.SetLocalX(0f);
            buttonText.horizontalAlignment = HorizontalAlignmentOptions.Center;

            var buttonCollider = button.GetComponent<BoxCollider2D>();
            if (scale.HasValue)
            {
                normalSprite.size = hoverSprite.size = buttonCollider.size = scale.Value;
            }
            // 当たり判定のズレを直す
            buttonCollider.offset = new(0f, 0f);

            return button;
        }

        // プレイメニュー，アカウントメニュー，クレジット画面が開かれたらロゴとボタンを消す
        [HarmonyPatch(nameof(MainMenuManager.OpenGameModeMenu))]
        [HarmonyPatch(nameof(MainMenuManager.OpenAccountMenu))]
        [HarmonyPatch(nameof(MainMenuManager.OpenCredits))]
        [HarmonyPostfix]
        public static void OpenMenuPostfix()
        {
            if (CredentialsPatch.TohkLogo != null)
            {
                CredentialsPatch.TohkLogo.gameObject.SetActive(false);
            }
        }
        [HarmonyPatch(nameof(MainMenuManager.ResetScreen)), HarmonyPostfix]
        public static void ResetScreenPostfix()
        {
            if (CredentialsPatch.TohkLogo != null)
            {
                CredentialsPatch.TohkLogo.gameObject.SetActive(true);
            }
        }
    }
    public class ModNews
    {
        public int Number;
        public int BeforeNumber;
        public string Title;
        public string SubTitle;
        public string ShortTitle;
        public string Text;
        public string Date;

        public Announcement ToAnnouncement()
        {
            var result = new Announcement
            {
                Number = Number,
                Title = Title,
                SubTitle = SubTitle,
                ShortTitle = ShortTitle,
                Text = Text,
                Language = (uint)DataManager.Settings.Language.CurrentLanguage,
                Date = Date,
                Id = "ModNews"
            };

            return result;
        }
    }

    //TOH_Yを参考にさせて貰いました ありがとうございます
    [HarmonyPatch]
    public class ModNewsHistory
    {
        public static List<ModNews> AllModNews = new();
        public static void Init()
        {
            {
                {
                    var news = new ModNews
                    {
                        Number = 100002,
                        //BeforeNumber = 0,
                        Title = "ハッピーハロウィンついにTOH-Kリリース！",
                        SubTitle = "やっとリリースしたよ！",
                        ShortTitle = "◆TOH-K v5.1.14",
                        Text = "ハロウィンにリリースしたのだぁー\n\rってことで(?)TOH-Kを使ってくれてありがとおおお！\n\r\n\rあ、詳しくは<nobr><link=\"https://github.com/KYMario/TownOfHost-K5.0.1Release/blob/Release/README.md\">README</nobr></link> 見てね～\n\r\n\rマジでここなに書いたらいいんやろな なにも思いつかないぜ(これただ独り言めっちゃ書いてるやばいやつだ)\n\rまぁTOH-Kのこと話します 初リリースってことで元々24役職(ﾈﾀ役職含め)あったのを12役職まで減らしたんだぜ！ 多分いつかアプデで一部は追加すると思う\n\rそ～し～て～実は隠し要素あります！ 1つはコマンド、もう一つは隠しコマンド(key)で使えるようになるよ！探してみてね\n\rあとYouTubeとかTwitter(X)でTOHkの動画とかじゃんじゃん投稿しちゃって！ あ、でもちゃんとMODで本家じゃなくてTOHkってことわかるようにしてね それだけ守ってくれれば.. 配信とか動画で使ってくれるとめちゃ喜びます！\n\rそれじゃあこのぐらいでいいかな、じゃあkを楽しんできてね～\n\r\n\rTOH-K開発者: けーわい,タイガー\n\r協力者: ねむa (もう開発者でいい気もしてきｔ(( )",
                        Date = "2023-10-31T00:00:00Z"

                    };
                    AllModNews.Add(news);
                }
                AnnouncementPopUp.UpdateState = AnnouncementPopUp.AnnounceState.NotStarted;
            }
        }

        [HarmonyPatch(typeof(PlayerAnnouncementData), nameof(PlayerAnnouncementData.SetAnnouncements)), HarmonyPrefix]
        public static bool SetModAnnouncements(PlayerAnnouncementData __instance, [HarmonyArgument(0)] ref Il2CppReferenceArray<Announcement> aRange)
        {
            if (AllModNews.Count < 1)
            {
                Init();
                AllModNews.Sort((a1, a2) => { return DateTime.Compare(DateTime.Parse(a2.Date), DateTime.Parse(a1.Date)); });
            }

            List<Announcement> FinalAllNews = new();
            AllModNews.Do(n => FinalAllNews.Add(n.ToAnnouncement()));
            foreach (var news in aRange)
            {
                if (!AllModNews.Any(x => x.Number == news.Number))
                    FinalAllNews.Add(news);
            }
            FinalAllNews.Sort((a1, a2) => { return DateTime.Compare(DateTime.Parse(a2.Date), DateTime.Parse(a1.Date)); });

            aRange = new(FinalAllNews.Count);
            for (int i = 0; i < FinalAllNews.Count; i++)
                aRange[i] = FinalAllNews[i];

            return true;
        }
    }
}