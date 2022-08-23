namespace Sinjector
{
	public interface IIoCTestContext
	{
		void SimulateShutdown();
		void SimulateRestart();
		void SimulateNewRequest();
	}
}