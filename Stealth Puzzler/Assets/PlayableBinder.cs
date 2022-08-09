using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayableBinder : MonoBehaviour
{
    private PlayableDirector _playableDirector;
    private TimelineAsset timeline;

    private IEnumerator Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        timeline = _playableDirector.playableAsset as TimelineAsset;
        var controllerTrack = timeline.GetOutputTrack(1);

        yield return new WaitForSeconds(.5f);

        var controllerManager = FindObjectOfType<ControllerManager>();
        _playableDirector.SetGenericBinding(controllerTrack, controllerManager);
        Debug.Log(controllerTrack + " and " + controllerManager);
        Debug.Log("Tracks assigned");
    }
}