using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class StartGame : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _Startgame;
    [SerializeField] private GameObject _PlayerChanged;
    [SerializeField] private ArenaController _ArenaControl;
    [SerializeField] private TMP_Text _playerOneScore;
    [SerializeField] private TMP_Text _playerTwoScore;
    private int ScoreOne;
    private int ScoreTwo;
    
    private void Awake()
    {
        _Startgame.SetActive(false);
        _PlayerChanged.SetActive(false);
        NullScore();
    }
    private void Start()
    {

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Entered");
        photonView.RPC(nameof(SetGravity), RpcTarget.Others, ValuesHolder.Instance.GravityPower);
        ShowGetReady();
        NullScore();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetPosition();
        StartCoroutine(HidePlayerChangedCo());
        NullScore();
        Debug.Log(otherPlayer.NickName + " has left the room.");
    }

    private IEnumerator HideGetReadyCo()
    {
        NullScore();
        yield return new WaitForSeconds(3f);
        NullScore();
        _Startgame.SetActive(false);
    }
    private IEnumerator HidePlayerChangedCo()
    {
        _PlayerChanged.SetActive(true);
        yield return new WaitForSeconds(3f);
        _PlayerChanged.SetActive(false);
    }
    public void ShowGetReady()
    {
        _Startgame.SetActive(true);
        StartCoroutine(HideGetReadyCo());
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    private void SetPosition()
    {
        _ArenaControl.spawnPosition = _ArenaControl.spawnPositionStaticA;
    }
    private void NullScore()
    {
        ScoreOne = 0;
        ScoreTwo = 0;
        _playerOneScore.text = "Player One Score: " + ScoreOne;
        _playerTwoScore.text = "Player Two Score: " + ScoreTwo;
    }
    public void SetScore(int scoreOne, int scoreTwo)
    {
        photonView.RPC(nameof(UpdateScore), RpcTarget.All, scoreOne, scoreTwo);
    }

    [PunRPC]
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
    
    [PunRPC]
    private void UpdateScore(int scoreOne, int scoreTwo)
    {
        ScoreOne += scoreOne;
        ScoreTwo += scoreTwo;
        _playerOneScore.text = "Player One Score: " + ScoreOne;
        _playerTwoScore.text = "Player Two Score: " + ScoreTwo;
    }
}
