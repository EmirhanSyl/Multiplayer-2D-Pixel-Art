using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerMovement : MonoBehaviourPun, IPunObservable
{
    protected float RemoteLookX;
    protected float RemoteLookY;

    protected Mevement movement;
    protected Vector3 RemotePlayerPosition;

    private void Awake()
    {
        movement = GetComponent<Mevement>();
    }

    public void Update()
    {
        if (photonView.IsMine)
            return;

        var LagDistance = RemotePlayerPosition - transform.position;

        //High distance => sync is to much off => send to position
        if (LagDistance.magnitude > 5f)
        {
            transform.position = RemotePlayerPosition;
            LagDistance = Vector2.zero;
        }

        //ignore the y distance
        LagDistance.z = 0;

        if (LagDistance.magnitude < 0.11f)
        {
            //Player is nearly at the point
            movement.moveVector.x = 0;
            movement.moveVector.y = 0;
        }
        else
        {
            //Player has to go to the point
            movement.moveVector.x = LagDistance.normalized.x;
            movement.moveVector.y = LagDistance.normalized.y;
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(movement.moveVector.x);
            stream.SendNext(movement.moveVector.y);
        }
        else
        {
            RemotePlayerPosition = (Vector3)stream.ReceiveNext();
            RemoteLookX = (float)stream.ReceiveNext();
            RemoteLookY = (float)stream.ReceiveNext();

        }
    }
}
