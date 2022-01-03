using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject ARCamera;
    public TextMeshProUGUI playerChooseText;
    public TextMeshProUGUI DragonNameText;
    public TextMeshProUGUI WinnerText;
    

    public GameObject SelectButtonsObj;
    public GameObject TurnsCanvasGB;
    public GameObject WinnerCanvasGB;
    [HideInInspector]
    public static GameObject Player1Dragon;
    [HideInInspector]
    public static DragonOros Player1DragonScript;
    [HideInInspector]
    public static GameObject Player2Dragon;
    [HideInInspector]
    public static DragonOros Player2DragonScript;

    private bool playMode = false;
    private bool showUpperText = false;
    private float dmgmultiplier12 = 1, dmgmultiplier21 = 1;
    private string spellPlayer1 = "";
    private string spellPlayer2 = "";


    void Update()
    {
        if (showUpperText)
        {
            if (Player1Dragon == null)
                playerChooseText.text = "Player 1 choose a Dragon";
            else if (Player2Dragon == null)
                playerChooseText.text = "Player 2 choose a Dragon";
        }

    }

    /// <summary>
    /// This button will hide the main Screen.
    /// Called from play button
    /// </summary>
    public void Play() {
        GameObject.Find("MainMenu").SetActive(false);
        ARCamera.SetActive(true);
        showUpperText = true;
    }

    /// <summary>
    /// This hides or shows the select button for the dragons.
    /// This will be called from the vuforia images gameobjects
    /// </summary>
    public void ShowUnshownSelectButton(GameObject SelectButton) {       
            SelectButton.SetActive(!SelectButton.activeSelf);
    }

    /// <summary>
    /// This methos shows the name of the dragon shown in the screen.
    /// Called from the AR image GB
    /// </summary>
    /// <param name="name">name displayed on screen</param>
    public void ShowDragonName(string name) {
        if(!playMode)
            DragonNameText.text = name;
    }

    /// <summary>
    /// This stablish the selected dragon for each player and the dmg multiplier.
    /// This method is called from the select button of each dragon.
    /// </summary>
    /// <param name="DragonSelectedGB"> Gb of the dragon shown on screen</param>
    public void SelectDragon(GameObject DragonSelectedGB)
    {
        if (Player1Dragon == null)
        {
            Player1Dragon = DragonSelectedGB;
            Player1DragonScript = Player1Dragon.GetComponent<DragonOros>();
        }          
        else if (Player2Dragon == null)
        {
            Player2Dragon = DragonSelectedGB;
            Player2DragonScript = Player2Dragon.GetComponent<DragonOros>();
            SelectButtonsObj.SetActive(false);
            DragonNameText.text = "";
            playerChooseText.text = "";
            playMode = true;

            if (Player1Dragon.name == "Purple" && Player2Dragon.name == "Red")//oros > Copas 
                dmgmultiplier12 = 1.5f;
            else if (Player1Dragon.name == "Red" && Player2Dragon.name == "Blue")//Copas > Espadas 
                dmgmultiplier12 = 1.5f;
            else if (Player1Dragon.name == "Blue" && Player2Dragon.name == "Green")//Espadas > Bastos 
                dmgmultiplier12 = 1.5f;
            else if (Player1Dragon.name == "Green" && Player2Dragon.name == "Purple")//Bastos > oros 
                dmgmultiplier12 = 1.5f;
            else if (Player2Dragon.name == "Purple" && Player1Dragon.name == "Red")//oros > Copas 
                dmgmultiplier21 = 1.5f;
            else if (Player2Dragon.name == "Red" && Player1Dragon.name == "Blue")//Copas > Espadas 
                dmgmultiplier21 = 1.5f;
            else if (Player2Dragon.name == "Blue" && Player1Dragon.name == "Green")//Espadas > Bastos 
                dmgmultiplier21 = 1.5f;
            else if (Player2Dragon.name == "Green" && Player1Dragon.name == "Purple")//Bastos > oros 
                dmgmultiplier21 = 1.5f;

            TurnPlayerReadyCanvas("Player1 turn", true);
        }
            

        
    }

    /// <summary>
    /// This method shows the player turns text and enables or disables the gb in order to show the button select or not
    /// </summary>
    /// <param name="text"></param>
    /// <param name="state"></param>
    public void TurnPlayerReadyCanvas(string text, bool state) {
        TurnsCanvasGB.GetComponentInChildren<TextMeshProUGUI>().text = text;
        TurnsCanvasGB.SetActive(state);
    }

    /// <summary>
    /// This method shows the Spell HUD of the selected dragon os each player in order so they can cho0se a spell.
    /// This is called from the ready button
    /// </summary>
    public void ShowPlayerTurnHUD() {

        if (spellPlayer1 == "")
        {
            TurnPlayerReadyCanvas("", false);
            Player1DragonScript.ActivateDeactivateCanvas();
            
        }
        else if (spellPlayer2 == "") {
            TurnPlayerReadyCanvas("", false);
            Player2DragonScript.ActivateDeactivateCanvas();
        }

    }



    /// <summary>
    /// This method stores the spell selected for each dragon in order to use it after
    /// Called from the spell buttons of each dragon
    /// </summary>
    /// <param name="spellName"></param>
    public void SelectSpell(string spellName)
    {
        if (spellPlayer1 == "")
        {
            spellPlayer1 = spellName;
            Player1DragonScript.ActivateDeactivateCanvas();
            TurnPlayerReadyCanvas("Player2 turn", true);
            
        }
        else if (spellPlayer2 == "")
        {
            spellPlayer2 = spellName;
            Player2DragonScript.ActivateDeactivateCanvas();
            playerChooseText.text = "Fight!";
            StartCoroutine(FightAnimations());
        }
            

    }

    /// <summary>
    /// This method calls the function of the dragons selected in function of the previous selected spell
    /// When the animations are done, it changes the dmg and mana of each one
    /// </summary>
    IEnumerator FightAnimations() {

        float player1DealtDmg = 0, player2DealtDmg = 0;
        int player1Mana = 0, player2Mana = 0;
        switch (spellPlayer1)
        {
            case "Basic":
                Player1DragonScript.BasicAttack();
                player1DealtDmg = Player1DragonScript.bassicAttackDmg;
                player1Mana = Player1DragonScript.bassicAttackMana;
                break;

            case "Heavy":
                Player1DragonScript.HeavyAttack();
                player1DealtDmg = Player1DragonScript.heavyAttackDmg;
                player1Mana = Player1DragonScript.heavyAttackMana;
                break;

            case "Defend":
                Player1DragonScript.Defend();
                player1DealtDmg = Player1DragonScript.defendDmg;
                player1Mana = Player1DragonScript.defendMana;
                break;

            case "Fire":
                Player1DragonScript.Fire();
                player1DealtDmg = Player1DragonScript.fireDmg;
                player1Mana = Player1DragonScript.fireMana;
                break;

            case "Cure":
                Player1DragonScript.Fire();
                player1DealtDmg = Player1DragonScript.fireDmg;
                player1Mana = Player1DragonScript.fireMana;
                break;

            case "HeavyFire":
                Player1DragonScript.HeavyFire();
                player1DealtDmg = Player1DragonScript.heavyFireDmg;
                player1Mana = Player1DragonScript.heavyFireMana;
                break;

        }

        switch (spellPlayer2)
        {
            case "Basic":
                Player2DragonScript.BasicAttack();
                player2DealtDmg = Player2DragonScript.bassicAttackDmg;
                player2Mana = Player2DragonScript.bassicAttackMana;

                break;

            case "Heavy":
                Player2DragonScript.HeavyAttack();
                player2DealtDmg = Player2DragonScript.heavyAttackDmg;
                player2Mana = Player2DragonScript.heavyAttackMana;
                break;

            case "Defend":
                Player2DragonScript.Defend();
                player2DealtDmg = Player2DragonScript.defendDmg;
                player2Mana = Player2DragonScript.defendMana;
                break;

            case "Fire":
                Player2DragonScript.Fire();
                player2DealtDmg = Player2DragonScript.fireDmg;
                player2Mana = Player2DragonScript.fireMana;
                break;

            case "Cure":
                Player2DragonScript.Fire();
                player2DealtDmg = Player2DragonScript.fireDmg;
                player2Mana = Player2DragonScript.fireMana;
                break;

            case "HeavyFire":
                Player2DragonScript.HeavyFire();
                player2DealtDmg = Player2DragonScript.heavyFireDmg;
                player2Mana = Player2DragonScript.heavyFireMana;
                break;

            

        }
        

        if (spellPlayer1 != "Cure" && spellPlayer2 != "Cure")
        {
            if (spellPlayer1 == "Defend" )
            {
                if (spellPlayer2 == "Defend")
                {
                    player1DealtDmg = 0;
                    player2DealtDmg = 0;
                }
                else
                {
                    player2DealtDmg = Mathf.Clamp(player2DealtDmg * dmgmultiplier21 - player1DealtDmg * dmgmultiplier12, 0, player2DealtDmg * dmgmultiplier21 - player1DealtDmg * dmgmultiplier12);
                }
            }
            else if (spellPlayer2 == "Defend")
            {
            
                 player1DealtDmg = Mathf.Clamp(player1DealtDmg * dmgmultiplier12 - player2DealtDmg * dmgmultiplier21 , 0, player1DealtDmg * dmgmultiplier12 - player2DealtDmg * dmgmultiplier21);
            
            }

        }



        yield return new WaitUntil(() => Player1Dragon.GetComponent<DragonOros>().CheckAnimationFinished() && Player2Dragon.GetComponent<DragonOros>().CheckAnimationFinished());

        if (spellPlayer1 != "Cure" && spellPlayer2 != "Cure")
        {
            Player1DragonScript.ShowDmg((player2DealtDmg * dmgmultiplier21).ToString());
            Player2DragonScript.ShowDmg((player1DealtDmg * dmgmultiplier12).ToString());
        }
        else if (spellPlayer1 == "Cure")
        {
            if (spellPlayer2 == "Defend")
            {
                Player1DragonScript.ShowDmg((0).ToString());
            }
            else
            {
                Player1DragonScript.ShowDmg((player2DealtDmg * dmgmultiplier21).ToString());
            }
                     
            Player2DragonScript.ShowDmg((0).ToString());

            yield return new WaitForSeconds(1f);

            Player1DragonScript.ShowHeal(player1DealtDmg.ToString());
        }
        else if (spellPlayer2 == "Cure")
        {
            if (spellPlayer1 == "Defend")
            {
                Player2DragonScript.ShowDmg((0).ToString());
            }
            else
            {
                Player2DragonScript.ShowDmg((player1DealtDmg * dmgmultiplier12).ToString());

            }
            
            Player1DragonScript.ShowDmg((0).ToString());

            yield return new WaitForSeconds(1f);

            Player2DragonScript.ShowHeal(player2DealtDmg.ToString());
        }


        yield return new WaitForSeconds(1f);

        Player1DragonScript.ShowMana(player1Mana.ToString());
        Player2DragonScript.ShowMana(player2Mana.ToString());

        yield return new WaitForSeconds(1f);

        if (spellPlayer1 != "Cure" && spellPlayer2 != "Cure")
        {
            Player1DragonScript.GetDMG(player2DealtDmg * dmgmultiplier21);
            Player2DragonScript.GetDMG(player1DealtDmg * dmgmultiplier12);
        }
        else if (spellPlayer1 == "Cure")
        {
            if (spellPlayer2 != "Defend")
            {
                float i = (player2DealtDmg * dmgmultiplier21) - player1DealtDmg * dmgmultiplier12;
                if (i >= 0)
                {
                    Player1DragonScript.GetDMG(i);
                }
                else
                {
                    Player1DragonScript.Heal(-i);
                }
            }
            else
                Player1DragonScript.Heal(player1DealtDmg);

            Player2DragonScript.GetDMG(0);
        }
        else if (spellPlayer2 == "Cure")
        {
            if (spellPlayer1 != "Defend")
            {
                float i = player1DealtDmg * dmgmultiplier12 - player2DealtDmg * dmgmultiplier21;
                if (i > 0)
                {
                    Player2DragonScript.GetDMG(i);
                }
                else
                {
                    Player2DragonScript.Heal(-i);
                }
            }
            else
                Player2DragonScript.Heal(player2DealtDmg);
            Player1DragonScript.GetDMG(0);

        }

        Player1DragonScript.ChangeMana(player1Mana);
        Player2DragonScript.ChangeMana(player2Mana);
        spellPlayer1 = "";
        spellPlayer2 = "";
        playerChooseText.text = "";
        yield return new WaitForSeconds(1f);

        if(Player1DragonScript.currentHealth <= 0)
        {
            Player1DragonScript.Die();
            Win("Player2");
        }
        else if(Player2DragonScript.currentHealth <= 0)
        {
            Player2DragonScript.Die();
            Win("Player1");
        }
        else
            TurnPlayerReadyCanvas("Player1 turn", true);

    }

    /// <summary>
    /// Set the winner of the game
    /// </summary>
    /// <param name="winner"></param>
    public void Win(string winner)
    {
        WinnerCanvasGB.SetActive(true);
        if(winner == "Player1")
        {
            WinnerText.text = "Player 1 Won!!";
        }
        else
            WinnerText.text = "Player 2 Won!!";
    }

    /// <summary>
    /// Restarts the game
    /// </summary>
    public void PlayAgain()
    {
        ARCamera.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }
}
