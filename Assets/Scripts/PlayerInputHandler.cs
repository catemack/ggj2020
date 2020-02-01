using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetLookInputHorizontal()
    {
        return Input.GetAxisRaw(GameConstants.k_MouseAxisNameHorizontal);
    }

    public float GetLookInputVertical()
    {
        return -Input.GetAxisRaw(GameConstants.k_MouseAxisNameVertical);
    }

    public Vector3 GetMoveInput()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw(GameConstants.k_AxisNameHorizontal), 0f, Input.GetAxisRaw(GameConstants.k_AxisNameVertical));
        move = Vector3.ClampMagnitude(move, 1);
        return move;
    }

    public bool GetInteractInputDown()
    {
        return Input.GetButtonDown(GameConstants.k_ButtonNameInteract);
    }
}
