using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpiritStoneIndicator : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI text;

    private void OnEnable()
    {
        var iconColor = icon.color;
        var textColor = text.color;
        iconColor.a = 0.5f;
        textColor.a = 0.5f;

        transform.DOMoveY(transform.position.y + 0.5f, 0.5f);
        
        icon.DOFade(1, 0.5f).OnComplete(() =>
        {
            icon.DOFade(0.5f, 0.5f);
        });

        text.DOFade(1, 0.5f).OnComplete(() => 
        { 
            text.DOFade(0.5f, 0.5f).OnComplete(() =>
            {
                PoolingManager.Despawn(gameObject);
            }); 
        });
    }
    
    public void ChangeIndicator(int amount)
    {
        text.text = "+" + amount;
    }
}