![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/ruby.png)

# Ruby(Furigana) Text Mesh Pro

TextMeshProに振り仮名(ふりがな、フリガナ、ルビ)タグを追加します。

This plugin adds ruby tag support to "Text Mesh Pro" Unity plugin. 

`TextMeshPro`が必要なため、`Package Manager`からインストールしてください。

You need to have `TextMeshPro` plugin in your project. You can install TMPro via `Package Manager`.

チェックしたUnityバージョンとTextMeshProは以下の通りです。

I checked Unity and TextMeshPro Version are below.

UnityVer:2022.3.13f1 

TextMeshProVer:3.0.6

[Ja]

TextMeshProは`Unity 2023.2`から[非推奨](https://forum.unity.com/threads/2023-2-latest-development-on-textmesh-pro.1434757/)になります。

この意味は、namespaceは変わらず、現状のベータ版ではTextMeshProが `com.unity.ui` に統合されたため、TextMeshProパッケージをパッケージマネージャからインストールすると重複しエラーが起こる状態になります。

そのため、RubyTexxtMeshProをそのまま使用することは現状可能だと思われます。

[En]

TextMeshPro will be [deprecated](https://forum.unity.com/threads/2023-2-latest-development-on-textmesh-pro.1434757/) starting `Unity 2023.2`.

What this means is that the namespace remains the same, but in the current beta version, TextMeshPro has been integrated into `com.unity.ui`, so installing the TextMeshPro package from the package manager will result in duplication and errors.

Therefore, it is currently possible to use RubyTexxtMeshPro as is.

# Disruptive change

### ver 1.2

[Ja]
追加 : `RubyTextMeshProDefinitions` に `BASE_NO_OVERRAP_RUBY_ALIGNMENT` を追加しました。
追加 : `BASE_NO_OVERRAP_RUBY_ALIGNMENT` 用に `rubyMargin` を追加しました。 ルビの隙間を指定します。
変更 : SerializeField パラメータを `_<xyz>` に変更しました。 以前のバージョンと互換性を保つため `FormerlySerializedAs(<xyz>)` を定義していますが、ご注意ください。
変更 :　`BASE_NO_OVERRAP_RUBY_ALIGNMENT` 用にルビとベースの文字の描画位置を計算する処理を追加しました。負荷が上がっている可能性があります。
削除 : `RubyTextMeshPro.UnditedText` / `RubyTextMeshProUGUI.UnditedText` : 削除されました。 `uneditedText` をご使用ください。

[En]
Added : `BASE_NO_OVERRAP_RUBY_ALIGNMENT` for `RubyTextMeshProDefinitions`.
Added : `rubyMargin` for `BASE_NO_OVERRAP_RUBY_ALIGNMENT`. Specify the ruby margin.
Changed : SerializeField parameter to `_<xyz>`. Note that `FormerlySerializedAs(<xyz>)` is defined for compatibility with previous versions.
Changed : Added processing for `BASE_NO_OVERRAP_RUBY_ALIGNMENT` to calculate the drawing position of ruby and base characters. This may have increased the load.
Removed : `RubyTextMeshPro.UnditedText` / `RubyTextMeshProUGUI.UnditedText` : removed. Please use `uneditedText`.

Translated with www.DeepL.com/Translator (free version)

### ver 1.1

[Ja]
削除 : `allVCompensationRuby` / `allVCompensationRubyLineHeight` : `rubyLineHeight` が空文字の場合、 いままでの `allVCompensationRuby:false` と同等の効果になります。また、 `rubyLineHeight` に値が入っている場合、 `allVCompensationRubyLineHeight` の値と同様の効果になります。
削除予定 : `RubyTextMeshPro.UnditedText` / `RubyTextMeshProUGUI.UnditedText` : 次のバージョンで削除されます。`uneditedText`をご使用ください。

[En]
Removed : `allVCompensationRuby` / `allVCompensationRubyLineHeight` : If `rubyLineHeight` is an empty string, it will be the `allVCompensationRuby:false` value up to now, and if `rubyLineHeight` is a value, it will be the `allVCompensationRubyLineHeight` value.
Obsolete : `RubyTextMeshPro.UnditedText` / `RubyTextMeshProUGUI.UnditedText` : Will be removed in the next version. Please use `uneditedText`.

# Features
### Realtime Ruby Text
あなたは`<ruby=にほんご>日本語</ruby>`タグもしくは省略した`<r=にほんご>日本語</r>`タグを使用できます。
また、半角ダブルクォーテーションで囲っても動作します。
`<ruby="にほんご">日本語</ruby>`タグも`<r="にほんご">日本語</r>`タグもOKです。

You can use `<ruby=ice>fire</ruby>` tag or `<r=ice>fire</r>` tag.  Both are the same.
It can also work with double quotes.
`<ruby="ice">fire</ruby>` tag or `<r="ice">fire</r>` tag.

# How To Use

## 1. Load from github

[Ja]

GitHubからインストールをすることが可能です。

この場合、TextMeshPro3.0.6の依存情報を残しているため、TextMeshProをインストールしていない場合、インストールされます。


[En]

There is a way to install from GitHub.

In this case, the dependency information for TextMeshPro 3.0.6 is retained, so if TextMeshPro is not installed, it will be installed.

[Install]

Unity > Window > PackageManager > + > Add package from git url... > Add the following

`https://github.com/jp-netsis/RubyTextMeshPro.git?path=/Assets/RubyTextMeshPro#v1.2.0`

## 2. Copy Source Only

ソースのみコピーする方法があります。 これは。`Unity2023.2` 以降で現在有効です。
Unity2023の安定版が出たら、RubyTextMeshProは com.unity.ui に依存するよう変更します。

There is a way to copy only the source. This is. Currently valid for `Unity2023.2` and later.
Once a stable version of Unity2023 is available, RubyTextMeshPro will be changed to depend on com.unity.ui.

* (1). あなたには`TextMeshPro`プラグインが必要です。`Package Manager`からインストールしてください。Asset StoreからのText Mesh Proのインストールはしないでください。
* (1). You need to have `TextMeshPro` plugin in your project. You can install TMPro via `Package Manager`. DO NOT Install Text Mesh Pro from Asset Store.

* (2). RubyTextMeshProディレクトリをコピーすれば使用可能です
* (2). You can use it by copying the RubyTextMeshPro directory

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/add_ruby.gif)

## Usage Description

`<ruby=かんじ>漢字</ruby>`

### RubyShowType

RUBY_ALIGNMENT ルビに合わせて文字を表示します

BASE_ALIGNMENT 元の文字に合わせて文字を表示します

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/align_width.gif)

