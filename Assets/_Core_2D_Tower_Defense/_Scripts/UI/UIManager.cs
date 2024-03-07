using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesLeftText;
    public TextMeshProUGUI waveNameText;

    protected override void Awake()
    {
        base.Awake();
        
        scoreText.text = "Score: " + 0;
        livesLeftText.text = "Lives: " + 0;
        waveNameText.text = "Wave 1";
        waveNameText.alpha = 0f;
    }

    public IEnumerator ShowWaveName(int waveID)
    {
        yield return new WaitForSeconds(1f);
        
        waveNameText.text = "Wave " + (waveID + 1);
        waveNameText.gameObject.SetActive(true);
        waveNameText.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(2f);
        waveNameText.DOFade(0f, 0.5f);
        
        yield return new WaitForSeconds(1f);
        
        LevelManager.Instance.CreateNextWave();
    }
}
