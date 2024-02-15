using System;
using System.Collections.Generic;
using UnityEngine;

namespace Devil_s13.Core
{
    [CreateAssetMenu(fileName = "GameStateData", menuName = "Data/Devil's13/GameStateData", order = 0)]
    public class GameStateDataAsset : ScriptableObject
    {
        public int MaxNumberOfWins = 2;
        public int WinningScore = 13;
        [SerializeField] private ParticipantsDataList _gameData = new ParticipantsDataList();

        public int CurrentPaticipentsCount => ListCount();

        private int ListCount()
        {
            if (GameData == null)
            {
                return 0;
            }
            return GameData.List.Count;
        }


        public ParticipantsDataList GameData
        {
            get => _gameData;
            set
            {
                _gameData = value;

                if (_gameData == null)
                {
                    return;
                }
                
                foreach (var participantGameData in _gameData.List)
                {
                    Debug.Log($"Participant index: {participantGameData.ParticipantIndex} score: {participantGameData.CurrentScore} wins: {participantGameData.NumberOfWins}");
                }
            }
        }

        public void EndGameReset()
        {
            GameData.List.Clear();
        }

        private void OnEnable()
        {
            GameData.List.Clear();
        }

        public void EndRoundReset()
        {
            for (int i = 0; i < GameData.List.Count; i++)
            {
                GameData.List[i].CurrentScore = 0;
            }
        }
        
        
        public void AddParticipantWithIndex(int participantIndex)
        {
            var participantData = new ParticipantGameData
            {
                ParticipantIndex = participantIndex,
                CurrentScore = 0,
                NumberOfWins = 0
            };
            
            GameData.List.Add(participantData);
        }
        
        public ParticipantGameData GetParticipantGameData(int participantIndex)
        {
            return GameData.List[participantIndex];
        }

        public ParticipantsDataList GetAllParticipantsGameData()
        {
            return GameData;
        }

        public void AddScoreToParticipant(int participantIndex, int addedScore)
        {
            var winnerData = GameData.List.Find((x) => x.ParticipantIndex == participantIndex);
            winnerData.CurrentScore += addedScore;
        }

        public void AddWinToParticipant(int participantIndex)
        {
            var winnerData = GameData.List.Find((x) => x.ParticipantIndex == participantIndex);
            winnerData.NumberOfWins++;
        }

        public ParticipantGameData GetPlayerWithIndex(int index)
        {
            return GameData.List.Find((x) => x.ParticipantIndex == index);
        }
    }
}