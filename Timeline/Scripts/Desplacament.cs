using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Desplacament : MonoBehaviour
{
    public enum Modes
    {
        Velocitat, Temps
    }

    [HideInInspector] public bool arribat = false;

    [Header("Elements")]
    [SerializeField] Transform element;
    [SerializeField] Transform objectiu;

    [Header("Opcions")]
    [Header("Moviment")]
    public Modes mode;
    [SerializeField] float valor;
    [SerializeField] bool suavitzatInici = true;
    [SerializeField] bool suavitzatFinal = true;
    [Header("Orientacio")]
    [SerializeField] bool orientarInici;
    [SerializeField] bool orientarFinal;
    [SerializeField] float orientarVelocitat;

    public UnityEvent esdeveniment;

    Vector3 posicioInicial;
    float interpolacio;



    float Distancia
    {
        get
        {
            return Vector3.Distance(element.position, objectiu.position);
        }
    }


    [ContextMenu("Desplacar")]
    public void Desplacar()
    {
        arribat = false;
        if (orientarInici)
        {
            StartCoroutine(OrientarIniciTemps());
        }
        else
        {
            StartCoroutine(DesplacarTemps());
        }
    }

    IEnumerator OrientarIniciTemps()
    {

        while (Mathf.Abs((element.forward - ((objectiu.position - element.position).normalized)).magnitude) > 0.1f)
        {
            Vector3 _dirEle = element.forward;
            Vector3 _dirObj = (objectiu.position - element.position);
            Vector3 _dirObjF = _dirObj / _dirObj.magnitude;
            Debug.DrawRay(element.position, _dirEle, Color.blue);
            Debug.DrawRay(element.position, _dirObjF, Color.yellow);

            //Orientar();
            
            element.rotation = Quaternion.RotateTowards(element.rotation, Quaternion.LookRotation(_dirObjF), orientarVelocitat);
            //element.rotation = Quaternion.FromToRotation(element.forward, _dirObj);
            yield return null;
        }

        StartCoroutine(DesplacarTemps());
    }

    IEnumerator DesplacarTemps()
    {
        Debug.Log("Inici");
        posicioInicial = element.position;
        while (mode == Modes.Temps ? (interpolacio / orientarVelocitat) < 1 : Distancia > 0.1f)
        {
            Debug.Log("interaccio");
            Moviment();

            yield return null;
        }

        interpolacio = 0;

        Debug.Log("final");
        if (orientarFinal)
        {
            StartCoroutine(OrientarTemps());
            yield return null;
        }
        else
        {
            arribat = true;
            if (esdeveniment != null) esdeveniment.Invoke();
        }
    }

    IEnumerator OrientarTemps()
    {

        while (Mathf.Abs((element.forward - objectiu.forward).magnitude) > 0.1f)
        {
            Vector3 _dirEle = element.forward;
            Vector3 _dirObj = objectiu.forward;
            Debug.DrawRay(element.position, _dirEle, Color.blue);
            Debug.DrawRay(element.position, _dirObj, Color.yellow);

            Orientar();
            yield return null;
        }

        arribat = true;
        if (esdeveniment != null) esdeveniment.Invoke();
    }



    void Moviment()
    {
        switch (mode)
        {
            case Modes.Temps:
                float _increment = (0.01f + (interpolacio / valor)) * Time.deltaTime * 10;
                float _decrement = (0.01f + ((valor - interpolacio) / valor)) * Time.deltaTime * 10;

                float _multiplicador = 1;
                if(suavitzatInici && suavitzatFinal)
                {
                    _multiplicador = 1.55f;
                }
                else
                {
                    if(suavitzatInici || suavitzatFinal)
                    {
                        _multiplicador = 1.2f;
                    }
                    else
                    {
                        _multiplicador = 1;
                    }
                }
                float _velocitatNormal = Time.deltaTime * _multiplicador;

                interpolacio += Mathf.Min(Mathf.Min(suavitzatInici ? _increment : _velocitatNormal, suavitzatFinal ? _decrement : _velocitatNormal), _velocitatNormal);

                element.position = Vector3.Lerp(posicioInicial, objectiu.position, interpolacio/valor);

                break;
            case Modes.Velocitat:
                if (suavitzatFinal)
                {
                    element.position = Distancia > 1 ? DistanciaMaxima() : DistanciaSuavitzada();
                }
                else
                {
                    element.position = DistanciaMaxima();
                }
                break;
        }

    }


    Vector3 DistanciaMaxima()
    {
        return element.position + ((objectiu.position - element.position).normalized * (valor * 0.01f));
    }
    Vector3 DistanciaSuavitzada()
    {
        return Vector3.Lerp(element.position, objectiu.position, valor * 0.01f);
    }

    void Orientar()
    {
        element.rotation = Quaternion.RotateTowards(element.rotation,  objectiu.rotation, orientarVelocitat);
    }

}
