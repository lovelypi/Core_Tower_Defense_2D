using DG.Tweening;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public Tower tower;
    public BuildChoice upgradeBuildChoice;
    public SaleChoice saleChoice;
    
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.On_Tower_Upgrade_Completed, HideMenu);
        EventDispatcher.Instance.RegisterListener(EventID.On_Tower_Sale_Completed, HideMenuAndResetLevel);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.On_Tower_Upgrade_Completed, HideMenu);
        EventDispatcher.Instance.RemoveListener(EventID.On_Tower_Sale_Completed, HideMenuAndResetLevel);
    }
    
    public void InitUpgradeMenu()
    {
        OnScaleUp();
        InitUpgradeChoice();
        InitSaleChoice();
        LoadUpgradeChoice();
    }

    public void HideMenu(object param)
    {
        OnScaleDown();
    }

    public void HideMenuAndResetLevel(object param)
    {
        upgradeBuildChoice.curLevel = 0;
        OnScaleDown();
    }

    public void OnScaleUp()
    {
        transform.localScale = Vector3.zero;
        saleChoice.saleButton.interactable = false;
        upgradeBuildChoice.buyButton.interactable = false;
        transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 0.5f).OnComplete(() =>
        {
            saleChoice.saleButton.interactable = true;
            upgradeBuildChoice.buyButton.interactable = true;
            GameController.Instance.canClickTower = true;
        });
    }

    public void OnScaleDown()
    {
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

    private void InitSaleChoice()
    {
        saleChoice.id = tower.towerID;
        saleChoice.Init();
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
