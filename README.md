# Tetris-WinForms 🎮

![C#](https://img.shields.io/badge/Language-C%23-blue.svg)
![Framework](https://img.shields.io/badge/Framework-WinForms-lightgrey.svg)
![License](https://img.shields.io/badge/License-MIT-green.svg)

本项目是一款基于 C# Windows Forms 开发的现代化俄罗斯方块游戏。
在保留经典核心玩法的基础上，本版本对视觉 UI 和底层操控逻辑进行了全面重构，致力于提供“街机级”的沉浸式游玩体验和极致顺滑的操作手感。

**预览 (Preview)**
<p align="center">
  <img width="300" alt="游戏运行截图" src="https://github.com/user-attachments/assets/ef38e351-9e7c-47a5-bb48-20cd1184a75e" />
</p>

## ✨ 核心特性 (Features)

* **纯净暗黑视效 (Dark Mode UI)**：摒弃冗杂的背景，采用纯正的碳黑背景底板，搭配高对比度的“霓虹发光”色块，大幅降低长时间游玩的视觉疲劳。
* **全息落点预测 (Ghost Block)**：新增半透明的幽灵方块投影，精准提示方块的最终落点，极大提升游戏策略性。
* **电竞级操控调优 (Smooth Controls)**：重写底层 `ProcessCmdKey` 键盘拦截逻辑，彻底解决传统 WinForms 焦点丢失、按键冲突问题，实现无延迟的顺滑手感。
* **沉浸式无缝体验**：移除系统默认的弹窗中断，引入高级半透明遮罩与一键回车重开（Seamless Restart）机制。

## ⌨️ 游戏操作指南 (Controls)

本游戏支持全键盘流畅操作，随时准备挑战高分：

| 按键 (Key) | 动作 (Action) |
| :--- | :--- |
| `←` / `→` | 左右移动方块 |
| `↑` | 旋转方块 |
| `↓` | 加速下落 (软降) |
| `Space` (空格键) | 瞬间触底 (硬降)，高手必备！ |
| `Enter` (回车键) | 游戏结束后，一键快速重新开始 |

## 🚀 如何运行 (How to Run)

### 方法一：直接游玩 (玩家推荐)
1. 点击本仓库右侧的 **[Releases]** 标签页。
2. 下载最新版本的 `.zip` 压缩包并解压。
3. 双击运行 `Tetris.exe` 即可直接开始游戏，无需安装。

### 方法二：源码编译 (开发者推荐)
如果你想查看源码或在此基础上进行二次开发：
1. 确保你的电脑上安装了 **Visual Studio** (推荐 2019 或更新版本)，并勾选了 `.NET 桌面开发` 工作负载。
2. 克隆本仓库到本地：
   ```bash
   git clone https://github.com/SuHamIce/Tetris-WinForms.git
