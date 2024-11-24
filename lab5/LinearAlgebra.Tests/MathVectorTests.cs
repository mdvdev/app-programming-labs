namespace LinearAlgebra.Tests;

public class MathVectorTests
{
    [Fact]
    public void Constructor_ValidComponents_ShouldCreateVector()
    {
        var components = new double[] { 1, 2, 3 };

        var vector = new MathVector(components);

        Assert.Equal(3, vector.Dimensions);
    }

    [Fact]
    public void Constructor_EmptyArray_ShouldCreateZeroDimensionVector()
    {
        var components = Array.Empty<double>();

        var vector = new MathVector(components);

        Assert.Equal(0, vector.Dimensions);
    }

    [Fact]
    public void Indexer_ValidIndex_ShouldReturnValue()
    {
        var vector = new MathVector(new double[] { 1, 2, 3 });

        var value = vector[1];

        Assert.Equal(2, value);
    }

    [Fact]
    public void Indexer_NegativeIndex_ShouldThrowIndexOutOfRangeException()
    {
        var vector = new MathVector(new double[] { 1, 2, 3 });

        Action act = () => _ = vector[-1];

        Assert.Throws<IndexOutOfRangeException>(act);
    }

    [Fact]
    public void Indexer_IndexOutOfRange_ShouldThrowIndexOutOfRangeException()
    {
        var vector = new MathVector(new double[] { 1, 2, 3 });

        Action act = () => _ = vector[3];

        Assert.Throws<IndexOutOfRangeException>(act);
    }

    [Fact]
    public void SumNumber_ShouldAddNumberToEachComponent_FirstElement()
    {
        var vector = new MathVector(new double[] { 1, 2, 3 });

        var result = vector.SumNumber(2);

        Assert.Equal(3, result[0]);
    }

    [Fact]
    public void MultiplyNumber_ShouldMultiplyEachComponent_FirstElement()
    {
        var vector = new MathVector(new double[] { 1, 2, 3 });

        var result = vector.MultiplyNumber(3);

        Assert.Equal(3, result[0]);
    }

    [Fact]
    public void Sum_VectorsWithSameDimensions_ShouldAddComponents_FirstElement()
    {
        var vector1 = new MathVector(new double[] { 1, 2, 3 });
        var vector2 = new MathVector(new double[] { 4, 5, 6 });

        var result = vector1.Sum(vector2);

        Assert.Equal(5, result[0]);
    }

    [Fact]
    public void Sum_VectorsWithDifferentDimensions_ShouldThrowArgumentException()
    {
        var vector1 = new MathVector(new double[] { 1, 2 });
        var vector2 = new MathVector(new double[] { 1, 2, 3 });

        Action act = () => vector1.Sum(vector2);

        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Multiply_VectorsWithSameDimensions_ShouldAddComponents_FirstElement()
    {
        var vector1 = new MathVector(new double[] { 1, 2, 3 });
        var vector2 = new MathVector(new double[] { 4, 5, 6 });
        
        var result = vector1.Multiply(vector2);
        
        Assert.Equal(4, result[0]);
    }

    [Fact]
    public void Multiply_VectorsWithDifferentDimensions_ShouldThrowArgumentException()
    {
        var vector1 = new MathVector(new double[] { 1, 2 });
        var vector2 = new MathVector(new double[] { 1, 2, 3 });
        
        Action act = () => vector1.Multiply(vector2);
        
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void ScalarMultiply_ShouldReturnCorrectResult()
    {
        var vector1 = new MathVector(new double[] { 1, 2, 3 });
        var vector2 = new MathVector(new double[] { 4, 5, 6 });

        var result = vector1.ScalarMultiply(vector2);

        Assert.Equal(32, result); // 1*4 + 2*5 + 3*6
    }

    [Fact]
    public void ScalarMultiply_DifferentDimensions_ShouldThrowArgumentException()
    {
        var vector1 = new MathVector(new double[] { 1, 2 });
        var vector2 = new MathVector(new double[] { 3, 4, 5 });

        Action act = () => vector1.ScalarMultiply(vector2);

        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void OperatorAddition_ShouldAddVectorsCorrectly()
    {
        var vector1 = new MathVector(new double[] { 1, 2 });
        var vector2 = new MathVector(new double[] { 3, 4 });

        var result = vector1 + vector2;

        Assert.Equal(4, result[0]);
    }

    [Fact]
    public void DivideByNumber_ZeroNumber_ShouldThrowDivideByZeroException()
    {
        var vector = new MathVector(new double[] { 1, 2 });

        Action act = () => _ = vector / 0;

        Assert.Throws<DivideByZeroException>(act);
    }

    [Fact]
    public void DivideByVector_ZeroVector_ShouldThrowDivideByZeroException()
    {
        var vector1 = new MathVector(new double[] { 1, 2 });
        var vector2 = new MathVector(new double[] { 0, 0 });
        
        Action act = () => _ = vector1 / vector2;
        
        Assert.Throws<DivideByZeroException>(act);
    }

    [Fact]
    public void CalcDistance_ShouldReturnCorrectResult()
    {
        var vector1 = new MathVector(new double[] { 1, 2, 3 });
        var vector2 = new MathVector(new double[] { 4, 5, 6 });

        var result = vector1.CalcDistance(vector2);

        Assert.Equal(Math.Sqrt(27), result, precision: 5);
    }
}