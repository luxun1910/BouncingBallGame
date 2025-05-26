using UnityEngine;

/// <summary>
/// 爆発コントローラー
/// </summary>
public class ExplosionController : MonoBehaviour
{
    /// <summary>
    /// 爆発オブジェクトの寿命
    /// </summary>
    private const float EXPLOSION_LIFETIME = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, EXPLOSION_LIFETIME);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
