using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Photon.Pun;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _MenuScreen;
    [SerializeField] private GameObject _SettingsScreen;
    [SerializeField] private GameObject _BackButton;
    [SerializeField] private GameObject _LoadingScreen;
    [SerializeField] private string RoomName = "Room";
    public enum PhysicsSettings
    {
        PlayableGravity,
        WeakerGravity,
        NormalGravity,
    }
    public PhysicsSettings Physsettings;

    public TMP_Dropdown dropdown;
    public List<string> options;
    public TMP_InputField InputField;
    void Start()
    {
        _LoadingScreen.SetActive(true);
        _MenuScreen.SetActive(false);
        _SettingsScreen.SetActive(false);
        _BackButton.SetActive(false);

        dropdown.ClearOptions();
        options = new List<string>();
        foreach (PhysicsSettings value in Enum.GetValues(typeof(PhysicsSettings)))
        {
            options.Add(value.ToString());
        }
        dropdown.AddOptions(options);
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void PlayButton()
    {
        ToGame();
    }
    public void SettingSButton()
    {
        _MenuScreen.SetActive(false);
        _SettingsScreen.SetActive(true);
        _BackButton.SetActive(true);
    }
    public void BackToMenuButton()
    {
        _MenuScreen.SetActive(true);
        _SettingsScreen.SetActive(false);
        _BackButton.SetActive(false);
    }
    public void SaveButton()
    {
        SetGravity();
        BackToMenuButton();
    }
    private void OnDropdownValueChanged(int value)
    {
        Physsettings = (PhysicsSettings)value;
    }
    private void SetGravity()
    {
        switch (Physsettings)
        {
            case PhysicsSettings.NormalGravity:
                ValuesHolder.Instance.GravityPower = -9.81f;
                break;
            case PhysicsSettings.WeakerGravity:
                ValuesHolder.Instance.GravityPower = -6f;
                break;
            case PhysicsSettings.PlayableGravity:
                ValuesHolder.Instance.GravityPower = -3.5f;
                break;
        }
        ValuesHolder.Instance.InputText = InputField.text;
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        _LoadingScreen.SetActive(false);
        _MenuScreen.SetActive(true);
    }

    private void ToGame()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
        _MenuScreen.SetActive(false);
        _SettingsScreen.SetActive(false);
        _BackButton.SetActive(false);
        _LoadingScreen.SetActive(true);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("CreateRoomFailed");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("JoinFailed");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("JoinRandomFailed");
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Created");
        SceneManager.LoadScene(1);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Connected");
        SceneManager.LoadScene(1);
    }
}
