using System;
using System.Collections.Generic;
using DevLocker.Utils;
using UnityEngine;

namespace MainMenu.MainMenuUI.Data
{
    [CreateAssetMenu(fileName = "MinigamesDataAsset", menuName = "Data/MinigamesDataAsset")]
    public class MinigamesDataAsset : ScriptableObject
    {
        public List<MinigameData> MinigamesData;
    }

    [Serializable]
    public class MinigameData
    {
        public SceneReference SceneReference;
        public string Name;
    }
}