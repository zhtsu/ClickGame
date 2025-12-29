using System;
using System.Collections.Generic;
using UnityEngine;

public class UT_UIRoot : MonoBehaviour
{
    [SerializeField] public GameObject BottomLayer;
    [SerializeField] public GameObject MainLayer;
    [SerializeField] public GameObject TopLayer;
    [SerializeField] public GameObject PopupLayer;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public GameObject GetLayerObject(UT_EUILayer Layer)
    {
        switch (Layer)
        {
            case UT_EUILayer.Bottom:    return BottomLayer;
            case UT_EUILayer.Main:      return MainLayer;
            case UT_EUILayer.Top:       return TopLayer;
            case UT_EUILayer.Popup:     return PopupLayer;
            default:                    return null;
        }
    }
}
