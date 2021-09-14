using UnityEngine;
using UnityEngine.UI;

public class UserStatusController : MonoBehaviour
{
    public Image healthBar;
    public void UpdateHealth(float percentage)
    {
        healthBar.fillAmount = percentage;
    }
}
