using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

/// <summary>
/// ゲームオーバーシーンのコントローラー
/// </summary>
public class EndSceneController : MonoBehaviour
{
    /// <summary>
    /// 最終スコアテキスト
    /// </summary>
    public Text finalScoreText;

    /// <summary>
    /// インタースティシャル広告
    /// </summary>
    public InterstitialAd InterstitialAd;

    // Start is called before the first frame update
    void Start()
    {
        finalScoreText.text = BallController.score.ToString();

        // iOSならGame Centerを使用する
#if UNITY_IOS
        //iOS�݂̂Ŏ��s�������������L�q
#endif

        // Google Play Game�p����
#if UNITY_ANDROID
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
#endif

    }

    /// <summary>
    /// 認証を処理する
    /// </summary>
    /// <param name="status">認証ステータス</param>
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

    }

    /// <summary>
    /// ゲームを続ける
    /// </summary>
    public void ContinuGame()
    {
        InterstitialAd.SceneToJump = GlobalVariants.SceneNames.GameScene;
        InterstitialAd.LoadAd();
        InterstitialAd.ShowAd();
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void EndGame()
    {
        InterstitialAd.SceneToJump = GlobalVariants.SceneNames.StartScene;
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
