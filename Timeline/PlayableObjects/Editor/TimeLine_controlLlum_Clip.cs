using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class TimeLine_controlLlum_Clip : PlayableAsset, ITimelineClipAsset
{
    //Base sobre la que modifiques els valors al editor.
    [SerializeField] private Timeline_controlLlum_Behaviour template = new Timeline_controlLlum_Behaviour();

    //Qualitats dels extrems dels Clip. None, no es pot mesclar. Blending es pot mesclar amb altres.
    public ClipCaps clipCaps => ClipCaps.Blending;

    //Funcio que es crida al crear el Clip al editor.
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<Timeline_controlLlum_Behaviour>.Create(graph, template);
    }
}
