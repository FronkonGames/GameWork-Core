<p align="center"><img src="Documentation/banner.png"/></p>

<p align="center"><b>A Modular Framework With Dependency Injection.</b></p>

<br>
<p align="center">
  <a style="text-decoration:none">
    <img src="https://img.shields.io/github/package-json/v/FronkonGames/GameWork-Core?style=flat-square" alt="version" />
  </a>  
  <a style="text-decoration:none">
    <img src="https://img.shields.io/github/license/FronkonGames/GameWork-Core?style=flat-square" alt="license" />
  </a>
  <a style="text-decoration:none">
    <img src="https://img.shields.io/github/languages/top/FronkonGames/GameWork-Core?style=flat-square" alt="top language" />
  </a>
</p>
<p align="center"><b>⚠️Still In Early Development ⚠️<b/></p>

## 🔧 Requisites

- Unity 2021.3 or higher.
- [Game:Work Foundation](https://github.com/FronkonGames/GameWork-Foundation).
- [Game:Work Core](https://github.com/FronkonGames/GameWork-Core).
- Addressables 1.19.19 or higher.

## 🚀 Installation

### Editing your 'manifest.json'

- Open the manifest.json file of your Unity project.
- In the section "dependencies" add:

```
{
  ...
  "dependencies":
  {
    ...
    "FronkonGames.GameWork.Foundation": "git+https://github.com/FronkonGames/GameWork-Foundation.git",
    "FronkonGames.GameWork.Core": "git+https://github.com/FronkonGames/GameWork-Core.git"
  }
  ...
}
```

## 📜 License

Code released under [MIT License](https://github.com/FronkonGames/GameWork-Core/blob/main/LICENSE.md).