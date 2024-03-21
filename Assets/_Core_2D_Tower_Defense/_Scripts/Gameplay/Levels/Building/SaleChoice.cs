using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaleChoice : MonoBehaviour
{
    public int id;
    public int curLevel = 0;
    public Button saleButton;
    public TextMeshProUGUI saleText;
    
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
        var curTowerPosition = GameController.Instance.curTowerPosition;
        var curTower = GameController.Instance.curTower;

        if (curTower != null)
        {
            curLevel = 0;
            LevelManager.Instance.SpiritStone += LevelManager.Instance.database.listTowersData[id].
                listSpecifications[curLevel].spiritStoneGetWhenSale;
            GameController.Instance.curTower = null;
            EventDispatcher.Instance.PostEvent(EventID.On_Tower_Sale_Completed);
            Destroy(curTower.gameObject);
        }
    }
}
