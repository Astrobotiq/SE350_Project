using UnityEngine;
using UnityEngine.UI;

public class BabySoundBar : MonoBehaviour
{
    public Text StressText;
    public Image StressBar;

    public GameObject babyGameObject;

    public Image tempBabyPic;
    public Sprite cryingBabySprite;
    public Sprite sleepingBabySprite;
    

    float stress, maxStress = 100;
    float lerpSpeed;

    public void Start()
    {
        stress = maxStress;
    }

    public void Update()
    {
        if(stress > maxStress)
        {
            stress = maxStress;
        }

        lerpSpeed = 3f * Time.deltaTime;

        StressBarFiller();
        ColorChanger();

    }

    void StressBarFiller()
    {
        StressBar.fillAmount = Mathf.Lerp(StressBar.fillAmount, stress / maxStress, lerpSpeed);
    }

    void ColorChanger()
    {
        Color stressColor = Color.Lerp(Color.red, Color.green, (stress/maxStress));

        StressBar.color = stressColor;
    }

    public void Damage(float damagePoints)
    {
        Debug.Log("Damage: " + damagePoints);
        if(stress > 0)
        {
            stress -= damagePoints;
        }
        Debug.Log("Here");

        if(stress <= 0)
        {
            tempBabyPic.sprite = cryingBabySprite;
            babyGameObject.GetComponent<SpriteRenderer>().sprite = cryingBabySprite;
        }
    }

    public void Heal(float healingPoints)
    {
        if (stress < maxStress)
        {
            stress += healingPoints;
        }

        if (stress > 0)
        {
            tempBabyPic.sprite = sleepingBabySprite;
        }
    }

}
