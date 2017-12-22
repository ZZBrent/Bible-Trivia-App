using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {

    public DateTime Time { get; set; }
    public string QuestionText { get; set; }
    public string CorrectAnswer { get; set; }
    public string WrongAnswer1 { get; set; }
    public string WrongAnswer2 { get; set; }
    public string WrongAnswer3 { get; set; }
    public string SubmissionName { get; set; }
    public string Passage { get; set; }

    public Question(DateTime time, string question, string correctAnswer, string wrongAnswer1, string wrongAnswer2, string wrongAnswer3, string submissionName, string passage)
    {
        Time = time;
        QuestionText = question;
        CorrectAnswer = correctAnswer;
        WrongAnswer1 = wrongAnswer1;
        WrongAnswer2 = wrongAnswer2;
        WrongAnswer3 = wrongAnswer3;
        SubmissionName = submissionName;
        Passage = passage;
    }

    public Question()
    {

    }

}

