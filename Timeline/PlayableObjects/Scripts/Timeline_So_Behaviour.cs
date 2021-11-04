using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class Timeline_So_Behaviour : PlayableBehaviour
{
    public So so;

    Transform transform;
    bool played;

    AudioSource audioSource;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!Application.isPlaying)
            return;

        transform = playerData as Transform;

        if (transform == null)
            return;

        if (!played)
        {
            played = true;
            audioSource = so.Play_Referencia(transform);
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (!Application.isPlaying)
            return;

        if (audioSource == null)
            return;

        played = false;

        so.Stop(audioSource);
    }
}
