# Town Of Host-K

# [DiscordServer](https://discord.gg/5DPqH8seFq)

Englishはいつか作る<br>

<p align="center"><a href="https://github.com/KYMario/TownOfHost-K/releases/"><img src="https://badgen.net/github/release/KYMario/TownOfHost-K"></a></p>

## この Mod について

この Mod は非公式のものであり、この Mod の開発に関して Among Us の開発元である"Innersloth"は一切関与していません。<br>
この Mod の問題などに関して公式に問い合わせないでください。<br>

この Mod はTOHに役職や機能など追加したModです
TOH-Kで起きたバグは本家や他MODには報告しないでまずはkに報告してください。

[本家TOHはこちら](https://github.com/tukasa0001/TownOfHost) TOH-k以外の説明を省いているので本家の役職などはこちらから

## リリース

AmongUsバージョン : **2023.10.24**<br>

**最新版は[こちら](https://github.com/KYMario/TownOfHost-K/releases/latest)**

過去バージョンは[こちら](https://github.com/KYMario/TownOfHost-K/releases)

## 機能
### ホットキー

#### ホストのみ
| キー                | 機能                            | 使えるシーン               |
| ------------------- | ------------------------------- | --------------------------- |
| `Shift`+`N`+`Enter` | ミーティングを票を変えずに終了   | ゲーム内                    |

### チャットコマンド
チャットコマンドはチャットで入力して使用できるコマンドです。

#### ホストのみ
| コマンド                                 | 機能                                          | 使えるシーン    |
| ---------------------------------------- | --------------------------------------------- | --------------- |
| /w                                       | 勝利した陣営を表示                             | どこでも        |
| /forceend<br>/fe                         | 廃村として終了する                             |  ゲーム内       |
| /allplayertp<br>/apt                     | 全てのプレイヤーをロビーの真ん中にテレポートする| ロビー内        |

#### 全クライアント
| コマンド                            | 機能                            | 使えるシーン    |
| ----------------------------------- | ------------------------------- | ---------------  |
| /timer<br>/tr                       | 部屋が落ちるまでのタイマーを表示 | ロビー内         |
| /voice<br>/vo                       | 読み上げの声設定 [詳細はこちら](#読み上げ機能)    | どこでも         |
| /help roles <役職><br>/help r <役職>| 現在の役職設定を表示             | どこでも         |

### テンプレート
定型文を送信できる機能です。<br>

| タグ           | シーン                      | 対象               |
| -------------- | --------------------------- | ------------------ |
| start          | ゲーム開始のカウントダウン前 | 全員 |

#### 変数展開

| 変数名               | 内容                       |
| -------------------- | ------------------------- |
| Timer                | 部屋が落ちるまでのタイマー  |
| Roles                | 有効な役職を表示           |

#### クライアント設定
| オプション　　                      | 機能                                           | 使えるシーン    |
| ----------------------------------- | ---------------------------------------------- | ---------------  |
| 廃村                                | ホストが押すと廃村が行われる                    | ゲーム内         |
| 読み上げ                            | 読み上げのON/OFF                                | どこでも         |
| 一部言語に対応させる                | /help rolesでも言語を変えるかのON/OFF            | どこでも         |
| ウェブフック                        | ゲーム開始時や終了時にWebHookが送信するかのON/OFF| どこでも         |
| ズームを有効にする                  | 画面のサイズ？を変えれるようにするかのON/OFF     | どこでも/死亡時  |
| 読み上げの同期                      | ホストと声の設定を同期                          | どこでも/参加時   |

### 読み上げ機能
棒読みちゃんを使用してチャットを読み上げることができる機能です<br>
コマンドで声を設定することができます<br>

**[使い方] /voice <音質> <音量> <速度> <音程>**


> **Note**
> - 棒読みちゃんを起動してる時のみ読み上げ設定をONにしてください (自動でOFFになります)
> - 棒読みちゃんのポートはデフォルトの50080でないと読み上げされません (次のアプデ辺りで設定できるようにします)

## 役職

### Bomber/爆弾魔
陣営：インポスター<br>
判定：シェイプシフター<br>

名前に◆がある時キルをすると対象に爆弾が付与される。<br>
爆弾は一定時間経過すると爆発し、対象の近くにいた人に巻き込まれる。<br>
爆弾が爆発する前に会議が始まると爆発しない<br>
シェイプシフトで切り替えれる<br>

### 設定
|設定名         |
| ------------- |
|爆発までの時間  |
|爆発範囲　　　  |
|爆発できる回数  |
|クールダウン　  |

### AntiReporter/アンチレポーター
陣営：インポスター<br>
判定：シェイプシフター<br>

シェイプシフトした後名前に◆がある状態でキルボタンを押すことで、対象が死体通報できなくすることができる<br>

お菓子(クルー)くれなきゃイタズラ(メガホン📢破壊)しちゃうぞ！<br>

### 設定
|設定名　　                         　|
|------------------------------------ |
|クールダウン　　　                  　|
|能力使用回数　　　　                　|
|会議後リセットする　　              　|
|効果時間(0秒で時間無効)　※動きません！|


### TeleportKiller/テレポートキラー
陣営：インポスター<br>
判定：シェイプシフター<br>

変身することでシェイプシフトで選んだ人をキルできる。<br>
この時、対象と自分の位置が入れ替わる<br>

あなたに渡したお菓子にテレポートできるやつ仕込んでたのよ(?)<br>

### 設定
|設定名　　　　　              　　　　|
|-------------------------------------- |
|キルクール　　　　　　　              　|
|クールダウン　　　　　　              　|
|相手がベント時自爆する　              　|
|能力使用回数　　　　              　　　|
|シェイプ持続時間　　　     　       　  |
|シェイプの跡を残す　※正常に動きません！|

### MadJester/マッドジェスター
陣営：インポスター<br>
判定：クルーメイト<br>
カウント：クルー<br>

クルーだがインポスターの味方をする。<br>
インポスターと互いに認識できない。<br>
タスクを全て完了させた状態で会議で追放された時にインポスター勝利できる。<br>

### 設定
|設定名　　　　　　　　 　 |
|------------------------- |
|マッドジェスターのタスク数 |

### VentMaster/ベントマスター
陣営：クルーメイト
判定：エンジニア

クールダウン、持続時間なしで何度でもベントに入ることができる。<br>
また、ベントが開閉するごとにベントマスターはフラッシュを見ることができる。<br>
自身がベントに出入りしている場合、フラッシュは見えない。<br>

### ToiletFan/トイレファン
陣営：クルーメイト
判定：エンジニア

[SNR](https://github.com/SuperNewRoles/SuperNewRoles/)より<br>

ベントに入ることでラウンジのトイレのドアが開く。<br>
ただしAirShip以外はただのタスクニートになるのだ<br>

### 設定
|設定名　　  |
|-----------  |
|クールダウン |

### Bakery/パン屋
陣営：クルーメイト<br>
判定：クルーメイト<br>

**美味しいパンが焼けました～！**<br>

会議開始時にパン屋が生存していると生存中メッセージが表示される。<br>

### FortuneTeller/占い師
陣営：クルーメイト<br>
判定：クルーメイト<br>

自投票することで占いモードが発動する。<br>
もう一度占いたい相手に投票することで占いが可能。<br>
設定次第では、自投票せずに投票するだけで占いは可能。<br>

### 設定
|設定名　　　　　　　　  |
|------------------------ |
|能力使用回数　　　　      |
|占い方法　　　　　　　　　|
|┣投票　　　　　　　　 　　|
|┣自投票　　　　　　　 　　|
|名前の上に結果を残す　　　|
|陣営ではなく役職を見れる  |

### JackalMafia/ジャッカルマフィア
陣営：ニュートラル(第三陣営)<br>
判定：インポスター<br>
カウント：ジャッカル<br>

初期状態ではキルが封じられている。<br>
ジャッカルが死亡後、自身もキルが可能になる。<br>

### 設定
|設定名　　　　           |
|------------------------ |
|キルクール　　　　　　　  |
|ベント使える　　　  　　　|
|サボタージュを使用できる  |
|インポスター視界　　　　  |

### RemoteKiller/リモートキラー
陣営：ニュートラル(第三陣営)<br>
判定：インポスター<br>
カウント：リモートキラー<br>

リモートキラーはすべてのプレイヤーを殺すことができる。キルして対象を決め、ベントに入ると対象を遠隔でキルできる。<br>
普通のキルを行うことはできない。<br>
生存者の半数以上がリモートキラーかつ、他の人が無害な役職でリモートキラーが勝利する。<br>

### 設定
|設定名　　　　       　|
|---------------------- |
|キルクール　　　　　　　|
|キル時にテレポートする　|

### Chef/シェフ
陣営：ニュートラル(第三陣営)<br>
判定：インポスター<br>
カウント：クルー<br>

キルボタンを使ってその人に料理を渡せる。<br>
全員に料理を渡した状態で吊られると単独勝利、また誰かが勝利した時生存していると追加勝利へとなる。<br>

たまにはお菓子作ってみようかな.. パン屋も誘ってみるか、<br>

### CountKiller/カウントキラー
陣営：ニュートラル(第三陣営)<br>
判定：インポスター<br>
カウント：クルー<br>

カウントキラーはすべてのプレイヤーを殺すことができる。<br>
設定回数キルすると単独勝利できる。<br>

### 設定
|設定名　　　　     |
|------------------ |
|キルクール　　　　　|
|ベント使える　　　　|
|インポスター視界　　|
|勝利できるキル回数　|

#### クレジット
[Town Of Host](https://github.com/tukasa0001/TownOfHost)本家！

[Town Of Host_Y](https://github.com/Yumenopai/TownOfHost_Y) 機能など

[Town Of Host For E](https://github.com/AsumuAkaguma/TownOfHost_ForE) 機能など

[Town Of Host-H](https://github.com/Hyz-sui/TownOfHost-H) 10.24アプデの対応

[SuperNewRoles](https://github.com/ykundesu/SuperNewRoles) 役職など
