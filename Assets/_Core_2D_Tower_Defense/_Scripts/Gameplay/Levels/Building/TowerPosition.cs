using UnityEngine;

public class TowerPosition : MonoBehaviour
{
    public Tower tower;
    private SpriteRenderer sr;
    [SerializeField] private BuildMenu buildMenu;
    public UpgradeMenu upgradeMenu;
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

        if (tower != null)
        {
            var collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;
        }
        else
        {
            var collider = GetComponent<BoxCollider2D>();
            collider.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    public void ShowBuildMenu()
    {
        buildMenu.gameObject.SetActive(true);
        buildMenu.InitBuildMenu();
    }

    public void HideBuildMenu()
    {
        buildMenu.HideMenu(null);
    }

    public void ShowUpgradeMenu()
    {
        upgradeMenu.gameObject.SetActive(true);
        upgradeMenu.InitUpgradeMenu();
    }

    public void HideUpgradeMenu()
    {
        upgradeMenu.HideMenu(null);
    }
}