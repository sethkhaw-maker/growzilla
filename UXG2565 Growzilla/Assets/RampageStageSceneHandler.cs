﻿using UnityEngine;
using UnityEngine.UI;

public class RampageStageSceneHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject KaijuuObject = null;
    [SerializeField] GameObject[] KaijuuSizes = new GameObject[3];
    [SerializeField] SpriteRenderer[] AllColorableParts = new SpriteRenderer[1];
    [SerializeField] Button Ability1Button = null;
    [SerializeField] Button Ability2Button = null;
    [SerializeField] Image Ability1CooldownImage = null;
    [SerializeField] Image Ability2CooldownImage = null;
    [SerializeField] GameObject[] MidUpgrades = new GameObject[1];
    [SerializeField] GameObject[] BigUpgrades = new GameObject[1];

    public string Ability1 { get; private set; }
    public string Ability2 { get; private set; }


    [Header("World")]
    [SerializeField] float WorldMoveSpeed = 1.0f;

    [SerializeField] float AbilityCooldown = 1.0f;
    [SerializeField] float AbilityDuration = 0.5f;
    float Ability1CooldownTimer = 0.0f;
    float Ability2CooldownTimer = 0.0f;

    [Header("Stats")]
    [SerializeField] float HornWorldSpeedModifier = 1.0f;
    [SerializeField] float JumpForce = 1.0f;

    [Header("Hitboxes")]
    [SerializeField] GameObject HornsCollisionObject = null;
    [SerializeField] GameObject TailCollisionObject = null;

    [Header("Movement")]
    [SerializeField] Rigidbody2D[] KaijuuRigidBody = new Rigidbody2D[3];

    void Start()
    {
        OnSceneLoad();
    }

    void Update()
    {
        Ability1Button.interactable = !(Ability1CooldownTimer > 0.0f);
        Ability1CooldownImage.fillAmount = (AbilityCooldown - Ability1CooldownTimer) / AbilityCooldown;
        if (Ability1CooldownTimer > 0.0f) { Ability1CooldownTimer -= Time.deltaTime; }
        else { if (Input.GetKey(KeyCode.Z)) { OnAbility1ButtonPressed(); } }
        
        Ability2Button.interactable = !(Ability2CooldownTimer > 0.0f);
        Ability2CooldownImage.fillAmount = (AbilityCooldown - Ability2CooldownTimer) / AbilityCooldown;
        if (Ability2CooldownTimer > 0.0f) { Ability2CooldownTimer -= Time.deltaTime; }
        else { if (Input.GetKey(KeyCode.X)) { OnAbility2ButtonPressed(); } }
    }

    public void OnSceneLoad()
    {
        // 1. Read data from data handler,
        CacheData CurrentData = DataHandler.Instance.ReadCacheData();

        Ability1 = CurrentData.Current.Ability1;
        Ability2 = CurrentData.Current.Ability2;

        int currentSize = (CurrentData.Current.FoodEaten < 8) ? 0 : (CurrentData.Current.FoodEaten < 15) ? 1 : 2;

        foreach(GameObject go in KaijuuSizes) { go.SetActive(false); }

        KaijuuSizes[currentSize].SetActive(true);

        int smallestIndex = 
            (CurrentData.Current.Ability1 != "Horns" && CurrentData.Current.Ability2 != "Horns")? 0 :
            (CurrentData.Current.Ability1 != "Tail" && CurrentData.Current.Ability2 != "Tail") ? 2 : 1;

        if (currentSize == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == smallestIndex) { MidUpgrades[i].GetComponent<Animator>().Play("Despawn"); }
                else { MidUpgrades[i].SetActive(true); }
            }
        }
        if (currentSize == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == smallestIndex) { BigUpgrades[i].GetComponent<Animator>().Play("Despawn"); ; }
                else { BigUpgrades[i].SetActive(true); }
            }
        }

        foreach (SpriteRenderer SR in AllColorableParts)
        {
            SR.color = new Color(CurrentData.Current.Color[0], CurrentData.Current.Color[1], CurrentData.Current.Color[2], 1.0f);
        }

        Ability1CooldownTimer = 0.0f;
        Ability2CooldownTimer = 0.0f;
    }
    public void HandleObstacleSpawning()
    {

    }
    public void HandleFloorSpawning()
    {

    }
    public void HandleMidgroundObjectSpawning()
    {

    }
    public void HandleBackgroundObjectSpawning()
    {

    }

    // Button Input
    public void OnAbility1ButtonPressed()
    {
        if (Ability1CooldownTimer > 0.0f) { return; }

        switch (Ability1)
        {
            case "Horns": ExecuteAbilityHorns(); break;
            case "Tail":  ExecuteAbilityTail();  break;
            case "Wings": ExecuteAbilityWings(); break;
            default: throw new System.Exception("Ability 1 is either null or unrecognized!");
        }

        Ability1CooldownTimer = AbilityCooldown;
    }
    public void OnAbility2ButtonPressed()
    {
        if (Ability2CooldownTimer > 0.0f) { return; }

        switch (Ability2)
        {
            case "Horns": ExecuteAbilityHorns(); break;
            case "Tail":  ExecuteAbilityTail();  break;
            case "Wings": ExecuteAbilityWings(); break;
            default: throw new System.Exception("Ability 2 is either null or unrecognized!");
        }

        Ability2CooldownTimer = AbilityCooldown;
    }
    private void ExecuteAbilityHorns()
    {
        foreach(GameObject go in KaijuuSizes)
        {
            if (go.activeSelf == true)
            {
                HornsCollisionObject.transform.position = 
                    new Vector3(
                        HornsCollisionObject.transform.position.x,
                        go.transform.position.y,
                        HornsCollisionObject.transform.position.z
                        );
            }
        }
        HornsCollisionObject.SetActive(true);
        Invoke("DisableHorns", AbilityDuration);
    }
    void DisableHorns() { HornsCollisionObject.SetActive(false); }
    private void ExecuteAbilityTail()
    {
        foreach (GameObject go in KaijuuSizes)
        {
            if (go.activeSelf == true)
            {
                TailCollisionObject.transform.position =
                    new Vector3(
                        TailCollisionObject.transform.position.x,
                        go.transform.position.y,
                        TailCollisionObject.transform.position.z
                        );
            }
        }
        TailCollisionObject.SetActive(true);
        Invoke("DisableTail", AbilityDuration);
    }
    void DisableTail() { TailCollisionObject.SetActive(false); }
    private void ExecuteAbilityWings()
    {
        foreach(Rigidbody2D rb2d in KaijuuRigidBody)
        {
            if (rb2d.gameObject.activeSelf == true)
            {
                rb2d.velocity = (Vector2.up * rb2d.mass * JumpForce);
            }
        }
    }

}
