using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class MovePanel : MonoBehaviour
{
    public static MovePanel activePanel;
    public float speed { get { return 0.01f + clickCount * speedUp; } }
    public float redutant;
    private float speedUp = 0.005f;
    private float offsetY { get {return 0.546f + clickCount * stepUp;} }
    private float startOffset = 1.227f;
    private float stepUp = 0.1f;
    private float fallPartPosition;
    private Vector3 startPosL, startPosR;
    private Vector3 endPosL, endPosR;
    private Vector3 startPos, endPos;
    private float inTimePos = 0f;
    private bool motionForward = true, active = true, panelSideLeft;
    private static int clickCount=0;
    public static Transform lastPosition;
    public GameObject prefab;
    public GameObject fallingPart;
    public Text endText;

    private void Awake()
    {   
        if (clickCount % 2 == 0)
        {
            panelSideLeft = true;
        }
        else
        {
            panelSideLeft = false;
        }
    }
    void Start()
    {   
        activePanel = this;

        if (lastPosition == null)
        {   
            lastPosition = BaseCube.by.transform;
        }
        startPosL = new Vector3(activePanel.transform.position.x, BaseCube.by.transform.position.y + offsetY, BaseCube.by.transform.position.z - startOffset);
        endPosL = new Vector3(activePanel.transform.position.x, BaseCube.by.transform.position.y + offsetY, BaseCube.by.transform.position.z + startOffset);

        startPosR = new Vector3(BaseCube.by.transform.position.x - startOffset, BaseCube.by.transform.position.y + offsetY, activePanel.transform.position.z);
        endPosR = new Vector3(BaseCube.by.transform.position.x + startOffset, BaseCube.by.transform.position.y + offsetY, activePanel.transform.position.z);
    }

    public void Stop()
    {
        active = false;
        if (panelSideLeft) // left
        {
             
            redutant = transform.position.z - lastPosition.position.z;
            CutCubeL(redutant);          
        }
        else
        {
            
            redutant = transform.position.x - lastPosition.position.x;
            CutCubeR(redutant);
        }
        clickCount++;
        newBlockSpawn();
        if (clickCount > 3)
        {
            CameraMovement.camGoUp.StepUp();
        }
    }

    private void CutCubeL(float redutant)
    {    
        float newSizeZ = transform.localScale.z - Mathf.Abs(redutant);
        if (newSizeZ > 0)
        {
            float newPosZ = transform.position.z - (redutant / 2);
            
            float fallPartSize = transform.localScale.z - newSizeZ;
            if (redutant < 0)
            {
                fallPartPosition = transform.position.z - (newSizeZ / 2);
            }
            else
            {
                fallPartPosition = transform.position.z + (newSizeZ / 2);
            }
            SpawnFallingPartL(fallPartSize, fallPartPosition);

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newSizeZ);
            transform.localPosition = new Vector3(transform.position.x, transform.position.y, newPosZ);
        }
        else
        {
            EndGame();
        }
     }

    private void CutCubeR(float redutant)
    {
        float newSizeX = transform.localScale.x - Mathf.Abs(redutant);
        if (newSizeX > 0)
        {
            float newPosX = transform.position.x - (redutant / 2);

            float fallPartSize = transform.localScale.x - newSizeX;
            
            if (redutant < 0)
            {
                fallPartPosition = transform.position.x - (newSizeX / 2);
            }
            else
            {
                fallPartPosition = transform.position.x + (newSizeX / 2);
            }
            SpawnFallingPartR(fallPartSize, fallPartPosition);

            transform.localScale = new Vector3(newSizeX, transform.localScale.y, transform.localScale.z);
            transform.localPosition = new Vector3(newPosX, transform.position.y, transform.position.z);
        }
        else
        {
            EndGame();
        }    
    }

    private void SpawnFallingPartL(float fallPartSize, float fallPartPosition) 
    {
        fallingPart = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fallingPart.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallPartSize);
        fallingPart.transform.position = new Vector3(transform.position.x, transform.position.y, fallPartPosition);

        fallingPart.AddComponent<Rigidbody>();
    }
    private void SpawnFallingPartR(float fallPartSize, float fallPartPosition)
    {
        fallingPart = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fallingPart.transform.localScale = new Vector3(fallPartSize, transform.localScale.y, transform.localScale.z);
        fallingPart.transform.position = new Vector3(fallPartPosition, transform.position.y, transform.position.z);

        fallingPart.AddComponent<Rigidbody>();
    }
    private void MoveLeftPanel()
    {
        if (motionForward == true)
        {
            startPos = startPosL;
            endPos = endPosL;
        }
        else
        {
            startPos = endPosL;
            endPos = startPosL;
        }

        transform.position = Vector3.Lerp(startPos, endPos, inTimePos);
        inTimePos += speed;

        if (transform.position.z == endPos.z)
        {
            motionForward = !motionForward;
            inTimePos = 0;
        }
    }

    private void MoveRightPanel()
    {
        if (motionForward == true)
        {
            startPos = startPosR;
            endPos = endPosR;
        }
        else
        {
            startPos = endPosR;
            endPos = startPosR;
        }

        transform.position = Vector3.Lerp(startPos, endPos, inTimePos);
        inTimePos += speed;

        if (transform.position.x == endPos.x)
        {
            motionForward = !motionForward;
            inTimePos = 0;
        }
    }

    private void newBlockSpawn()
    {   
        lastPosition = activePanel.transform;
        Vector3 pos = activePanel.transform.position;
        Instantiate(prefab, pos, Quaternion.identity);
    }
    private void EndGame()
    {
        transform.localScale = new Vector3(0, 0, 0);
        endText.text = clickCount.ToString();
    }
    void FixedUpdate()
    {
        if (active)
        {
            if (panelSideLeft)
            {
                MoveLeftPanel();
            }
            else
            {
                MoveRightPanel();
            }
            if (Input.GetKeyDown("space"))
            {
                Stop();
            }
        }
    }
}