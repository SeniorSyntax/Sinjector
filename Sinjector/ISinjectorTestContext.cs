namespace Sinjector
{
	public interface ISinjectorTestContext
	{
		void SimulateShutdown();
		void SimulateRestart();
		void SimulateNewRequest();
	}
}