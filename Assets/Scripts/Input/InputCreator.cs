using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ControllerInputCreator
{

    [CreateAssetMenu(fileName = "InputCreator")]
    public class InputCreator : ScriptableObject
    {
        [Header("General")]
        [SerializeField]
        private int _numJoysticks = 16;
        [SerializeField]
        private int _ignoreCount = 20;
        [SerializeField]
        private bool _generateKeyboardInput = true;

        [Header("Thumbstick")]
        // these are the recommended settings for xbox 360 controllers: http://wiki.unity3d.com/index.php/Xbox360Controller
        [SerializeField]
        private float _axisGravity = 0;
        [SerializeField]
        private float _axisDead = 0.19f;
        [SerializeField]
        private float _axisSensitivity = 1;
        [SerializeField]
        private bool _invertY = false;
        [SerializeField]
        private bool _invertX = false;

        [Header("Axises")]
        [SerializeField]
        private string _moveAxisXName = "move_x";
        [SerializeField]
        private string _moveAxisYName = "move_y";
        [SerializeField]
        private string _viewAxisXName = "view_x";
        [SerializeField]
        private string _viewAxisYName = "view_y";
        [SerializeField]
        private string _leftTriggerName = "left_trigger";
        [SerializeField]
        private string _rightTriggerName = "right_trigger";


        [Header("Buttons")]
        // these are the standard settings for keyboard and controller buttons
        [SerializeField]
        private float _buttonAxisGravity = 1000;
        [SerializeField]
        private float _buttonAxisDead = 0.001f;
        [SerializeField]
        private float _buttonAxisSensitivity = 1000;

        [Header("ButtonNames")]
        [SerializeField]
        private string[] _buttonNames = { "a", "b", "x", "y", "left_bumper", "right_bumper", "back", "start", "left_stick", "right_stick" };

        [Header("P0 Keyboard")]
        [SerializeField]
        private string[] _p0KeyboardButtons = { "q", "e", "q", "e", "left ctrl", "space", "r", "y", "f", "h" };

        [Header("Move Axis")]
        [SerializeField]
        private string _p0KeyMoveHorizontalPos = "d";
        [SerializeField]
        private string _p0KeyMoveHorizontalNeg = "a";
        [SerializeField]
        private string _p0KeyMoveVerticalPos = "w";
        [SerializeField]
        private string _p0KeyMoveVerticalNeg = "s";

        [Header("View Axis")]
        [SerializeField]
        private string _p0KeyViewHorizontalPos = "l";
        [SerializeField]
        private string _p0KeyViewHorizontalNeg = "j";
        [SerializeField]
        private string _p0KeyViewVerticalPos = "k";
        [SerializeField]
        private string _p0KeyViewVerticalNeg = "i";

        [Header("P1 Keyboard")]
        [SerializeField]
        private string[] _p1KeyboardButtons = { "[7]", "[9]", "[1]", "[3]", "right ctrl", "[0]", "[plus]", "[enter]", "[*]", "[2]" };

        [Header("Move Axis")]
        [SerializeField]
        private string _p1KeyMoveHorizontalPos = "right";
        [SerializeField]
        private string _p1KeyMoveHorizontalNeg = "left";
        [SerializeField]
        private string _p1KeyMoveVerticalPos = "up";
        [SerializeField]
        private string _p1KeyMoveVerticalNeg = "down";

        [Header("View Axis")]
        [SerializeField]
        private string _p1KeyViewHorizontalPos = "[6]";
        [SerializeField]
        private string _p1KeyViewHorizontalNeg = "[4]";
        [SerializeField]
        private string _p1KeyViewVerticalPos = "[5]";
        [SerializeField]
        private string _p1KeyViewVerticalNeg = "[8]";


        [ContextMenu("UpdateInput")]
        public void Init()
        {
            List<Axis> allAxises = new List<Axis>();

            CreateJoystickAxises(allAxises);

            if (_generateKeyboardInput == false)
            {
#if UNITY_EDITOR
                UnityInputChanger.WriteInput(_ignoreCount, allAxises);
#endif
            }
            else if (_generateKeyboardInput == true && (_buttonNames.Length == _p0KeyboardButtons.Length && _buttonNames.Length == _p1KeyboardButtons.Length))
            {
                CreateKeyboardAxises(allAxises);
#if UNITY_EDITOR
                UnityInputChanger.WriteInput(_ignoreCount, allAxises);
#endif
            }
            else
            {
                Debug.LogError("The number of KeyboardButtons must match the number of ButtonNames!");
            }
        }

        private void CreateKeyboardAxises(List<Axis> axises)
        {
            // player 1
            axises.Add(Axis.CreateKeyboardAxis(_moveAxisXName + "_0", _p0KeyMoveHorizontalPos, _p0KeyMoveHorizontalNeg, _buttonAxisGravity, _buttonAxisDead, _buttonAxisSensitivity));
            axises.Add(Axis.CreateKeyboardAxis(_moveAxisYName + "_0", _p0KeyMoveVerticalPos, _p0KeyMoveVerticalNeg, _buttonAxisGravity, _buttonAxisDead, _buttonAxisSensitivity));
            axises.Add(Axis.CreateKeyboardAxis(_viewAxisXName + "_0", _p0KeyViewHorizontalPos, _p0KeyViewHorizontalNeg, _buttonAxisGravity, _buttonAxisDead, _buttonAxisSensitivity));
            axises.Add(Axis.CreateKeyboardAxis(_viewAxisYName + "_0", _p0KeyViewVerticalPos, _p0KeyViewVerticalNeg, _buttonAxisGravity, _buttonAxisDead, _buttonAxisSensitivity));

            for (int i = 0; i < _buttonNames.Length; i++)
                axises.Add(Axis.CreateButton(_buttonNames[i] + "_0", _p0KeyboardButtons[i], -1, _buttonAxisGravity, _buttonAxisDead, _buttonAxisSensitivity));

            // player 2 
            axises.Add(Axis.CreateKeyboardAxis(_moveAxisXName + "_1", _p1KeyMoveHorizontalPos, _p1KeyMoveHorizontalNeg, _buttonAxisGravity, _buttonAxisDead, _buttonAxisSensitivity));
            axises.Add(Axis.CreateKeyboardAxis(_moveAxisYName + "_1", _p1KeyMoveVerticalPos, _p1KeyMoveVerticalNeg, _buttonAxisGravity, _buttonAxisDead, _buttonAxisSensitivity));
            axises.Add(Axis.CreateKeyboardAxis(_viewAxisXName + "_1", _p1KeyViewHorizontalPos, _p1KeyViewHorizontalNeg, _buttonAxisGravity, _buttonAxisDead, _buttonAxisSensitivity));
            axises.Add(Axis.CreateKeyboardAxis(_viewAxisYName + "_1", _p1KeyViewVerticalPos, _p1KeyViewVerticalNeg, _buttonAxisGravity, _buttonAxisDead, _buttonAxisSensitivity));


            for (int i = 0; i < _buttonNames.Length; i++)
                axises.Add(Axis.CreateButton(_buttonNames[i] + "_1", _p1KeyboardButtons[i], -1, _buttonAxisGravity, _buttonAxisDead, _buttonAxisSensitivity));
        }

        private void CreateJoystickAxises(List<Axis> axises)
        {
            for (int i = 0; i < _numJoysticks; i++)
            {
                const int X_AXIS = 1;
                string name = _moveAxisXName + "_" + i;
                axises.Add(Axis.CreateJoystickStick(name, X_AXIS, i + 1, _axisGravity, _axisDead, _axisSensitivity, _invertX));
            }

            for (int i = 0; i < _numJoysticks; i++)
            {
                const int Y_AXIS = 2;
                string name = _moveAxisYName + "_" + i;
                axises.Add(Axis.CreateJoystickStick(name, Y_AXIS, i + 1, _axisGravity, _axisDead, _axisSensitivity, _invertY));
            }

            for (int i = 0; i < _numJoysticks; i++)
            {
                const int X_AXIS_RS = 4;
                string name = _viewAxisXName + "_" + i;
                axises.Add(Axis.CreateJoystickStick(name, X_AXIS_RS, i + 1, _axisGravity, _axisDead, _axisSensitivity, _invertX));
            }

            for (int i = 0; i < _numJoysticks; i++)
            {
                const int Y_AXIS_RS = 5;
                string name = _viewAxisYName + "_" + i;
                axises.Add(Axis.CreateJoystickStick(name, Y_AXIS_RS, i + 1, _axisGravity, _axisDead, _axisSensitivity, _invertY));
            }

            for (int i = 0; i < _numJoysticks; i++)
            {
                const int L_TRIGGER = 9;
                string name = _leftTriggerName + "_" + i;
                axises.Add(Axis.CreateJoystickStick(name, L_TRIGGER, i + 1, _axisGravity, _axisDead, _axisSensitivity, false));
            }

            for (int i = 0; i < _numJoysticks; i++)
            {
                const int R_TRIGGER = 10;
                string name = _rightTriggerName + "_" + i;
                axises.Add(Axis.CreateJoystickStick(name, R_TRIGGER, i + 1, _axisGravity, _axisDead, _axisSensitivity, false));
            }

            for (int i = 0; i < _buttonNames.Length * _numJoysticks; i++)
            {
                int buttonId = i % _buttonNames.Length;
                int joyId = i / _buttonNames.Length;
                string name = string.Format("joystick {0} button {1}", joyId + 1, buttonId);
                axises.Add(Axis.CreateButton(_buttonNames[buttonId] + "_" + joyId, name, joyId + 1, _buttonAxisGravity, _buttonAxisDead, _buttonAxisSensitivity));
            }
        }

        [System.Serializable]
        public class Axis
        {
            public enum Type
            {
                KeyOrMouseButton = 0,
                MouseMovement = 1,
                JoystickAxis = 2
            };

            public string name = "";
            public string descriptiveName = "";
            public string descriptiveNegativeName = "";
            public string negativeButton = "";
            public string positiveButton = "";
            public string altNegativeButton = "";
            public string altPositiveButton = "";

            public float gravity = 0.0f;
            public float dead = 0.0f;
            public float sensitivity = 0.1f;

            public bool snap = false;
            public bool invert = false;

            public Type axisType = Type.KeyOrMouseButton;

            public int axis = 0;
            public int joyNum = 0;

            public static Axis CreateKeyboardAxis(string name, string posButton, string negButton, float gravity, float dead, float sensitivity)
            {
                Axis result = new Axis();

                result.name = name;
                result.positiveButton = posButton;
                result.negativeButton = negButton;
                result.axisType = Type.KeyOrMouseButton;
                result.gravity = gravity;
                result.dead = dead;
                result.sensitivity = sensitivity;

                return result;
            }

            public static Axis CreateJoystickStick(string name, int axis, int joyNum, float gravity, float dead, float sensitivity, bool invert)
            {
                Axis result = new Axis();

                result.name = name;
                result.axisType = Type.JoystickAxis;
                result.axis = axis;
                result.joyNum = joyNum;
                result.gravity = gravity;
                result.dead = dead;
                result.sensitivity = sensitivity;
                result.invert = invert;

                return result;
            }

            public static Axis CreateButton(string name, string posPutton, int joyNum, float gravity, float dead, float sensitivity)
            {
                Axis result = new Axis();

                result.name = name;
                result.positiveButton = posPutton;
                result.joyNum = 0;
                result.gravity = gravity;
                result.dead = dead;
                result.sensitivity = sensitivity;

                result.axis = 1;

                return result;
            }
        }
    }
}