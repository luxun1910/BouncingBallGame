using System.Collections;
using UnityEngine;

/// <summary>
/// ラインコントローラー
/// </summary>
public class LineController : MonoBehaviour
{
    /// <summary>
    /// ラインのプレハブ
    /// </summary>
    public GameObject linePrefab;

    /// <summary>
    /// 予測ラインのプレハブ
    /// </summary>
    public GameObject predictionLinePrefab;

    /// <summary>
    /// ラインの長さ
    /// </summary>
    public float lineLength = 0.2f;

    /// <summary>
    /// ラインの幅
    /// </summary>
    public float lineWidth = 0.1f;

    /// <summary>
    /// ラインの破棄遅延時間
    /// </summary>
    private const float LINE_DESTROY_DELAY = 2.0f;

    /// <summary>
    /// 予測タッチ位置
    /// </summary>
    private Vector3 predictionTouchPos;

    /// <summary>
    /// タッチ位置
    /// </summary>
    private Vector3 touchPos;

    void Start()
    {

    }

    void Update()
    {
        HandleLineDrawing();
    }

    /// <summary>
    /// ライン描画を処理する
    /// </summary>
    private void HandleLineDrawing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouchStart();
        }

        if (Input.GetMouseButton(0))
        {
            HandleTouchHold();
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleTouchEnd();
        }
    }

    /// <summary>
    /// タッチ開始時の処理
    /// </summary>
    private void HandleTouchStart()
    {
        touchPos = GetWorldTouchPosition();
        predictionTouchPos = GetWorldTouchPosition();
    }

    /// <summary>
    /// タッチ中（ドラッグ中）の処理
    /// </summary>
    private void HandleTouchHold()
    {
        Vector3 startPos = predictionTouchPos;
        Vector3 endPos = GetWorldTouchPosition();

        if (IsLineLongEnough(startPos, endPos))
        {
            CreatePredictionLine(startPos, endPos);
            predictionTouchPos = endPos;
        }
    }

    /// <summary>
    /// タッチ終了時の処理
    /// </summary>
    private void HandleTouchEnd()
    {
        ClearPredictionLines();

        Vector3 startPos = touchPos;
        Vector3 endPos = GetWorldTouchPosition();

        if (IsLineLongEnough(startPos, endPos))
        {
            GameObject line = CreateLine(startPos, endPos);
            touchPos = endPos;
            StartCoroutine(DestroyWithDelay(line));
        }
    }

    /// <summary>
    /// 予測線を作成
    /// </summary>
    /// <param name="start">開始位置</param>
    /// <param name="end">終了位置</param>
    /// <returns>作成した予測線のオブジェクト</returns>
    private GameObject CreatePredictionLine(Vector3 start, Vector3 end)
    {
        GameObject obj = Instantiate(predictionLinePrefab, transform.position, transform.rotation);
        ConfigureLine(obj, start, end);
        obj.transform.parent = this.transform;
        return obj;
    }

    /// <summary>
    /// 実際の線を作成
    /// </summary>
    /// <param name="start">開始位置</param>
    /// <param name="end">終了位置</param>
    /// <returns>作成した実際の線のオブジェクト</returns>
    private GameObject CreateLine(Vector3 start, Vector3 end)
    {
        GameObject obj = Instantiate(linePrefab, transform.position, transform.rotation);
        ConfigureLine(obj, start, end);
        obj.transform.parent = this.transform;
        return obj;
    }

    /// <summary>
    /// 線のプロパティ設定
    /// </summary>
    /// <param name="lineObject">線のオブジェクト</param>
    /// <param name="start">開始位置</param>
    /// <param name="end">終了位置</param>
    private void ConfigureLine(GameObject lineObject, Vector3 start, Vector3 end)
    {
        lineObject.transform.position = (start + end) / 2;
        lineObject.transform.right = (end - start).normalized;
        lineObject.transform.localScale = new Vector3((end - start).magnitude, lineWidth, lineWidth);
    }

    /// <summary>
    /// タッチ位置をワールド座標に変換
    /// </summary>
    /// <returns>ワールド座標に変換されたタッチ位置</returns>
    private Vector3 GetWorldTouchPosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    /// <summary>
    /// 予測線をクリア
    /// </summary>
    private void ClearPredictionLines()
    {
        var predictionLines = GameObject.FindGameObjectsWithTag(GlobalVariants.Tags.PredictionLine);
        foreach (var line in predictionLines)
        {
            Destroy(line);
        }
    }

    /// <summary>
    /// 線の長さが十分かチェック
    /// </summary>
    /// <param name="start">開始位置</param>
    /// <param name="end">終了位置</param>
    /// <returns>線の長さが十分かどうか</returns>
    private bool IsLineLongEnough(Vector3 start, Vector3 end)
    {
        return (end - start).magnitude > lineLength;
    }

    /// <summary>
    /// 遅延破棄用コルーチン
    /// </summary>
    /// <param name="obj">破棄するオブジェクト</param>
    /// <returns>破棄用コルーチン</returns>
    private IEnumerator DestroyWithDelay(Object obj)
    {
        yield return new WaitForSeconds(LINE_DESTROY_DELAY);
        Destroy(obj);
    }
}
