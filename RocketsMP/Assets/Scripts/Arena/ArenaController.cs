using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class ArenaController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _Rocket;
    [SerializeField] private StartGame _startGame;
    public static Action RocketDestroyed;
    public Vector3 spawnPosition;
    public Vector3 spawnPositionStaticA;
    public Vector3 spawnPositionStaticB;
    bool IamPlayerOne;
    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            IamPlayerOne = true;
        }
        else
        {
            IamPlayerOne = false;
        }
    }
    void Start()
    {
        spawnPositionStaticA = Camera.main.ViewportToWorldPoint(new Vector3(0.25f, 0.5f, 20f));
        spawnPositionStaticB = Camera.main.ViewportToWorldPoint(new Vector3(0.75f, 0.5f, 20f));
        SetGravity(ValuesHolder.Instance.GravityPower);
        if (IamPlayerOne)
        {
            spawnPosition = spawnPositionStaticA;
            PhotonNetwork.Instantiate(_Rocket.name, spawnPosition, Quaternion.identity);
        }
        else
        {
            spawnPosition = spawnPositionStaticB;
            SpawnNweRocket();
            _startGame.ShowGetReady();
        }
    }
    private void OnEnable()
    {
        RocketDestroyed += SpawnNweRocket;
        RocketDestroyed += SetScore;
    }
    private void OnDisable()
    {
        RocketDestroyed -= SpawnNweRocket;
        RocketDestroyed -= SetScore;
    }

    private void SpawnNweRocket()
    {
        StartCoroutine(SpawnTimeCo());
    }

    private IEnumerator SpawnTimeCo()
    {
        yield return new WaitForSeconds(3f);
        //Instantiate(_Rocket, spawnPosition, Quaternion.identity);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.Instantiate(_Rocket.name, spawnPosition, Quaternion.identity);
        }
    }
    public void QuitApp()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        Debug.Log("quit");        
    }

    public void SetGravity(float _gravity)
    {
        float gravity = _gravity;
        if (gravity == 0)
        {
            gravity = -3.5f;
        }
        Physics.gravity = new Vector3(0, gravity * 0.5f, 0);
        Debug.Log("gravity is " + gravity);
    }
    private void SetScore()
    {
        if (IamPlayerOne)
        {
            _startGame.SetScore(1,0);
        }
        else
        {
            _startGame.SetScore(0,1);
        }
    }
}
