using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpSpeed;
    public Text scoreText;
    public Text countdownText;
    public Text livesText;
    public Text winText;
    public AudioClip winning;

    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource musicPlayer;
    private int score;
    private int lives;
    private bool endgame;
    // Start is called before the first frame update
    void Start()
    {
        endgame = false;
        score = 0;
        if (SceneManager.GetActiveScene().name == "Stage2")
            score = 4;
        lives = 3;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        musicPlayer = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        SetScore();
        StartCoroutine(StartCountdown(25));
    }

    public IEnumerator StartCountdown(float countdownValue = 25)
    {
        while (countdownValue > 0)
        {
            if (countdownValue < 10)
                countdownText.color = Color.red;
            countdownText.text = countdownValue.ToString();
            yield return new WaitForSeconds(1.0f);
            countdownValue--;
        }
        Lose();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0);
        
        rb.AddForce(movement * speed);
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.eulerAngles = new Vector3 (0.0f, 180.0f, 0.0f);
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            transform.eulerAngles = new Vector3 (0.0f, 0.0f, 0.0f);
        }
        anim.SetFloat("Speed", rb.velocity.magnitude);
    }

    void OnCollisionStay2D(Collision2D collision) 
    {
        if (collision.collider.CompareTag("Ground"))
        {
            anim.SetBool("Grounded", true);
            if (Input.GetKey(KeyCode.UpArrow)) {
                rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            anim.SetBool("Grounded", false);
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Pickup"))
        {
            collider.transform.GetChild(0).gameObject.SetActive(false);
            collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            score++;
            AudioSource pickupAudio = collider.gameObject.GetComponent<AudioSource>();
            pickupAudio.Play();
        }
        else if (collider.gameObject.CompareTag("Enemy"))
        {
            if (!transform.GetChild(0).gameObject.activeSelf) {
                collider.gameObject.SetActive(false);
                lives--;
            }
        }
        else if (collider.gameObject.CompareTag("Invincable"))
        {
            collider.transform.GetChild(0).gameObject.SetActive(false);
            collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        SetScore();
    }

    void SetScore() {
        if (!endgame)
        {
            scoreText.text = "Score: " + score.ToString();
            livesText.text = "Lives: " + lives.ToString();
            if (score >= 4 && SceneManager.GetActiveScene().name == "Stage1") {
                SceneManager.LoadScene("Stage2");
            } else if (score >= 8) {
                winText.text = "YOU WIN!";
                musicPlayer.loop = false;
                musicPlayer.clip = winning;
                musicPlayer.Stop();
                musicPlayer.Play();
                endgame = true;
            } else if (lives <= 0) {
                endgame = true;
                Lose();
            } else {
                winText.text = "";
            }
        }
    }

    void Lose()
    {
        winText.color = Color.red;
        winText.text = "You Lose...";
        Destroy(gameObject);
    }
}
