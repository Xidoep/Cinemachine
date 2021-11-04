using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Cinematica_MixingInfluencia : MonoBehaviour
{
    CinemachineVirtualCameraBase virtualCamera;
    CinemachineMixingCamera mixing;
    SphereCollider exterior;
    SphereCollider interior;

    public bool activat;
    public Transform objectiu;
    public Transform oldParent;

    public float valor;


    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCameraBase>();
        virtualCamera.enabled = false;
        mixing = FindObjectOfType<CinemachineMixingCamera>();

        SphereCollider[] cols = GetComponents<SphereCollider>();
        if(cols[0].radius > cols[1].radius)
        {
            exterior = cols[0];
            interior = cols[1];
        }
        else
        {
            exterior = cols[1];
            interior = cols[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!activat || objectiu == null)
            return;

        valor = Mathf.Clamp01(1 - ((Vector3.Distance(transform.position, objectiu.position) - interior.radius) / (exterior.radius - interior.radius)));
        ControlarPes(valor);
    }

    private void OnTriggerEnter(Collider other)
    {
        virtualCamera.enabled = true;

        activat = true;
        objectiu = other.transform;

        oldParent = transform.parent;
        mixing.XS_AfegirCamara(virtualCamera);
    }

    private void OnTriggerExit(Collider other)
    {
        activat = false;
        objectiu = null;

        mixing.XS_TreureCamara(virtualCamera, oldParent);
        oldParent = null;
    }

    void ControlarPes(float pes)
    {
        //mixing.SetWeight(indexMixing, pes);
        for (int i = 0; i < mixing.XS_CamaresNumero(); i++)
        {
            //mixing.SetWeight(i, i == indexMixing ? pes : 1 - pes);
            mixing.XS_CanviarPes(virtualCamera, mixing.ChildCameras[i] == virtualCamera ? pes : 1 - pes);
            
        }
        
    }
}
