using System.ComponentModel;
using Devil_s13.Core;
using Devil_s13.Core.Devils13UI;
using Devil_s13.Core.GameLoop;
using UnityEngine;
using Zenject;

public class Devils13CoreInstaller : MonoInstaller
{
    [SerializeField]
    private GameStateDataAsset _gameStateDataAsset;
    public override void InstallBindings()
    {
        Container.Bind<GameStateDataAsset>().FromInstance(_gameStateDataAsset).AsSingle();
        Container.Bind<IPlayerInitializer>().To<OfflinePlayerInitializer>().AsSingle().NonLazy();
        Container.Bind<Devils13GameModel>().AsSingle().NonLazy(); 
        Container.Bind<Devils13GameView>().FromComponentInHierarchy().AsSingle();
        
    }
}