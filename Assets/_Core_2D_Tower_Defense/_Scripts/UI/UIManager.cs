using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Button hackBtn;
    public Button pauseBtn;
    public Button speedButton;
    public TextMeshProUGUI spiritStoneText;
    public TextMeshProUGUI livesLeftText;
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI waveNameText;
    public TextMeshProUGUI speedText;

    private int timeScale;
    private bool isPause = false;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite resumeSprite;

    protected override void Awake()
    {
        base.Awake();

        Time.timeScale = timeScale = 1;
        speedText.text = "X" + Time.timeScale;
        spiritStoneText.text = "0";
        livesLeftText.text = "0";
        waveNameText.text = "Wave 1";
        waveNameText.alpha = 0f;
        waveNumberText.text = "1/" + LevelManager.Instance.levelData.wavesData.Count;
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.On_Spirit_Stone_Change,
            param => UpdateSpiritStone((int)param));
        EventDispatcher.Instance.RegisterListener(EventID.On_Lives_Change,
            param => UpdateLives((int)param));

        hackBtn.onClick.AddListener(() => { LevelManager.Instance.SpiritStone += 1000; });

        pauseBtn.onClick.AddListener(PauseResumeGame);

        speedButton.onClick.AddListener(ChangeGameSpeed);
    }

    public void UpdateSpiritStone(int amount)
    {
        spiritStoneText.text = amount.ToString();
    }

    public void UpdateLives(int amount)
    {
        livesLeftText.text = amount.ToString();
    }

    // Hiển thị tên Wave và sinh quái
    public IEnumerator ShowWaveName(int waveID)
    {
        yield return new WaitForSeconds(1f);

        waveNameText.text = "Wave " + (waveID + 1);
        waveNumberText.text = (waveID + 1) + "/" + LevelManager.Instance.levelData.wavesData.Count;
        waveNameText.gameObject.SetActive(true);
        waveNameText.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(2f);
        waveNameText.DOFade(0f, 0.5f);

        yield return new WaitForSeconds(1f);

        // Sinh quái cho wave kế tiếp
        LevelManager.Instance.CreateWave(waveID);
    }

    private void PauseResumeGame()
    {
        //Đang dừng thì cho tiếp tục
        if (isPause)
        {
            Time.timeScale = timeScale;
            isPause = false;
            pauseBtn.image.sprite = pauseSprite;
        }
        else
        {
            Time.timeScale = 0f;
            isPause = true;
            pauseBtn.image.sprite = resumeSprite;
        }
    }

    private void ChangeGameSpeed()
    {
        if (timeScale == 1)
        {
            timeScale = 2;
        }
        else if (timeScale == 2)
        {
            timeScale = 4;
        }
        else
        {
            timeScale = 1;
        }

        speedText.text = "X" + timeScale;

        if (!isPause)
        {
            Time.timeScale = timeScale;
        }
    }
}