### rubyLineHeight

この機能により、rubyを使用しない場合でも、同じ隙間を持つことができます。
この文字列を空にすることで、この機能はスキップされます。

This function allows you to have the same gap even if you don't use ruby.
Empty this string to skip this feature.

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/vcompensation.gif)

# Known Issues
* (1).TextMeshPro のソースは改変していません。アラインでいくつか問題が起こります。
* (1).TextMeshPro source has not changed. So text alignment is problematic.

* (2).ルビの最大文字数よりもテキストボックスを小さくしないでください。表示崩れが起きます。
* (2).Do not make the text box smaller than the maximum number of characters in ruby. Display collapse will occur.

* (1)_1.'BASE_ALIGN'で左寄せの場合かつルビが行頭にありつつ元の文字より多い場合、枠の外まで表示されます
* (1)_1.'BASE_ALIGN' setting is left align and the ruby is at the beginning of the line but more than the original character, it will be displayed outside the frame.

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/issue_lefttop.png)

* (1)_2.'BASE_ALIGN'で中央寄せの場合かつルビが元の文字より多い場合、中央に表示されません。 'RUBY_ALIGN' を使用すると解消される場合があります。
* (1)_2.'BASE_ALIGN' setting is center align and the ruby is at more than the original character, can't displayed center. 'RUBY_ALIGN' used, it may be solved.

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/issue_base_alignment_center.png)

* (1)_3.'BASE_ALIGN'で右寄せの場合かつルビが行頭にありつつ元の文字より多い場合、(1)_1と違い、枠の内まで表示されます
* (1)_3.'BASE_ALIGN' setting is left align and the ruby is at the beginning of the line but more than the original character, Different from (1)_1 it will be displayed in the frame.

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/issue_base_alignment_bottomright.png)

# Use Font File

Rounded M+

http://jikasei.me/font/rounded-mplus

ありがとうございます！

Thank You!

# Reference list

https://forum.unity.com/threads/how-to-display-extra-little-characters-above-characters-in-a-text.387772/

http://baba-s.hatenablog.com/entry/2019/01/10/122500

ありがとうございます！

Thank You!

# Other
* (1).TextMeshProは素晴らしいプラグインですので、rubyタグの追加はいつか行われるでしょう。そのときこのプロジェクトは削除します。
* (1).TextMeshPro is very nice Plugin. If TextMeshPro add ruby tag well, delete my git project.

* (2).日本語以外のチェックはしていません。
* (2).Not checked anything other than Japanese

# Contribution

すべての貢献を歓迎します。必ずプロジェクトのコードスタイルに従ってください。

All contributions are welcomed. Just make sure you follow the project's code style.  

Contact: jenomoto@netsis.jp

