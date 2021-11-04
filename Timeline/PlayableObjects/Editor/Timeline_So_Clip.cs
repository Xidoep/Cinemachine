using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class Timeline_So_Clip : PlayableAsset, ITimelineClipAsset
{
    //Base sobre la que modifiques els valors al editor.
    [SerializeField] private Timeline_So_Behaviour template = new Timeline_So_Behaviour();

    //Qualitats dels extrems dels Clip. None, no es pot mesclar. Blending es pot mesclar amb altres.
    public ClipCaps clipCaps => ClipCaps.None;

    //Funcio que es crida al crear el Clip al editor.
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<Timeline_So_Behaviour>.Create(graph, template);
    }
}
