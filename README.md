![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/ruby.png)

[![日本語はこちら](https://img.shields.io/badge/lang-日本語-red.svg)](README.ja.md)

Depends : https://github.com/jp-netsis/RubyTextAbstractions

If you need RubyLabel for UI Toolkit : https://github.com/jp-netsis/RubyLabel

# Ruby(Furigana) Text Mesh Pro

This plugin adds ruby tag support to "Text Mesh Pro" Unity plugin. 

TextMeshPro has been included in the UGUI (com.unity.ugui) package since Unity6.

I checked Unity and UGUI(com.unity.ugui) Version are below.

```
UnityVer:6000.0.23f1(LTS)
UGUI:2.0.0
```

Versions prior to Unity6 should use the `v1.2.0` tag.

It is possible to install `v1.2.0` in Unity6, but it is not recommended because package.json contains dependency information on the old `TextMeshPro v3.0.6`.

# Disruptive change

## ver 2.0

Unity version up to 6000.0.23f1(LTS)

Updated : package.json

Updated : Readme.md

Added : Readme.ja.md

## ver 1.2

Added : `BASE_NO_OVERRAP_RUBY_ALIGNMENT` for `RubyTextMeshProDefinitions`.

Added : `rubyMargin` for `BASE_NO_OVERRAP_RUBY_ALIGNMENT`. Specify the ruby margin.

Changed : SerializeField parameter to `_<xyz>`. Note that `FormerlySerializedAs(<xyz>)` is defined for compatibility with previous versions.

Changed : Added processing for `BASE_NO_OVERRAP_RUBY_ALIGNMENT` to calculate the drawing position of ruby and base characters. This may have increased the load.

Removed : `RubyTextMeshPro.UnditedText` / `RubyTextMeshProUGUI.UnditedText` : removed. Please use `uneditedText`.

## ver 1.1

Removed : `allVCompensationRuby` / `allVCompensationRubyLineHeight` : If `rubyLineHeight` is an empty string, it will be the `allVCompensationRuby:false` value up to now, and if `rubyLineHeight` is a value, it will be the `allVCompensationRubyLineHeight` value.

Obsolete : `RubyTextMeshPro.UnditedText` / `RubyTextMeshProUGUI.UnditedText` : Will be removed in the next version. Please use `uneditedText`.

# Features

## Realtime Ruby Text

Ruby tags allow you to annotate text with small "ruby" characters (e.g., for pronunciation or additional information). Various styles of ruby tags are supported, providing flexibility and backward compatibility.

### Supported Ruby Tag Formats

1. **Legacy Ruby Tag Styles**:
   These styles have been supported since earlier versions and are fully compatible.
   - `<ruby=ice>fire</ruby>`: Annotates "fire" with "ice".
   - `<r=ice>fire</r>`: Equivalent to the above.
   - These tags also support double quotes:
     - `<ruby="ice">fire</ruby>`
     - `<r="ice">fire</r>`

2. **Standard Ruby Tag Styles (from v1.3.0)**:
   Starting from **v1.3.0**, the standard ruby tag format is supported. This enables additional features, such as the ability to use rich text formatting within ruby annotations.
   - `<ruby>fire<rt>ice</rt></ruby>`: Annotates "fire" with "ice".
   - `<r>fire<rt>ice</rt></r>`: Equivalent to the standard style.
   - `<r>fire<rt><color="blue">ice</color></rt></r>`: Enables rich text formatting (e.g., coloring "ice" in blue).

---

### Examples

| Input Syntax                                      | Rendered Output                           |
|--------------------------------------------------|-------------------------------------------|
| `<ruby=ice>fire</ruby>`                          | <ruby>fire<rt>ice</rt></ruby>             |
| `<r=ice>fire</r>`                                | <ruby>fire<rt>ice</rt></ruby>             |
| `<ruby="ice">fire</ruby>`                        | <ruby>fire<rt>ice</rt></ruby>             |
| `<ruby>fire<rt>ice</rt></ruby>`                  | <ruby>fire<rt>ice</rt></ruby>             |
| `<r>fire<rt>ice</rt></r>`                        | <ruby>fire<rt>ice</rt></ruby>             |

---

> [!TIP]
> - **Rich Text Support**: The standard ruby tag style (`<ruby><rt>`) allows for advanced formatting such as color, bold, or italic within the ruby annotations.
> - **Backward Compatibility**: Legacy styles such as `<ruby="ice">` and `<r="ice">` are fully supported alongside the new standard ruby tag style.
> - **Standard Syntax Recommendation**: It is recommended to use the standard ruby tag style (`<ruby><rt>`) for new projects to take full advantage of the rich text features.

# How To Use

## 1. Load from github

There is a way to install from GitHub.

In this case, the dependency information for TextMeshPro 3.0.6 is retained, so if TextMeshPro is not installed, it will be installed.

[Install]

Unity > Window > PackageManager > + > Add package from git url... > Add the following

+ `https://github.com/jp-netsis/RubyTextAbstractions.git?path=/RubyTextAbstractions/PackageData#v0.1.0`

+ `https://github.com/jp-netsis/RubyTextMeshPro.git?path=/Assets/RubyTextMeshPro#v2.0.0`

## 2. Copy Source Only

There is a way to copy only the source. 

* (1). You need to have `TextMeshPro` plugin in your project. You can install TMPro via `Package Manager`. DO NOT Install Text Mesh Pro from Asset Store.

* (2). You can use it by copying the RubyTextMeshPro directory and RubyTextAbstractions source codes.

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/add_ruby.gif)

# Usage Description

`<ruby=xyz>XYZ</ruby>`

or

`<r=xyz>XYZ</r>`


## RubyShowType

RUBY_ALIGNMENT : Display characters according to ruby

BASE_ALIGNMENT : Display characters according to the original characters

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/align_width.gif)

### rubyLineHeight

This function allows you to have the same gap even if you don't use ruby.
Empty this string to skip this feature.

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/vcompensation.gif)

# Known Issues
* (1).TextMeshPro source has not changed. So text alignment is problematic.

* (2).Do not make the text box smaller than the maximum number of characters in ruby. Display collapse will occur.

* (1)_1.'BASE_ALIGN' setting is left align and the ruby is at the beginning of the line but more than the original character, it will be displayed outside the frame.

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/issue_lefttop.png)

* (1)_2.'BASE_ALIGN' setting is center align and the ruby is at more than the original character, can't displayed center. 'RUBY_ALIGN' used, it may be solved.

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/issue_base_alignment_center.png)

* (1)_3.'BASE_ALIGN' setting is left align and the ruby is at the beginning of the line but more than the original character, Different from (1)_1 it will be displayed in the frame.

![Example](https://github.com/jp-netsis/RubyTextMeshPro/blob/main/Screenshots/issue_base_alignment_bottomright.png)

# Use Font File

Rounded M+

http://jikasei.me/font/rounded-mplus

Thank You!

# Reference list

https://forum.unity.com/threads/how-to-display-extra-little-characters-above-characters-in-a-text.387772/

http://baba-s.hatenablog.com/entry/2019/01/10/122500

Thank You!

# Other
* (1).TextMeshPro is very nice Plugin. If TextMeshPro add ruby tag well, delete my git project.

* (2).Not checked anything other than Japanese

# Contribution

All contributions are welcomed. Just make sure you follow the project's code style.  

Contact: netsis.jenomoto@gmail.com

