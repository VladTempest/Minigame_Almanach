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
        
        [Inject]
        public void Construct(Devils13GameModel model)
        {
            _model = model;
        }
        
        private void Start()
        {
            var rootElement = _uiDocument.rootVisualElement;
            var throwButton = rootElement.Q<Button>(throwButtonId);
            var betButton = rootElement.Q<Button>(betButtonId);
            throwButton.clicked += () => TriggerOnThrowButtonClicked();
            betButton.clicked += () => TriggerOnBetButtonClicked();
            
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
            _model.SetDiceValues();
        }
    }
}