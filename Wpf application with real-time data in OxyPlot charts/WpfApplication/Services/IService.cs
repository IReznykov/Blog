namespace WpfApplication.Services
{
	public interface IService
	{
		/// <summary>
		/// Start service.
		/// </summary>
		void OnStart();

		/// <summary>
		/// Stop service, cleanup data.
		/// </summary>
		void OnStop();
	}
}