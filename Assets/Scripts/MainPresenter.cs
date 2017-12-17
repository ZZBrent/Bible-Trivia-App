using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.IO;
using System.Xml;

public class MainPresenter : MonoBehaviour {

    [SerializeField]
    GameObject questionObj;
    [SerializeField]
    GameObject button1;
    [SerializeField]
    GameObject button1Text;
    [SerializeField]
    GameObject button2;
    [SerializeField]
    GameObject button2Text;
    [SerializeField]
    GameObject button3;
    [SerializeField]
    GameObject button3Text;
    [SerializeField]
    GameObject button4;
    [SerializeField]
    GameObject button4Text;
    [SerializeField]
    GameObject answerResultWindow;
    [SerializeField]
    GameObject answerResult;
    [SerializeField]
    GameObject answerText;
    [SerializeField]
    GameObject passageText;
    [SerializeField]
    Button nextQuestionButton;
    [SerializeField]
    GameObject submissionName;
    Question question;

    public string filePath;
    string result = "";
    XmlNode node;

    // Use this for initialization
    void Start ()
    {
        filePath = Application.streamingAssetsPath + "/Questions.xml";
        //if (Application.platform == RuntimePlatform.WebGLPlayer)
        //{
            if (filePath.Contains("://") || filePath.Contains(":///"))
            {
                WWW www = new WWW(filePath);

                result = www.text;

                print(result);
            }
            else
            {
                result = File.ReadAllText(filePath);

                print(result);
            }
        //}

        nextQuestionButton.onClick.AddListener(delegate { newQuestion(); });
        newQuestion();
    }

    void FetchQuestion()
    {
        XmlDocument userXml1 = new XmlDocument();

        userXml1.LoadXml(result);

        int rand = UnityEngine.Random.Range(0, userXml1.DocumentElement.ChildNodes[4].ChildNodes.Count - 1);

        node = userXml1.DocumentElement.ChildNodes[4].ChildNodes[rand];
    }

    void correctAnswer()
    {
        answerResult.GetComponent<Text>().text = "Correct";
        answerResult.GetComponent<Text>().color = new Color(0, 0.4875f, 0);
        showAnswerWindow();
    }

    void incorrectAnswer()
    {
        answerResult.GetComponent<Text>().text = "Wrong";
        answerResult.GetComponent<Text>().color = new Color(0.4875f, 0, 0);
        showAnswerWindow();
    }

    void showAnswerWindow()
    {
        answerText.GetComponent<Text>().text = string.Format("Answer: {0}", question.CorrectAnswer);
        passageText.GetComponent<Text>().text = string.Format("From: {0}", question.Passage);
        answerResultWindow.SetActive(true);
        button1.GetComponent<Button>().onClick.RemoveAllListeners();
        button2.GetComponent<Button>().onClick.RemoveAllListeners();
        button3.GetComponent<Button>().onClick.RemoveAllListeners();
        button4.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    void newQuestion()
    {
        answerResultWindow.SetActive(false);

        Question q = new Question();

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            FetchQuestion();
            q.QuestionText = node.ChildNodes[0].InnerText;
            q.CorrectAnswer = node.ChildNodes[1].InnerText;
            q.WrongAnswer1 = node.ChildNodes[2].InnerText;
            q.WrongAnswer2 = node.ChildNodes[3].InnerText;
            q.WrongAnswer3 = node.ChildNodes[4].InnerText;
            q.SubmissionName = node.ChildNodes[5].InnerText;
            q.Passage = node.ChildNodes[6].InnerText;
        }
        else
        {
            dbAccess db = GetComponent<dbAccess>();

            db.OpenDB("Questions.db");

            ArrayList result = db.RandomSelect("QUESTIONS", "*");

            string[] s = ((string[])result[0]);
            q.QuestionText = s[0];
            q.CorrectAnswer = s[1];
            q.WrongAnswer1 = s[2];
            q.WrongAnswer2 = s[3];
            q.WrongAnswer3 = s[4];
            q.SubmissionName = s[5];
            q.Passage = s[6];

            db.CloseDB();
        }

        question = q;
        questionObj.GetComponent<Text>().text = question.QuestionText;
        List<string> answerList = new List<string>();
        answerList.Add(question.CorrectAnswer);
        answerList.Add(question.WrongAnswer1);
        answerList.Add(question.WrongAnswer2);
        answerList.Add(question.WrongAnswer3);
        Shuffle(answerList);
        setupAnswerButton(button1Text, button1, answerList[0], question.CorrectAnswer);
        setupAnswerButton(button2Text, button2, answerList[1], question.CorrectAnswer);
        setupAnswerButton(button3Text, button3, answerList[2], question.CorrectAnswer);
        setupAnswerButton(button4Text, button4, answerList[3], question.CorrectAnswer);
        if(question.SubmissionName != "")
            submissionName.GetComponent<Text>().text = question.SubmissionName;
        else
            submissionName.GetComponent<Text>().text = "Anonymous";
    }

    void setupAnswerButton(GameObject textObj, GameObject buttonObj, string text, string answer)
    {
        textObj.GetComponent<Text>().text = text;
        buttonObj.GetComponent<Button>().onClick.RemoveAllListeners();
        Button button = buttonObj.GetComponent<Button>();
        if (text == answer)
        {
            button.onClick.AddListener(delegate { correctAnswer(); });
        }
        else
        {
            button.onClick.AddListener(delegate { incorrectAnswer(); });
        }
    }

    static System.Random _random = new System.Random();

    static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + _random.Next(n - i);
            T t = list[r];
            list[r] = list[i];
            list[i] = t;
        }
    }
}
