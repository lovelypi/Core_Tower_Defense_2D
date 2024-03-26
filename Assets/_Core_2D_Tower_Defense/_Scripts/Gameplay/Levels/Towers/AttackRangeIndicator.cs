using UnityEngine;

public class AttackRangeIndicator : MonoBehaviour
{
    public GameObject rangeIndicatorPrefab;
    private GameObject rangeIndicatorObject;
    private SpriteRenderer rangeIndicatorRenderer;

    public void ShowRangeIndicator(Vector3 position, float range, Tower tower)
    {
        if (rangeIndicatorObject == null)
        {
            rangeIndicatorObject = Instantiate(rangeIndicatorPrefab, position, Quaternion.identity);
            rangeIndicatorRenderer = rangeIndicatorObject.GetComponent<SpriteRenderer>();
        }
        else
        {
            rangeIndicatorObject.SetActive(true);
            rangeIndicatorObject.transform.position = position;
        }

        rangeIndicatorRenderer.size = new Vector2(range * 2, range * 2);
    }

    public void HideRangeIndicator()
    {
        if (rangeIndicatorObject != null)
        {
            rangeIndicatorObject.SetActive(false);
        }
    }

    public void ChangeRangeIndicator(float range)
    {
        rangeIndicatorObject.SetActive(true);
        rangeIndicatorRenderer.size = new Vector2(range * 2, range * 2);
    }
}
