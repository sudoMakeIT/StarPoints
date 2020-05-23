using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class CommentHandler : MonoBehaviour
{

    ///change to list of comments later
    public List<Comment> commentListDB;
    public List<Comment> userCommentListDB;

    public int commIndex = 0;
    public string favToDel = "";
    private string logText = "";
    public Text comment;
    public Text commentNew;
    public Text constName;
    public Text rating;
    public Text userComment;
    private string commentText = "";
    private string commentNewText = "";
    private string commentUserText = "";
    private string constNameText = "";
    private string ratingText = "";

    private string autoUser;
    public UserValue userValue;
    public GameObject likeBtn;
    public GameObject dislikeBtn;

    private bool putlike;

    const int kMaxLogSize = 16382;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    ///aux
    Comment favCoom;

    void Start()
    {
        commentListDB = null;
        autoUser = userValue.userName;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
                LoadComment();
            }
            else
            {
                Debug.LogError(
                "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }


    private void Update()
    {
        if (!(commentListDB == null))
        {
            if (!commentListDB[commIndex].hasLike)
            {
                likeBtn.SetActive(true);
                dislikeBtn.SetActive(false);
            }
            else
            {
                likeBtn.SetActive(false);
                dislikeBtn.SetActive(true);
            }
        }

    }

    // Initialize the Firebase database:
    protected virtual void InitializeFirebase()
    {
        DebugLog("Setting up Firebase Database");

        FirebaseApp app = FirebaseApp.DefaultInstance;
        // NOTE: You'll need to replace this url with your Firebase App's database
        // path in order for the database connection to work correctly in editor.
        app.SetEditorDatabaseUrl("https://starpoints-v1.firebaseio.com/");
        if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
    }

    // Output text to the debug log text field, as well as the console.
    public void DebugLog(string s)
    {
        Debug.Log(s);
        logText += s + "\n";

        while (logText.Length > kMaxLogSize)
        {
            int index = logText.IndexOf("\n");
            logText = logText.Substring(index + 1);
        }
    }

    //Save Comment //TODO check if logged in
    public void AddComment()
    {
        commentNewText = commentNew.text;
        print(commentNewText);
        //TODO: ir buscar automaticamente o user logado
        commentUserText = "devBruno";
        constNameText = "Ursa";
        ratingText = "0";

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Comments");

        DebugLog("Running Transaction... add commets");
        // Use a transaction to ensure that we do not encounter issues with
        // simultaneous updates that otherwise might create more than MaxScores top scores.
        reference.RunTransaction(AddScoreTransaction)
        .ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                DebugLog(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                DebugLog("Transaction complete. add comments");
            }
        });
    }

    TransactionResult AddScoreTransaction(MutableData mutableData)
    {
        List<object> commentList = mutableData.Value as List<object>;

        if (commentList == null)
        {
            DebugLog("nodata -- adding");
            commentList = new List<object>();
        }

        foreach (var x in commentList)
        {
            DebugLog(x.ToString());
        }
        // Now we add the new comment
        // Comment newComment = new Comment(constNameText, commentUserText, commentNewText, ratingText);
        // string jsonData = JsonUtility.ToJson(newComment);


        Dictionary<string, object> newComment = new Dictionary<string, object>();
        newComment["Const"] = constNameText;
        newComment["User"] = commentUserText;
        newComment["Text"] = commentNewText;
        newComment["Likes"] = ratingText;

        commentList.Add(newComment);

        // You must set the Value to indicate data at that location has changed.
        mutableData.Value = commentList;
        //return and log success
        return TransactionResult.Success(mutableData);
    }

    //load comments //TODO : check if fav & escolher a const
    public void LoadComment()
    {
        commIndex = 0;
        Debug.Log("Função loadComennts");
        string constSearch = "Ursa";
        bool hasComment = false;
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Comments");
        reference.ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
        {
            if (e2.DatabaseError != null)
            {
                Debug.LogError(e2.DatabaseError.Message);
                return;
            }
            commentListDB = new List<Comment>();
            if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
            {

                foreach (var childSnapshot in e2.Snapshot.Children)
                {
                    if (childSnapshot.Child("Text") == null
                        || childSnapshot.Child("Text").Value == null)
                    {
                        Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                        break;
                    }
                    else
                    {
                        Debug.Log("Comments entrys : " +
                        childSnapshot.Child("Text").Value.ToString() + " - " +
                        childSnapshot.Child("User").Value.ToString() + " - " +
                        childSnapshot.Key.ToString()
                        );
                        if (childSnapshot.Child("Const").Value.ToString().Equals(constSearch))
                        {
                            commentListDB.Add(new Comment(
                                childSnapshot.Child("Const").Value.ToString(),
                                childSnapshot.Child("User").Value.ToString(),
                                childSnapshot.Child("Text").Value.ToString(),
                                childSnapshot.Child("Likes").Value.ToString(),
                                Convert.ToInt32(childSnapshot.Key).ToString(),
                                false));
                            hasComment = true;
                        }
                    }
                }
                ///Change screen
                if (hasComment.Equals(true))
                    check4like();
                updateText();
            }
        };


    }

    public void updateText()
    {
        comment.text = commentListDB[commIndex].Text;
        constName.text = commentListDB[commIndex].Const;
        rating.text = commentListDB[commIndex].Likes;
        userComment.text = commentListDB[commIndex].User;

    }

    //TODO mensagem de erro
    public void nextComment()
    {
        commIndex++;
        if (commIndex < commentListDB.Count)
        {
            check4like();
            updateText();
        }
        else
        {
            commIndex--;
        }
    }

    //TODO mensagem de erro
    public void prevComment()
    {
        commIndex--;
        if (commIndex >= 0)
        {
            check4like();
            updateText();
        }
        else
        {
            commIndex++;
        }
    }

    public void check4like()
    {
        //Ir buscar user autmatico
        string coomFavId = commentListDB[commIndex].Id;
        Debug.Log("comment index for check is " + commentListDB[commIndex].ToString());
        FirebaseDatabase.DefaultInstance
        .GetReference("Likes").OrderByChild("User").EqualTo("devBruno").OrderByChild("Comment").EqualTo(coomFavId)
        .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
        {
            if (e2.DatabaseError != null)
            {
                Debug.LogError(e2.DatabaseError.Message);
                return;
            }
            Debug.Log("Loading likes. check for likes iddex is " + coomFavId.ToString() + " chill count " + e2.Snapshot.ChildrenCount.ToString());
            if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
            {
                //btn de dislike
                foreach (var childSnap in e2.Snapshot.Children)
                {
                    favToDel = childSnap.Key.ToString();
                }

                DebugLog("has like dislike btn" + likeBtn.ToString());
                putlike = false;
                commentListDB[commIndex].hasLike = !putlike;
            }
            else
            {
                //btn de like
                DebugLog("like btn");
                putlike = true;
                commentListDB[commIndex].hasLike = !putlike;
            }
        };
    }


    public void AddFav()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Likes");

        DebugLog("Running Transaction... add fav");
        // Use a transaction to ensure that we do not encounter issues with
        // simultaneous updates that otherwise might create more than MaxScores top scores.
        reference.RunTransaction(AddFavTransaction)
        .ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                DebugLog(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                DebugLog("Transaction complete. add fav");
                putlike = false;
                commentListDB[commIndex].hasLike = !putlike;
                int newLikes = Convert.ToInt32(commentListDB[commIndex].Likes) + 1;
                updateLikes(newLikes);
            }
        });
    }

    TransactionResult AddFavTransaction(MutableData mutableData)
    {
        List<object> favList = mutableData.Value as List<object>;

        //TODO: ir buscar automaticamente o user logado
        string favUserText = "devBruno";

        if (favList == null)
        {
            DebugLog("nodata -- adding fav");
            favList = new List<object>();
        }


        Dictionary<string, object> newfav = new Dictionary<string, object>();
        newfav["User"] = favUserText;
        newfav["Comment"] = commentListDB[commIndex].Id;

        DebugLog("index: " + commIndex.ToString() + "add fav on comments n " + newfav["Comment"].ToString());

        favList.Add(newfav);

        // You must set the Value to indicate data at that location has changed.
        mutableData.Value = favList;
        //return and log success
        return TransactionResult.Success(mutableData);
    }

    public void unlike()
    {
        DebugLog("remove like from LIKES " + favToDel);
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Likes");
        reference.Child(favToDel).SetValueAsync(null)
        .ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                DebugLog(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                DebugLog("Transaction complete. unfaving");
                putlike = true;
                commentListDB[commIndex].hasLike = !putlike;
                //update no of like
                int newLikes = Convert.ToInt32(commentListDB[commIndex].Likes) - 1;
                updateLikes(newLikes);
            }
        });
    }

    public void updateLikes(int newLikes)
    {
        // int newLikes = Convert.ToInt32(commentListDB[commIndex].Likes) + 1;
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Comments");
        reference.Child(commentListDB[commIndex].Id).Child("Likes").SetValueAsync(newLikes.ToString())
        .ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                DebugLog(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                DebugLog("Transaction complete. update num of favs");
                updateText();
            }
        });
    }

    public void getUserComments()
    {
        Debug.Log("Função loadUserComennts");
        FirebaseDatabase.DefaultInstance.GetReference("Comments").OrderByChild("User").EqualTo("devBruno")
        .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
        {
            if (e2.DatabaseError != null)
            {
                Debug.LogError(e2.DatabaseError.Message);
                return;
            }
            commentListDB = new List<Comment>();
            if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
            {

                foreach (var childSnapshot in e2.Snapshot.Children)
                {
                    if (childSnapshot.Child("Text") == null
                        || childSnapshot.Child("Text").Value == null)
                    {
                        Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                        break;
                    }
                    else
                    {
                        Debug.Log("Comments entrys : " +
                        childSnapshot.Child("Text").Value.ToString() + " - " +
                        childSnapshot.Child("User").Value.ToString() + " - " +
                        childSnapshot.Key.ToString()
                        );
                        commentListDB.Add(new Comment(
                            childSnapshot.Child("Const").Value.ToString(),
                            childSnapshot.Child("User").Value.ToString(),
                            childSnapshot.Child("Text").Value.ToString(),
                            childSnapshot.Child("Likes").Value.ToString(),
                            Convert.ToInt32(childSnapshot.Key).ToString(),
                            false));
                    }
                }
                ///Change screen

            }
        };
    }
}


