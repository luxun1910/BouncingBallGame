using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
using System;

public class EndSceneController : MonoBehaviour
{
    public Text finalScoreText;

    public InterstitialAd InterstitialAd;

    // Start is called before the first frame update
    void Start()
    {
        finalScoreText.text = BallController.score.ToString();

        Screen.orientation = ScreenOrientation.AutoRotation;

        // Game Center�p����
#if UNITY_IOS
        //iOS�݂̂Ŏ��s�������������L�q
#endif

        // Google Play Game�p����
#if UNITY_ANDROID
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
#endif

    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("success");

            // Continue with Play Games Services
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_bouncing_ball_game_ranking);
            Social.ShowLeaderboardUI();

            Social.ReportScore((int)BallController.score, GPGSIds.leaderboard_bouncing_ball_game_ranking, (bool success) => {
                // handle success or failure
            });
        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
            Debug.Log("failure");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GlobalVariants.screenOrientation = Screen.orientation;
    }

    public void ContinuGame()
    {
        InterstitialAd.SceneToJump = "GameScene";
        InterstitialAd.LoadAd();
        InterstitialAd.ShowAd();
    }

    public void EndGame()
    {
        InterstitialAd.SceneToJump = "StartScene";
        InterstitialAd.LoadAd();
        InterstitialAd.ShowAd();
        /*
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        */
    }
}
