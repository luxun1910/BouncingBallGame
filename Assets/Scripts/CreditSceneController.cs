using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditSceneController : MonoBehaviour
{
    public void JumpToTitleScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void JumpToPrivacyPolicy()
    {
        Application.OpenURL("https://luxun1910.github.io/unanimousworks_privacy_policy/bouncing_ball_game.html");
    }
}
