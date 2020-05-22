using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Like
{
    public string User;

    public string Id;

    // Start is called before the first frame update
    public Like() {

    }

    // Update is called once per frame
    public Like(string user, string id) {
        User = user;
        Id = id;         
    }

}
