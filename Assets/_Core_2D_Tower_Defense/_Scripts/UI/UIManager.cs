using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Button hackBtn;
    public TextMeshProUGUI spiritStoneText;
    public TextMeshProUGUI livesLeftText;
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI waveNameText;

    protected override void Awake()
    {
        base.Awake();
        
        spiritStoneText.text = "Spirit Stone: " + 0;
        livesLeftText.text = "Lives: " + 0;
        waveNameText.text = "Wave 1";
        waveNameText.alpha = 0f;
        waveNumberText.text = "Wave 1 / " + LevelManager.Instance.levelData.wavesData.Count;
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.On_Spirit_Stone_Change, 
            param => UpdateSpiritStone((int) param));
        EventDispatcher.Instance.RegisterListener(EventID.On_Lives_Change, 
            param => UpdateLives((int) param));
        hackBtn.onClick.AddListener(() =>
        {
            LevelManager.Instance.SpiritStone += 1000;
        });
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
        waveNumberText.text = "Wave " + (waveID + 1) + " / " + LevelManager.Instance.levelData.wavesData.Count;
        waveNameText.gameObject.SetActive(true);
        waveNameText.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(2f);
        waveNameText.DOFade(0f, 0.5f);
        
        yield return new WaitForSeconds(1f);
        
        // Sinh quái cho wave kế tiếp
        LevelManager.Instance.CreateWave(waveID);
    }
}
