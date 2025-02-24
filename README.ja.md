![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/ruby.png)

[![English is here](https://img.shields.io/badge/lang-english-blue.svg)](README.md)

依存 : https://github.com/jp-netsis/RubyTextAbstractions

UI Toolkit の RubyLabel はこちら : https://github.com/jp-netsis/RubyLabel

# Ruby(Furigana) Text Mesh Pro

TextMeshProに振り仮名(ふりがな、フリガナ、ルビ)タグを追加します。

TextMeshProは Unity6から UGUI(com.unity.ugui) パッケージに内包されました。

私がチェックしたUnityバージョンとUGUI(com.unity.ugui)は以下の通りです。

```
UnityVer:6000.0.23f1(LTS)
UGUI:2.0.0
```

Unity6以前のバージョンでは、`v1.3.0`タグを使用してください。

Unity6でも`v1.3.0`をインストールすることは可能ですが、package.json に旧`TextMeshPro v3.0.6`への依存情報が含まれているためおすすめしません。

# Changes

## ver 2.2

RubyTextAbstractions バージョンの更新
SetUneditedText 関数の追加

## ver 2.1

追加 : スタンダードルビタグスタイル <rt> タグ

## ver 2.0

Unity バージョンアップ 6000.0.23f1(LTS)

更新 : package.json

更新 : Readme.md

追加 : Readme.ja.md

## ver 1.3

追加 : スタンダードルビタグスタイル <rt> タグ

## ver 1.2

追加 : `RubyTextMeshProDefinitions` に `BASE_NO_OVERRAP_RUBY_ALIGNMENT` を追加しました。

追加 : `BASE_NO_OVERRAP_RUBY_ALIGNMENT` 用に `rubyMargin` を追加しました。 ルビの隙間を指定します。

変更 : SerializeField パラメータを `_<xyz>` に変更しました。 以前のバージョンと互換性を保つため `FormerlySerializedAs(<xyz>)` を定義していますが、ご注意ください。

変更 :　`BASE_NO_OVERRAP_RUBY_ALIGNMENT` 用にルビとベースの文字の描画位置を計算する処理を追加しました。負荷が上がっている可能性があります。

削除 : `RubyTextMeshPro.UnditedText` / `RubyTextMeshProUGUI.UnditedText` : 削除されました。 `uneditedText` をご使用ください。

## ver 1.1

削除 : `allVCompensationRuby` / `allVCompensationRubyLineHeight` : `rubyLineHeight` が空文字の場合、 いままでの `allVCompensationRuby:false` と同等の効果になります。また、 `rubyLineHeight` に値が入っている場合、 `allVCompensationRubyLineHeight` の値と同様の効果になります。

削除予定 : `RubyTextMeshPro.UnditedText` / `RubyTextMeshProUGUI.UnditedText` : 次のバージョンで削除されます。`uneditedText`をご使用ください。

# Features

## Realtime Ruby Text

振り仮名を追加するには、以下のようなタグ形式を使用します。

### サポートされるタグ形式

1. **レガシールビタグ形式**:
   - `<ruby=にほんご>日本語</ruby>`: 「日本語」に「にほんご」を注釈。
   - `<r=にほんご>日本語</r>`: 上記と同じ。
   - また、以下のようにダブルクォートも使用可能です:
     - `<ruby="にほんご">日本語</ruby>`
     - `<r="にほんご">日本語</r>`

2. **標準ルビタグ形式 (v1.3.0以降)**:
   - v1.3.0 以降では、標準的なルビタグ形式がサポートされています。
   - `<ruby>日本語<rt>にほんご</rt></ruby>`: 「日本語」に「にほんご」を注釈。
   - `<r>日本語<rt>にほんご</rt></r>`: 同じ形式。
   - `<r>日本語<rt><color="blue">にほんご</color></rt></r>`: 注釈内にリッチテキスト (例: 青色文字) を使用可能。

---

### 例

| 入力形式                                        | 表示結果                                       |
|-------------------------------------------------|-----------------------------------------------|
| `<ruby=にほんご>日本語</ruby>`                  | <ruby>日本語<rt>にほんご</rt></ruby>          |
| `<r=にほんご>日本語</r>`                        | <ruby>日本語<rt>にほんご</rt></ruby>          |
| `<ruby="にほんご">日本語</ruby>`                | <ruby>日本語<rt>にほんご</rt></ruby>          |
| `<ruby>日本語<rt>にほんご</rt></ruby>`          | <ruby>日本語<rt>にほんご</rt></ruby>          |
| `<r>日本語<rt><color="blue">にほんご</color></rt></r>` | <ruby>日本語<rt><color="blue">にほんご</color></rt></ruby> |

---

> [!TIP]
> - **リッチテキストサポート**: 標準ルビタグ形式 (`<ruby><rt>`) を使用すると、色や太字などのリッチテキストを注釈内に適用可能です。
> - **互換性**: レガシー形式 (`<ruby="value">`) と標準形式が同時にサポートされています。
> - **推奨**: 新しいプロジェクトでは、標準タグ形式 (`<ruby><rt>`) を使用することを推奨します。

# How To Use

## 1. Load from github

GitHubからインストールをすることが可能です。

[Install]

Unity > Window > PackageManager > + > Add package from git url... > Add the following

+ `https://github.com/jp-netsis/RubyTextAbstractions.git?path=/RubyTextAbstractions/PackageData#v0.2.0`

+ `https://github.com/jp-netsis/RubyTextMeshPro.git?path=/Assets/RubyTextMeshPro#v2.1.0`

## 2. Copy Source Only

ソースのみコピーする方法があります。 

* (1). あなたには`UGUI(com.unity.ugui)`が必要です。もし無い場合、`Package Manager`からインストールしてください。Asset StoreからのText Mesh Proのインストールはしないでください。

* (2). RubyTextMeshProディレクトリとRubyTextAbstractionsのソースをコピーすれば使用可能です。

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/add_ruby.gif)

# 使用説明

`<ruby=かんじ>漢字</ruby>`

もしくは

`<r=かんじ>漢字</r>`

もしくは

`<r>漢字<rt>かんじ</rt></r>`

## RubyShowType

RUBY_ALIGNMENT : ルビに合わせて文字を表示します

BASE_ALIGNMENT : 元の文字に合わせて文字を表示します

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/align_width.gif)

