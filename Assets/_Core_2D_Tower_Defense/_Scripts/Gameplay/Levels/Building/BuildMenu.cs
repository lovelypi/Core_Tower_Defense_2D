using DG.Tweening;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    public BuildChoice[] buildChoice = new BuildChoice[5];

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.On_Tower_Build_Completed, HideMenu);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.On_Tower_Build_Completed, HideMenu);
    }

    public void InitBuildMenu()
    {
        OnScaleUp();
        InitBuildChoices();
        LoadBuildChoice();
    }

    public void HideMenu(object param)
    {
        OnScaleDown();
    }

    public void OnScaleUp()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 0.5f).OnComplete(() =>
        {
            GameController.Instance.canClickTowerPosition = true;
        });
    }

    public void OnScaleDown()
    {
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            GameController.Instance.canClickTowerPosition = true;
        });
    }

    private void InitBuildChoices()
    {
        for (int i = 0; i < buildChoice.Length; i++)
        {
            buildChoice[i].id = i;
            buildChoice[i].Init();
        }
    }

    private void LoadBuildChoice()
    {
        var listTowers = TowerBuildManager.Instance.towersInLevel;
        for (int i = 0; i < listTowers.Count; i++)
        {
            // Tháp không được cho phép -> Lock, tháp được cho phép -> hiển thị giá
            if (listTowers[i].towerAllowed.Count == 0)
            {
                buildChoice[i].enabledObj.SetActive(false);
                buildChoice[i].blockedObj.SetActive(true);
            }
            else
            {
                buildChoice[i].enabledObj.SetActive(true);
                buildChoice[i].blockedObj.SetActive(false);
            }
        }
    }
}
