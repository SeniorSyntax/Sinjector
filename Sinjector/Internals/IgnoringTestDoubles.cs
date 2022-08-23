using System;
using Autofac;

namespace Sinjector.Internals
{
	internal class IgnoringTestDoubles : ContainerAdaptor
	{
		public IgnoringTestDoubles(ContainerBuilder builder, ExtensionQuerier extensions) : base(null, builder, extensions)
		{
		}

		public override ITestDoubleFor UseTestDouble<TTestDouble>()
		{
			return new ignoreTestDouble();
		}

		public override ITestDoubleFor UseTestDouble<TTestDouble>(TTestDouble instance)
		{
			return new ignoreTestDouble();
		}

		public override ITestDoubleFor UseTestDoubleForType(Type type)
		{
			return new ignoreTestDouble();
		}

		private class ignoreTestDouble : ITestDoubleFor
		{
			public void For<T>()
			{
			}

			public void For<T1, T2>()
			{
			}

			public void For<T1, T2, T3>()
			{
			}

			public void For<T1, T2, T3, T4>()
			{
			}

			public void For<T1, T2, T3, T4, T5>()
			{
			}

			public void For<T1, T2, T3, T4, T5, T6>()
			{
			}

			public void For<T1, T2, T3, T4, T5, T6, T7>()
			{
			}

			public void For(Type type)
			{
			}
		}
	}
}
