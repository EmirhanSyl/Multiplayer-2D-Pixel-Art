using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwordAttack : MonoBehaviour
{
    public float maxDamageAmount;
    public float minDamageAmount;

    private BoxCollider2D boxCollider;
    public PhotonView view;

    private void Awake() => boxCollider = GetComponent<BoxCollider2D>();
    private void Start() => view = GetComponentInParent<PhotonView>();

    public void SelectTransform(float direction)
    {
        StartCoroutine(OpenDurationCoroutine());

        switch (direction)
        {
            case 0:
                transform.localPosition = Vector2.down;
                break;
            case 0.33f:
                transform.localPosition = Vector2.up;
                break;
            case 0.66f:
                transform.localPosition = Vector2.left;
                break;
            case 1:
                transform.localPosition = Vector2.right;
                break;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!view.IsMine && PhotonNetwork.IsConnected) return;

    //    if (collision.gameObject.CompareTag("Player") && collision.gameObject != transform.parent.gameObject)
    //    {
    //       collision.gameObject.GetComponent<Health>().PlayerHitted(Random.Range(minDamageAmount, maxDamageAmount));
    //    }
    //}

    IEnumerator OpenDurationCoroutine()
    {
        boxCollider.enabled = true;
        yield return new WaitForSeconds(1f);
        boxCollider.enabled = false;
    }
}
