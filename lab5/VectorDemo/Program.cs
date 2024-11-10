namespace VectorDemo;

using LinearAlgebra;

public static class Assert
{
    public static void Equal(double expected, double actual, double tolerance = 1e-6)
    {
        if (Math.Abs(expected - actual) > tolerance)
            throw new Exception($"Assert.Equal Failed: Expected {expected}, but got {actual}");
    }

    public static void Equal(int expected, int actual)
    {
        if (expected != actual)
            throw new Exception($"Assert.Equal Failed: Expected {expected}, but got {actual}");
    }

    public static void True(bool condition)
    {
        if (!condition)
            throw new Exception("Assert.True Failed: Condition is not true");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Test: Adding a number to each component of the vector
        TestSumNumber();

        // Test: Multiplying each component of the vector by a number
        TestMultiplyNumber();

        // Test: Adding two vectors together
        TestSum();

        // Test: Element-wise multiplication of two vectors
        TestMultiply();

        // Test: Scalar product of two vectors
        TestScalarMultiply();

        // Test: Distance calculation between two vectors
        TestDistance();

        // Test: Accessing and setting vector components using indexer
        TestIndexer();

        // Test: Vector dimension property
        TestDimensions();

        // Test: Enumerator iteration over vector components
        TestEnumerator();

        // Test: Vector addition operator
        TestOperatorAddition();

        // Test: Vector subtraction operator
        TestOperatorSubtraction();

        // Test: Element-wise multiplication operator
        TestOperatorMultiplication();

        // Test: Element-wise division operator
        TestOperatorDivision();

        // Test: Scalar multiplication operator
        TestOperatorScalarMultiply();

        Console.WriteLine("All tests passed successfully!");
    }

    // Test 1: Adding a number to each component of the vector
    static void TestSumNumber()
    {
        var vector = new MathVector(new double[] { 1, 2, 3 });
        var sumWithNumber = vector.SumNumber(2);
        Assert.Equal(3, sumWithNumber[0]);
        Assert.Equal(4, sumWithNumber[1]);
        Assert.Equal(5, sumWithNumber[2]);
    }

    // Test 2: Multiplying each component of the vector by a number
    static void TestMultiplyNumber()
    {
        var vector = new MathVector(new double[] { 1, 2, 3 });
        var multiplyWithNumber = vector.MultiplyNumber(2);
        Assert.Equal(2, multiplyWithNumber[0]);
        Assert.Equal(4, multiplyWithNumber[1]);
        Assert.Equal(6, multiplyWithNumber[2]);
    }

    // Test 3: Adding two vectors together
    static void TestSum()
    {
        var vector1 = new MathVector(new double[] { 1, 2, 3 });
        var vector2 = new MathVector(new double[] { 4, 5, 6 });
        var sumVector = vector1.Sum(vector2);
        Assert.Equal(5, sumVector[0]);
        Assert.Equal(7, sumVector[1]);
        Assert.Equal(9, sumVector[2]);
    }

    // Test 4: Element-wise multiplication of two vectors
    static void TestMultiply()
    {
        var vector1 = new MathVector(new double[] { 1, 2, 3 });
        var vector2 = new MathVector(new double[] { 4, 5, 6 });
        var multiplyVector = vector1.Multiply(vector2);
        Assert.Equal(4, multiplyVector[0]);
        Assert.Equal(10, multiplyVector[1]);
        Assert.Equal(18, multiplyVector[2]);
    }

    // Test 5: Scalar product of two vectors
    static void TestScalarMultiply()
    {
        var vector1 = new MathVector(new double[] { 1, 2, 3 });
        var vector2 = new MathVector(new double[] { 4, 5, 6 });
        var result = vector1.ScalarMultiply(vector2);
        Assert.Equal(32, result); // 1*4 + 2*5 + 3*6 = 32
    }

    // Test 6: Distance calculation between two vectors
    static void TestDistance()
    {
        var vector1 = new MathVector(new double[] { 1, 2, 3 });
        var vector2 = new MathVector(new double[] { 4, 5, 6 });
        var distance = vector1.CalcDistance(vector2);
        Assert.Equal(Math.Sqrt(27), distance);
    }

    // Test 7: Accessing and setting vector components using indexer
    static void TestIndexer()
    {
        var vector = new MathVector(new double[] { 1, 2, 3 });
        Assert.Equal(1, vector[0]);
        Assert.Equal(2, vector[1]);
        vector[1] = 5;
        Assert.Equal(5, vector[1]);
    }

    // Test 8: Vector dimension property
    static void TestDimensions()
    {
        var vector = new MathVector(new double[] { 1, 2, 3 });
        Assert.Equal(3, vector.Dimensions);
    }

    // Test 9: Enumerator iteration over vector components
    static void TestEnumerator()
    {
        var vector = new MathVector(new double[] { 1, 2, 3 });
        var count = 0;
        foreach (var component in vector)
        {
            Assert.Equal((double)vector[count], (double)component);
            count++;
        }
    }

    // Test 10: Vector addition operator
    static void TestOperatorAddition()
    {
        var vector1 = new MathVector(new double[] { 1, 2, 3 });
        var vector2 = new MathVector(new double[] { 4, 5, 6 });
        var result = vector1 + vector2;
        Assert.Equal(5, result[0]);
        Assert.Equal(7, result[1]);
        Assert.Equal(9, result[2]);
    }

    // Test 11: Vector subtraction operator
    static void TestOperatorSubtraction()
    {
        var vector1 = new MathVector(new double[] { 5, 6, 7 });
        var vector2 = new MathVector(new double[] { 1, 2, 3 });
        var result = vector1 - vector2;
        Assert.Equal(4, result[0]);
        Assert.Equal(4, result[1]);
        Assert.Equal(4, result[2]);
    }

    // Test 12: Element-wise multiplication operator
    static void TestOperatorMultiplication()
    {
        var vector1 = new MathVector(new double[] { 1, 2, 3 });
        var vector2 = new MathVector(new double[] { 4, 5, 6 });
        var result = vector1 * vector2;
        Assert.Equal(4, result[0]);
        Assert.Equal(10, result[1]);
        Assert.Equal(18, result[2]);
    }

    // Test 13: Element-wise division operator
    static void TestOperatorDivision()
    {
        var vector1 = new MathVector(new double[] { 4, 6, 8 });
        var vector2 = new MathVector(new double[] { 2, 3, 4 });
        var result = vector1 / vector2;
        Assert.Equal(2, result[0]);
        Assert.Equal(2, result[1]);
        Assert.Equal(2, result[2]);
    }

    // Test 14: Scalar multiplication operator
    static void TestOperatorScalarMultiply()
    {
        var vector = new MathVector(new double[] { 1, 2, 3 });
        var result = vector * 2;
        Assert.Equal(2, result[0]);
        Assert.Equal(4, result[1]);
        Assert.Equal(6, result[2]);
    }
}