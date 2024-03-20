using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildChoice : MonoBehaviour
{
    public int id;
    public int curLevel = 0;
    public GameObject enabledObj;
    public GameObject blockedObj;
    public Button buyButton;
    public Image towerImage;
    public TextMeshProUGUI costText;

    public void Init()
    {
        if (id >= LevelManager.Instance.database.listTowersData.Count)
        {
            return;
        }

        if (curLevel >= LevelManager.Instance.database.listTowersData[id].listSpecifications.Count)
        {
            return;
        }

        towerImage.sprite = LevelManager.Instance.database.listTowersData[id].listSpecifications[curLevel].towerSprite;
        costText.text = LevelManager.Instance.database.listTowersData[id].listSpecifications[curLevel].
            spiritStoneToBuy.ToString();
        
        buyButton.onClick.AddListener(BuyTower);
    }

    private void Update()
    {
        if (id >= LevelManager.Instance.database.listTowersData.Count)
        {
            return;
        }

        if (curLevel >= LevelManager.Instance.database.listTowersData[id].listSpecifications.Count)
        {
            return;
        }
        
        if (LevelManager.Instance.SpiritStone >= 
            LevelManager.Instance.database.listTowersData[id].listSpecifications[curLevel].spiritStoneToBuy)
        {
            HandlePlayerEnoughSpiritStone();
        }
        else
        {
            HandlePlayerNotEnoughSpiritStone();
        }
    }

    private void HandlePlayerNotEnoughSpiritStone()
    {
        buyButton.interactable = false;
        costText.color = Color.red;
    }

    private void HandlePlayerEnoughSpiritStone()
    {
        buyButton.interactable = true;
        costText.color = Color.green;
    }
    
    // Kiểm tra xem có đủ đá linh lực để mua tháp không 
    private void BuyTower()
    {
        if (LevelManager.Instance.SpiritStone >=
            LevelManager.Instance.database.listTowersData[id].listSpecifications[id].spiritStoneToBuy)
        {
            LevelManager.Instance.SpiritStone -=
                LevelManager.Instance.database.listTowersData[id].listSpecifications[id].spiritStoneToBuy;
            CreateTower();
        }
        else
        {
            Debug.Log("Not Enough Money To Buy This  " + name);
        }
    }
    
    private void CreateTower()
    {
        var curTowerPosition = GameController.Instance.curTowerPosition;
        var curTower = GameController.Instance.curTower;
        
        // Nâng cấp tháp hiện tại
        if (curTower != null)
        {
            UpgradeTower(curTower);
            curTower.towerPosition.upgradeMenu.upgradeBuildChoice.curLevel++;
            curTower.curLevel++;
            GameController.Instance.curTower = null;
            EventDispatcher.Instance.PostEvent(EventID.On_Tower_Upgrade_Completed);
        }
        // Xây tháp mới
        else
        {
            var tower = Instantiate(TowerBuildManager.Instance.towerPrefab,
                curTowerPosition.transform.position, Quaternion.identity).GetComponent<Tower>();
            curTowerPosition.upgradeMenu.tower = tower;
            tower.transform.SetParent(curTowerPosition.transform);
            tower.towerPosition = curTowerPosition;
            curTowerPosition.tower = tower;
            tower.InitTower(LevelManager.Instance.database.listTowersData[id]);
            curTowerPosition.upgradeMenu.upgradeBuildChoice.curLevel++;
            tower.curLevel++;

            // Bắn sự kiện hủy UI chọn tháp sau khi xây tháp
            EventDispatcher.Instance.PostEvent(EventID.On_Tower_Build_Completed);
        }
    }

    private void UpgradeTower(Tower tower)
    {
        tower.LoadSpecification(LevelManager.Instance.database.listTowersData[id].
            listSpecifications[tower.curLevel]);
        
    }
}
