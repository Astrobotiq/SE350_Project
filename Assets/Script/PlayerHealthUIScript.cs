using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUIScript : MonoBehaviour
{
    public Text StressText;
    public Image StressBar;

    public GameObject PlayerObject;


    float health, maxHealth = 100;
    float lerpSpeed;

    public void Start()
    {
        health = maxHealth;
    }

    public void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        lerpSpeed = 3f * Time.deltaTime;

        StressBarFiller();
        ColorChanger();

    }

    void StressBarFiller()
    {
        StressBar.fillAmount = Mathf.Lerp(StressBar.fillAmount, health / maxHealth, lerpSpeed);
    }

    void ColorChanger()
    {
        Color stressColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));

        StressBar.color = stressColor;
    }

    public void Damage(float damagePoints)
    {
        Debug.Log("Player damage yedi");
        Debug.Log("Damage: " + damagePoints);
        if (health > 0)
        {
            health -= damagePoints;
        }
        Debug.Log("Here");

    }

    public void Heal(float healingPoints)
    {
        health += healingPoints;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

       
    }

}