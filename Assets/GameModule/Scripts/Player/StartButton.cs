using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StartButton : MonoBehaviour
{
    public Button startButton;
    public List<WaveManager> waveManagers = new List<WaveManager>();
    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        foreach (var wm in waveManagers)
        {
            if (wm != null)
            {
                wm.OnWaveEnded.AddListener(OnAnyWaveEnded);
            }
        }
    }

    void OnStartButtonClicked()
    {
        foreach (var wm in waveManagers)
        {
            if (wm == null) continue;
            wm.StartNextWave();
        }
        startButton.interactable = false;
    }
    void OnAnyWaveEnded()
    {
        foreach (var wm in waveManagers)
        {
            if (wm != null && wm.IsSpawning())
            {
                return;
            }
        }
        if (startButton != null)
            startButton.interactable = true;
    }

    void OnDestroy()
    {
        foreach (var wm in waveManagers)
        {
            if (wm != null)
                wm.OnWaveEnded.RemoveListener(OnAnyWaveEnded);
        }

        if (startButton != null)
            startButton.onClick.RemoveListener(OnStartButtonClicked);
    }
}
