using DG.Tweening;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public Tower tower;
    public bool isReady;
    public BuildChoice upgradeBuildChoice;
    public SaleChoice saleChoice;
    
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.On_Tower_Upgrade_Completed, HideMenu);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.On_Tower_Upgrade_Completed, HideMenu);
    }
    
    public void InitUpgradeMenu()
    {
        OnScaleUp();
        InitUpgradeChoice();
        LoadUpgradeChoice();
    }

    public void HideMenu(object param)
    {
        OnScaleDown();
    }

    public void OnScaleUp()
    {
        transform.localScale = Vector3.zero; 
        isReady = false;
        transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 0.5f).OnComplete(() =>
        {
            isReady = true;
            GameController.Instance.canClickTower = true;
        });
    }

    public void OnScaleDown()
    {
        isReady = false;
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            GameController.Instance.canClickTower = true;
        });
    }

    private void InitUpgradeChoice()
    {
        upgradeBuildChoice.id = tower.towerID;
        upgradeBuildChoice.Init();
    }

    private void LoadUpgradeChoice()
    {
        var listTowers = TowerBuildManager.Instance.towersInLevel;
        
        // Nếu số level tháp cho phép <= level hiện tại thì không được phép nâng cấp 
        // Nếu > thì được nâng cấp
        if (listTowers[tower.towerID].towerAllowed.Count <= tower.curLevel)
        {
            upgradeBuildChoice.enabledObj.SetActive(false);
            upgradeBuildChoice.blockedObj.SetActive(true);
        }
        else
        {
            upgradeBuildChoice.enabledObj.SetActive(true);
            upgradeBuildChoice.blockedObj.SetActive(false);
        }
    }
}
