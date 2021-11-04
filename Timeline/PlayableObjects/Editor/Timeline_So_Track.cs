using UnityEngine;
using UnityEngine.Timeline;

[TrackColor(1, 1, 0)]//El color del Track/Clips
[TrackBindingType(typeof(Transform))]//L'objecte associat del Track
[TrackClipType(typeof(Timeline_So_Clip))]//El tipus de Clip que accepta el Track.
public class Timeline_So_Track : TrackAsset
{
    /*public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<Timeline_controlLlum_Mixer>.Create(graph, inputCount);
    }*/
}
