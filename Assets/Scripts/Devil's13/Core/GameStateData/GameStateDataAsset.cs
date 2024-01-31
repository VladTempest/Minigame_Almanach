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
        
        private List<ParticipantGameData> participantGameData = new List<ParticipantGameData>();

        public void EndGameReset()
        {
            participantGameData.Clear();
        }

        public void EndRoundReset()
        {
            for (int i = 0; i < participantGameData.Count; i++)
            {
                participantGameData[i].CurrentScore = 0;
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
            
            participantGameData.Add(participantData);
        }
        
        public ParticipantGameData GetParticipantGameData(int participantIndex)
        {
            return participantGameData[participantIndex];
        }

        public List<ParticipantGameData> GetAllParticipantsGameData()
        {
            return participantGameData;
        }

        public void AddScoreToParticipant(int participantIndex, int addedScore)
        {
            var winnerData = participantGameData.Find((x) => x.ParticipantIndex == participantIndex);
            winnerData.CurrentScore += addedScore;
        }

        public void AddWinToParticipant(int participantIndex)
        {
            var winnerData = participantGameData.Find((x) => x.ParticipantIndex == participantIndex);
            winnerData.NumberOfWins++;
        }

        public ParticipantGameData GetPlayerWithIndex(int index)
        {
            return participantGameData.Find((x) => x.ParticipantIndex == index);
        }
    }
}