using System;
using Mirror;
using UnityEngine;
using Zenject;

namespace Devil_s13.Core.GameLoop
{
    public class Player : NetworkBehaviour
    {
     
        private GameStateDataAsset _gameStateDataAsset;
        public void Start()
        {
            if (!isLocalPlayer) return;
            
            Debug.Log($"Player with id  connected to game. from Player.cs");
            
            NetworkSolver networkSolver = FindObjectOfType<NetworkSolver>();
            _gameStateDataAsset = networkSolver._gameStateDataAsset;
            
            networkSolver.CmdAddPlayerToGame();
            base.OnStartClient();
        }
        
        public void UpdateLocalGameData(ParticipantsDataList participantGameData)
        {
            if (!isLocalPlayer) return;
            
            _gameStateDataAsset.GameData = participantGameData;
        }
    }
}