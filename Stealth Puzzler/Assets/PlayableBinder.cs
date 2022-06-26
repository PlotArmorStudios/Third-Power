using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
        var controllerTrack = timeline.GetOutputTrack(0);
        var camTrack = timeline.GetOutputTrack(2);
        
        foreach (var output in _playableDirector.playableAsset.outputs)
        {
            Debug.Log(output.sourceObject.GetType() );
            if (output.streamName == "Cinemachine Track" )
            {
                _playableDirector.SetGenericBinding(output.sourceObject, Camera.main.GetComponent<CinemachineBrain>() );
           
                var cinemachineTrack = output.sourceObject as CinemachineTrack;
                foreach( var clip in cinemachineTrack.GetClips() ){
 
                    var cinemachineShot = clip.asset as CinemachineShot;
                    _playableDirector.SetReferenceValue(cinemachineShot.VirtualCamera.exposedName, RigVCamReference.Instance.VCam);
 
                }
            }
        }
        
        yield return new WaitForSeconds(.5f);

        Debug.Log("Tracks assigned");
        var controllerManger = FindObjectOfType<ControllerManager>();
        var bindingCam = Camera.main;
        _playableDirector.SetGenericBinding(controllerTrack, controllerManger);
        _playableDirector.SetGenericBinding(camTrack, bindingCam);

    }
}
