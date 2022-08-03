using UnityEngine;
using System.Collections;
using Firebase.Database;
using Firebase.Auth;
using System;
using Google;
using System.Threading.Tasks;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Linq;

public class db : MonoBehaviour
{
    DatabaseReference dbRef;
    FirebaseAuth auth;
    FirebaseFirestore dbs;

    [Serializable]
    public class TestData 
    {
        public string someName;
        public int someProperty;
        [Range(0f,100f)]
        public float blablabla;
    }

    public List<TestData> testDatas;
    public List<string> testStings => testDatas.Select(x => x.someName).ToList();

    [Dropdown("testStings")]
    public string testSelectStroka;

    const string email = "secondmail2@mail.com";
    const string password = "password22";

    private void Awake()
    {
        dbs = FirebaseFirestore.DefaultInstance;
    }

    void Start()
    {
        //    dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        //    auth = FirebaseAuth.DefaultInstance;
    }

    public void SaveData(string str, string val = "")
    {
        User user = new User(str, 15, "offline");

        //        dbRef.Child("users").Child("name").SetValueAsync(str);
        string json = JsonUtility.ToJson(user);
        dbRef.Child("users").Child(str).SetRawJsonValueAsync(json);
        Debug.Log("Save to Realtime Database complete");
    }

    public void CreateAcc()
    {
        try
        {
            Debug.Log("CreateAcc");

            auth.CreateUserWithEmailAndPasswordAsync(UILogic.instance.linkIF.text, password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    UILogic.instance.UpdateText("CreateUserWithEmailAndPasswordAsync was canceled.");
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    UILogic.instance.UpdateText("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.FirebaseUser newUser = task.Result;

                string textMessage = $"Firebase user created successfully: {newUser.DisplayName} ({newUser.UserId})";

                UILogic.instance.UpdateText(textMessage);
                Debug.LogFormat(textMessage);
            });
        }
        catch (Exception ex)
        {
            UILogic.instance.UpdateText(ex.Message);
        }
    }

    public void ButtonC()
    {
        StartCoroutine(LoadData(""));
    }

    public IEnumerator LoadData(string str)
    {
        Debug.Log("LoadData");
        var user = FirebaseDatabase.DefaultInstance.RootReference.Child("users").GetValueAsync();

        yield return new WaitUntil(predicate: () => user.IsCompleted);
        Debug.Log("WaitUntil Complete");

        if (user.Exception != null)
        {
            Debug.LogError(user.Exception);
        }
        else if (user.Result == null)
        {
            Debug.Log("Null");
        }
        else
        {
            DataSnapshot snapshot = user.Result;
            Debug.Log(snapshot.Child("age").Value.ToString() + snapshot.Child("name").Value.ToString());
        }
    }

    public void gitExample()
    {
        try
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                // Copy this value from the google-service.json file.
                // oauth_client with type == 3
                WebClientId = "1098604486006-jlj269uh99jlsie5hlb1e1g7spffs1r3.apps.googleusercontent.com"
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
    }

    public IEnumerator SaveFileStore()
    {
        var testDoc = new FBData()
        {
            lastTime = 3214567,
            Name = "abracadabra!",
        };

        var col = dbs.Collection("collect2");// Place of Crash

        var colw = col.GetSnapshotAsync();

        var tsk = col.AddAsync(testDoc);


        yield return new WaitUntil(predicate: () => tsk.IsCompleted);

        Debug.Log("Complete SaveFileStore");
    }

    public void SaveToCloudStore()
    {
        StartCoroutine(SaveFileStore());
    }
}

public class User
{
    public string name;
    public int age;
    public string status;

    public User(string name, int age, string status)
    {
        this.name = name;
        this.age = age;
        this.status = status;
    }
}

[FirestoreData]
public class FBData
{
    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public int lastTime { get; set; }
}