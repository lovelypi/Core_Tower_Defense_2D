using UnityEngine;

public class TowerPosition : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private BuildMenu buildMenu;
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

    public void ShowBuildMenu()
    {
        buildMenu.gameObject.SetActive(true);
        buildMenu.InitBuildMenu();
    }

    public void HideBuildMenu()
    {
        buildMenu.HideMenu(null);
    }
}