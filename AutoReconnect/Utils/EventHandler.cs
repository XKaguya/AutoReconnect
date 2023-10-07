using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
using MEC;
using PlayerRoles;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;


namespace AutoReconnect.EventHandler
{
    public class EventHandlers
    {
        private AutoReconnect _pluginInstance;

        public CoroutineHandle _timerHandle;

        public EventHandlers(AutoReconnect pluginInstance)
        {
            _pluginInstance = pluginInstance;
        }

        public List<Player> DisconnectedPlayers = new List<Player>();
        public void OnDisconnected(LeftEventArgs ev)
        {
            Player player = ev.Player;
            if (player.Role.Type != RoleTypeId.Spectator && player.Role.Type != RoleTypeId.None)
            {
                DisconnectedPlayers.Add(player);
                Log.Info($"Player {player.Nickname} has joined the list.");
            }

        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            Player player = ev.Player;
            DisconnectedPlayers.Remove(player);
            PlayerHandler playerData = AutoReconnect.Instance.Handler.GetPlayerData(player);
            if (playerData != null)
            {
                AutoReconnect.Instance.Handler.ReductionPlayer(player, playerData);
                Log.Info($"Player {player.Nickname}'s data has restored.");
                if (player.Role.Side == Side.Scp)
                {
                    player.Broadcast(5, "你已重连，你会以数据存储时的状态重生。", Broadcast.BroadcastFlags.Normal, true);
                }
                else
                {
                    player.Broadcast(5, "你已重连，你会以数据存储时的状态重生。\n请记得低头捡起你的物品。", Broadcast.BroadcastFlags.Normal, true);
                }
            }
        }

        public void OnWaitingForPlayers()
        {
            DisconnectedPlayers.Clear();
            _pluginInstance.Handler.ClearPlayerData();
            Log.Info("Data Cleared.");
        }

        public void OnRoundstarted()
        {
            _timerHandle = Timing.RunCoroutine(ATimer());
            Log.Info("Timer started.");
        }

        public IEnumerator<float> ATimer()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(20f);
                var AllPlayers = Player.List.Where(p => p.IsAlive).ToList();
                foreach (var player in AllPlayers)
                {
                    AutoReconnect.Instance.Handler.AddPlayer(player);
                }
                AutoReconnect.Instance.Handler.DisplayPlayersInfo();
            }
        }
    }
}