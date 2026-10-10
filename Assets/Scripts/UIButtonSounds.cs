using UnityEngine;
using UnityEngine.UI;

public class UIButtonSounds : MonoBehaviour
{
    AudioManager audioManager;
    Button[] buttons;

    void Start()
    {
        audioManager =
            FindAnyObjectByType<AudioManager>();

        buttons =
            GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    void PlayClickSound()
    {
        if (audioManager != null)
        {
            audioManager.PlayButtonClick();
        }
    }

    void OnDestroy()
    {
        if (buttons == null)
        {
            return;
        }

        foreach (Button button in buttons)
        {
            if (button != null)
            {
                button.onClick.RemoveListener(
                    PlayClickSound
                );
            }
        }
    }
}