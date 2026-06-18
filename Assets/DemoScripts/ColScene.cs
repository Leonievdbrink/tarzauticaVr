using System;
using UnityEngine;

public class ColScene : MonoBehaviour
{
   ChangeScene changeScene;

   private void OnTriggerEnter(Collider other)
   {
      if (other.tag == "Player")
      {
         changeScene.changeScene();
      }
   }
}
