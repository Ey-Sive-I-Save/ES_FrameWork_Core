using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ES;
public class ESDebugMaster : MonoBehaviour
{
    public static int entityNum = 0; 
    public TMP_Text TMP_Text;
    public bool UseTargetFrame = false;
    public int TatetFrame = 60;
    public float next = 0.2f;
    private void Awake()
    {
       if(UseTargetFrame) Application.targetFrameRate = TatetFrame;
    }

    void Update()
    {
        next -= Time.deltaTime;
        if (next < 0)
        {
            if (TMP_Text != null)
            {
                TMP_Text.text = "frame" + (int)((1 / Time.deltaTime)) + "ob" + entityNum;
            }
            next = 0.35f;
        }
       
    }
}
