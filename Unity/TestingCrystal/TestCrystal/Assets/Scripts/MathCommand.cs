using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathCommand : MonoBehaviour
{
    public static bool userSayMathOperation(string whatUserSaid)
    {
        bool result = false;
        if(whatUserSaid != null)
        {
            whatUserSaid = whatUserSaid.ToLower();
            string[] wordsUserSaid = whatUserSaid.Split(null);
            if (commandContainsFiveOrSixWords(wordsUserSaid)&&commandContainWhatIs(wordsUserSaid) && commandHasTwoNumbers(wordsUserSaid) && commandHasOneOperation(wordsUserSaid) && commandIsInCorrectFormat(wordsUserSaid))
            {
                result = true;
            }
        }

        return result;
    }

    public static bool commandContainsFiveOrSixWords(string[] wordsUserSaid)
    {

        return (wordsUserSaid != null && (wordsUserSaid.Length == 5 || wordsUserSaid.Length == 6));
    }

    public static bool commandIsInCorrectFormat(string[] wordsUserSaid)
    {
        bool result = false;
        if(wordsUserSaid != null)
        {
            if(wordsUserSaid.Length == 5)
            {
                if(wordsUserSaid[0].Equals("what") && wordsUserSaid[1].Equals("is") && isNumber(wordsUserSaid[2]) && isNumber(wordsUserSaid[4]))
                {
                    result = true;
                }
            }
            else
            {
                if (wordsUserSaid[0].Equals("what") && wordsUserSaid[1].Equals("is") && isNumber(wordsUserSaid[2]) && isNumber(wordsUserSaid[5]))
                {
                    result = true;
                }
            }
        }

        return result;
    }

    public static bool isNumber(string num)
    {
        int number;
        return int.TryParse(num, out number);
    }

    public static bool commandHasOneOperation(string[] wordsUserSaid)
    {
        bool result = false;
        if(wordsUserSaid != null)
        {
            if(Array.IndexOf(wordsUserSaid, "plus") > -1 || Array.IndexOf(wordsUserSaid, "minus") > -1
                || Array.IndexOf(wordsUserSaid, "divided") > -1 || Array.IndexOf(wordsUserSaid, "times") > -1
                || Array.IndexOf(wordsUserSaid, "multiplied") > -1 || Array.IndexOf(wordsUserSaid, "subtracted") > -1
                || Array.IndexOf(wordsUserSaid, "-") > -1)
            {
                result = true;
            }
        }

        return result;
    }

    public static bool commandHasTwoNumbers(string[] wordsUserSaid)
    {
        bool result = false;
        int numCount = 0, number;
        if(wordsUserSaid != null)
        {
            for(int i = 0; i < wordsUserSaid.Length; i++)
            {
                if(int.TryParse(wordsUserSaid[i], out number))
                {
                    numCount++;
                }
            }
            if(numCount == 2)
            {
                result = true;
            }
        }


        return result;
    }

    public static bool commandContainWhatIs(string[] wordsUserSaid)
    {
        bool result = false;
        if(wordsUserSaid != null)
        {
            string firstWord = wordsUserSaid[0].ToLower();
            string secondWord = wordsUserSaid[1].ToLower();

            if (firstWord.Equals("what") && secondWord.Equals("is"))
            {
                result = true;
            }
        }

        return result;
    }

    internal static string getResultOf(string whatUserSaid)
    {
        string crystalResponse = "";
        string[] wordsUserSaid = whatUserSaid.Split(null);

        //crystal cannot say '-' so music convert to 'minus'
        if (wordsUserSaid[3].Equals("-"))
        {
            wordsUserSaid[3] = "minus";
        }


        if(wordsUserSaid.Length == 5)
        {
            crystalResponse = "" + wordsUserSaid[2] + " " + wordsUserSaid[3] + " " + wordsUserSaid[4] + " is " + calculate(wordsUserSaid[3], wordsUserSaid[2], wordsUserSaid[4]);
        }
        else
        {
            crystalResponse = "" + wordsUserSaid[2] + " " + wordsUserSaid[3] + " " + wordsUserSaid[4] +" "+ wordsUserSaid[5]+ " is " + calculate(wordsUserSaid[3], wordsUserSaid[2], wordsUserSaid[5]);

        }

        return crystalResponse;
    }

    public static string calculate(string typeOfCalculation, string num1, string num2)
    {
        int firstNum;
        int.TryParse(num1, out firstNum);
        int secondNum;
        int.TryParse(num2, out secondNum);
        float result = 0;

        switch (typeOfCalculation.ToLower())
        {
            case "plus":
                result = firstNum + secondNum;
                break;
            case "divided":
                result = (float)firstNum / secondNum;
                break;
            case "multiplied":
            case "times":
                result = firstNum * secondNum;
                break;
            case "minus":
            case "-":
            case "subtracted":
                result = firstNum - secondNum;
                break;
        }

        return "" + result;
        
    }
}
