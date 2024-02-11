using Mirror;
using UnityEngine;
using Zenject;

namespace Devil_s13.Core
{
    public class NetworkSolver : NetworkBehaviour
    {
        private GameStateDataAsset _gameStateDataAsset;
        
        [Inject]
        public void Construct(GameStateDataAsset gameStateDataAsset)
        {
            _gameStateDataAsset = gameStateDataAsset;
        }
        
        [Command(requiresAuthority = false)]
        public void CmdAddPlayerToGame(int index)
        {
            Debug.Log($"Player with id {index} connected to game. from NetworkSolver.cs");
            RpcAddPlayerToGame(index);
        }
        
        [ClientRpc]
        public void RpcAddPlayerToGame(int index)
        {
            var lastParticipantIndex = _gameStateDataAsset.CurrentPaticipentsCount;
            _gameStateDataAsset.AddParticipantWithIndex(lastParticipantIndex);
            Debug.Log($"   Player with index {lastParticipantIndex} added to game. ");
        }
    }
}