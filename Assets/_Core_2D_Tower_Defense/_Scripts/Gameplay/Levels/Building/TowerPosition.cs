using UnityEngine;

public class TowerPosition : MonoBehaviour
{
    private GameObject tower;

    private SpriteRenderer sr;
    private int towerID = 0;
    [SerializeField] private Color startColor;
    [SerializeField] private Color selectedColor;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = startColor;
    }

    private void OnMouseEnter()
    {
        sr.color = selectedColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (tower != null)
        {
            return;
        }

        var towerToBuild = TowerBuildManager.Instance.GetSelectedTower();
        tower = Instantiate(towerToBuild, transform.position, Quaternion.identity);
        tower.transform.SetParent(transform);
        var turret = tower.GetComponent<Turret>();
        turret.towerPosition = this;
        turret.InitTurret(LevelManager.Instance.database.ListTurretsData[towerID]);
    }
}
