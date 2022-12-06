using System;
using System.Collections.ObjectModel;
using Lljxww.ApiCaller.Client.Util;
using Lljxww.ApiCaller.Models.Config;

namespace Lljxww.ApiCaller.Client.Models
{
	public class CallerConfig
	{
		public ObservableCollection<ManagedApiCallerConfig> Configs { get; set; }
			= new ObservableCollection<ManagedApiCallerConfig>();

		public CallerConfig() => LoadConfigs();

		public void LoadConfigs()
		{
			Configs.Clear();

			var configs = ConfigFileUtil.GetAllConfigInConfigFiles();

			foreach(var config in configs)
			{
				Configs.Add(config);
			}
		}
	}
}

