using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : Singleton<GameController>
{
    public TowerPosition curTowerPosition;
    public Tower curTower;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsMouseOverUIElement())
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Kiểm tra xem chuột có nhấn vào 1 Game Object không 
            if (hit.collider != null)
            {
                var towerPosition = hit.collider.GetComponent<TowerPosition>();
                var tower = hit.collider.GetComponent<Tower>();

                // Kiểm tra xem chuột có nhấn vào 1 Tower không 
                if (tower != null)
                {
                    HandleTowerClick(tower);
                    HideCurrentBuildMenu();
                }

                // Kiểm tra xem chuột có nhấn vào 1 TowerPosition không 
                if (towerPosition != null)
                {
                    HandleTowerPositionClick(towerPosition);
                    HideCurrentUpgradeMenu();
                }
            }
            else
            {
                HideCurrentBuildMenu();
                HideCurrentUpgradeMenu();
            }
        }
    }

    private void HandleTowerClick(Tower tower)
    {
        HideCurrentUpgradeMenu();
        curTower = tower;
        tower.towerPosition.ShowUpgradeMenu();
    }

    private void HandleTowerPositionClick(TowerPosition towerPosition)
    {
        HideCurrentBuildMenu();
        curTowerPosition = towerPosition;
        towerPosition.ShowBuildMenu();
    }

    private void HideCurrentUpgradeMenu()
    {
        if (curTower != null)
        {
            curTower.towerPosition.HideUpgradeMenu();
            curTower = null;
        }
    }

    private void HideCurrentBuildMenu()
    {
        if (curTowerPosition != null)
        {
            curTowerPosition.HideBuildMenu();
            curTowerPosition = null;
        }
    }

    // Kiểm tra xem người chơi có click vào UI hay không 
    private bool IsMouseOverUIElement()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        // Kiểm tra results có chứa ít nhất một kết quả từ raycasting hay không. Nếu có, có nghĩa là chuột
        // đang nằm trên một phần tử UI, hàm trả về true. Nếu danh sách rỗng, hàm trả về false,
        // cho biết con trỏ chuột đang nằm trên các Game Object thay vì các phần tử UI.
        return results.Count > 0;
    }
}