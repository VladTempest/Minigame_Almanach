using Devil_s13.Core.ThrowResultsProvider.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Devil_s13.Core.Devils13UI
{
    public class Devils13GameModel
    {
        public IReactiveProperty<int> firstDiceValue = new ReactiveProperty<int>();
        public IReactiveProperty<int> secondDiceValue = new ReactiveProperty<int>();
        
        public IReactiveProperty<int> opponentScore = new ReactiveProperty<int>();
        public IReactiveProperty<string> opponentName = new ReactiveProperty<string>();
        public IReactiveProperty<int> opponentWins = new ReactiveProperty<int>();
        public IReactiveProperty<string> opponentStatus = new ReactiveProperty<string>();
        public IReactiveProperty<int> opponentBet = new ReactiveProperty<int>();
        
        public IReactiveProperty<int> playerScore = new ReactiveProperty<int>();
        public IReactiveProperty<string> playerName = new ReactiveProperty<string>();
        public IReactiveProperty<int> playerWins = new ReactiveProperty<int>();
        public IReactiveProperty<int> playerBet = new ReactiveProperty<int>();
        
        public IReactiveProperty<bool> isBetButtonEnabled = new ReactiveProperty<bool>();
        public IReactiveProperty<bool> isThrowButtonEnabled = new ReactiveProperty<bool>();
        
        public IReactiveProperty<string> betResult = new ReactiveProperty<string>();
        
        
        public void UpdateModelWithGameData(GameStateDataAsset gameStateDataAsset)
        {
            var playerGameData = gameStateDataAsset.GetPlayerWithIndex(0);
            var opponentGameData = gameStateDataAsset.GetPlayerWithIndex(1);
            
            if (playerGameData == null || opponentGameData == null)
            {
                playerScore.Value = 0;
                playerName.Value = "Player X0";
                playerWins.Value = 0;
                
                opponentScore.Value = 0;
                opponentName.Value = "Player X1";
                opponentWins.Value = 0;
                
                opponentStatus.Value = "Waiting...";
                
                return;
            }
            
            playerScore.Value = playerGameData.CurrentScore;
            playerName.Value = $"Player {playerGameData.ParticipantIndex}";
            playerWins.Value = playerGameData.NumberOfWins;
            
            opponentScore.Value = opponentGameData.CurrentScore;
            opponentName.Value = $"Player {opponentGameData.ParticipantIndex}";
            opponentWins.Value = opponentGameData.NumberOfWins;
            
            //ToDo: Implement logic for opponent status
            opponentStatus.Value = "Playing...";
        }

        public void UpdateBetScoreForPlayer(int playerIndex, int betScore)
        {
            if (playerIndex == 0)
            {
                playerBet.Value = betScore;
            }
            else
            {
                opponentBet.Value = betScore;
            }
        }

        public void UpdateBetResultText(int winnerIndex)
        {
            if (winnerIndex == 0)
            {
                betResult.Value = ">";
            }
            else if (winnerIndex == 1)
            {
                betResult.Value = "<";
            }
            else
            {
                betResult.Value = "=";
            }
        }
        
        public void ResetBetResultText()
        {
            betResult.Value = "waiting...";
        }
        
        public void ResetDiceValues()
        {
            firstDiceValue.Value = 0;
            secondDiceValue.Value = 0;
        }
        
        public void UpdateDiceValues(ThrowResultData throwResultData)
        {
            var firstDiceValue = throwResultData.BetValue;
            var secondDiceValue = throwResultData.OtherDiceValues[0];
            
            this.firstDiceValue.Value = firstDiceValue;
            this.secondDiceValue.Value = secondDiceValue;
        }

        public void ResetBets()
        {
            playerBet.Value = 0;
            opponentBet.Value = 0;
        }
    }
}