using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Rendering.SplashScreen.isFinished && Input.GetMouseButtonUp(0))
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, result);
            if(result.Count == 0)
            {
                JumpToGameScene();
            }
        }

        GlobalVariants.screenOrientation = Screen.orientation;
    }

    public void JumpToCreditScene()
    {
        SceneManager.LoadScene("CreditScene");
    }

    public void JumpToGameScene()
    {
        GlobalVariants.screenOrientation = Screen.orientation;

        SceneManager.LoadScene("GameScene");

    }
}
