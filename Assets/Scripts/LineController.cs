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
        drawLine();
    }

    void drawLine()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPos.z = 0;

            predictionTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            predictionTouchPos.z = 0;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 startPos = predictionTouchPos;
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPos.z = 0;

            if ((endPos - startPos).magnitude > lineLength)
            {
                GameObject obj = Instantiate(predictionLinePrefab, transform.position, transform.rotation) as GameObject;
                obj.transform.position = (startPos + endPos) / 2;
                obj.transform.right = (endPos - startPos).normalized;

                obj.transform.localScale = new Vector3((endPos - startPos).magnitude, lineWidth, lineWidth);

                obj.transform.parent = this.transform;

                predictionTouchPos = endPos;

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            var predictionLines = GameObject.FindGameObjectsWithTag("PredictionLine");
            foreach(var line in predictionLines)
            {
                Destroy(line);
            }

            Vector3 startPos = touchPos;
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPos.z = 0;

            if ((endPos - startPos).magnitude > lineLength)
            {
                GameObject obj = Instantiate(linePrefab, transform.position, transform.rotation);
                obj.transform.position = (startPos + endPos) / 2;
                obj.transform.right = (endPos - startPos).normalized;

                obj.transform.localScale = new Vector3((endPos - startPos).magnitude, lineWidth, lineWidth);

                obj.transform.parent = this.transform;

                touchPos = endPos;

                StartCoroutine(DestroyMethod(obj));
            }
        }

    }

    private IEnumerator DestroyMethod(Object obj)
    {
        yield return new WaitForSeconds(2.0f);

        Destroy(obj);
    }
}
