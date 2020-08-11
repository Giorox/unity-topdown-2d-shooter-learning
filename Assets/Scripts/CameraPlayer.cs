using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        this.transform.right = (this.transform.right / 0.1f) * 0.1f;
        this.transform.up = (this.transform.up / 0.1f) * 0.1f;
        if (!this.transform.parent.GetComponent<PlayerController>().isLocalPlayer)
        {
            gameObject.GetComponent<Camera>().enabled = false;
            gameObject.GetComponent<AudioListener>().enabled = false;
        }
    }
}