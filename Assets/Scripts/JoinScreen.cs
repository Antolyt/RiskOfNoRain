using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class JoinScreen : MonoBehaviour
{
    public class ControllerData
    {
        public ControllerData(int id, TextMeshProUGUI text)
        {
            InputIndex = id;
            Team = Team.Sand;
            TextRef = text;
        }

        public int InputIndex { get; private set; }
        public Team Team { get; set; }

        public TMPro.TextMeshProUGUI TextRef;

        public void UpdateText()
        {
            TextRef.text = "Controller - " + InputIndex + " is in Team " + Team;
        }
    }

    [SerializeField]
    int maxControllerID;

    [SerializeField]
    float yOffset = -50;

    [SerializeField]
    RectTransform mainText;

    [SerializeField]
    TextMeshProUGUI playerInfoPrefab;

    List<ControllerData> controller;

    InputRequester input;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputRequester>();
        controller = new List<ControllerData>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < maxControllerID; i++)
        {
            if (input.InputButtonDown(EInputButtons.A, i) && controller.Select(p => p.InputIndex).Contains(i) == false)
            {
                TextMeshProUGUI text = Instantiate(playerInfoPrefab);
                text.rectTransform.SetParent(mainText);
                text.rectTransform.localPosition = new Vector3(0, yOffset, 0) * controller.Count;

                ControllerData data = new ControllerData(i, text);
                controller.Add(data);
                
                Debug.Log("Received Input from Controller " + i);
            }
        }

        for (int i = 0; i < controller.Count; i++)
        {
            ControllerData data = controller[i];
            if (input.InputButtonDown(EInputButtons.B, data.InputIndex))
            {
                data.Team = (data.Team + 1);
                if (data.Team == Team.LastIndex)
                {
                    data.Team = 0;
                }
            }

            data.UpdateText();
        }

        bool startGame = true;
        foreach (ControllerData data in controller)
        {
            if(input.InputButton(EInputButtons.Y, data.InputIndex) == false)
            {
                startGame = false;
            }
        }

        if(controller.Count > 0 && startGame)
        {
            ConnectedController.Instance.controllerData = controller;
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainScene");
        }
    }
}
