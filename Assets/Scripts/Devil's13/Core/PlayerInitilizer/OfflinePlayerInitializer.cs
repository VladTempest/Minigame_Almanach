using Cysharp.Threading.Tasks;
using Devil_s13.Core.Devils13UI;
using Mirror;
using UnityEngine;
using Zenject;

namespace Devil_s13.Core.GameLoop
{
    public class OfflinePlayerInitializer : IPlayerInitializer
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

            for (int i = 0; i < maxPlayers; i++)
            {
                AddPlayerToGame(i);
            }
        }

        public void AddPlayerToGame(int index)
        {
            _gameStateDataAsset.AddParticipantWithIndex(index);
        }
    }
        
    }