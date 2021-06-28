using UnityEngine;

namespace cleon
{
    public class PressAnyKey : MonoBehaviour
    {
        // The press any key screen game object and the main menu game object
        [SerializeField] GameObject mainMenuGO;
        [SerializeField] GameObject pressAnyKeyGO;

        // Update is called once per frame
        void Update()
        {
            // If we press any key then open the main menu screen and close the press any key screen
            if (Input.anyKeyDown)
            {
                mainMenuGO.SetActive(true);
                pressAnyKeyGO.SetActive(false);
            }
        }
    }
}
