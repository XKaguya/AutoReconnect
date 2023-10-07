using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API.Features;
using Mirror;
using PlayerRoles;

namespace AutoReconnect.Main
{
    public class Handler
    {
        public Dictionary<string, PlayerHandler> _players = new Dictionary<string, PlayerHandler>();
        public void AddPlayer(Player player)
        {
            if (player != null && player.IsAlive)
            {
                PlayerHandler newPlayer = new PlayerHandler
                {
                    Name = player.Nickname,
                    Class = player.Role.Type,
                    Position_X = player.Position.x,
                    Position_Y = player.Position.y,
                    Position_Z = player.Position.z,
                    //Inventory = player.Items,
                    Health = player.Health,
                };
                _players[player.Nickname] = newPlayer;
            }
        }

        public void ClearPlayerData()
        {
            _players.Clear();
        }

        public PlayerHandler GetPlayerData(Player player)
        {
            if (_players.ContainsKey(player.Nickname))
            {
                return _players[player.Nickname];
            }
            return null;
        }

        public void ReductionPlayer(Player player, PlayerHandler playerData)
        {
            if (player != null && playerData != null)
            {
                player.Role.Set(playerData.Class, RoleSpawnFlags.None);
                Log.Info($"Player's Role is now {playerData.Class}.");
                player.Position = new UnityEngine.Vector3(playerData.Position_X, playerData.Position_Y, playerData.Position_Z);
                Log.Info($"Player's pos is now {playerData.Position_X}, {playerData.Position_Y}, {playerData.Position_Z}.");
                player.Health = playerData.Health;
                Log.Info($"Player's health is now {playerData.Health}.");
            }
        }

        public void DisplayPlayersInfo()
        {
            Log.Info("------------------------------------------------------");
            foreach (var playerName in _players.Keys)
            {
                PlayerHandler playerData = _players[playerName];
                Log.Info($"Name: {playerData.Name}, Health: {playerData.Health}, Class: {playerData.Class}, POS: {playerData.Position_X}, {playerData.Position_Y}, {playerData.Position_Z}");
            }
            Log.Info("------------------------------------------------------");
        }
    }
}
