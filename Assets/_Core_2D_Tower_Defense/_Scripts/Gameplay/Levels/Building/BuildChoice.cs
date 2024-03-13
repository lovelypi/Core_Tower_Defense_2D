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
    public TextMeshProUGUI costText;

    public void Init()
    {
        if (id >= LevelManager.Instance.database.listTowersData.Count)
        {
            return;
        }
        costText.text = LevelManager.Instance.database.listTowersData[id].listSpecifications[curLevel].
            spiritStoneToBuy.ToString();
        
        buyButton.onClick.AddListener(BuyTower);
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
            Debug.Log("Not Enough Money To Buy This Tower");
        }
    }
    
    private void CreateTower()
    {
        var tower = Instantiate(TowerBuildManager.Instance.towerPrefab, 
            GameController.Instance.curTowerPosition.transform.position, Quaternion.identity).GetComponent<Tower>();
        tower.towerPosition = GameController.Instance.curTowerPosition;
        tower.InitTower(LevelManager.Instance.database.listTowersData[id]);

        // Bắn sự kiện Hủy UI chọn tháp sau khi xây tháp
        EventDispatcher.Instance.PostEvent(EventID.On_Tower_Build_Completed);
    }
}
