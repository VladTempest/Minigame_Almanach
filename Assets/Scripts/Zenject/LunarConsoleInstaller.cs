using LunarConsolePlugin;
using UnityEngine;
using Zenject;

public class LunarConsoleInstaller : MonoInstaller
{
    [SerializeField] private LunarConsole _lunarConsolePrefab;
    public override void InstallBindings()
    {
        DontDestroyOnLoad(Container.InstantiatePrefab(_lunarConsolePrefab));
    }
}