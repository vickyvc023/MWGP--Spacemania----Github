using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject GameManagerGO;
    public GameObject PlayerBullet;
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    public GameObject Explosion;
    public AudioSource LaserSound;

    public Text LifeUIText;

    const int MaxLife = 3;
    int life;

    public float speed;

    float accelStartY;

    public void Init()
    {
        life = MaxLife;

        LifeUIText.text = life.ToString ();

        transform.position = new Vector2 (0, 0);

        gameObject.SetActive (true);
    }
    // Start is called before the first frame update
    void Start()
    {
        accelStartY = Input.acceleration.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            LaserSound.Play();

            GameObject bullet01 = (GameObject)Instantiate (PlayerBullet);
            bullet01.transform.position = bulletPosition01.transform.position;

            GameObject bullet02 = (GameObject)Instantiate (PlayerBullet);
            bullet02.transform.position = bulletPosition02.transform.position;
        }
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        /* float x = Input.acceleration.x;
         float y = Input.acceleration.y - accelStartY;

         Vector2 direction = new Vector2 (x, y);

         if(direction.sqrMagnitude > 1)
         {
             direction.Normalize();
         }*/

        Vector2 direction = new Vector2 (x, y).normalized;

        Move(direction);
    }

    void Move(Vector2 direction)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        max.x = max.x - 0.225f;
        min.x = min.x + 0.225f;

        max.y = max.y - 0.225f;
        min.y = min.y + 0.225f;

        Vector2 pos = transform.position;

        pos += direction * speed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag"))
        {
            PlayExplosion();

            life--;
            LifeUIText.text = life.ToString();

            if (life == 0)
            {
                GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);

                gameObject.SetActive(false);
            }
        }
    }

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(Explosion);

        explosion.transform.position = transform.position;
    }
}