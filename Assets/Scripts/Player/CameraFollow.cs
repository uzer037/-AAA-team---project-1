using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject objectToFollow;
    Camera cameraObj;
    public float speed = 0.5f;
    public float camZ = -10;
    public float normalCameraScale = 5;

    public float minLimit = 0.0005f;
    // Start is called before the first frame update
    void Start()
    {
        cameraObj = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 delta = objectToFollow.transform.position - this.transform.position;
        delta.z = 0;
        Vector3 deltaFrame = delta*Time.deltaTime*speed;
        float deltaLSq = deltaFrame.x*deltaFrame.x + deltaFrame.y*deltaFrame.y;
        
        if(deltaLSq >= minLimit*minLimit)
        {
            this.transform.position += deltaFrame;
            // cameraObj.orthographicSize = normalCameraScale + Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
        }
        else
        {
            this.transform.position = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, camZ);
            cameraObj.orthographicSize = normalCameraScale;
        }
    }
}
