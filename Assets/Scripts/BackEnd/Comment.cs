using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comment
{
    public string Text;
    public string Likes;

    public string User;

    public string Const;

    public string Id;

    public bool hasLike;

    // Start is called before the first frame update
    public Comment() {

    }

    // Update is called once per frame
    public Comment(string constName, string user, string text, string likes, string id, bool putLike) {
        Const = constName;
        User = user;
        Text = text;
        Likes = likes;         
        Id = id;
        hasLike = putLike;
    }

}
