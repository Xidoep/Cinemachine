using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[TrackColor(0, 1, 1)]//El color del Track/Clips
[TrackBindingType(typeof(Light))]//L'objecte associat del Track
[TrackClipType(typeof(TimeLine_controlLlum_Clip))]//El tipus de Clip que accepta el Track.
public class Timeline_controlLLum_Track : TrackAsset
{
    //Funcio per crear un el "Comportament" (Mixer) quan es mesclin dos Clips al Track.
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<Timeline_controlLlum_Mixer>.Create(graph, inputCount);
    }
}


