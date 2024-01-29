using Devil_s13.Core.Devils13UI;
using UnityEngine;
using Zenject;

public class Devils13CoreInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Devils13GameModel>().AsSingle().NonLazy(); 
        Container.Bind<Devils13GameView>().FromComponentInHierarchy().AsSingle();
    }
}