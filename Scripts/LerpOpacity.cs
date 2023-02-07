using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LerpOpacity : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Image>().color = Color.Lerp(gameObject.GetComponent<Image>().color, new Color(0, 0, 0, 0f), 0.02f);
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.Lerp(gameObject.GetComponentInChildren<TextMeshProUGUI>().color, new Color(1, 1, 1, 0f), 0.02f);
    }
}
