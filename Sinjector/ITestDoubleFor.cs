using System;

namespace Sinjector;

public interface ITestDoubleFor
{
	void For<T>();
	void For<T1, T2>();
	void For<T1, T2, T3>();
	void For<T1, T2, T3, T4>();
	void For<T1, T2, T3, T4, T5>();
	void For<T1, T2, T3, T4, T5, T6>();
	void For<T1, T2, T3, T4, T5, T6, T7>();
	void For(Type type);
}