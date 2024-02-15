using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;
    
    private void Update()
    {
        bool isAnimationShouldPlay = false;
        
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isAnimationShouldPlay = true;
        }
#endif
        
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            isAnimationShouldPlay = true;
        }
#endif
        
        if (isAnimationShouldPlay)
        {
            if (_playableDirector.state != PlayState.Playing)
            {
                _playableDirector.Play();
            }
        }

    }
}
