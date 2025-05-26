using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆発コントローラー
/// </summary>
public class ExplosionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