## rubyLineHeight

この機能により、rubyを使用しない場合でも、同じ隙間を持つことができます。
この文字列を空にすることで、この機能はスキップされます。

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/vcompensation.gif)

# Known Issues
* (1).TextMeshPro のソースは改変していません。アラインでいくつか問題が起こります。

* (2).ルビの最大文字数よりもテキストボックスを小さくしないでください。表示崩れが起きます。

* (1)_1.'BASE_ALIGN'で左寄せの場合かつルビが行頭にありつつ元の文字より多い場合、枠の外まで表示されます

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/issue_lefttop.png)

* (1)_2.'BASE_ALIGN'で中央寄せの場合かつルビが元の文字より多い場合、中央に表示されません。 'RUBY_ALIGN' を使用すると解消される場合があります。

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/issue_base_alignment_center.png)

* (1)_3.'BASE_ALIGN'で右寄せの場合かつルビが行頭にありつつ元の文字より多い場合、(1)_1と違い、枠の内まで表示されます

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/issue_base_alignment_bottomright.png)

# Use Font File

Rounded M+

http://jikasei.me/font/rounded-mplus

ありがとうございます！

# Reference list

https://forum.unity.com/threads/how-to-display-extra-little-characters-above-characters-in-a-text.387772/

http://baba-s.hatenablog.com/entry/2019/01/10/122500

ありがとうございます！

Thank You!

# Other
* (1).TextMeshProは素晴らしいプラグインですので、rubyタグの追加はいつか行われるでしょう。そのときこのプロジェクトは削除します。

* (2).日本語以外のチェックはしていません。

# Contribution

すべての貢献を歓迎します。必ずプロジェクトのコードスタイルに従ってください。

Contact: netsis.jenomoto@gmail.com

