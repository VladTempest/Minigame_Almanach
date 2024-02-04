using System;
using System.Collections;
using System.Linq;
using Devil_s13.Core.Devils13UI;
using Devil_s13.Core.ThrowResultsProvider;
using Devil_s13.Core.ThrowResultsProvider.Data;
using UnityEngine;
using Zenject;

namespace Devil_s13.Core.GameLoop
{
    public class GameJudge : MonoBehaviour
    {
        [SerializeField]
        private GameStateDataAsset _gameStateDataAsset;
        private IThrowResultProvider _throwResultProvider;
        
        private Devils13GameModel _model;
        
        private bool _isThereWinnerOfGame = false;
        private bool _isThereWinnerOfRound = false;


        [Inject]
        public void Construct(Devils13GameModel model)
        {
         _model = model;   
        }
        
        public async void Start()
        {
            _throwResultProvider = new RandomThrowResultProvider();
            
            StartCoroutine(JudgeGame());
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
            _model.UpdateBetResultText(winnerIndex);
            
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
        
        private IEnumerator JudgeGame()
    {
    const int cycleStopValue = 1000;
    var cycleCounter = 0;
    
    ResetAllData();
    yield return new WaitForSeconds(1); 
    
    CreatePlayers();
    UpdateModel();
    
    while (!_isThereWinnerOfGame && cycleCounter < cycleStopValue)
    {
        cycleCounter++;
        _isThereWinnerOfRound = false;
        
        _isThereWinnerOfGame = _gameStateDataAsset.GetAllParticipantsGameData().Any(participantGameData =>
            participantGameData.NumberOfWins >= _gameStateDataAsset.MaxNumberOfWins);
        
        if (_isThereWinnerOfGame)
        {
            ResetAllData();
            UpdateModel();
            yield return new WaitForSeconds(1); 
            break;
        }

        while (!_isThereWinnerOfRound && cycleCounter < cycleStopValue)
        {
            cycleCounter++;
            ResetDicesAndBets();
            var playersThrowResultData = _throwResultProvider.GetThrowResult(_gameStateDataAsset);
            UpdateModel();
            yield return new WaitForSeconds(1); 
            UpdateDicesAndBets(playersThrowResultData);
            yield return new WaitForSeconds(1); 
            
            JudgeThrow(playersThrowResultData);
            UpdateModel();
            yield return new WaitForSeconds(1); 
            ResetDicesAndBets();

            _isThereWinnerOfRound = _gameStateDataAsset.GetAllParticipantsGameData().Any(participantGameData =>
                participantGameData.CurrentScore >= _gameStateDataAsset.WinningScore);
           
            if (_isThereWinnerOfRound)
            {
                _gameStateDataAsset.EndRoundReset();
                UpdateModel();
                yield return new WaitForSeconds(1); 
                
                break;
            }
        }
    }
}

        private void ResetDicesAndBets()
        {
            _model.ResetBetResultText();
            _model.ResetDiceValues();
            _model.ResetBets();

        }

        private void UpdateDicesAndBets(PlayersThrowResultData playersThrowResultData)
        {
            _model.UpdateDiceValues(playersThrowResultData.ThrowResultData[0]);
            _model.UpdateBetScoreForPlayer(playersThrowResultData.ThrowResultData[0].ParticipantIndex,
                playersThrowResultData.ThrowResultData[0].BetValue);
            _model.UpdateBetScoreForPlayer(playersThrowResultData.ThrowResultData[1].ParticipantIndex,
                playersThrowResultData.ThrowResultData[1].BetValue);
        }

        private void UpdateModel()
        {
            _model.UpdateModelWithGameData(_gameStateDataAsset);
        }

        private void ResetAllData()
        {
            _gameStateDataAsset.EndGameReset();
            _model.ResetBetResultText();
            _model.ResetDiceValues();
        }

        private void CreatePlayers()
        {
            for (int i = 0; i < 2; i++)
            {
                _gameStateDataAsset.AddParticipantWithIndex(i);
            }
        }
    }
}