using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

public class AnimationSwtcher : MonoBehaviour
{
    public TimelineAsset _verticalTimeline;
    public TimelineAsset _horizontalTimeline;

    public PlayableDirector MainPlayableDirector;
    public PlayableDirector BlendingPlayableDirector;

    private void Start()
    {
        var cloner = Cloning.GetCloner(MainPlayableDirector, MainPlayableDirector.GetType());
        BlendingPlayableDirector = MainPlayableDirector.Clone(cloner, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (MainPlayableDirector.playableAsset == _verticalTimeline)
            {
                SmothlySwitchToAnotherTimeline(_horizontalTimeline);
            }
            else
            {
                SmothlySwitchToAnotherTimeline(_verticalTimeline);
            }
        }
    }

    private void SmothlySwitchToAnotherTimeline(TimelineAsset horizontalTimeline)
    {
        BlendingPlayableDirector.Play(horizontalTimeline);
        BlendingPlayableDirector.extrapolationMode = DirectorWrapMode.Loop;
        BlendingPlayableDirector.Play();
        
        MainPlayableDirector.extrapolationMode = DirectorWrapMode.None;
    }
}
