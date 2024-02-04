using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TestMono : MonoBehaviour
{
    public bool isTriggered = false;
    private async void Start()
    {
       await CreateUniTask();
        Debug.Log("Start method is done.");
    }

    private async UniTask CreateUniTask()
    {
        await UniTask.WaitUntil(() => isTriggered);
        Debug.Log("UniTask is done.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isTriggered = true;
            Debug.Log("Space key was pressed.");
        }
    }
    
}
