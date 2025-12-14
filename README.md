# AutoHeliCall

[English](#english) | [Japanese](#japanese)

---

<a name="english"></a>
## English

**AutoHeliCall** is a simple Rust Oxide plugin that automatically calls patrol helicopters at fixed intervals.

### Features
- **Auto Spawn:** Automatically calls **1** Patrol Helicopter every 60 minutes.
- **Announcements:** Broadcasts the time remaining until the next spawn every 5 minutes.
- **Commands:** check remaining time or force spawn helicopters manually.

### Installation
1. Download `AutoHeliCall.cs`.
2. Place the file into your server's `oxide/plugins` folder.
3. The plugin will load automatically.

### Chat Commands
- `/time` : Check how many minutes are left until the next helicopter spawn.

### Configuration
Currently, settings are hardcoded in the script. You can edit the following variables in the `.cs` file directly if needed:
- `heliInterval`: Spawn interval in seconds (Default: 3600 = 60 mins)
- `heliCount`: Number of helicopters to spawn (Default: 1)

---

<a name="japanese"></a>
## Japanese

**AutoHeliCall** は、一定時間ごとにパトロールヘリコプターを自動で呼び出すRust Oxide用プラグインです。

### 特徴
- **自動召喚:** 60分ごとにパトロールヘリコプターを **1機** 自動で出現させます。
- **アナウンス:** 出現までの残り時間を5分ごとに全体チャットで通知します。
- **コマンド機能:** 残り時間の確認や、手動での即時召喚が可能です。

### インストール方法
1. `AutoHeliCall.cs` をダウンロードします。
2. サーバーの `oxide/plugins` フォルダに入れます。
3. 自動的に読み込まれます。

### チャットコマンド
- `/time` : 次の出現までの残り時間を表示します。

### 設定について
現在、設定ファイル（Config）は生成されません。設定を変更したい場合は、`.cs` ファイル内の変数を直接編集してください：
- `heliInterval`: 出現間隔（秒）（デフォルト: 3600 = 60分）
- `heliCount`: 出現数（デフォルト: 1）
