using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coutdown : MonoBehaviour
{
    float currentTime = 0f;
    float startTime = 0f;

    public Text CountDownText;
    void Start()
    {
        currentTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= 1 * Time.deltaTime;
            print(currentTime);
            CountDownText.text = currentTime.ToString("0");
        }

        
        if (currentTime >= 3.5f) { CountDownText.color = Color.black; }

    }
}