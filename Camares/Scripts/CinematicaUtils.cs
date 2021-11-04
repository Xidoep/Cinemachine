using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//Brain => Gestiona la camara.
//Brain.BlendTime => Canvia el temps de blending entre camara.

//TargetGroup => Apunta al punt mitg entre varis punts.
//TargetGroup.Affegir => Afageix un element al grup.
//TargetGroup.TrobatIndex => Retorna l'index d'un element del grup.
//TargetGroup.CanviaPes => Canvia l'importancia d'un element
//TargetGroup.Treure => Treure un element al grup.

//Mixing => Apunta a un punt entre 2 elements.
//Mixing.Pes(CMC, index, pes) => Canvia l'importancia de l'element.
//Mixing.Pes(CMC, index) => Retorna l'importancia de l'element.

//Impuls => Tremolor de la camara
//Impuls.Generar(CIS, posicio, velocitat) => Genera un tremolor.
//Impuls.Generar(CIS, força) => Genera un tremolor.

public static class XS_Brain
{
    public static void XS_BlendTime(this CinemachineBrain brain, float temps)
    {
        brain.m_DefaultBlend.m_Time = temps;
    }
}

public static class XS_FreeLook
{
    public static void XS_AddOrientar(this CinemachineFreeLook freeLook, Vector2 orientacio)
    {
        freeLook.m_YAxis.m_InputAxisValue = orientacio.y;
        freeLook.m_XAxis.m_InputAxisValue = orientacio.x;
    }

    public static void XS_OrbitaSuperior(this CinemachineFreeLook freeLook, float altura, float radi) => freeLook.XS_Orbita(0, altura, radi);
    public static void XS_OrbitaMitja(this CinemachineFreeLook freeLook, float altura, float radi) => freeLook.XS_Orbita(1, altura, radi);
    public static void XS_OrbitaInferior(this CinemachineFreeLook freeLook, float altura, float radi) => freeLook.XS_Orbita(2, altura, radi);
    static void XS_Orbita(this CinemachineFreeLook freeLook, int index, float altura, float radi)
    {
        freeLook.m_Orbits[index].m_Height = altura;
        freeLook.m_Orbits[index].m_Radius = radi;
    }
}

public static class XS_TargetGroup
{
    public static void XS_Affegir(this CinemachineTargetGroup targetGroup, Transform transform, float pes = 1, float radi = 1)
    {
        targetGroup.AddMember(transform, pes, radi);
    }
    public static int XS_TrobarIndex(this CinemachineTargetGroup targetGroup, Transform transform)
    {
        return targetGroup.FindMember(transform);
    }
    public static void XS_CanviaPes(this CinemachineTargetGroup targetGroup, int index, float pes)
    {
        targetGroup.m_Targets[index].weight = pes;
    }
    public static void XS_CanviaPes(this CinemachineTargetGroup targetGroup, Transform transform, float pes)
    {
        targetGroup.m_Targets[targetGroup.FindMember(transform)].weight = pes;
    }
    public static void XS_CanviarRadi(this CinemachineTargetGroup targetGroup, int index, float radi)
    {
        targetGroup.m_Targets[index].radius = radi;
    }
    public static void XS_CanviarRadi(this CinemachineTargetGroup targetGroup, Transform transform, float radi)
    {
        targetGroup.m_Targets[targetGroup.FindMember(transform)].radius = radi;
    }
    public static void XS_Treure(this CinemachineTargetGroup targetGroup, Transform transform)
    {
        targetGroup.RemoveMember(transform);
    }
}



public static class XS_Impuls
{
    public static void XS_Generar(this CinemachineImpulseSource impulseSource, Vector3 posicio, Vector3 velocitat)
    {
        impulseSource.GenerateImpulseAt(posicio, velocitat);
    }
    public static void XS_Generar(this CinemachineImpulseSource impulseSource, float força)
    {
        impulseSource.GenerateImpulse(força);
    }
}

