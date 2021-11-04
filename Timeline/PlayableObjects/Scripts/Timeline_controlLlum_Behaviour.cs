using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;


//El "Comportament" es el que crea els comportaments al Timeline. 
//Ha d'estar Serialitzat per poder editar les seves variables publiques
[System.Serializable]
public class Timeline_controlLlum_Behaviour: PlayableBehaviour
{
    //Variables publiques editables
    public Color color = Color.white;
    public float intensity = 1f;
    public float bounceIntensity = 1f;
    public float range = 10f;

    //Encapsula la llum per editarla fora del "ProcessFrame".
    Light light;

    //Guarda un bool com a flanc
    bool firstFrameHappened;
    //Guarda valor inicials de la llum abans de modificarla per aplicarli despres
    Color defaultColor;
    float defaultIntensity;
    float defaultBounceIntensity;
    float defaultRange;

    //La funsio que es crida a cada frame dins el Timeline.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        //PlayerData retorna informacio del "Binding" del track.
        light = playerData as Light;

        if (light == null)
            return;

        //Guardem els valors inicials de la llum.
        if (!firstFrameHappened)
        {
            defaultColor = light.color;
            defaultIntensity = light.intensity;
            defaultBounceIntensity = light.bounceIntensity;
            defaultRange = light.range;

            firstFrameHappened = true;
        }

        //Apliquem les variables.
        light.color = color;
        light.intensity = intensity;
        light.bounceIntensity = bounceIntensity;
        light.range = range;
        //RenderSettings.ambientLight = 
    }

    //Funcio llançada quan el timeline acaba el Clip
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        firstFrameHappened = false;

        if (light == null)
            return;

        light.color = defaultColor;
        light.intensity = defaultIntensity;
        light.bounceIntensity = defaultBounceIntensity;
        light.range = defaultRange;
    }
}
