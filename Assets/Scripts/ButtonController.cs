using Firebase.Auth;
using Google;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public InputField input;

    private db db;

    private void Start()
    {
        db = GetComponent<db>();
    }

    public void Button()
    {
        //        db.SaveData(input.text, "testValue");
    //    StartCoroutine(db.LoadData(input.text));
        //        db.LoadData(input.text);
    }

/*    public void gitExample()
    {
        try
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                // Copy this value from the google-service.json file.
                // oauth_client with type == 3
                WebClientId = "80220968136-kifm38auro4ld12d0084pj3s1gjpokd4.apps.googleusercontent.com"
            };
            Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

            TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
            signIn.ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    signInCompleted.SetCanceled();
                }
                else if (task.IsFaulted)
                {
                    signInCompleted.SetException(task.Exception);
                }
                else
                {
                    Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                    auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
                    {
                        if (authTask.IsCanceled)
                        {
                            signInCompleted.SetCanceled();
                        }
                        else if (authTask.IsFaulted)
                        {
                            signInCompleted.SetException(authTask.Exception);
                        }
                        else
                        {
                            signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                        }
                    });
                }
            });
        }
        catch (Exception ex)
        {
            UILogic.instance.UpdateText(ex.Message);
        }
    }/**/
}
