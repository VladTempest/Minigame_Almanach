using System;
using System.Collections.Generic;
using Devil_s13.Core.Devils13UI;
using Devil_s13.Core.ThrowResultsProvider.Data;
using Random = UnityEngine.Random;

namespace Devil_s13.Core.ThrowResultsProvider
{
    public class RandomThrowResultProvider : IThrowResultProvider
    {
        public PlayersThrowResultData GetThrowResult(GameStateDataAsset gameStateDataAsset)
        {
            List<ThrowResultData> throwResultDatas = new List<ThrowResultData>();
            foreach (var participantGameData in gameStateDataAsset.GetAllParticipantsGameData())
            {
                var newPlayerThrowResultData = new ThrowResultData();
                newPlayerThrowResultData.OtherDiceValues = GetRandomDiceValues();
                newPlayerThrowResultData.ParticipantIndex = participantGameData.ParticipantIndex;
                
                throwResultDatas.Add(newPlayerThrowResultData);
            }
            
            return new PlayersThrowResultData()
            {
                ThrowResultData = throwResultDatas
            };
        }

        private List<int> GetRandomDiceValues()
        {
            var diceValues = new List<int>();
            for (int i = 0; i < 2; i++)
            {
                diceValues.Add(Random.Range(1, 7));
            }
            return diceValues;
        }
    }
}