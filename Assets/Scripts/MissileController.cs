using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ミサイルコントローラー
/// </summary>
public class MissileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ミサイルを画面下に移動
        transform.position += new Vector3(0, -2, 0) * Time.deltaTime;

        // ミサイルが画面の下に出てフェードアウトしたら破棄
        if (transform.position.y <= -10)
        {
            Destroy(this.gameObject);
        }
    }
}
