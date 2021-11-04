using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//Un comportament creat pel mateix limeline quan 2 Timeline_controlLlum_Behaviour es mesclen.
public class Timeline_controlLlum_Mixer : PlayableBehaviour
{
    Light light;

    //Guarda un bool com a flanc
    bool firstFrameHappened;
    //Guarda valor inicials de la llum abans de modificarla per aplicarli despres
    Color defaultColor;
    float defaultIntensity;
    float defaultBounceIntensity;
    float defaultRange;

    //Funcio com Update del Timeline.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        light = playerData as Light;

        if (light == null)
            return;

        if (!firstFrameHappened)
        {
            defaultColor = light.color;
            defaultIntensity = light.intensity;
            defaultBounceIntensity = light.bounceIntensity;
            defaultRange = light.range;

            firstFrameHappened = true;
        }

        //Input = Clip.
        //Agafa tots els inputs/Clips mesclats del Track. De normal és 1, quan es mesclen dos Clips és 2.
        int inputCount = playable.GetInputCount();

        //Crea colors blendeds
        Color blendedColor = Color.clear;
        float blendedIntensity = 0f;
        float blendedBouncedIntensity = 0f;
        float blendedRange = 0f;
        float totalWeight = 0f;


        for (int i = 0; i < inputCount; i++)
        {
            //Agafa el pes de cada input.
            float inputWeight = playable.GetInputWeight(i);
            //Encapsula el Clip pertintent de la llista del Track. (No es pot agafar el Behaviour directament, per aixo primer l'encapsula)
            ScriptPlayable<Timeline_controlLlum_Behaviour> inputPlayer = (ScriptPlayable<Timeline_controlLlum_Behaviour>)playable.GetInput(i);
            //Encapsula el "Comportament" programat de dins el Clip.
            Timeline_controlLlum_Behaviour behaviour = inputPlayer.GetBehaviour();

            //Mescla les variables segons el pes del Clip en aquell moment.
            blendedColor += behaviour.color * inputWeight;
            blendedIntensity += behaviour.intensity * inputWeight;
            blendedBouncedIntensity += behaviour.bounceIntensity * inputWeight;
            blendedRange += behaviour.range * inputWeight;
            totalWeight += inputWeight; //Tots els pesos sumats haurien de fer 1. A no se que es mescli amb res.
        }

        //Per tant de no acabar el Clip amb pes 0. Es captura el pes que falta per arribar a 1.
        float remainingWeight = 1 - totalWeight;

        //S'apliquen les variables a la llum.
        light.color = blendedColor + defaultColor * remainingWeight; //Per tal de no passar valors 0 al acabar el Clip, se l'hi sumen els valors inicials multiplicar pel pes que falta per arribar a 1.
        light.intensity = blendedIntensity + defaultIntensity * remainingWeight;
        light.bounceIntensity = blendedRange + defaultBounceIntensity * remainingWeight;
        light.range = blendedRange + defaultRange * remainingWeight;
    }

    //Funcio cridada al superar el Clip.
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
