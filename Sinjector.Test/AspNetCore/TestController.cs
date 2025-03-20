using Microsoft.AspNetCore.Mvc;

namespace Sinjector.Test.AspNetCore
{
	[ApiController]
	public class TestController : ControllerBase
	{
		private readonly ITestService _service;

		public TestController(ITestService service)
		{
			_service = service;
		}

		[HttpGet]
		[Route("controller/value")]
		public string ControllerValue() => "controller";

		[HttpGet]
		[Route("service/value")]
		public string ServiceValue() => _service.Value();

	}
}
