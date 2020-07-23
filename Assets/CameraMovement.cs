using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement camGoUp;
    private float step = 0.1f;
    // Start is called before the first frame update
    private void Awake()
    {
        camGoUp = this;
    }

    public void StepUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + step, transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
