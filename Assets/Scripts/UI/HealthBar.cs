using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Visual health bar
    public RectTransform healthBarFill;
    // Visual exp bar
    public RectTransform expBarFill;

    private void Update()
    {
        // Don't update if player isnt set currently, update bars otherwise
        if (UIManager.instance.PC != null)
        {
            UpdateHealthBar();
            UpdateExpBar();
        }
    }

    // Fill the fraction of the health bar representing player's currentHP (out of their entire maxHP)
    private void UpdateHealthBar()
    {
        float healthScale = (float)UIManager.instance.PC.currentHP / UIManager.instance.PC.maxHP;
        healthBarFill.localScale = new Vector3(healthScale, 1, 1);
    }

    // Fill the fraction of the exp bar representing player's currentExp (out of their neededExp)
    private void UpdateExpBar()
    {
        float expScale = (float)UIManager.instance.PC.currentExp / UIManager.instance.PC.neededExp;
        expBarFill.localScale = new Vector3(expScale, 1, 1);
    }
}