using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    public Image healthBar1;
    public Image healthBar2;
    public Image jetFuelBar;
    public Image oxygenBar;
    public Image dashCooldownBar1;
    public Image dashCooldownBar2;
    public Image grenadeCooldownBar;
    public Image grenadeChargeBar;
    public Text credits;

    public Color lineColor;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public float flashSpeed = 5f;

    private PlayerMoveV3 playerScript;
    public GrenadeThrower throwerScript;

    private float grenadeTimer;
    private bool dashed;
    private bool ignoreButtonUp;

    public UIMaskScroll uiScroll;
    public float scrollSpeed = 0.01f;
    public float warningScrollSpeed = 0.1f;

    private void Start()
    {
        playerScript = GameManager.instance.player.GetComponent<PlayerMoveV3>();
        uiScroll.imageColor = lineColor;
        uiScroll.scrollSpeed = scrollSpeed;
    }

    private void Update()
    {
        healthBar1.fillAmount = GameManager.instance.playerHP / GameManager.instance.maxPlayerHP;
        healthBar2.fillAmount = GameManager.instance.playerHP / GameManager.instance.maxPlayerHP;

        if (GameManager.instance.playerHP / GameManager.instance.maxPlayerHP < 0.3 || GameManager.instance.oxygen / GameManager.instance.maxOxygen < 0.2)
        {
            uiScroll.imageColor = flashColour;
            uiScroll.scrollSpeed = warningScrollSpeed;
        }
        else
        {
            uiScroll.imageColor = lineColor;
            uiScroll.scrollSpeed = scrollSpeed;
        }

        oxygenBar.fillAmount = GameManager.instance.oxygen / GameManager.instance.maxOxygen;

        jetFuelBar.fillAmount = playerScript.jetFuel / playerScript.maxJetpackFuel;

        if (Input.GetButton("Fire1") && grenadeCooldownBar.fillAmount == 1)
        {
            grenadeChargeBar.fillAmount = throwerScript.throwPower / throwerScript.maxThrowPower;
        }
        else
        {
            grenadeChargeBar.fillAmount = 0;
        }


        credits.text = GameManager.instance.enemiesKilled.ToString();

        if (playerScript.dash && !dashed)
        {
            StartCoroutine(DashCooldownBar());
            dashed = true;
        }

        if (GameManager.instance.playerDamaged)
        {
            uiScroll.imageColor = flashColour;
        }
        else
        {
            uiScroll.imageColor = Color.Lerp(uiScroll.imageColor, lineColor, flashSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Fire1") && grenadeCooldownBar.fillAmount < 1)
        {
            ignoreButtonUp = true;
        }

        if (Input.GetButtonDown("Fire1") && grenadeCooldownBar.fillAmount >= 1)
        {
            ignoreButtonUp = false;
        }

        if (Input.GetButtonUp("Fire1") && grenadeTimer <= 0 && !ignoreButtonUp)
        {
            grenadeTimer = throwerScript.cooldown;
            StartCoroutine(GrenadeCooldownBar());
        }
        grenadeTimer -= Time.deltaTime;


    }

    IEnumerator GrenadeCooldownBar()
    {
        grenadeCooldownBar.fillAmount = 0;
        float timeRunning = 0;
        while (true)
        {
            timeRunning += Time.deltaTime;
            grenadeCooldownBar.fillAmount = timeRunning / throwerScript.cooldown;
            if (grenadeCooldownBar.fillAmount >= 1)
            {
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator DashCooldownBar()
    {
        dashCooldownBar1.fillAmount = 0;
        dashCooldownBar2.fillAmount = 0;
        float timeRunning = 0;
        while (true)
        {
            timeRunning += Time.deltaTime;
            dashCooldownBar1.fillAmount = timeRunning / playerScript.dashCooldown;
            dashCooldownBar2.fillAmount = timeRunning / playerScript.dashCooldown;
            //print(timeRunning + " / " + playerScript.dashCooldown + " = " + dashCooldownBar.fillAmount);

            if (dashCooldownBar1.fillAmount >= 1)
            {
                dashed = false;
                yield break;
            }
            yield return null;
        }

    }
}
