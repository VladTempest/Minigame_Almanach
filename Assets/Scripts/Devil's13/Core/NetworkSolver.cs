using System;
using System.Collections.Generic;
using Devil_s13.Core.GameLoop;
using Mirror;
using UnityEngine;
using Zenject;

namespace Devil_s13.Core
{
    public class NetworkSolver : NetworkBehaviour
    {
       public GameStateDataAsset _gameStateDataAsset;
        
        [SyncVar]
        public string SerializedGameData;
        
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
            _gameStateDataAsset.AddParticipantWithIndex(sender.connectionId);
            SerializedGameData = SerializeGameData(_gameStateDataAsset.GameData);
            
            RpcAddPlayerToGame(sender.connectionId);
            sender.identity.GetComponent<Player>().UpdateLocalGameData(_gameStateDataAsset.GameData);
        }

        private string SerializeGameData(ParticipantsDataList participantGameData)
        {
            string json = JsonUtility.ToJson(participantGameData);
            return json;
        }
        
        private ParticipantsDataList DeserializeGameData(string json)
        {
            return JsonUtility.FromJson<ParticipantsDataList>(json);
        }

        [ClientRpc]
        public void RpcAddPlayerToGame(int index)
        {
            /*_gameStateDataAsset.AddParticipantWithIndex(index);
            
            SerializedGameData = SerializeGameData(_gameStateDataAsset.GameData);*/
            //_gameStateDataAsset.AddParticipantWithIndex(index);
            Debug.Log($"   Player with index {index} added to game. ");
            //_gameStateDataAsset.GameData = DeserializeGameData(SerializedGameData);
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            
            if (String.IsNullOrEmpty(SerializedGameData)) return;
            _gameStateDataAsset.GameData = DeserializeGameData(SerializedGameData);
        }
    }
}