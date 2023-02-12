using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;

public class ThirdWebManager : MonoBehaviour
{
    public static ThirdWebManager Instance;

    public ThirdwebSDK SDK;

    public GameObject InfoTab;

    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SDK=new ThirdwebSDK("mumbai");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowAndHide()
    {
        InfoTab.SetActive(!InfoTab.activeSelf);
    }
}
