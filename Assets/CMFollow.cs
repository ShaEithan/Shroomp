using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CMFollow : MonoBehaviour
{
    public string targetName;
    public float orthoSetter;
    private CinemachineVirtualCamera cm;
    // Start is called before the first frame update
    void Start()
    {
        cm = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cm.Follow == null)
            cm.Follow = GameObject.Find(targetName).transform;
        if (cm.Follow != null)
        {
            if (!cm.Follow.name.Contains(targetName))
            {
                cm.Follow = GameObject.Find(targetName).transform;
            }
            if (cm.m_Lens.OrthographicSize != orthoSetter)
                cm.m_Lens.OrthographicSize = orthoSetter;
        }
    }
}
