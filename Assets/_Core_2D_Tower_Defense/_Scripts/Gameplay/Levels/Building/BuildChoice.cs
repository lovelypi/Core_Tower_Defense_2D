using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildChoice : MonoBehaviour
{
    public bool canCreateTower;
    public int id;
    public int curLevel = 0;
    public GameObject enabledObj;
    public GameObject blockedObj;
    public Button buyButton;
    public Image towerImage;
    public TextMeshProUGUI costText;

    [SerializeField] private Tower curTower;
    [SerializeField] private TowerPosition curTowerPosition;

    public void Init()
    {
        if (IsLegit())
        {
            canCreateTower = true;
            towerImage.sprite = LevelManager.Instance.database.listTowersData[id].listSpecifications[curLevel].towerSprite;
            costText.text = LevelManager.Instance.database.listTowersData[id].listSpecifications[curLevel].spiritStoneToBuy.ToString();
            buyButton.onClick.AddListener(BuyTower);
        }
    }

    private void Update()
    {
        if (IsLegit())
        {
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
        if (IsLegit())
        {
            if (LevelManager.Instance.SpiritStone >= LevelManager.Instance.database.listTowersData[id].listSpecifications[curLevel].spiritStoneToBuy)
            {
                CreateTower();
            }
            else
            {
                Debug.Log("Not Enough Money To Buy " + LevelManager.Instance.SpiritStone);
            }
        }
    }

    private void CreateTower()
    {
        buyButton.interactable = false;
        curTowerPosition = GameController.Instance.curTowerPosition;
        curTower = GameController.Instance.curTower;

        if (canCreateTower)
        {
            LevelManager.Instance.SpiritStone -= LevelManager.Instance.database.listTowersData[id].
                listSpecifications[curLevel].spiritStoneToBuy;
            canCreateTower = false;
            
            // Nâng cấp tháp hiện tại
            if (curTower != null)
            {
                UpgradeTower(curTower);
                curTower.towerPosition.upgradeMenu.upgradeBuildChoice.curLevel++;
                curTower.towerPosition.upgradeMenu.saleChoice.curLevel++;
                curTower.curLevel++;
                curLevel++;
                GameController.Instance.curTower = null;
                GameController.Instance.rangeIndicator.ChangeIndicator(curTower.shootingRange);
                EventDispatcher.Instance.PostEvent(EventID.On_Tower_Upgrade_Completed);
            }
            // Xây tháp mới
            else
            {
                // Kiểm tra nếu chỗ đó có tháp rồi thì không cho xây
                if (curTowerPosition.tower != null)
                {
                    return;
                }
                
                var tower = Instantiate(TowerBuildManager.Instance.towerPrefab,
                    curTowerPosition.transform.position, Quaternion.identity).GetComponent<Tower>();
                curTowerPosition.upgradeMenu.tower = tower;
                tower.transform.SetParent(curTowerPosition.transform);
                tower.towerPosition = curTowerPosition;
                curTowerPosition.tower = tower;
                tower.InitTower(LevelManager.Instance.database.listTowersData[id]);
                curTowerPosition.upgradeMenu.upgradeBuildChoice.curLevel++;
                tower.curLevel++;
                GameController.Instance.curTowerPosition = null;


                // Bắn sự kiện hủy UI chọn tháp sau khi xây tháp
                EventDispatcher.Instance.PostEvent(EventID.On_Tower_Build_Completed);
            }
        }
    }

    private void UpgradeTower(Tower tower)
    {
        tower.LoadSpecification(LevelManager.Instance.database.listTowersData[id].listSpecifications[tower.curLevel]);
    }

    private bool IsLegit()
    {
        if (id >= LevelManager.Instance.database.listTowersData.Count)
        {
            return false;
        }

        if (curLevel >= LevelManager.Instance.database.listTowersData[id].listSpecifications.Count)
        {
            return false;
        }

        return true;
    }
}