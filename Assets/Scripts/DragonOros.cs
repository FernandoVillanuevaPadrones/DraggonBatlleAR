using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DragonOros : MonoBehaviour
{
    public int m_Health;
    public int m_Mana;
    public GameObject canvas;
    public TextMeshProUGUI dmgShowTM;
    public TextMeshProUGUI manaShowTM;
    public TextMeshProUGUI healShowTM;
    public Image healthBar;
    public Image ManaBar;


    [Header("Spells Damage")]
    public int bassicAttackDmg;
    public int heavyAttackDmg;
    public int defendDmg;
    public int fireDmg;
    public int heavyFireDmg;

    [Header("Spells Mana")]
    public int bassicAttackMana;
    public int heavyAttackMana;
    public int defendMana;
    public int fireMana;
    public int heavyFireMana;

    [Header("Spells Buttons")]
    public Button bassicAttackButton;
    public Button heavyAttackButton;
    public Button defendButton;
    public Button fireButton;
    public Button heavyFireButton;

    [Header("Spells Particles")]
    public ParticleSystem fireParticles;
    public ParticleSystem heavyFireParticles;



    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentMana;
    [HideInInspector]
    public bool animationFinished;

    private Animator animator;
    private float lerpSpeed;    



    void Start()
    {
        currentHealth = m_Health;
        currentMana = m_Mana;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;
        HealthBarFiller();
        ColorChanger();


        if (currentMana < -heavyAttackMana)
        {
            heavyAttackButton.interactable = false;
        }
        if (currentMana < -defendMana)
        {
            defendButton.interactable = false;
        }
        if (currentMana < -fireMana)
        {
            fireButton.interactable = false;
        }
        if (currentMana < -heavyFireMana)
        {
            heavyFireButton.interactable = false;
        }
        if (currentMana >= -heavyAttackMana)
        {
            heavyAttackButton.interactable = true;
        }
        if (currentMana >= -defendMana)
        {
            defendButton.interactable = true;
        }
        if (currentMana >= -fireMana)
        {
            fireButton.interactable = true;
        }
        if (currentMana >= -heavyFireMana)
        {
            heavyFireButton.interactable = true;
        }
    }

    /// <summary>
    /// This method updates the health and mana bar value in a determinated speed
    /// </summary>
    void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth/m_Health, lerpSpeed);
        ManaBar.fillAmount = Mathf.Lerp(ManaBar.fillAmount, currentMana/m_Mana, lerpSpeed);
    }

    /// <summary>
    /// This method updates the health and mana bar color in a determinated speed
    /// </summary>
    void ColorChanger() {
        Color healthColor = Color.Lerp(Color.red, Color.green, (currentHealth/m_Health));
        healthBar.color = healthColor;
    }

    /// <summary>
    /// This functions changes the animator booleans for the spells animations.
    /// Called from each spell button
    /// </summary>
    public void BasicAttack() {
        if (animationFinished)
            animationFinished = false;
        animator.SetBool("BasicAttack", !animator.GetBool("BasicAttack")); 
        
    }
    public void HeavyAttack() {
        if (animationFinished)
            animationFinished = false;
        animator.SetBool("HeavyAttack", !animator.GetBool("HeavyAttack"));
    }
    public void Defend() {
        if (animationFinished)
            animationFinished = false;
        animator.SetBool("Defend", !animator.GetBool("Defend"));

    }
    public void Fire() {
        if (animationFinished)
            animationFinished = false;
        animator.SetBool("Fire", !animator.GetBool("Fire"));
    }
    public void HeavyFire() {
        if(animationFinished)
            animationFinished = false;
        animator.SetBool("HeavyFire", !animator.GetBool("HeavyFire"));

    }

    /// <summary>
    /// This methods shows the dmg/Mana/heal dealt to the dragon and starts/stops the animation of its canvas
    /// Called from the GameManager script to begin and from the animation to stop
    /// </summary>
    /// <param name="dmg">Amount of dmg/mana/heal shown</param>
    public void ShowDmg(string dmg)
    {
        dmgShowTM.text = dmg;
        if (animationFinished)
            animationFinished = false;
        animator.SetBool("ShowDmg", !animator.GetBool("ShowDmg"));
    }

    public void ShowMana(string mana)
    {
        
        if (animationFinished)
            animationFinished = false;
        animator.SetBool("ShowMana", !animator.GetBool("ShowMana"));
        manaShowTM.text = mana;
    }

    public void ShowHeal(string heal)
    {
        
        if (animationFinished)
            animationFinished = false;
        animator.SetBool("ShowHeal", !animator.GetBool("ShowHeal"));
        healShowTM.text = heal;
    }

    /// <summary>
    /// This methods starts/stops the particles of the animations
    /// Called from the animation
    /// </summary>
    public void FireParticles()
    {
        if(fireParticles != null)
        {
            if (fireParticles.isPlaying)
            {
                fireParticles.Stop(); 
            }
            else
            {
                fireParticles.Play();
            }               
        }
        
    }

    public void HeavyFireParticles()
    {
        if (heavyFireParticles != null)
        {
            if (heavyFireParticles.isPlaying)
                heavyFireParticles.Stop();
            else
                heavyFireParticles.Play();

        }

    }



    public void Die()
    {
        animator.SetBool("Die", true);
    }

    public void SetAnimationFinished()
    {
        animationFinished = true;
    }
    public bool CheckAnimationFinished()
    {
        return animationFinished;
    }

    /// <summary>
    /// This enables or disables the Hud of each dragons spells, health and mana
    /// </summary>
    public void ActivateDeactivateCanvas() {
        canvas.SetActive(!canvas.activeSelf);      
    }

    /// <summary>
    /// This method changes the current health 
    /// </summary>
    /// <param name="dmg">amount of dmg dealt</param>
    public void GetDMG(float dmg)
    {
        currentHealth -= dmg;
    }

    public void ChangeMana(int total)
    {      
        currentHealth = Mathf.Clamp(currentMana + total, currentMana + total, currentMana);
    }

    public void Heal(float heal)
    {
        currentHealth = Mathf.Clamp(currentHealth + heal, currentHealth + heal, m_Health);
    }
}
