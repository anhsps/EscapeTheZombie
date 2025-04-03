using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeTxt;
    [SerializeField] private int maxTime = 30;
    [HideInInspector] public float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = maxTime;
        DisplayTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            DisplayTime();
        }
        else timeTxt.text = "00 : 00";
    }

    private void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timeTxt.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
