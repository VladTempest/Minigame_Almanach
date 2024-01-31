using System;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Devil_s13.Core.Devils13UI
{
    public class Devils13GameView : MonoBehaviour
    {
        [SerializeField]
        private UIDocument _uiDocument;
        
        private Devils13GameModel _model;

        private const string throwButtonId = "throwButton";
        private const string betButtonId = "betButton";
        
        private const string firstDiceValueLabelId = "firstDiceNumber";
        private const string secondDiceValueLabelId = "secondDiceNumber";
        
        private const string opponentNameLabelId = "opponentTitle";
        private const string opponentStatusLabelId = "opponentStatusTitle";
        private const string opponentScoreValueId = "opponentRoundScoreValue";
        private const string opponentWinsValueId = "opponentWinsValue";
        private const string opponentBetNameLabelId = "opponentBetTitle";
        private const string opponentBetValueLabelId = "opponentBetRoundScoreValue";
        
        private const string playerNameLabelId = "playerTitle";
        private const string playerScoreValueId = "playerRoundScoreValue";
        private const string playerWinsValueId = "playerWinsValue";
        private const string playerBetNameLabelId = "playerBetTitle";
        private const string playerBetValueLabelId = "playerBetRoundScoreValue";
        
        private const string betResultLabelId = "resulMarkTitle";
        
        
        
        [Inject]
        public void Construct(Devils13GameModel model)
        {
            _model = model;
        }
        
        private void Start()
        {
            var rootElement = _uiDocument.rootVisualElement;
            SetUpButtons(rootElement);
            SetUpDiceUI(rootElement);
            SetUpOpponentUI(rootElement);
            SetUpPlayerUI(rootElement);
            SetUpBetResultUI(rootElement);
        }

        private void SetUpBetResultUI(VisualElement rootElement)
        {
            var betResultLabel = rootElement.Q<Label>(betResultLabelId);
            _model.betResult.Subscribe(value => betResultLabel.text = value);
        }

        private void SetUpOpponentUI(VisualElement rootElement)
        {
            var opponentNameLabel = rootElement.Q<Label>(opponentNameLabelId);
            var opponentStatusLabel = rootElement.Q<Label>(opponentStatusLabelId);
            var opponentScoreValue = rootElement.Q<Label>(opponentScoreValueId);
            var opponentWinsValue = rootElement.Q<Label>(opponentWinsValueId);
            var opponentBetNameLabel = rootElement.Q<Label>(opponentBetNameLabelId);
            var opponentBetValueLabel = rootElement.Q<Label>(opponentBetValueLabelId);
            
            _model.opponentName.Subscribe(value => opponentNameLabel.text = value);
            _model.opponentName.Subscribe(value => opponentBetNameLabel.text = value);
            _model.opponentStatus.Subscribe(value => opponentStatusLabel.text = value);
            _model.opponentScore.Subscribe(value => opponentScoreValue.text = value.ToString());
            _model.opponentWins.Subscribe(value => opponentWinsValue.text = value.ToString());
            _model.opponentBet.Subscribe(value => opponentBetValueLabel.text = value.ToString());
        }

        private void SetUpPlayerUI(VisualElement rootElement)
        {
            var playerNameLabel = rootElement.Q<Label>(playerNameLabelId);
            var playerScoreValue = rootElement.Q<Label>(playerScoreValueId);
            var playerWinsValue = rootElement.Q<Label>(playerWinsValueId);
            var playerBetNameLabel = rootElement.Q<Label>(playerBetNameLabelId);
            var playerBetValueLabel = rootElement.Q<Label>(playerBetValueLabelId);
            
            _model.playerName.Subscribe(value => playerNameLabel.text = value);
            _model.playerName.Subscribe(value => playerBetNameLabel.text = value);
            _model.playerScore.Subscribe(value => playerScoreValue.text = value.ToString());
            _model.playerWins.Subscribe(value => playerWinsValue.text = value.ToString());
            _model.playerBet.Subscribe(value => playerBetValueLabel.text = value.ToString());
        }

        private void SetUpButtons(VisualElement rootElement)
        {
            var throwButton = rootElement.Q<Button>(throwButtonId);
            var betButton = rootElement.Q<Button>(betButtonId);
            throwButton.clicked += () => TriggerOnThrowButtonClicked();
            betButton.clicked += () => TriggerOnBetButtonClicked();
        }

        private void SetUpDiceUI(VisualElement rootElement)
        {
            var firstDiceValueLabel = rootElement.Q<Label>(firstDiceValueLabelId);
            var secondDiceValueLabel = rootElement.Q<Label>(secondDiceValueLabelId);

            _model.firstDiceValue.Subscribe(value => firstDiceValueLabel.text = value.ToString());
            _model.secondDiceValue.Subscribe(value => secondDiceValueLabel.text = value.ToString());
        }

        private void TriggerOnBetButtonClicked()
        {
            Debug.Log("Bet button clicked");
        }

        private void TriggerOnThrowButtonClicked()
        {
            Debug.Log("Throw button clicked");
        }
    }
}