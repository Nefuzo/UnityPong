using Mirror;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : NetworkBehaviour
{
    public ImportantSettings settings;
    [SerializeField] private GameObject Ball;
    [NonSerialized] public GameObject Copy;
    int player_number;
    [SerializeField] private float sensivity;
    Vector2 mousePos;
    Vector2 new_mousePos;
    public Vector2 delta_mousePos;

    void Start()
    {
        if (!isLocalPlayer) return;
        player_number = GameObject.FindGameObjectsWithTag("PlayerPrefab").Length;
        switch (player_number)
        {
            case 1:
                transform.position = new Vector2(0, -4);
                break;

            case 2:
                transform.position = new Vector2(0, 4);
                break;
        }
    }
    public void InstantiateCommand()
    {
        Copy = NetworkIdentity.Instantiate(Ball, new Vector2(0, 0), Quaternion.identity);
        NetworkServer.Spawn(Copy);
        Copy.transform.position = new Vector2(0, 0);
    }

    void Update() // -2.3 , 2.3
    {
        if (settings.IsAndroidVersion == false) WindowsMovement();
        if (settings.IsAndroidVersion == true) AndroidMovement();
        if (!Input.GetMouseButton(0) && settings.IsAndroidVersion == false) return;
        if (settings.IsAndroidVersion == true)
        {
            if (Input.touchCount == 0) return;
        }
        if (!isLocalPlayer) return;
        if ((transform.position.x > -2.3 && transform.position.x < 2.3) ||
            (transform.position.x <= 2.3 && delta_mousePos.x > 0) ||
            (transform.position.x >= 2.3 && delta_mousePos.x < 0))
        {
            transform.Translate(new Vector2(delta_mousePos.x, 0) * Time.deltaTime * sensivity);
            mousePos = new_mousePos;
        }
    }

    void WindowsMovement()
    {
        new_mousePos = Input.mousePosition;
        delta_mousePos = new_mousePos - mousePos;
    }

    void AndroidMovement()
    {
        delta_mousePos = Input.GetTouch(0).deltaPosition;
    }
}
