using System;

namespace Sinjector;

public interface IIsolate : IQueryAttributes
{
	ITestDoubleFor UseTestDouble<TTestDouble>() where TTestDouble : class;
	ITestDoubleFor UseTestDouble<TTestDouble>(TTestDouble instance) where TTestDouble : class;
	ITestDoubleFor UseTestDoubleForType(Type type);
}