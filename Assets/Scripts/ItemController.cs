using UnityEngine;
using System;
using System.Linq;
using System.Collections;

public class ItemController : MonoBehaviour
{
    private float dt;

    private float itemInstantiatePercentage;

    public GameObject slowDownItemPrefab;

    public GameObject speedUpItemPrefab;

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

        // 一度アイテムを生成してから1秒間はアイテムを生成しない
        if(dt < 1)
        {
            return;
        }

        // スローダウンアイテム
        var randomX = UnityEngine.Random.Range(-2f, 2f);
        var randomY = UnityEngine.Random.Range(-4f, 4f);

        var isSpawn = !System.Convert.ToBoolean(UnityEngine.Random.Range(0, (int)itemInstantiatePercentage));
        if (isSpawn)
        {
            dt = 0.0f;
            var obj = Instantiate(slowDownItemPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
            StartCoroutine(DestroyMethod(obj));
            return;
        }

        // スピードアップアイテム
        var randomXForSpeed = UnityEngine.Random.Range(-2f, 2f);
        var randomYForSpeed = UnityEngine.Random.Range(-4f, 4f);

        var isSpawnSpeed = !System.Convert.ToBoolean(UnityEngine.Random.Range(0, (int)itemInstantiatePercentage));
        if (isSpawnSpeed)
        {
            dt = 0.0f;
            var obj = Instantiate(speedUpItemPrefab, new Vector3(randomXForSpeed, randomYForSpeed, 0), Quaternion.identity);
            StartCoroutine(DestroyMethod(obj));
            return;
        }
        
        // ミサイル（妨害アイテム）
        var randomXForMissile = UnityEngine.Random.Range(-2f, 2f);

        var isSpawnMissile = !System.Convert.ToBoolean(UnityEngine.Random.Range(0, (int)itemInstantiatePercentage));
        if (isSpawnMissile)
        {
            dt = 0.0f;
            var obj = Instantiate(missilePrefab, new Vector3(randomXForMissile, 5, 0), Quaternion.LookRotation(Vector3.down));
            return;
        }

    }

    private IEnumerator DestroyMethod(GameObject obj)
    {
        for (var i = 0; i < 85; i++)
        {
            obj.GetComponent<Renderer>().material.color -= new Color32(0, 0, 0, 3);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(obj);
    }
}
