using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKey : MonoBehaviour
{
    [SerializeField] GameObject mainMenuGO;
    [SerializeField] GameObject pressAnyKeyGO;

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            mainMenuGO.SetActive(true);
            pressAnyKeyGO.SetActive(false);
        }
    }
}
