using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            for(int i = GameManager.Instance.PlayerObjects.Count - 1; i >= 0; i--)
            {
                GameManager.Instance.PlayerRemove(GameManager.Instance.PlayerObjects[i]);
            }
            Destroy(GameObject.Find(nameof(Camera)).gameObject);

            PunManager.Instance.GoToStartScene();
            Destroy(transform.parent.parent.gameObject);
        }
    }
}