public static class BlendList
{
    /// <summary>
    /// Afageix una camara a la jerarquia, el component ja ho detecta
    /// </summary>
    /// <param name="blendList"></param>
    /// <param name="camaraVirtual"></param>
    public static void XS_AfegirCamara(this CinemachineBlendListCamera blendList, CinemachineVirtualCameraBase camaraVirtual)
    {
        camaraVirtual.transform.SetParent(blendList.transform);
    }
    public static void XS_AfegirCamara(this CinemachineBlendListCamera blendList, Transform camaraVirtual)
    {
        camaraVirtual.SetParent(blendList.transform);
    }
    /// <summary>
    /// Treu una camara de la jerarquia, el component ja l'elimina.
    /// </summary>
    /// <param name="blendList"></param>
    /// <param name="camaraVirtual"></param>
    public static void XS_TreureCamara(this CinemachineBlendListCamera blendList, CinemachineVirtualCameraBase camaraVirtual)
    {
        camaraVirtual.transform.SetParent(null);
    }
    public static void XS_TreureCamara(this CinemachineBlendListCamera blendList, Transform camaraVirtual)
    {
        camaraVirtual.SetParent(null);
    }

    /// <summary>
    /// Canvia la camara d'una transicio existent.
    /// </summary>
    /// <param name="blendList"></param>
    /// <param name="index"></param>
    /// <param name="camaraVirtual"></param>
    public static void XS_CanviarCamara(this CinemachineBlendListCamera blendList, int index, CinemachineVirtualCameraBase camaraVirtual)
    {
        blendList.m_Instructions[index].m_VirtualCamera = camaraVirtual;
    }
    /// <summary>
    /// Canvia els valors d'una transicio existent.
    /// </summary>
    /// <param name="blendList"></param>
    /// <param name="index"></param>
    /// <param name="camaraVirtual"></param>
    /// <param name="style"></param>
    /// <param name="tempsBlending"></param>
    /// <param name="tempsHold"></param>
    public static void XS_CanviarInstruccio(this CinemachineBlendListCamera blendList, int index, CinemachineVirtualCameraBase camaraVirtual, CinemachineBlendDefinition.Style style = CinemachineBlendDefinition.Style.EaseInOut, float tempsBlending = 1, float tempsHold = 1)
    {
        blendList.m_Instructions[index] = new CinemachineBlendListCamera.Instruction() {
            m_VirtualCamera = camaraVirtual,
            m_Blend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut,tempsBlending),
            m_Hold = tempsHold
        };
    }
}

public static class XS_Mixing
{

    public static void XS_AfegirCamara(this CinemachineMixingCamera mixingCamera, CinemachineVirtualCameraBase camara, float pes = 0)
    {
        camara.enabled = true;
        camara.transform.SetParent(mixingCamera.transform);
        mixingCamera.SetWeight(camara, pes);
    }
    public static void XS_TreureCamara(this CinemachineMixingCamera mixingCamera, CinemachineVirtualCameraBase camara, Transform parent = null)
    {
        mixingCamera.SetWeight(camara, 0);
        camara.transform.SetParent(null);
        camara.enabled = false;
    }

    public static void XS_CanviarPes(this CinemachineMixingCamera mixingCamera, int index, float pes)
    {
        mixingCamera.SetWeight(index, pes);
    }
    public static void XS_CanviarPes(this CinemachineMixingCamera mixingCamera, CinemachineVirtualCameraBase camara, float pes)
    {
        mixingCamera.SetWeight(camara, pes);
    }
    public static float XS_AgafarPes(this CinemachineMixingCamera mixingCamera, int index)
    {
        return mixingCamera.GetWeight(index);
    }
    public static float XS_AgafarPes(this CinemachineMixingCamera mixingCamera, CinemachineVirtualCameraBase camara)
    {
        return mixingCamera.GetWeight(camara);
    }
    public static int XS_CamaresNumero(this CinemachineMixingCamera mixingCamera)
    {
        return mixingCamera.ChildCameras.Length;
    }
    public static CinemachineVirtualCameraBase[] XS_Camares(this CinemachineMixingCamera mixingCamera)
    {
        return mixingCamera.ChildCameras;
    }
}