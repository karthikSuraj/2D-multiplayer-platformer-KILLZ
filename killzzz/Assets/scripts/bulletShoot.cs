
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bulletShoot : MonoBehaviourPunCallbacks
{
    public bool moveDrrict = false; //false right, true left
    private SpriteRenderer sr;
    public float MoveSpeed;
    private float destroyTime=2f;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(DestroyByTime());
     
    }
    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(destroyTime);
        this.GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void ChangeDirection_left()
    {
        moveDrrict = true;
    }

    [PunRPC]
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        sr.flipX = moveDrrict;
        if (!moveDrrict)
            transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
        else
            transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime*-1);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
            return;
        PhotonView target = collision.gameObject.GetComponent<PhotonView>();
        if (  (target != null && (!target.IsMine || target.IsRoomView)) || (collision.CompareTag("enemy")))
        {
            this.GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.AllBuffered);
        }
    }
}
