using System;
using Mirror;
using UnityEngine;
using Zenject;

namespace Devil_s13.Core.GameLoop
{
    public class Player : NetworkBehaviour
    {
        public void Start()
        {
            if (!isLocalPlayer) return;
            
            Debug.Log($"Player with id  connected to game. from Player.cs");
            
            NetworkSolver networkSolver = FindObjectOfType<NetworkSolver>();
            networkSolver.CmdAddPlayerToGame();
            base.OnStartClient();
        }
    }
}