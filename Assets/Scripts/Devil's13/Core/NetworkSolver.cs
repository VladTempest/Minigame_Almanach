using System.Collections.Generic;
using Devil_s13.Core.GameLoop;
using Mirror;
using UnityEngine;
using Zenject;

namespace Devil_s13.Core
{
    public class NetworkSolver : NetworkBehaviour
    {
        private GameStateDataAsset _gameStateDataAsset;
        
        internal static readonly Dictionary<NetworkConnectionToClient, int> connNames = new Dictionary<NetworkConnectionToClient, int>();

        public override void OnStartServer()
        {
            connNames.Clear();
        }
        
        [Inject]
        public void Construct(GameStateDataAsset gameStateDataAsset)
        {
            _gameStateDataAsset = gameStateDataAsset;
        }
        
        [Command(requiresAuthority = false)]
        public void CmdAddPlayerToGame(NetworkConnectionToClient sender = null)
        {
            if (!connNames.ContainsKey(sender))
                connNames.Add(sender, sender.identity.connectionToClient.connectionId);
            
            Debug.Log($"Player with id {sender.connectionId} connected to game. from NetworkSolver.cs");
            RpcAddPlayerToGame(sender.connectionId);
        }
        
        [ClientRpc]
        public void RpcAddPlayerToGame(int index)
        {
            _gameStateDataAsset.AddParticipantWithIndex(index);
            Debug.Log($"   Player with index {index} added to game. ");
        }
    }
}