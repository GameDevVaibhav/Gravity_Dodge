using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;

public class GoogleIntegration : MonoBehaviour
{
    public static GoogleIntegration instance;
    public string GooglePlayToken;
    public string GooglePlayError;

    

    public bool connectedToGooglePlay=false;
   

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if(status == SignInStatus.Success)
        {
            Debug.Log("Login success");
        }
        else
        {
            Debug.Log("Login failed");
        }
    }


    public void ShowLeaderboardUI()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
       
    }

    public void DoLeaderboardPost(int score)
    {

        PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_highscore, (bool success) =>
        {
            if (success)
            {
                NotificationManager.Instance.ShowNotification("Highscore");
            }
            else
            {
                Debug.Log("Score posting failed");
            }
        });

       
    }
}
