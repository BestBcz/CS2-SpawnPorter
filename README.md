# CS2-SpawnPorter - CS2 出生点传送插件 | CS2 Spawn Teleport Plugin
> [!IMPORTANT]
> _当前版本需要在换图之后使用css_plugins reload SpawnPorter才有效_



## 插件简介 | Introduction
CS2-SpawnPorter 是一款为 CS2 跑图模式设计的极简出生点传送插件，玩家可通过按 E 键快速回到最近的出生点。
CS2-SpawnPorter is a minimalist spawn teleport plugin for CS2 practice, allowing players to quickly return to the nearest spawn by pressing the E key.

## 功能特性 | Features
- 智能识别地图出生点
- 按 E 键一键传送
- 冷却时间与距离限制，防止误触
- 支持多地图与多队伍
- 控制台与聊天提示
- 易于安装和配置
- 拥有良好的兼容性

- Smart detection of map spawn points
- One-key teleport by pressing E
- Cooldown and distance limit to prevent abuse
- Supports multiple maps and teams
- Console and chat notifications
- Easy to install and configure
- Good compatibility with other plugins

## 安装要求 | Requirements

- CS Sharp
- 显示出生点的插件Show spawns plugins (MathZy etc.)


## 安装步骤 | Installation
1. 将插件文件放入 `csgo/`
2. 重启服务器或使用命令重新加载插件

1. Put the plugin files into `csgo/`
2. Restart your server or reload the plugin via command

## 使用方法 | Usage
1. 进入游戏，靠近出生点按下 E 键即可传送
2. 控制台和聊天会有相关提示

1. Join the game, approach a spawn point and press E to teleport
2. Console and chat will show related messages

## 配置说明 | Configuration
插件支持通过 `config.json` 文件自定义以下参数：
- Cooldown：传送冷却时间（秒）
- MaxDistance：最大传送距离
- TraceDistance：视线检测距离
- DebugMode：调试模式开关

The plugin supports customizing the following parameters in `config.json`:
- Cooldown: Teleport cooldown (seconds)
- MaxDistance: Maximum teleport distance
- TraceDistance: Trace distance for detection
- DebugMode: Enable debug mode

