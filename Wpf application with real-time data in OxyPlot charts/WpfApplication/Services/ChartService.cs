using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Ikc5.TypeLibrary;
using WpfApplication.Models;

namespace WpfApplication.Services
{
	public class ChartService : IService
	{
		private readonly IChartRepository _chartRepository;

		public ChartService(IChartRepository chartRepository)
		{
			chartRepository.ThrowIfNull(nameof(chartRepository));
			_chartRepository = chartRepository;

			_countTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(250),
				IsEnabled = false
			};
			_countTimer.Tick += CountTimerTick;
		}

		#region IChartService

		/// <summary>
		/// Start service.
		/// </summary>
		public void OnStart()
		{
			_countTimer.Start();
		}

		/// <summary>
		/// Stop service, cleanup data.
		/// </summary>
		public void OnStop()
		{
			_countTimer.Stop();
		}

		#endregion

		#region Service data
		private readonly DispatcherTimer _countTimer;
		private readonly Random _countRandom = new Random(987654321);
		private readonly Random _indexRandom = new Random(123456789);

		/// <summary>
		/// Method emulates new data.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CountTimerTick(object sender, EventArgs e)
		{
			var value = _countRandom.Next(150);
			_chartRepository.AddLineCount(value);

			var index = _indexRandom.Next(50);
			value = _countRandom.Next(100);
			_chartRepository.AddColumnCount(index, value);

			_countTimer.Start();
		}

		#endregion
	}
}
