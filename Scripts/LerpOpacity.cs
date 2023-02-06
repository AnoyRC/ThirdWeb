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
        gameObject.GetComponent<Image>().color -= new Color(0, 0, 0, 0.004f);
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color -= new Color(0, 0, 0, 0.004f);
    }
}
