using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lljxww.ApiCaller.Client.Models;
using Lljxww.ApiCaller.Models.Config;

namespace Lljxww.ApiCaller.Client.Util;

/// <summary>
/// 配置文件工具
/// </summary>
public static class ConfigFileUtil
{
	private readonly static string CONFIG_FILE_PATH = Path.Combine(FileSystem.AppDataDirectory, "CallerClient");

	static ConfigFileUtil()
	{
		if (!Path.Exists(CONFIG_FILE_PATH)) {
			Directory.CreateDirectory(CONFIG_FILE_PATH);
		}
	}

	/// <summary>
	/// 从本地文件中加载已存储的配置文件
	/// </summary>
	/// <returns></returns>
	public static IEnumerable<ManagedApiCallerConfig> GetAllConfigInConfigFiles()
	{
		IEnumerable<ManagedApiCallerConfig> configs = Directory.EnumerateFiles(CONFIG_FILE_PATH, "*.config.txt")
			.Select(filename => JsonSerializer.Deserialize<ManagedApiCallerConfig>(File.ReadAllText(filename)))
			.OrderByDescending(c => c.ModificationTime);

		return configs;
    }

	/// <summary>
	/// 查询配置文件是否存在
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	private static bool IfConfigExist(string id)
	{
		var filePath = Path.Combine(CONFIG_FILE_PATH, $"{id}.config.txt");
		return File.Exists(filePath);
	}

	/// <summary>
	/// 删除指定的配置文件
	/// </summary>
	/// <param name="id"></param>
	public static void DeleteConfigFile(string id)
	{
		if (IfConfigExist(id))
		{
            var filePath = Path.Combine(CONFIG_FILE_PATH, $"{id}.config.txt");
			File.Delete(filePath);
		}
	}

	/// <summary>
	/// 保存文件
	/// </summary>
	/// <param name="config"></param>
	public static void SaveConfigFile(ManagedApiCallerConfig config)
	{
		var jsonContent = JsonSerializer.Serialize(config);
		var filename = $"{config.Id}.config.txt";
		var path = Path.Combine(CONFIG_FILE_PATH, filename);
		File.WriteAllText(path, jsonContent);
	}

	/// <summary>
	/// 通过ID加载指定配置
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static ManagedApiCallerConfig LoadConfigById(string id)
	{
		if (!IfConfigExist(id))
		{
			return null;
		}

		var path = Path.Combine(CONFIG_FILE_PATH, $"{id}.config.txt");
		var config = JsonSerializer.Deserialize<ManagedApiCallerConfig>(File.ReadAllText(path));
		return config;

    }
}

