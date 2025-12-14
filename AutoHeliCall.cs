using Oxide.Core.Plugins;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("AutoHeliCall", "ochapi", "1.2.6")]
    [Description("Automatically calls 1 helicopter every 60 minutes and broadcasts remaining time. Use /time to check.")]
    public class AutoHeliCall : RustPlugin
    {
        private const float heliInterval = 3600f; // 60 minutes (in seconds)
        private const float announceStep = 300f;  // 5 minutes (in seconds)
        private const int heliCount = 1;          // Number of helicopters to spawn

        // Store remaining time in seconds
        private float timeRemaining = heliInterval;

        void OnServerInitialized()
        {
            // Set initial remaining time
            timeRemaining = heliInterval;

            // Decrease remaining time every second. If 0, spawn heli and reset.
            timer.Every(1f, () =>
            {
                timeRemaining -= 1f;
                if (timeRemaining <= 0f)
                {
                    CallHelis();
                    timeRemaining = heliInterval;
                }
            });

            // Announce remaining time to global chat every 5 minutes
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
                // Spawn position: 50m above map origin (can be changed if needed)
                BaseEntity heli = GameManager.server.CreateEntity(
                    "assets/prefabs/npc/patrol helicopter/patrolhelicopter.prefab",
                    Vector3.up * 50f
                );
                if (heli != null)
                    heli.Spawn();
            }

            // Message in English
            PrintToChat($"<color=#ff0000>[Helicopter]</color> <color=#ffff00>Spawned {heliCount} attack helicopter!</color>");
        }

        private void AnnounceTime()
        {
            int minutes = Mathf.CeilToInt(timeRemaining / 60f);
            PrintToChat(
                $"<color=#ff0000>[Helicopter]</color> " +
                $"<color=#00ffff>Next helicopter spawn in</color> <color=#ffff00>{minutes} minutes</color><color=#00ffff>.</color>"
            );
        }

        [ChatCommand("time")]
        private void CmdTime(BasePlayer player, string command, string[] args)
        {
            int minutes = Mathf.CeilToInt(timeRemaining / 60f);
            player.ChatMessage(
                $"<color=#ff0000>[Helicopter]</color> " +
                $"<color=#00ffff>Next helicopter spawn in</color> <color=#ffff00>{minutes} minutes</color><color=#00ffff>.</color>"
            );
        }

        void Unload()
        {
            // No special cleanup needed as timers are handled by Oxide
        }
    }
}
