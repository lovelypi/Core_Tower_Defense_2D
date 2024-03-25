using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaleChoice : MonoBehaviour
{
    public int id;
    public int curLevel = 0;
    public Button saleButton;
    public TextMeshProUGUI saleText;
    
    [SerializeField] private Tower curTower;
    [SerializeField] private TowerPosition curTowerPosition;
    
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

        saleText.text = LevelManager.Instance.database.listTowersData[id].
            listSpecifications[curLevel].spiritStoneGetWhenSale.ToString();
        
        saleButton.onClick.AddListener(SaleTower);
    }

    private void SaleTower()
    {
        saleButton.interactable = false;
        curTower = GameController.Instance.curTower;

        if (curTower != null)
        {
            LevelManager.Instance.SpiritStone += LevelManager.Instance.database.listTowersData[id].
                listSpecifications[curLevel].spiritStoneGetWhenSale;
            curTower.towerPosition.tower = null;
            curTower.towerPosition.GetComponent<BoxCollider2D>().enabled = true;
            GameController.Instance.curTower = null;
            EventDispatcher.Instance.PostEvent(EventID.On_Tower_Sale_Completed);
            Destroy(curTower.gameObject);
        }
    }
}
