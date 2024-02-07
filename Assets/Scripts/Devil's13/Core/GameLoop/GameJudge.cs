using System.Linq;
using Cysharp.Threading.Tasks;
using Devil_s13.Core.Devils13UI;
using Devil_s13.Core.ThrowResultsProvider;
using Devil_s13.Core.ThrowResultsProvider.Data;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Devil_s13.Core.GameLoop
{
    public class GameJudge : MonoBehaviour
    {
        [SerializeField]
        private GameStateDataAsset _gameStateDataAsset;
        private IThrowResultProvider _throwResultProvider;
        private Devils13GameModel _model;
        private bool _isThereWinnerOfRound = false;
        
        [Inject]
        public void Construct(Devils13GameModel model)
        {
         _model = model;   
        }
        
        public async void Start()
        {
            _throwResultProvider = new RandomThrowResultProvider();
            
            await RunGame();
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
            
            if (winnerIndex == -1) return;
            
            _gameStateDataAsset.AddScoreToParticipant(winnerIndex, playersData.ThrowResultData[winnerIndex].OtherDiceValues.Sum());
            
            if (_gameStateDataAsset.GetAllParticipantsGameData()[winnerIndex].CurrentScore >= _gameStateDataAsset.WinningScore) _gameStateDataAsset.AddWinToParticipant(winnerIndex);
        }
        

        private void UpdatedBets(PlayersThrowResultData playersThrowResultData)
        {
            _model.UpdateBetScoreForPlayer(playersThrowResultData.ThrowResultData[0].ParticipantIndex,
                playersThrowResultData.ThrowResultData[0].BetValue);
            _model.UpdateBetScoreForPlayer(playersThrowResultData.ThrowResultData[1].ParticipantIndex,
                playersThrowResultData.ThrowResultData[1].BetValue);
        }

        private void UpdateDices(PlayersThrowResultData playersThrowResultData)
        {
            _model.UpdateDiceValues(playersThrowResultData.ThrowResultData[0]);
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

        private async UniTask RunGame()
        {
            var maxNumberOfWins = _gameStateDataAsset.MaxNumberOfWins;
            var maxRoundsCounts = (2 * maxNumberOfWins) - 1;
            
            CreatePlayers();
            UpdateModel();
            
            for (int i = 0; i < maxRoundsCounts; i++)
            {
                _gameStateDataAsset.EndRoundReset();
                _isThereWinnerOfRound = false;
                
                UpdateModel();
                if (_gameStateDataAsset.GetAllParticipantsGameData().Any(participantGameData =>
                    participantGameData.NumberOfWins >= maxNumberOfWins)) break;
                
                while (!_isThereWinnerOfRound)
                {
                    _model.isThrowButtonEnabled.Value = true;
                    var results = await ThrowDices();
                    UpdateDices(results);
                    
                    await Bet(results);
                    UpdatedBets(results);
                    
                    await UniTask.WaitForSeconds(2f);

                    JudgeThrow(results);
                    UpdateModel();
                    _isThereWinnerOfRound = _gameStateDataAsset.GetAllParticipantsGameData().Any(participantGameData =>
                        participantGameData.CurrentScore >= _gameStateDataAsset.WinningScore);
                    
                    await UniTask.WaitForSeconds(2f);
                    
                    _model.ResetDiceValues();
                    _model.ResetBets();
                    _model.isThrowButtonEnabled.Value = true;
                }
            }
            
            JudgeGameResult(_gameStateDataAsset);
            ResetAllData();
            UpdateModel();
        }

        private void JudgeGameResult(GameStateDataAsset gameStateDataAsset)
        {
            var winnerIndex = gameStateDataAsset.GetAllParticipantsGameData().OrderByDescending(participantGameData => participantGameData.NumberOfWins).First().ParticipantIndex;
            Debug.Log($"Player {winnerIndex} won the game.");
        }

        private async UniTask<PlayersThrowResultData> ThrowDices()
        {
            await UniTask.WaitUntil(() => _model.throwButtonClicked.Value);
            _model.throwButtonClicked.Value = false;
            _model.isThrowButtonEnabled.Value = false;
            
            PlayersThrowResultData results = _throwResultProvider.GetThrowResult(_gameStateDataAsset);
            return results;
            }

        private async UniTask Bet(PlayersThrowResultData results)
        {
            await UniTask.WaitUntil(() => _model.clickedDiceWithIndex.Value != -1);
            _model.isBetButtonEnabled.Value = true;          

            await UniTask.WaitUntil(() => _model.betButtonClicked.Value);
            _model.betButtonClicked.Value = false;
            _model.isBetButtonEnabled.Value = false;

            results.ThrowResultData[0].BetValue = results.ThrowResultData[0].OtherDiceValues[_model.clickedDiceWithIndex.Value];
            results.ThrowResultData[0].OtherDiceValues.RemoveAt(_model.clickedDiceWithIndex.Value);
            
            //Imitate opponent's bet
            var randomIndex = Random.Range(0, 2);
            results.ThrowResultData[1].BetValue =  results.ThrowResultData[1].OtherDiceValues[randomIndex];
            results.ThrowResultData[1].OtherDiceValues.RemoveAt(randomIndex);
        }
    }
}