using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    GameObject player;
    Collider playerColl;
    bool scriptEndCheck = false;

    [SerializeField] List<PopUp> PopUp = new List<PopUp>();

    private void Start()
    {
        StartCoroutine(PlayerFindCoroutine());
    }

    IEnumerator PlayerFindCoroutine()
    {
        yield return new WaitForSeconds(3f);
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(Tag.Player);
            playerColl = player.GetComponent<Collider>();

            foreach (PopUp up in PopUp)
            {
                Debug.Log(up);
                up.player = player;
                up.playerCol = playerColl;
            }
        }

        if (!scriptEndCheck)
        {
            scriptEndCheck = true;
            this.enabled = false;
        }
    }
}
