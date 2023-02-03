using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ContinuePage : MonoBehaviour
{

    public GameObject[] pages;
    public TextMeshProUGUI[] textMeshPros;
    public int current = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (current > -1)
        {
            pages[current].GetComponent<Image>().color += new Color(0,0,0,0.01f);
            textMeshPros[current].color += new Color(0, 0, 0, 0.01f);
        }

        if (current == -1)
        {
            foreach (GameObject i in pages)
            {
                i.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1);
            }
            foreach (TextMeshProUGUI i in textMeshPros)
            {
                i.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 1);
            }
        }
    }

    public void onTap()
    {
        current++;
        if(current == pages.Length)
        {
            current = -1;
        }
    }
}
