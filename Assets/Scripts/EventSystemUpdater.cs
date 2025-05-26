using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemUpdater : MonoBehaviour
{
    private float t;

    private const float INVALIDATE_T = 0.33f;

    void Awake()
    {
        t = 0.0f;
        GetComponent<EventSystem>().enabled = false;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        t = t + Time.deltaTime;
        if (t >= INVALIDATE_T)
        {
            if (GetComponent<EventSystem>().enabled == false)
            {
                GetComponent<EventSystem>().enabled = true;
            }
        }
    }
}
