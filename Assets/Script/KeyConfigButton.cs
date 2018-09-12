using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ConfigButtonKind
{
    Fire,
    Jump
}

public class KeyConfigButton : MonoBehaviour
{
    Button button;
    bool isSetting = false;
    public ConfigButtonKind kind = ConfigButtonKind.Fire;

    private void Start()
    {
        button = GetComponent<Button>();
        button.interactable = true;
    }
    public void OnClick()
    {
        isSetting = true;
        button.interactable = false;
    }

    void Update()
    {
        if(!isSetting)
        {
            return;
        }
        string prevButton = "";
        switch (kind)
        {
            case ConfigButtonKind.Fire:
                prevButton = KeyConfig.Fire1;
                if (Input.GetButtonDown("Joy0"))
                {
                    KeyConfig.Fire1 = "Joy0";
                }
                if (Input.GetButtonDown("Joy1"))
                {
                    KeyConfig.Fire1 = "Joy1";
                }
                if (Input.GetButtonDown("Joy2"))
                {
                    KeyConfig.Fire1 = "Joy2";
                }
                if (Input.GetButtonDown("Joy3"))
                {
                    KeyConfig.Fire1 = "Joy3";
                }
                if (prevButton != KeyConfig.Fire1)
                {
                    isSetting = false;
                    button.interactable = true;
                }
                break;
            case ConfigButtonKind.Jump:
                prevButton = KeyConfig.Jump;
                if (Input.GetButtonDown("Joy0"))
                {
                    KeyConfig.Jump = "Joy0";
                }
                if (Input.GetButtonDown("Joy1"))
                {
                    KeyConfig.Jump = "Joy1";
                }
                if (Input.GetButtonDown("Joy2"))
                {
                    KeyConfig.Jump = "Joy2";
                }
                if (Input.GetButtonDown("Joy3"))
                {
                    KeyConfig.Jump = "Joy3";
                }
                if (prevButton != KeyConfig.Jump)
                {
                    isSetting = false;
                    button.interactable = true;
                }
                    break;
        }

    }


}
