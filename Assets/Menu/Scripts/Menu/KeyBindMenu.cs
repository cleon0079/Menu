using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cleon
{
    public class KeyBindMenu : MonoBehaviour
    {
        // Make a dictionary to save the keys
        public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

        [SerializeField] Text up, down, left, right, jump;

        GameObject currentKey;

        // Start is called before the first frame update
        void Start()
        {
            // Set a default key or get the key from playerprefs and converts the string to keycode and save it to the dictionary
            keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
            keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
            keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
            keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
            keys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space")));

            // Shows the default keys at the text
            up.text = keys["Up"].ToString();
            down.text = keys["Down"].ToString();
            left.text = keys["Left"].ToString();
            right.text = keys["Right"].ToString();
            jump.text = keys["Jump"].ToString();
        }

        private void OnGUI()
        {
            string newKey = "";
            Event currentEvent = Event.current;

            // If we have clicked a button
            if (currentKey != null)
            {
                // If the key we press is a keyboard key
                if (currentEvent.isKey)
                {
                    // Save the key we press to a string
                    newKey = currentEvent.keyCode.ToString();
                }

                //if we have set a key
                if (newKey != "")
                {
                    //we change our dictionary (that means our keybind changes too)
                    keys[currentKey.name] = (KeyCode)System.Enum.Parse(typeof(KeyCode), newKey);
                    //change the text of our button
                    currentKey.GetComponentInChildren<Text>().text = newKey;
                    currentKey = null;
                    SaveKeys();
                }
            }
        }

        public void ChangeKey(GameObject _clickKey)
        {
            // If we click on the button then set the current key to this key
            currentKey = _clickKey;
        }

        public void SaveKeys()
        {
            // Save all the keys we changed and save it at playerprefs
            foreach (var key in keys)
            {
                PlayerPrefs.SetString(key.Key, key.Value.ToString());
            }
            PlayerPrefs.Save();
        }
    }
}
