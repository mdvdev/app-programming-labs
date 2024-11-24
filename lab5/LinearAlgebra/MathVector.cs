using System.Collections;

namespace LinearAlgebra;

public class MathVector(double[] components) : IMathVector
{
    private readonly double[] _components = components;

    public int Dimensions => _components.Length;

    public double this[int i]
    {
        get
        {
            if (i < 0 || i >= Dimensions)
                throw new IndexOutOfRangeException();
            return _components[i];
        }
        set
        {
            if (i < 0 || i >= Dimensions)
                throw new IndexOutOfRangeException();
            _components[i] = value;
        }
    }
    
    public double Length => Math.Sqrt(_components.Select(x => x * x).Sum());

    public IMathVector SumNumber(double number)
    {
        return new MathVector(_components.Select(x => x + number).ToArray());
    }

    public IMathVector MultiplyNumber(double number)
    {
        return new MathVector(_components.Select(x => x * number).ToArray());
    }

    public IMathVector Sum(IMathVector vector)
    {
        if (vector.Dimensions != Dimensions)
            throw new ArgumentException("Dimensions must be the same length");
        
        var result = new double[Dimensions];
        for (var i = 0; i < Dimensions; i++)
            result[i] = _components[i] + vector[i];

        return new MathVector(result);
    }

    public IMathVector Multiply(IMathVector vector)
    {
        if (vector.Dimensions != Dimensions)
            throw new ArgumentException("Dimensions must be the same length");
        
        var result = new double[Dimensions];
        for (var i = 0; i < Dimensions; i++)
            result[i] = _components[i] * vector[i];
        
        return new MathVector(result);
    }

    public double ScalarMultiply(IMathVector vector)
    {
        if (vector.Dimensions != Dimensions)
            throw new ArgumentException("Dimensions must be the same length");
        
        var result = 0.0;
        for (var i = 0; i < Dimensions; i++)
            result += _components[i] * vector[i];
        
        return result;
    }

    public double CalcDistance(IMathVector vector)
    {
        if (vector.Dimensions != Dimensions)
            throw new ArgumentException("Dimensions must be the same length");

        var result = 0.0;
        for (var i = 0; i < Dimensions; i++)
            result += Math.Pow(_components[i] - vector[i], 2);
        
        return Math.Sqrt(result);
    }

    public IEnumerator GetEnumerator()
    {
        return _components.GetEnumerator();
    }
    
    public static IMathVector operator +(MathVector v1, MathVector v2)
    {
        return v1.Sum(v2);
    }

    public static IMathVector operator +(MathVector v1, double n)
    {
        return v1.SumNumber(n);
    }

    public static IMathVector operator -(MathVector v1, MathVector v2)
    {
        if (v1.Dimensions != v2.Dimensions)
            throw new ArgumentException("Dimensions must be the same length");
        
        var result = new double[v1.Dimensions];
        for (var i = 0; i < v1.Dimensions; i++)
            result[i] = v1[i] - v2[i];

        return new MathVector(result);
    }

    public static IMathVector operator -(MathVector v1, double n)
    {
        return new MathVector(v1._components.Select(x => x - n).ToArray());
    }

    public static IMathVector operator *(MathVector v1, MathVector v2)
    {
        return v1.Multiply(v2);
    }

    public static IMathVector operator *(MathVector v1, double n)
    {
        return v1.MultiplyNumber(n);
    }

    public static IMathVector operator /(MathVector v1, MathVector v2)
    {
        if (v1.Dimensions != v2.Dimensions)
            throw new ArgumentException("Dimensions must be the same length");

        var result = new double[v1.Dimensions];
        for (var i = 0; i < v1.Dimensions; i++)
        {
            if (v2[i] == 0)
                throw new DivideByZeroException("Division by zero is not allowed");
            result[i] = v1[i] / v2[i];
        }

        return new MathVector(result);
    }

    public static IMathVector operator /(MathVector v1, double n)
    {
        if (n == 0)
            throw new DivideByZeroException("Division by zero is not allowed");
        
        return new MathVector(v1._components.Select(x => x / n).ToArray());
    }

    public static double operator %(MathVector v1, MathVector v2)
    {
        return v1.ScalarMultiply(v2);
    }
}