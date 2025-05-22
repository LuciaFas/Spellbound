using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private RectTransform bar;
    private Image barImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bar = GetComponent<RectTransform>();
        barImage = GetComponent<Image>();
        if (Health.totalHealth < 0.3f)
        {
            barImage.color = new Color32(224, 49, 49, 255);
        }
        else if (Health.totalHealth < 0.5f)
        {
            barImage.color = new Color32(242, 201, 76, 255);
        }
        SetSize(Health.totalHealth);
    }

    public void Damage(float damage)
    {
        if ((Health.totalHealth -= damage) >= 0f)
        {
            Health.totalHealth -= damage;
        }
        else
        {
            Health.totalHealth = 0f;
        }
        
        SetSize(Health.totalHealth);
    }

    public void Heal(float heal)
    {
        Health.totalHealth += heal;

        if (Health.totalHealth > 1f)
        {
            Health.totalHealth = 1f;
        }

        SetSize(Health.totalHealth);
    }

    public void SetSize(float size)
    {
        if (Health.totalHealth < 0.3f)
        {
            barImage.color = new Color32(224, 49, 49, 255);
        }
        else if (Health.totalHealth < 0.5f)
        {
            barImage.color = new Color32(242, 201, 76, 255);
        }
        else
        {
            barImage.color = new Color32(43, 217, 91, 255);
        }

        bar.localScale = new Vector3(size, 1f);
    }
}
