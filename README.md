![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/ruby.png)

# Ruby(Furigana) Text Mesh Pro

TextMeshProに振り仮名(ふりがな、フリガナ、ルビ)タグを追加します。

This plugin adds ruby tag support to "Text Mesh Pro" Unity plugin. 

`TextMeshPro`が必要なため、`Package Manager`からインストールしてください。

You need to have `TextMeshPro` plugin in your project. You can install TMPro via `Package Manager`.

チェックしたUnityバージョンとTextMeshProは以下の通りです。

I checked Unity and TextMeshPro Version are below.

UnityVer:2021.3.11f1 

TextMeshProVer:3.0.6

# Disruptive change

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

