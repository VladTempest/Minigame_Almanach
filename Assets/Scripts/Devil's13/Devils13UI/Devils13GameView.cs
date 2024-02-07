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
        private const string firstDiceVisualId = "firstDiceVisual";
        private const string secondDiceVisualId = "secondDiceVisual";
        private const string ussDiceAsBet = "styleDiceChecked";
        private const string ussDiceAsNotBet = "styleDice";
        
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
            throwButton.clicked += () =>
            {
                ResetDiceAsBet();
                _model.throwButtonClicked.Value = true;
            };
            betButton.clicked += () =>
            {
                _model.betButtonClicked.Value = true;
            };
            
            
            _model.isThrowButtonEnabled.Subscribe(value => throwButton.SetEnabled(value));
            _model.isBetButtonEnabled.Subscribe(value => betButton.SetEnabled(value));
        }

        private void SetUpDiceUI(VisualElement rootElement)
        {
            _model.clickedDiceWithIndex.Value = -1;
            
            var firstDiceValueLabel = rootElement.Q<Label>(firstDiceValueLabelId);
            var secondDiceValueLabel = rootElement.Q<Label>(secondDiceValueLabelId);

            _model.firstDiceValue.Subscribe(value => firstDiceValueLabel.text = value.ToString());
            _model.secondDiceValue.Subscribe(value => secondDiceValueLabel.text = value.ToString());
            
            var firstDiceVisuals = rootElement.Q<VisualElement>(firstDiceVisualId);
            var secondDiceVisuals = rootElement.Q<VisualElement>(secondDiceVisualId);
            
            firstDiceVisuals.RegisterCallback((ClickEvent evt) =>
            {
                
                OnDiceVisualClicked(0);
            });
            secondDiceVisuals.RegisterCallback((ClickEvent evt) =>
            {
                OnDiceVisualClicked(1);
            });

            _model.clickedDiceWithIndex.Subscribe(value => SetDiceAsBet(value));
        }
        
        private void OnDiceVisualClicked(int diceIndex)
        {
            if (_model.firstDiceValue.Value == 0 || _model.secondDiceValue.Value == 0)
            {
                return;
            }
            _model.clickedDiceWithIndex.Value = diceIndex;
        }

        private void SetDiceAsBet(int diceIndex)
        {
            ResetDiceAsBet();
            
            var root = GetComponent<UIDocument>().rootVisualElement;
            VisualElement dice = null;
            switch (diceIndex)
            {
                case 0:
                    dice = root.Q<VisualElement>(firstDiceVisualId);
                    break;
                case 1:
                    dice = root.Q<VisualElement>(secondDiceVisualId);
                    break;
            }
            
            if (dice != null)
            {
                dice.AddToClassList(ussDiceAsBet);
                dice.RemoveFromClassList(ussDiceAsNotBet);
            }
            
            if (diceIndex == -1)
            {
                ResetDiceAsBet();
            }
        }
        
        public void ResetDiceAsBet()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            var firstDice = root.Q<VisualElement>(firstDiceVisualId);
            var secondDice = root.Q<VisualElement>(secondDiceVisualId);
            firstDice.RemoveFromClassList(ussDiceAsBet);
            firstDice.AddToClassList(ussDiceAsNotBet);
            secondDice.RemoveFromClassList(ussDiceAsBet);
            secondDice.AddToClassList(ussDiceAsNotBet);
        }
    }
}