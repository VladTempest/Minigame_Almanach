namespace MainMenu
{
    public class SceneLoader : ISceneLoader
    {
        public void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}