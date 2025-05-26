using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// ボールのコントローラー
/// </summary>
public class BallController : MonoBehaviour
{
    /// <summary>
    /// ボールが衝突した時のパーティクル
    /// </summary>
    public GameObject particleObject;

    /// <summary>
    /// ボールの速度
    /// </summary>
    public float speed = 2f;

    private float speedBonus = 1f;

    /// <summary>
    /// ゲーム開始経過時間
    /// </summary>
    private float time;

    Rigidbody myRigidbody;

    Transform myTransform;

    int collisionCount;

    int bonusPoint;

    /// <summary>
    /// スコアテキスト
    /// </summary>
    public Text scoreText;

    /// <summary>
    /// スコア
    /// </summary>
    public static float score { get; set; }

    /// <summary>
    /// ボールが衝突した時の音
    /// </summary>
    public AudioSource bouncing;

    /// <summary>
    /// スローダウンアイテムを取得した時の音
    /// </summary>
    public AudioSource getSlowDown;

    /// <summary>
    /// スピードアップアイテムを取得した時の音
    /// </summary>
    public AudioSource getSpeedUp;

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

    /// <summary>
    /// ボールが画面外に出た時
    /// </summary>
    void OnBecameInvisible()
    {
        SceneManager.LoadScene(GlobalVariants.SceneNames.EndScene);
    }

    /// <summary>
    /// ボールが衝突した時
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        // ライン衝突時
        if (collision.gameObject.CompareTag(GlobalVariants.Tags.Line))
        {
            // 衝突時のパーティクルを生成
            Instantiate(particleObject, this.transform.position, Quaternion.identity);

            // 衝突したラインを破棄
            Destroy(collision.gameObject);

            // 衝突回数を加算
            collisionCount++;

            // 衝突音を再生
            bouncing.Play();

            Vector3 playerPos = collision.transform.position;
            Vector3 ballPos = myTransform.position;
            Vector3 direction = (ballPos - playerPos).normalized;
            float speed = myRigidbody.velocity.magnitude;

            // ボールの速度を更新
            myRigidbody.velocity = direction * speed;
        }
    }

    /// <summary>
    /// ボールがアイテムに接触した時
    /// </summary>
    private void OnTriggerEnter(Collider collision)
    {
        // スローダウンアイテム
        if (collision.gameObject.CompareTag(GlobalVariants.Tags.SlowDownItem))
        {
            speedBonus *= 0.9f;
            bonusPoint += 10;

            // 取得したアイテムを破棄
            Destroy(collision.gameObject);

            // スローダウン音を再生
            getSlowDown.Play();
        }

        // スピードアップアイテム
        if (collision.gameObject.CompareTag(GlobalVariants.Tags.SpeedUpItem))
        {
            speedBonus *= 1.1f;
            bonusPoint += 600;

            // 取得したアイテムを破棄
            Destroy(collision.gameObject);

            // スピードアップ音を再生
            getSpeedUp.Play();
        }
    }

    private float GetSpeed(float speed, float time, int collisionCount)
    {
        collisionCount++;
        var x = (time / 60f) * 0.2f * speed * (collisionCount / 3f) * speedBonus;

        return Math.Max(1.5f, x);
    }
}
