using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RocketController : MonoBehaviourPunCallbacks
{
    private Rigidbody rb;
    private Vector3 _Force;
    [SerializeField] private float _YForce = 10f;
    [SerializeField] private float _RotationForce = 0.05f;
    private float currentVelocity = 10;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _Force = new Vector3(0, _YForce, 0);
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MyInput();
            RocketOutOfScreen();
        }
        if (PhotonNetwork.PlayerListOthers.Length == 0)
        {

        }
    }

    private void Accelerate()
    {
        rb.AddForce(transform.up * _YForce, ForceMode.Impulse);
    }
    private void OnDestroy()
    {
        if (photonView.IsMine)
        {
            ArenaController.RocketDestroyed?.Invoke();
        }
    }
    private void SlowDown()
    {
        if (rb.velocity.y > 0)
        {
            rb.AddForce(transform.up * -_YForce, ForceMode.Impulse);
        }
    }
    private void Rotate(float force)
    {
        rb.AddTorque(Vector3.forward * force, ForceMode.Impulse);
    }

    private void RocketOutOfScreen()
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(gameObject.transform.position);
        bool onScreen = viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
        if (!onScreen)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.NameToLayer("Ground") == collision.gameObject.layer)
        {
            Destroy(gameObject);
        }
    }
    private void MyInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Accelerate();
        }
        if (Input.GetKey(KeyCode.A))
        {
            Rotate(_RotationForce);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Rotate(-_RotationForce);
        }
        if (Input.GetKey(KeyCode.S))
        {
            SlowDown();
        }
        if (Input.GetKey(KeyCode.F))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Destroy(gameObject);
    }
}
