using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ValuesHolder : MonoBehaviourPunCallbacks
{
    public float GravityPower;
    public string InputText;
    public static ValuesHolder Instance { get; set; }
    private void Awake()
    {
        Instance = this; 
    }
}

