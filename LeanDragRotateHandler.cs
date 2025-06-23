using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.iOS;
using Lean.Touch;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LeanDragRotateHandler : MonoBehaviour
{
    //timer = the clock, timerVal = how much the clock increases per second, increment = how much timerVal increases perSecond
    //public float timer, timerVal, increment;

    //counts real time-seconds
    float secondCounter;

    //public RB_MDCodes_SceneController sc;
    float time;
    float timeDelay;
    float increment;

    Vector3 oldPos = Vector3.zero;
    Vector3 currentPos = Vector3.zero;
    bool incrementTime;
    bool doResetScale;
    public Vector3 resetScaleValue;
    public static LeanDragRotateHandler instance;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        time = 0f;
        timeDelay = 3.0f;

    }

    public void Update()
    {
        //if secondCounter has reached one-second or more
        if (secondCounter >= 1f)
        {
            //add to the clock
            time += timeDelay;
            //add to timerValue
            timeDelay += increment;
            //reset secondCounter
            secondCounter = 0f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            time = 0f;
            if (oldPos == Vector3.zero)
                oldPos = Input.mousePosition;
            incrementTime = true;
            //Debug.Log("oldPos"+ oldPos);
        }
        if (Input.GetMouseButton(0) && incrementTime)
        {
            secondCounter += Time.deltaTime;
            if (time >= timeDelay && !IsPointerOverUIObject())
            {
                time = 0f;
                incrementTime = false;
                currentPos = Input.mousePosition;
                if(oldPos == currentPos && !doResetScale)
                {
                    
                    //if (EventSystem.current.IsPointerOverGameObject(Input.touchCount))
                    //    return;

                    doResetScale = true;
                    resetScaleValue = DissectionSceneUI.Instance.faceParent.transform.localScale;
                    DissectionSceneUI.Instance.faceParent.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                    LeanDragCamera.instance.gameObject.GetComponent<LeanDragCamera>().enabled = true;
                    LeanPitchYaw.instance.gameObject.GetComponent<LeanPitchYaw>().enabled = false;
                    LeanMultiUpdate.instance.gameObject.GetComponent<LeanMultiUpdate>().enabled = false;
                    if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
                    {
                        GameObject.Find("Camera Pivot").GetComponent<LeanMultiUpdate>().enabled = false;
                    }
                }   
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            time = 0f;
            oldPos = Vector3.zero;
            if (doResetScale)
            {
                doResetScale = false;
                if(!InCameraDetector.instance.isOutOfCameraView)
                    DissectionSceneUI.Instance.faceParent.transform.localScale = resetScaleValue;
                LeanDragCamera.instance.gameObject.GetComponent<LeanDragCamera>().enabled = false;
                LeanPitchYaw.instance.gameObject.GetComponent<LeanPitchYaw>().enabled = true;
                LeanMultiUpdate.instance.gameObject.GetComponent<LeanMultiUpdate>().enabled = true;
                if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
                {
                    gameObject.GetComponent<TutorialMasterEvenCallback>().ExecuteEventDistance();
                    GameObject.Find("Camera Pivot").GetComponent<LeanMultiUpdate>().enabled = true;
                }
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}