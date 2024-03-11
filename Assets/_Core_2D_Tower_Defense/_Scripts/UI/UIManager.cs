using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI spiritStoneText;
    public TextMeshProUGUI livesLeftText;
    public TextMeshProUGUI waveNameText;

    protected override void Awake()
    {
        base.Awake();
        
        spiritStoneText.text = "Spirit Stone: " + 0;
        livesLeftText.text = "Lives: " + 0;
        waveNameText.text = "Wave 1";
        waveNameText.alpha = 0f;
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.On_Spirit_Stone_Change, 
            param => UpdateSpiritStone((int) param));
        EventDispatcher.Instance.RegisterListener(EventID.On_Lives_Change, 
            param => UpdateLives((int) param));
    }

    public void UpdateSpiritStone(int amount)
    {
        spiritStoneText.text = "Spirit Stone: " + amount;
    }

    public void UpdateLives(int amount)
    {
        livesLeftText.text = "Lives: " + amount;
    }

    // Hiển thị tên Wave và sinh quái
    public IEnumerator ShowWaveName(int waveID)
    {
        yield return new WaitForSeconds(1f);
        
        waveNameText.text = "Wave " + (waveID + 1);
        waveNameText.gameObject.SetActive(true);
        waveNameText.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(2f);
        waveNameText.DOFade(0f, 0.5f);
        
        yield return new WaitForSeconds(1f);
        
        // Sinh quái cho wave kế tiếp
        LevelManager.Instance.CreateWave(waveID);
    }
}