using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EInputButtons {
    A,
    B,
    X, 
    Y, 
    LB, 
    RB, 
    Back, 
    Start
}

public enum EInputAxis
{
    movementHorizontal, 
    movementVertical,
    viewHorizontal, 
    viewVertical,
    triggerRight, 
    triggerLeft
}

public enum EInputVector
{
    movement, 
    view
}

public class InputRequester : MonoBehaviour {

    int MAX_PLAYER_ID = 8;
    public bool dualInput = true;

    public static InputRequester Instance;

    public void Awake()
    {
        Instance = this;
    }

    public bool InputButtonDown(EInputButtons button, int playerID = -1)
    {
        string playerString = getPlayerString(playerID);
        switch (button)
        {
            case EInputButtons.A:
                return Input.GetButtonDown("a" + playerString);
            case EInputButtons.B:
                return Input.GetButtonDown("b" + playerString);
            case EInputButtons.X:
                return Input.GetButtonDown("x" + playerString);
            case EInputButtons.Y:
                return Input.GetButtonDown("y" + playerString);
            case EInputButtons.LB:
                return Input.GetButtonDown("left_bumper" + playerString);
            case EInputButtons.RB:
                return Input.GetButtonDown("right_bumper" + playerString);
            case EInputButtons.Back:
                return Input.GetButtonDown("back" + playerString);
            case EInputButtons.Start:
                return Input.GetButtonDown("start" + playerString);
            default:
                return false;
        }
    }
    
    /// <summary>
    /// signals if button is pressed
    /// </summary>
    /// <param name="button">enum of the button</param>
    /// <param name="playerID">Id of the player</param>
    /// <returns>boolean that signals if button is currently pressed</returns>
    public bool InputButton(EInputButtons button, int playerID = -1)
    {
        string playerString = getPlayerString(playerID);
        switch (button)
        {
            case EInputButtons.A:
                return Input.GetButton("a" + playerString);
            case EInputButtons.B:
                return Input.GetButton("b" + playerString);
            case EInputButtons.X:
                return Input.GetButton("x" + playerString);
            case EInputButtons.Y:
                return Input.GetButton("y" + playerString);
            case EInputButtons.LB:
                return Input.GetButton("left_bumper" + playerString);
            case EInputButtons.RB:
                return Input.GetButton("right_bumper" + playerString);
            case EInputButtons.Back:
                return Input.GetButton("back" + playerString);
            case EInputButtons.Start:
                return Input.GetButton("start" + playerString);
            default:
                return false;
        }
    }

    // fix AxisInput for KeyboardPlayers

    /// <summary>
    /// returns input value for given axis and player
    /// </summary>
    /// <param name="axis">enum for movement Axis</param>
    /// <param name="playerID">ID of player</param>
    /// <returns>value between -1 and 1</returns>
    public float InputAxis(EInputAxis axis, int playerID = -1)
    {
        string playerString = getPlayerString(playerID);

        float inputValue = 0f;

        switch (axis)
        {
            case EInputAxis.movementHorizontal:
                inputValue = Input.GetAxis("move_x" + playerString);
                break;
            case EInputAxis.movementVertical:
                inputValue = Input.GetAxis("move_y" + playerString);
                break;
            case EInputAxis.viewHorizontal:
                inputValue = Input.GetAxis("view_x" + playerString);
                break;
            case EInputAxis.viewVertical:
                inputValue = Input.GetAxis("view_y" + playerString);
                break;
            case EInputAxis.triggerLeft:
                inputValue = Input.GetAxis("left_button" + playerString);
                break;
            case EInputAxis.triggerRight:
                inputValue = Input.GetAxis("right_button" + playerString);
                break;
        }
        return inputValue;
    }

    /// <summary>
    /// returns Inputvector for given vector and playerID
    /// </summary>
    /// <param name="vector">enum for Vector(view or movement)</param>
    /// <param name="playerID">ID of player</param>
    /// <returns>Vector for given axis, every value of each axis is between -1 and 1</returns>
    public Vector2 InputVector(EInputVector vector, int playerID = -1)
    {
        switch(vector)
        {
            case EInputVector.movement:
                return new Vector2(InputAxis(EInputAxis.movementHorizontal, playerID), InputAxis(EInputAxis.movementVertical, playerID));
            case EInputVector.view:
                return new Vector2(InputAxis(EInputAxis.viewHorizontal, playerID), InputAxis(EInputAxis.viewVertical, playerID));
            default:
                Debug.LogError("Fatal Error in " + this.name + ". THIS SHOULD NOT HAPPEN");
                return Vector2.zero;
        }
    }

    string getPlayerString(int playerID)
    {
        if (playerID < 0)
            return "_0";
        if (playerID <= MAX_PLAYER_ID)
            return "_" + playerID.ToString();
        Debug.LogError("InputManager not set up for Player with id " + playerID.ToString());
        return null;

    }
}
