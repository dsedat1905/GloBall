using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bildirim : MonoBehaviour
{
    void Start()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
    }

  public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token){
      Debug.Log("sedat" +token.Token);
  }
  public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs GelenMesage) {
    Debug.Log("sedat oyna: " + GelenMesage.Message.From);
  }
}
