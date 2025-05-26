using UnityEngine;
using System.Linq;
using System.Collections;

/// <summary>
/// 各種アイテムコントローラー
/// </summary>
public class ItemController : MonoBehaviour
{
    /// <summary>
    /// アイテム生成のクールダウン時間
    /// </summary>
    private const float ITEM_SPAWN_COOLDOWN = 1.0f;

    /// <summary>
    /// 画面内に存在できるアイテムの最大数
    /// </summary>
    private const int MAX_ITEMS = 2;

    /// <summary>
    /// アイテム生成のX座標の最小値
    /// </summary>
    private const float X_RANGE_MIN = -2f;

    /// <summary>
    /// アイテム生成のX座標の最大値
    /// </summary>
    private const float X_RANGE_MAX = 2f;

    /// <summary>
    /// アイテム生成のY座標の最小値
    /// </summary>
    private const float Y_RANGE_MIN = -4f;

    /// <summary>
    /// アイテム生成のY座標の最大値
    /// </summary>
    private const float Y_RANGE_MAX = 4f;

    /// <summary>
    /// ミサイル生成のY座標
    /// </summary>
    private const float MISSILE_SPAWN_Y = 5f;

    /// <summary>
    /// アイテムのフェードアニメーションのアルファ値
    /// </summary>
    private const byte FADE_AMOUNT = 3;

    /// <summary>
    /// アイテムのフェードアニメーションの待機時間
    /// </summary>
    private const float FADE_WAIT = 0.1f;

    /// <summary>
    /// アイテムのフェードアニメーションのステップ数
    /// </summary>
    private const int FADE_STEPS = 85;

    /// <summary>
    /// アイテム生成の時間
    /// </summary>
    private float dt;

    /// <summary>
    /// アイテム生成確率
    /// </summary>
    private float itemInstantiatePercentage;

    /// <summary>
    /// スローダウンアイテムのプレハブ
    /// </summary>
    public GameObject slowDownItemPrefab;

    /// <summary>
    /// スピードアップアイテムのプレハブ
    /// </summary>
    public GameObject speedUpItemPrefab;

    /// <summary>
    /// ミサイルのプレハブ
    /// </summary>
    public GameObject missilePrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        itemInstantiatePercentage = Time.time * 8;
        //Debug.Log(itemInstantiatePercentage);

        // 画面内にアイテムが最大数以上あったら生成しない
        if (GameObject.FindGameObjectsWithTag(GlobalVariants.Tags.SlowDownItem).Count() + GameObject.FindGameObjectsWithTag(GlobalVariants.Tags.SpeedUpItem).Count() >= MAX_ITEMS)
        {
            return;
        }

        dt += Time.deltaTime;

        // クールダウン時間が経過していなければ処理しない
        if (dt < ITEM_SPAWN_COOLDOWN)
        {
            return;
        }

        // アイテム生成を試行
        TrySpawnItem();
    }

    /// <summary>
    /// アイテム生成を試行するメソッド
    /// </summary>
    private void TrySpawnItem()
    {
        // スローダウンアイテムの生成試行
        if (TrySpawnRandomItem(slowDownItemPrefab, X_RANGE_MIN, X_RANGE_MAX, Y_RANGE_MIN, Y_RANGE_MAX))
        {
            return;
        }

        // スピードアップアイテムの生成試行
        if (TrySpawnRandomItem(speedUpItemPrefab, X_RANGE_MIN, X_RANGE_MAX, Y_RANGE_MIN, Y_RANGE_MAX))
        {
            return;
        }

        // ミサイルの生成試行
        TrySpawnMissile();
    }

    /// <summary>
    /// ランダムなアイテム生成を試行するメソッド
    /// </summary>
    /// <param name="prefab">生成するアイテムのプレハブ</param>
    /// <param name="xMin">生成位置のX座標の最小値</param>
    /// <param name="xMax">生成位置のX座標の最大値</param>
    /// <param name="yMin">生成位置のY座標の最小値</param>
    private bool TrySpawnRandomItem(GameObject prefab, float xMin, float xMax, float yMin, float yMax)
    {
        float randomX = UnityEngine.Random.Range(xMin, xMax);
        float randomY = UnityEngine.Random.Range(yMin, yMax);

        bool shouldSpawn = !System.Convert.ToBoolean(UnityEngine.Random.Range(0, (int)itemInstantiatePercentage));
        if (shouldSpawn)
        {
            dt = 0.0f;
            GameObject obj = Instantiate(prefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
            StartCoroutine(DestroyMethod(obj));
            return true;
        }
        return false;
    }

    /// <summary>
    /// ミサイル生成を試行するメソッド
    /// </summary>
    private bool TrySpawnMissile()
    {
        float randomX = UnityEngine.Random.Range(X_RANGE_MIN, X_RANGE_MAX);

        bool shouldSpawn = !System.Convert.ToBoolean(UnityEngine.Random.Range(0, (int)itemInstantiatePercentage));
        if (shouldSpawn)
        {
            dt = 0.0f;
            Instantiate(missilePrefab, new Vector3(randomX, MISSILE_SPAWN_Y, 0), Quaternion.LookRotation(Vector3.down));
            return true;
        }
        return false;
    }

    /// <summary>
    /// アイテム消滅のコルーチン
    /// </summary>
    /// <param name="obj">消滅するアイテムのオブジェクト</param>
    private IEnumerator DestroyMethod(GameObject obj)
    {
        for (var i = 0; i < FADE_STEPS; i++)
        {
            obj.GetComponent<Renderer>().material.color -= new Color32(0, 0, 0, FADE_AMOUNT);
            yield return new WaitForSeconds(FADE_WAIT);
        }

        Destroy(obj);
    }
}
