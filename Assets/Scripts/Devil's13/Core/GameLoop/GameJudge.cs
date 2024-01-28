using System;
using System.Linq;
using Devil_s13.Core.ThrowResultsProvider;
using Devil_s13.Core.ThrowResultsProvider.Data;
using UnityEngine;

namespace Devil_s13.Core.GameLoop
{
    public class GameJudge : MonoBehaviour
    {
        [SerializeField]
        private GameStateDataAsset _gameStateDataAsset;
        private IThrowResultProvider _throwResultProvider;
        
        private bool _gameIsOver = false;
        private bool _roundIsOver = false;


        public void Start()
        {
            _throwResultProvider = new RandomThrowResultProvider();

            for (int i = 0; i < 2; i++)
            {
                _gameStateDataAsset.AddParticipantWithIndex(i);
            }
            
            JudgeGame();
        }

        private int DetermineWinnerIndex(PlayersThrowResultData playersData)
        {
            var throwResultData = playersData.ThrowResultData;
            
            if (throwResultData[0].BetValue == throwResultData[1].BetValue)
            {
                return -1;
            }

            if (throwResultData[0].BetValue > throwResultData[1].BetValue)
            {
                return 0;
            }

            return 1;
        }
        
        private void JudgeThrow(PlayersThrowResultData playersData)
        {
            var winnerIndex = DetermineWinnerIndex(playersData);
            
            if (winnerIndex == -1)
            {
                Debug.Log($"Draw, no score added, current score: {playersData.ThrowResultData[0].ParticipantIndex} player: {_gameStateDataAsset.GetAllParticipantsGameData()[playersData.ThrowResultData[0].ParticipantIndex].CurrentScore}, {playersData.ThrowResultData[1].ParticipantIndex} player: {_gameStateDataAsset.GetAllParticipantsGameData()[playersData.ThrowResultData[1].ParticipantIndex].CurrentScore}");
                return;
            }
            
            _gameStateDataAsset.AddScoreToParticipant(winnerIndex, playersData.ThrowResultData[winnerIndex].OtherDiceValues.Sum());
            Debug.Log($"Player {winnerIndex} won the throw, current score: {playersData.ThrowResultData[0].ParticipantIndex} player: {_gameStateDataAsset.GetAllParticipantsGameData()[playersData.ThrowResultData[0].ParticipantIndex].CurrentScore}, {playersData.ThrowResultData[1].ParticipantIndex} player: {_gameStateDataAsset.GetAllParticipantsGameData()[playersData.ThrowResultData[1].ParticipantIndex].CurrentScore}");
            
            if (_gameStateDataAsset.GetAllParticipantsGameData()[winnerIndex].CurrentScore >= _gameStateDataAsset.WinningScore)
            {
                _gameStateDataAsset.AddWinToParticipant(winnerIndex);
                Debug.Log($"Player {winnerIndex} won the round, current score: {playersData.ThrowResultData[0].ParticipantIndex} player: {_gameStateDataAsset.GetAllParticipantsGameData()[playersData.ThrowResultData[0].ParticipantIndex].CurrentScore}, {playersData.ThrowResultData[1].ParticipantIndex} player: {_gameStateDataAsset.GetAllParticipantsGameData()[playersData.ThrowResultData[1].ParticipantIndex].CurrentScore}");
            }
        }

        private void JudgeGame()
        {
            const int cycleStopValue= 1000;
            var cycleCounter = 0;
            Debug.Log("Game started");
            while (!_gameIsOver && cycleCounter < cycleStopValue)
            {
                cycleCounter++;
                _roundIsOver = false;
                
                Debug.Log("Round started");
                _gameIsOver = _gameStateDataAsset.GetAllParticipantsGameData().Any(participantGameData =>
                    participantGameData.NumberOfWins >= _gameStateDataAsset.MaxNumberOfWins);
                if (_gameIsOver)
                {
                    Debug.Log("Game ended");
                    _gameStateDataAsset.EndGameReset();
                    break;
                }
                
                while (!_roundIsOver && cycleCounter < cycleStopValue)
                {
                    cycleCounter++;
                    Debug.Log("Throw started");
                    var playersThrowResultData = _throwResultProvider.GetThrowResult(_gameStateDataAsset);
                    Debug.Log($"Player {playersThrowResultData.ThrowResultData[0].ParticipantIndex} throw: Bet: {playersThrowResultData.ThrowResultData[0].BetValue}, with other {playersThrowResultData.ThrowResultData[0].OtherDiceValues[0]} on dice");
                    Debug.Log($"Player {playersThrowResultData.ThrowResultData[1].ParticipantIndex} throw: Bet: {playersThrowResultData.ThrowResultData[1].BetValue}, with other {playersThrowResultData.ThrowResultData[1].OtherDiceValues[0]} on dice");
                    JudgeThrow(playersThrowResultData);
                    Debug.Log("Throw ended");
                    _roundIsOver = _gameStateDataAsset.GetAllParticipantsGameData().Any(participantGameData =>
                        participantGameData.CurrentScore >= _gameStateDataAsset.WinningScore);
                    if (_roundIsOver)
                    {
                        Debug.Log($"Current wins: Player {playersThrowResultData.ThrowResultData[0].ParticipantIndex} wins: {_gameStateDataAsset.GetAllParticipantsGameData()[playersThrowResultData.ThrowResultData[0].ParticipantIndex].NumberOfWins}, Player {playersThrowResultData.ThrowResultData[1].ParticipantIndex} wins: {_gameStateDataAsset.GetAllParticipantsGameData()[playersThrowResultData.ThrowResultData[1].ParticipantIndex].NumberOfWins}");
                        Debug.Log("Round ended");
                        Debug.Log($"Winner of game: Player {_gameStateDataAsset.GetAllParticipantsGameData().FindIndex(x => x.NumberOfWins >= _gameStateDataAsset.MaxNumberOfWins)}");
                        _gameStateDataAsset.EndRoundReset();
                        break;
                    }
                }
            }
        }
    }
}