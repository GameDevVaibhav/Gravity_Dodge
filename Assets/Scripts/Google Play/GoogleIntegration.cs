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
        if(!connectedToGooglePlay)
        {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }
        else
        {
            NotificationManager.Instance.ShowNotification("Already Connected");
        }
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if(status == SignInStatus.Success)
        {
            Debug.Log("Login success");
            connectedToGooglePlay = true;
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
