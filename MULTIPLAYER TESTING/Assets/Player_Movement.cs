using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour {

    public Camera myCam;

    Joystick Mjoystick;
    Joystick Rjoystick;

    public Transform RotBody;

    public float Health;

    Vector2 Move;
    public float Speed;
    public float StartTimeBtwShots;
    private float TimeBtwShots;

    public Rigidbody2D rb;

    public GameObject Bullet;
    public Transform ShootPos;
    PhotonView photonView;

    AudioSource audioSource;
    public AudioSource audioSource1;

    public bool isWin;

    Slider HealthSlider;

    private void Start() {
        myCam.backgroundColor = GameObject.FindGameObjectWithTag("PublicCam").GetComponent<Camera>().backgroundColor;
        Mjoystick = GameObject.Find("Move").GetComponent<Joystick>();
        Rjoystick = GameObject.Find("Shoot").GetComponent<Joystick>();
        TimeBtwShots  = StartTimeBtwShots;
        photonView = GetComponent<PhotonView>();
        GameManager.instance.NamePlayer(gameObject);
        GameManager.instance.inputField.gameObject.SetActive(false);
        HealthSlider = GameManager.instance.SliderEnable(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {

        if(!photonView.IsMine)
            return;
        GameObject[] Cams = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject C in Cams)
        {
            if(GetComponentInChildren<Camera>() != C.GetComponent<Camera>())
            {
                C.SetActive(false);
            }
        }

        if(isWin)
        {
            GameManager.instance.Win();
            return;
        }
        
        if(TimeBtwShots <= 0f)
        {
            if(Rjoystick.Horizontal >= 0.5f || Rjoystick.Horizontal <= -0.5f || Rjoystick.Vertical >= 0.5f || Rjoystick.Vertical <= -0.5f){
                photonView.RPC("RPC_shoot",RpcTarget.All);
                TimeBtwShots = StartTimeBtwShots;
            }
        }
        else{
            TimeBtwShots -= Time.deltaTime;
        }

        Move.x = Mjoystick.Horizontal;
        Move.y = Mjoystick.Vertical;

        float Angle = Mathf.Atan2(Rjoystick.Direction.y,Rjoystick.Direction.x) * Mathf.Rad2Deg - 90f;
        if(Angle != -90f)
            RotBody.rotation = Quaternion.Euler(0f,0f,Angle);

        if(Move.x != 0f || Move.y != 0f)
        {
            audioSource1.volume = .3f;
        }
        else{
            audioSource1.volume = 0f;
        }

        rb.MovePosition(rb.position + Move * Speed * Time.deltaTime);
    }

    [PunRPC]
    public void RPC_shoot()
    {
        audioSource.Play();
        Instantiate(Bullet,ShootPos.position,RotBody.rotation);
    }

    public void DamagePlayer(float damage)
    {
        Health -= damage;
        HealthSlider.value = Health;
        if(Health <= 0f && gameObject.tag == "Red"){
            PhotonNetwork.Instantiate("Explosion Red",transform.position,transform.rotation,0);
            GameObject.FindGameObjectWithTag("Blue").GetComponentInParent<Player_Movement>().isWin = true;
            GameManager.instance.Lose();
            Destroy(gameObject);
        }
        else if(Health <= 0f && gameObject.tag == "Blue"){
            PhotonNetwork.Instantiate("Explosion Blue",transform.position,transform.rotation,0);
            GameObject.FindGameObjectWithTag("Red").GetComponentInParent<Player_Movement>().isWin = true;
            GameManager.instance.Lose();
            Destroy(gameObject);
        }
    }
}