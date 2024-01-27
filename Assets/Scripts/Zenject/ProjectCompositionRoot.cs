using System.Collections;
using System.Collections.Generic;
using MainMenu;
using MainMenu.MainMenuUI;
using UnityEngine;
using Zenject;

public class ProjectCompositionRoot : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();

        Container.Bind<MainMenuView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<MainMenuModel>().AsSingle().NonLazy(); 
    }
}
