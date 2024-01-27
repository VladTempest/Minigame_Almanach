using Zenject;

namespace MainMenu.MainMenuUI
{
    public class MainMenuModel
    {
        private ISceneLoader _sceneLoader;
        private MainMenuView _view;
        
        [Inject]
        public void Construct(ISceneLoader sceneLoader, MainMenuView view)
        {
            _sceneLoader = sceneLoader;
            _view = view;
            
            _view.OnMiniGameButtonClicked += OnMiniGameButtonClicked;
        }
        
        public void OnMiniGameButtonClicked(string miniGameSceneName)
        {
            _sceneLoader.LoadScene(miniGameSceneName);
        }
    }
}