using UnityEngine;
using Photon.Pun;
public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float DamageAmount = 20f;
    public string BulletTag = "BlueBullet";
    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * speed * Time.deltaTime;     
        Destroy(gameObject,4f);   
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(gameObject.tag == BulletTag && other.tag == "Red")
        {
            other.GetComponentInParent<Player_Movement>().DamagePlayer(DamageAmount);
        }
        Destroy(gameObject);
    }
    private void OnDestroy() {
        PhotonNetwork.Instantiate("Explosion",transform.position,transform.rotation,0);
    }
}
