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

    /// <summary>
    /// ボールの最低速度
    /// </summary>
    private const float MIN_SPEED = 1.5f;

    /// <summary>
    /// 時間係数
    /// </summary>
    private const float TIME_FACTOR = 0.2f;

    /// <summary>
    /// 衝突回数係数
    /// </summary>
    private const float COLLISION_FACTOR = 1f / 3f;

    /// <summary>
    /// ボールスピードボーナス
    /// </summary>
    private float speedBonus = 1f;

    /// <summary>
    /// ゲーム開始経過時間
    /// </summary>
    private float time;

    /// <summary>
    /// ボールのRigidbody
    /// </summary>
    private Rigidbody myRigidbody;

    /// <summary>
    /// ボールのTransform
    /// </summary>
    private Transform myTransform;

    /// <summary>
    /// ボールとラインの衝突回数
    /// </summary>
    private int collisionCount;

    /// <summary>
    /// ボーナスポイント
    /// </summary>
    private int bonusPoint;

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

            // 衝突したラインの位置
            Vector3 linePos = collision.transform.position;

            // ボールの位置
            Vector3 ballPos = myTransform.position;

            // ラインの位置から見たボールの方向
            Vector3 direction = (ballPos - linePos).normalized;

            // 現在のボールの速さ
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

    /// <summary>
    /// ボールの速度を計算する
    /// </summary>
    /// <param name="baseSpeed">ボールの基本速度</param>
    /// <param name="elapsedTime">ゲーム開始経過時間</param>
    /// <param name="collisionCount">ボールとラインの衝突回数</param>
    private float GetSpeed(float baseSpeed, float elapsedTime, int collisionCount)
    {
        // 衝突回数は最低1としてカウント
        int effectiveCollisionCount = collisionCount + 1;

        // 時間経過、衝突回数、スピードボーナスを考慮した速度計算
        float calculatedSpeed = (elapsedTime / 60f) * TIME_FACTOR * baseSpeed * (effectiveCollisionCount * COLLISION_FACTOR) * speedBonus;

        // 最低速度を下回らないようにする
        return Math.Max(MIN_SPEED, calculatedSpeed);
    }
}
