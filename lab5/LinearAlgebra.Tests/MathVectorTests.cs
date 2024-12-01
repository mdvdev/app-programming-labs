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
    public void GetIndexer_ValidIndex_ShouldReturnValue()
    {
        var vector = new MathVector([1, 2, 3]);

        var value = vector[1];

        Assert.Equal(2, value);
    }

    [Fact]
    public void GetIndexer_NegativeIndex_ShouldThrowIndexOutOfRangeException()
    {
        var vector = new MathVector([1, 2, 3]);

        Action act = () => _ = vector[-1];

        Assert.Throws<IndexOutOfRangeException>(act);
    }

    [Fact]
    public void GetIndexer_IndexOutOfRange_ShouldThrowIndexOutOfRangeException()
    {
        var vector = new MathVector([1, 2, 3]);

        Action act = () => _ = vector[3];

        Assert.Throws<IndexOutOfRangeException>(act);
    }

    [Fact]
    public void SetIndexer_ValidIndex_ShouldReturnValue()
    {
        var vector = new MathVector([1, 2, 3]);

        vector[1] = 4;
        
        Assert.Equal(4, vector[1]);
    }

    [Fact]
    public void SetIndexer_NegativeIndex_ShouldThrowIndexOutOfRangeException()
    {
        var vector = new MathVector([1, 2, 3]);

        Action act = () => vector[-1] = 4;
        
        Assert.Throws<IndexOutOfRangeException>(act);
    }

    [Fact]
    public void SetIndexer_IndexOutOfRange_ShouldThrowIndexOutOfRangeException()
    {
        var vector = new MathVector([1, 2, 3]);
        
        Action act = () => vector[3] = 4;
        
        Assert.Throws<IndexOutOfRangeException>(act);
    }

    [Fact]
    public void Length_ShouldReturnCorrectLength()
    {
        var vector = new MathVector([1, 2, 3]);
        
        var length = vector.Length;
        
        Assert.Equal(Math.Sqrt(1 + 4 + 9), length);
    }

    [Fact]
    public void SumNumber_ShouldAddNumberToEachComponent_FirstElement()
    {
        var vector = new MathVector([1, 2, 3]);

        var result = vector.SumNumber(2);

        Assert.Equal(3, result[0]);
    }

    [Fact]
    public void MultiplyNumber_ShouldMultiplyEachComponent_FirstElement()
    {
        var vector = new MathVector([1, 2, 3]);

        var result = vector.MultiplyNumber(3);

        Assert.Equal(3, result[0]);
    }

    [Fact]
    public void Sum_VectorsWithSameDimensions_ShouldAddComponents_FirstElement()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([4, 5, 6]);

        var result = vector1.Sum(vector2);

        Assert.Equal(5, result[0]);
    }

    [Fact]
    public void Sum_VectorsWithDifferentDimensions_ShouldThrowArgumentException()
    {
        var vector1 = new MathVector([1, 2]);
        var vector2 = new MathVector([1, 2, 3]);

        Action act = () => vector1.Sum(vector2);

        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Sum_NullVector_ShouldThrowArgumentNullException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        IMathVector vector2 = null;
        
        Action act = () => vector1.Sum(vector2);
        
        Assert.Throws<ArgumentNullException>(act);
    }
    
    [Fact]
    public void Multiply_VectorsWithSameDimensions_ShouldAddComponents_FirstElement()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([4, 5, 6]);
        
        var result = vector1.Multiply(vector2);
        
        Assert.Equal(4, result[0]);
    }

    [Fact]
    public void Multiply_VectorsWithDifferentDimensions_ShouldThrowArgumentException()
    {
        var vector1 = new MathVector([1, 2]);
        var vector2 = new MathVector([1, 2, 3]);
        
        Action act = () => vector1.Multiply(vector2);
        
        Assert.Throws<ArgumentException>(act);
    }
    
    [Fact]
    public void Multiply_NullVector_ShouldThrowArgumentNullException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        IMathVector vector2 = null;

        Action act = () => vector1.Multiply(vector2);

        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void ScalarMultiply_ShouldReturnCorrectResult()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([4, 5, 6]);

        var result = vector1.ScalarMultiply(vector2);

        Assert.Equal(32, result); // 1*4 + 2*5 + 3*6
    }

    [Fact]
    public void ScalarMultiply_DifferentDimensions_ShouldThrowArgumentException()
    {
        var vector1 = new MathVector([1, 2]);
        var vector2 = new MathVector([3, 4, 5]);

        Action act = () => vector1.ScalarMultiply(vector2);

        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void ScalarMultiply_NullVector_ShouldThrowArgumentNullException()
    {
        var vector1 = new MathVector([1, 2]);
        IMathVector vector2 = null;

        Action act = () => vector1.ScalarMultiply(vector2);

        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void CalcDistance_VectorsWithSameDimensions_ShouldReturnCorrectResult()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([4, 5, 6]);

        var result = vector1.CalcDistance(vector2);

        Assert.Equal(Math.Sqrt(27), result, precision: 5);
    }

    [Fact]
    public void CalcDistance_VectorsWithDifferentDimensions_ShouldThrowArgumentException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([4, 5]);
        
        Action act = () => vector1.CalcDistance(vector2);
        
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void CalcDistance_NullVector_ShouldThrowArgumentNullException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        IMathVector vector2 = null;
        
        Action act = () => vector1.CalcDistance(vector2);
        
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void GetEnumerator_ShouldReturnCorrectResult()
    {
        var vector = new MathVector([1, 2, 3]);
        
        var result = vector.GetEnumerator();
        
        result.MoveNext();
        Assert.Equal((double)1, result.Current);
        result.MoveNext();
        Assert.Equal((double)2, result.Current);
        result.MoveNext();
        Assert.Equal((double)3, result.Current);
    }
    
    [Fact]
    public void OperatorAddition_VectorsWithSameDimensions_ShouldReturnCorrectResult()
    {
        var vector1 = new MathVector([1, 2]);
        var vector2 = new MathVector([3, 4]);

        var result = vector1 + vector2;

        Assert.Equal(4, result[0]);
        Assert.Equal(6, result[1]);
    }

    [Fact]
    public void OperatorAddition_VectorsWithDifferentDimensions_ShouldThrowArgumentException()
    {
        var vector1 = new MathVector([1, 2]);
        var vector2 = new MathVector([3]);
        
        Action act = () => _ = vector1 + vector2;
        
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void OperatorAddition_NullVector_ShouldThrowArgumentNullException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        MathVector vector2 = null;
        
        Action act = () => _ = vector1 + vector2;
        
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void OperatorAdditionWithNumber_ShouldAddNumberToEachComponent()
    {
        var vector = new MathVector([1, 2, 3]);
        
        var result = vector + 5;
        
        Assert.Equal(6, result[0]);
        Assert.Equal(7, result[1]);
        Assert.Equal(8, result[2]);
    }

    [Fact]
    public void OperatorSubtraction_VectorsWithSameDimensions_ShouldReturnCorrectResult()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([4, 6, 10]);
        
        var result = vector1 - vector2;
        
        Assert.Equal(-3, result[0]);
        Assert.Equal(-4, result[1]);
        Assert.Equal(-7, result[2]);
    }

    [Fact]
    public void OperatorSubtraction_VectorsWithDifferentDimensions_ShouldThrowArgumentException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([4, 5]);
        
        Action act = () => _ = vector1 - vector2;
        
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void OperatorSubtraction_NullVector_ShouldThrowArgumentNullException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        MathVector vector2 = null;
        
        Action act = () => _ = vector1 - vector2;
        
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void OperatorMultiplication_VectorsWithSameDimensions_ShouldReturnCorrectResult()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([4, 5, 6]);
        
        var result = vector1 * vector2;
        
        Assert.Equal(4, result[0]);
        Assert.Equal(10, result[1]);
        Assert.Equal(18, result[2]);
    }

    [Fact]
    public void OperatorMultiplication_VectorsWithDifferentDimensions_ShouldThrowArgumentException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([4, 5]);
        
        Action act = () => _ = vector1 * vector2;
        
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void OperatorMultiplication_NullVector_ShouldThrowArgumentNullException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        MathVector vector2 = null;
        
        Action act = () => _ = vector1 * vector2;
        
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void OperatorMultiplicationByNumber_ShouldMultiplyEachComponent()
    {
        var vector = new MathVector([1, 2, 3]);
        
        var result = vector * 5;
        
        Assert.Equal(5, result[0]);
        Assert.Equal(10, result[1]);
        Assert.Equal(15, result[2]);
    }

    [Fact]
    public void OperatorDivision_VectorsWithSameDimensions_ShouldReturnCorrectResult()
    {
        var vector1 = new MathVector([4, 6, 10]);
        var vector2 = new MathVector([2, 3, 5]);
        
        var result = vector1 / vector2;
        
        Assert.Equal(2, result[0]);
        Assert.Equal(2, result[1]);
        Assert.Equal(2, result[2]);
    }

    [Fact]
    public void OperatorDivision_VectorsWithDifferentDimensions_ShouldThrowArgumentException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([4, 5]);
        
        Action act = () => _ = vector1 / vector2;
        
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void OperatorDivision_NullVector_ShouldThrowArgumentNullException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        MathVector vector2 = null;
        
        Action act = () => _ = vector1 / vector2;
        
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void OperatorDivision_ZeroComponent_ShouldThrowDivideByZeroException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([0, 0, 0]);
        
        Action act = () => _ = vector1 / vector2;
        
        Assert.Throws<DivideByZeroException>(act);
    }

    [Fact]
    public void OperatorDivisionByNumber_NotZeroNumber_ShouldDivideEachComponent()
    {
        var vector = new MathVector([2, 4, 6]);
        
        var result = vector / 2;
        
        Assert.Equal(1, result[0]);
        Assert.Equal(2, result[1]);
        Assert.Equal(3, result[2]);
    }

    [Fact]
    public void OperatorDivisionByNumber_ZeroNumber_ShouldThrowDivideByZeroException()
    {
        var vector = new MathVector([1, 2]);

        Action act = () => _ = vector / 0;

        Assert.Throws<DivideByZeroException>(act);
    }

    [Fact]
    public void OperatorModulus_VectorsWithSameDimensions_ShouldReturnCorrectResult()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([4, 5, 6]);
        
        var result = vector1 % vector2;
        
        Assert.Equal(32, result); // 1*4 + 2*5 + 3*6
    }

    [Fact]
    public void OperatorModulus_VectorsWithDifferentDimensions_ShouldThrowArgumentException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        var vector2 = new MathVector([4, 5]);
        
        Action act = () => _ = vector1 % vector2;
        
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void OperatorModulus_NullVector_ShouldThrowArgumentNullException()
    {
        var vector1 = new MathVector([1, 2, 3]);
        MathVector vector2 = null;
        
        Action act = () => _ = vector1 % vector2;
        
        Assert.Throws<ArgumentNullException>(act);
    }
}