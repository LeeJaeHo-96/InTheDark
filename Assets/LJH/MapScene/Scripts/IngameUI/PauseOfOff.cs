using UnityEngine;

public class PauseOfOff : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.visible = true;
        if (Application.isPlaying)
            Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnDisable()
    {
        Cursor.visible = false;

        if(Application.isPlaying)
        Cursor.lockState = CursorLockMode.Locked;
    }
}
