using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// スタートシーンのコントローラー
/// </summary>
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
        // Unityで最初に表示されるスプラッシュ画面が終了していて、かつマウスの左クリックが離されたら
        if (UnityEngine.Rendering.SplashScreen.isFinished && Input.GetMouseButtonUp(0))
        {
            // マウスの位置を取得
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            // マウスの位置が画面の外にあるかどうかを確認
            List<RaycastResult> result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, result);

            // マウスの位置が画面の外にある場合はゲームシーンに遷移
            if (result.Count == 0)
            {
                JumpToGameScene();
            }
        }

        GlobalVariants.screenOrientation = Screen.orientation;
    }

    /// <summary>
    /// クレジットシーンに遷移する
    /// </summary>
    public void JumpToCreditScene()
    {
        SceneManager.LoadScene(GlobalVariants.SceneNames.CreditScene);
    }

    /// <summary>
    /// ゲームシーンに遷移する
    /// </summary>
    public void JumpToGameScene()
    {
        SceneManager.LoadScene(GlobalVariants.SceneNames.GameScene);
    }
}
