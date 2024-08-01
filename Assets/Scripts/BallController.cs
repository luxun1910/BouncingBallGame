using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class BallController : MonoBehaviour
{
    public GameObject particleObject;

    public float speed = 2f;

    private float speedBonus = 1f;

    private float time;

    Rigidbody myRigidbody;

    Transform myTransform;

    int collisionCount;

    int bonusPoint;

    public Text scoreText;

    public static float score { get; set; }

    public AudioSource bouncing;
    public AudioSource getSlowDown;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.velocity = new Vector3(0f, -1 * speed, 0f);
        myTransform = transform;

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        score = ((int)time * (collisionCount + 1)) + bonusPoint;
        scoreText.text = score.ToString();

        Vector3 velocity = myRigidbody.velocity;
        myRigidbody.velocity = velocity.normalized * GetSpeed(speed, time, collisionCount);
    }

    void OnBecameInvisible()
    {
        SceneManager.LoadScene("EndScene");
    }

    void OnCollisionEnter(Collision collision)
    {
        // ライン衝突時
        if (collision.gameObject.CompareTag("Line"))
        {
            Instantiate(particleObject, this.transform.position, Quaternion.identity);

            Destroy(collision.gameObject);

            collisionCount++;
            bouncing.Play();

            Vector3 playerPos = collision.transform.position;
            Vector3 ballPos = myTransform.position;
            Vector3 direction = (ballPos - playerPos).normalized;
            float speed = myRigidbody.velocity.magnitude;
            myRigidbody.velocity = direction * speed;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // スローダウンアイテム
        if (collision.gameObject.CompareTag("SlowDownItem"))
        {
            speedBonus *= 0.9f;
            bonusPoint += 10;
            Destroy(collision.gameObject);
            getSlowDown.Play();
        }

        // スピードアップアイテム
        if (collision.gameObject.CompareTag("SpeedUpItem"))
        {
            speedBonus *= 1.1f;
            bonusPoint += 600;
            Destroy(collision.gameObject);
            getSlowDown.Play();
        }
    }

    private float GetSpeed(float speed, float time, int collisionCount)
    {
        collisionCount++;
        var x = (time / 60f) * 0.2f * speed * (collisionCount / 3f) * speedBonus;

        return Math.Max(1.5f, x);
    }
}
