using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerNotifierColorChanger : MonoBehaviour
{
    public Slider timerSlider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(timerSlider.value <= 0.3)
        {
            gameObject.GetComponent<Image>().color = Color.Lerp(gameObject.GetComponent<Image>().color, new Color(0.6f, 0.2f, 0.2f, 1f), 0.02f);
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1f);
        }
    }
}
