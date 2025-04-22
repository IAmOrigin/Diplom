using System;

namespace _28._01ui
{
	internal class ProjectDirectory
	{
		public static string GetProjectDirectory()
		{
			// Получаем путь к директории, из которой запущено приложение
			string projectPath = AppDomain.CurrentDomain.BaseDirectory;
			// Если путь заканчивается на "bin\Debug\" или "bin\Release\", то удаляем эту часть
			if (projectPath.EndsWith("bin\\Debug\\") || projectPath.EndsWith("bin\\Release\\"))
			{
				projectPath = projectPath.Substring(0, projectPath.LastIndexOf("bin\\"));
			}
			return projectPath;
		}
	}
}
