using System;
using Lljxww.ApiCaller.Models.Config;

namespace Lljxww.ApiCaller.Client.Models
{
	/// <summary>
	/// 用于后台使用的ApiCallerConfig对象
	/// </summary>
	public class ManagedApiCallerConfig
	{
		public string Id { get; set; }

		/// <summary>
		/// 配置名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		public string Note { get; set; }

		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime ModificationTime { get; set; }

		/// <summary>
		/// 配置信息
		/// </summary>
		public ApiCallerConfig Config { get; set; }
	}
}

