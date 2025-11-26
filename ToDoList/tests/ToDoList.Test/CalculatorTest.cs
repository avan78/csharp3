namespace ToDoList.Test;

public class CalculatorTest
{
    [Fact]
    public void Calculator_Add_ShouldReturnCorrectResult()
    {
        // Arrange
        var calculator = new Calculator();

        // Act
        var result = calculator.Add(2, 3);

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Calculator_Divide_ShouldReturnCorrectResult()
    {
        // Arrange
        var calculator = new Calculator();

        // Act
        var result = calculator.Divide(10, 2);

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Calculator_Divide_ThrowZero()
    {
        // Arrange
        var calculator = new Calculator();

        // Act + Assert
        Assert.Throws<DivideByZeroException>(() => calculator.Divide(6, 0));
    }
}

public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
    public int Divide(int a, int b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }
        return a / b;
    }
}
