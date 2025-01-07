using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAuthObserver
{
    void OnAuthStateChanged(FirebaseUser user);
    void OnAuthError(string error);
}
