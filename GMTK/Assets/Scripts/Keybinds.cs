using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Keybinds : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public TextMeshProUGUI jumpText, pauseText, interactText;
    private GameObject currentKey;
    Player playerScript;
    public Canvas keybindCanvas, settingsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Jump Key")) PlayerPrefs.SetString("Jump Key", "Space");
        if (!PlayerPrefs.HasKey("Pause Key")) PlayerPrefs.SetString("Pause Key", "Escape");
        if (!PlayerPrefs.HasKey("Interact Key")) PlayerPrefs.SetString("Interact Key", "F");

        keys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump Key")));
        keys.Add("Pause", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause Key")));
        keys.Add("Interact", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact Key")));

        jumpText.SetText(keys["Jump"].ToString());
        pauseText.SetText(keys["Pause"].ToString());
        interactText.SetText(keys["Interact"].ToString());
    }

    public void OpenPage()
    {
        keys["Jump"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump Key"));
        keys["Pause"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause Key"));
        keys["Interact"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact Key"));

        jumpText.SetText(keys["Jump"].ToString());
        pauseText.SetText(keys["Pause"].ToString());
        interactText.SetText(keys["Interact"].ToString());
    }

    // Update is called once per frame
    public void Back()
    {
        keys["Jump"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), jumpText.GetParsedText());
        keys["Pause"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), pauseText.GetParsedText());
        keys["Interact"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), interactText.GetParsedText());

        PlayerPrefs.SetString("Jump Key", keys["Jump"].ToString());
        PlayerPrefs.SetString("Pause Key", keys["Pause"].ToString());
        PlayerPrefs.SetString("Interact Key", keys["Interact"].ToString());

        settingsCanvas.enabled = true;
        keybindCanvas.enabled = false;
    }

    private void OnGUI()
    {
        if(currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(e.keyCode.ToString());
                currentKey = null;
            }
        }
    }
    public void ChangeKey(GameObject clicked)
    {
        currentKey = clicked;
    }
}
