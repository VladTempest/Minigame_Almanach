using System;
using System.Linq;
using MainMenu.MainMenuUI.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Zenject;

namespace MainMenu.MainMenuUI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField]
        private UIDocument _uiDocument;
        [SerializeField]
        private MinigamesDataAsset _minigamesDataAsset;
        private VisualElement _rootElement;
        private VisualElement _miniGameButtonsContainer;


        public event Action<string> OnMiniGameButtonClicked ;
        
        public void Start()
        {
           UpdateMiniGameButtons();
        }

        private void UpdateMiniGameButtons()
        {
            _rootElement = _uiDocument.rootVisualElement;
            _miniGameButtonsContainer = _rootElement.Q<VisualElement>("miniGameButtonsContainer");
            var miniGamesButtons = _miniGameButtonsContainer.Children().ToList();
            int updatedButtonsCount = 0;
            foreach (var minigameData in _minigamesDataAsset.MinigamesData)
            {
                miniGamesButtons[updatedButtonsCount].Q<Button>().text = minigameData.Name;
                miniGamesButtons[updatedButtonsCount].Q<Button>().clicked += () =>
                    TriggerOnMiniGameButtonClicked(minigameData.SceneReference.SceneName);
                updatedButtonsCount++;
            }

            for (int i = updatedButtonsCount; i < miniGamesButtons.Count; i++)
            {
                _miniGameButtonsContainer.Remove(miniGamesButtons[i]);
            }
        }

        public void TriggerOnMiniGameButtonClicked(string miniGameSceneName)
        {
            OnMiniGameButtonClicked?.Invoke(miniGameSceneName);
        }
    }
}