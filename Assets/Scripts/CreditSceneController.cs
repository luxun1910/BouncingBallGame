using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルクレジットシーンのコントローラー
/// </summary>
public class CreditSceneController : MonoBehaviour
{
    /// <summary>
    /// タイトルシーンに戻る
    /// </summary>
    public void JumpToTitleScene()
    {
        SceneManager.LoadScene(GlobalVariants.SceneNames.StartScene);
    }

    /// <summary>
    /// プライバシーポリシーを開く
    /// </summary>
    public void JumpToPrivacyPolicy()
    {
        Application.OpenURL("https://luxun1910.github.io/unanimousworks_privacy_policy/bouncing_ball_game.html");
    }
}
