using Cysharp.Threading.Tasks;
using Devil_s13.Core.Devils13UI;
using Mirror;
using UnityEngine;
using Zenject;

namespace Devil_s13.Core.GameLoop
{
    public class OnlinePlayerInitializer : IPlayerInitializer
    {
        private GameStateDataAsset _gameStateDataAsset;

        [Inject]
        public void Construct(GameStateDataAsset gameStateDataAsset)
        {
            _gameStateDataAsset = gameStateDataAsset;
        }

        public async UniTask CreatePlayers()
        {
            var maxPlayers = NetworkManager.singleton.maxConnections;

            await UniTask.WaitUntil(() => _gameStateDataAsset.CurrentPaticipentsCount == maxPlayers);
        }

        public void AddPlayerToGame(int index)
        {
            //var networkSolver = GameObject.FindObjectOfType<NetworkSolver>();
            //networkSolver.CmdAddPlayerToGame(index);
           // _gameStateDataAsset.AddParticipantWithIndex(index);
        }
    }
}