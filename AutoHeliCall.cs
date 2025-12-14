using Oxide.Core.Plugins;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("AutoHeliCall", "ochapi", "1.2.5")]
    [Description("60分おきにヘリを1機呼び、残り時間を放送。/helicall で即時召喚可能、/time で残り時間確認")]
    public class AutoHeliCall : RustPlugin
    {
        private const float heliInterval = 3600f; // 60分 (秒)
        private const float announceStep = 300f;  // 5分 (秒)
        private const int heliCount = 1;          // 出現させるヘリの数 (ここを1に変更しました)

        // 残り時間を秒で保持
        private float timeRemaining = heliInterval;

        void OnServerInitialized()
        {
            // 初期残り時間
            timeRemaining = heliInterval;

            // 1秒ごとに残り時間を減らして、0ならヘリ召喚してリセット
            timer.Every(1f, () =>
            {
                timeRemaining -= 1f;
                if (timeRemaining <= 0f)
                {
                    CallHelis();
                    timeRemaining = heliInterval;
                }
            });

            // 5分ごとに全体チャットで残り時間を告知
            timer.Every(announceStep, () =>
            {
                if (timeRemaining > 0f)
                    AnnounceTime();
            });
        }

        private void CallHelis()
        {
            for (int i = 0; i < heliCount; i++)
            {
                // 出現位置：マップ原点の上空50m（必要に応じて変更可能）
                BaseEntity heli = GameManager.server.CreateEntity(
                    "assets/prefabs/npc/patrol helicopter/patrolhelicopter.prefab",
                    Vector3.up * 50f
                );
                if (heli != null)
                    heli.Spawn();
            }

            // メッセージも少し自然になるよう調整（複数形helicoptersのままでも通じますが、数は自動で入ります）
            PrintToChat($"<color=#ff0000>[Helicopter]</color> <color=#ffff00>{heliCount}機のアタックヘリコプターが出現しました！</color> (Spawned {heliCount} attack helicopter!)");
        }

        private void AnnounceTime()
        {
            int minutes = Mathf.CeilToInt(timeRemaining / 60f);
            PrintToChat(
                $"<color=#ff0000>[Helicopter]</color> " +
                $"<color=#00ffff>次のヘリコプター出現まで残り</color> <color=#ffff00>{minutes}分</color> <color=#00ffff>です。</color> " +
                $"(Next helicopter spawn in <color=#ffff00>{minutes} minutes</color>)"
            );
        }

        [ChatCommand("helicall")]
        private void CmdHeliCall(BasePlayer player, string command, string[] args)
        {
            CallHelis();
            timeRemaining = heliInterval; // 即時召喚したらサイクルをリセット
            player.ChatMessage(
                $"<color=#ff0000>[Helicopter]</color> <color=#00ff00>{heliCount}機のヘリを即時召喚しました！</color> " +
                $"(Spawned {heliCount} helicopter immediately!)"
            );
        }

        [ChatCommand("time")]
        private void CmdTime(BasePlayer player, string command, string[] args)
        {
            int minutes = Mathf.CeilToInt(timeRemaining / 60f);
            player.ChatMessage(
                $"<color=#ff0000>[Helicopter]</color> " +
                $"<color=#00ffff>次のヘリコプター出現まで残り</color> <color=#ffff00>{minutes}分</color> <color=#00ffff>です。</color> " +
                $"(Next helicopter spawn in <color=#ffff00>{minutes} minutes</color>)"
            );
        }

        void Unload()
        {
            // タイマー参照は保持していないため特別な破棄処理は不要
        }
    }
}