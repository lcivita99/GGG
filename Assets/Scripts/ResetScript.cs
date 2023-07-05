using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management

public class ResetScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reloads the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}