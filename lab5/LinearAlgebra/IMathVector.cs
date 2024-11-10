﻿namespace LinearAlgebra;

using System.Collections;

public interface IMathVector : IEnumerable {
    int Dimensions { get; }
    
    double this[int i] { get; set; }
    
    double Length { get; }
    
    IMathVector SumNumber(double number);
    
    IMathVector MultiplyNumber(double number);
    
    IMathVector Sum(IMathVector vector);
    
    IMathVector Multiply(IMathVector vector);
    
    double ScalarMultiply(IMathVector vector);
    
    double CalcDistance(IMathVector vector);
}