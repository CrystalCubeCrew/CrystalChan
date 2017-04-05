using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class mathCommandTests {

 

    [SetUp]
    public void SetUp()
    {
       

    }

    [Test]
	public void EditorTest() {
		//Arrange
		var gameObject = new GameObject();

		//Act
		//Try to rename the GameObject
		var newGameObjectName = "My game object";
		gameObject.name = newGameObjectName;

		//Assert
		//The object has a new name
		Assert.AreEqual(newGameObjectName, gameObject.name);
	}

    [Test]
    public void ifUserEntersCommandWithWhatIsFirstThenReturnTrue()
    {
        string[] command = { "What", "is", "two", "plus", "four" };

        Assert.True(MathCommand.commandContainsWhatIsOrWhats(command));


    }

    [Test]
    public void ifUserEntersCommandWithWhatsFirstThenReturnTrue()
    {
        string[] command = { "What's", "two", "plus", "four" };

        Assert.True(MathCommand.commandContainsWhatIsOrWhats(command));


    }

    [Test]
    public void ifUserEnterCommandWithWhatIsInBetweenThenReturnFalse()
    {
        string[] command = { "so", "what", "is", "what", "is" };

        Assert.False(MathCommand.commandContainsWhatIsOrWhats(command));
    }

    [Test]
    public void ifUserEntersCommandWithNoWhatIsThenReturnFalse()
    {
        string[] command = { "so", "I", "have", "a", "dog" };

        Assert.False(MathCommand.commandContainsWhatIsOrWhats(command));
    }

    [Test]
    public void ifUserEntersNullCommandReturnFalse()
    {
        string[] command = null;

        Assert.False(MathCommand.commandContainsWhatIsOrWhats(command));
    }

    [Test]
    public void ifCommandHasTwoNumbersInCorrectSpotsReturnTrue()
    {
        string[] command = { "what", "is", "3", "plus", "4" };
        string[] command2 = { "what", "is", "3", "subtracted", "by", "4" };

        Assert.True(MathCommand.commandHasTwoNumbers(command));
        Assert.True(MathCommand.commandHasTwoNumbers(command2));
    }

    [Test]
    public void ifCommandHasThreeNumbersReturnFalse()
    {
        string[] command = { "what", "is", "3", "5", "4" };

        Assert.False(MathCommand.commandHasTwoNumbers(command));
    }

    [Test]
    public void ifCommandHasNoNumbersReturnFalse()
    {
        string[] command = { "what", "is", "no", "way", "mean" };

        Assert.False(MathCommand.commandHasTwoNumbers(command));
    }


    [Test]
    public void ifCommandisNullReturnFalse()
    {
        string[] command = null;

        Assert.False(MathCommand.commandHasTwoNumbers(command));
    }

    [Test]
    public void ifCommandHadFourWordsReturnTrue()
    {
        string[] command = { "what's", "no", "way", "me" };

        Assert.True(MathCommand.commandContainsFourFiveOrSixWords(command));
    }


    [Test]
    public void ifCommandHadFourWordsInCorrectFormatReturnTrue()
    {
        string[] command = { "what's", "5", "plus", "7" };

        Assert.True(MathCommand.commandIsInCorrectFormat(command));
    }

    [Test]
    public void ifCommandHasFiveWordsWithContractionInCorrectFormatReturnTrue()
    {
        string[] command = { "what's", "5", "divided", "by", "7" };

        Assert.True(MathCommand.commandIsInCorrectFormat(command));
    }

    [Test]
    public void ifCommandHadFiveWordsReturnTrue()
    {
        string[] command = { "what", "is", "no", "way", "me" };

        Assert.True(MathCommand.commandContainsFourFiveOrSixWords(command));
    }

    [Test]
    public void ifCommandHasSixWordsReturnTrue()
    {
        string[] command = { "what", "is", "no", "way" , "me", "too"};

        Assert.True(MathCommand.commandContainsFourFiveOrSixWords(command));
    }

    [Test]
    public void ifCommandHasLessThanFourWordsReturnFalse()
    {
        string[] command = { "what", "is", "no"};

        Assert.False(MathCommand.commandContainsFourFiveOrSixWords(command));
    }

    [Test]
    public void ifCommandHasMoreThanSixWordsReturnFalse()
    {
        string[] command = { "what", "is", "no", "way", "mean", "bat", "seven" };

        Assert.False(MathCommand.commandContainsFourFiveOrSixWords(command));
    }

    [Test]
    public void ifCommandHasNoWordsReturnFalse()
    {
        string[] command = null;

        Assert.False(MathCommand.commandContainsFourFiveOrSixWords(command));
    }

    [Test]
    public void ifCommandHasMinusOperatorReturnTrue()
    {
        string[] command = { "what", "is", "no", "minus", "mean", "bat"};

        Assert.True(MathCommand.commandHasOneOperation(command));
    }

    [Test]
    public void ifCommandHasMultipliedByOperatorReturnTrue()
    {
        string[] command = { "what", "is", "3", "multiplied", "by", "8" };

        Assert.True(MathCommand.commandHasOneOperation(command));
    }

    [Test]
    public void ifCommandHasNoOperatorReturnFalse()
    {
        string[] command = { "what", "is", "3", "something", "by", "8" };

        Assert.False(MathCommand.commandHasOneOperation(command));
    }

    [Test]
    public void ifCommandIsNulloOperatorReturnFalse()
    {
        string[] command = null;

        Assert.False(MathCommand.commandHasOneOperation(command));
    }

    [Test]
    public void ifUserSaysCorrectShortMathOperationReturnTrue()
    {
        string operation = "What is 5 plus 8";

        Assert.True(MathCommand.userSayMathOperation(operation));
    }

    [Test]
    public void ifUserSaysCorrectLongMathOperationReturnTrue()
    {
        string operation = "What is 5 divided by 8";

        Assert.True(MathCommand.userSayMathOperation(operation));
    }

    [Test]
    public void ifUserSaysCorrectContractionMathOperationReturnTrue()
    {
        string operation = "What's 5 divided by 8";

        Assert.True(MathCommand.userSayMathOperation(operation));
    }

    [Test]
    public void ifUserSaysCorrectContractionShorterMathOperationReturnTrue()
    {
        string operation = "What's 5 plus  8";

        Assert.True(MathCommand.userSayMathOperation(operation));
    }

    [Test]
    public void ifUserSaysMathOperationInInCorrectFormatReturnFalse()
    {
        string operation = "Tell me 5 plus 4";

        Assert.False(MathCommand.userSayMathOperation(operation));
    }

    [Test]
    public void ifCalculationOf5Plus2is7ReturnTrue()
    {
        Assert.True(MathCommand.calculate("plus", "5", "2").Equals("7"));
    }

    [Test]
    public void ifCalculationOf5DividedBy2is7ReturnTrue()
    {
        Debug.Log(MathCommand.calculate("divided", "5", "2"));
        Assert.True(MathCommand.calculate("divided", "5", "2").Equals("2.5"));
    }
}
