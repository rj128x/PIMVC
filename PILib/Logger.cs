using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Config;

namespace PILib
{
	public static class Logger
	{
		public static log4net.ILog logger;
		static Logger() {
			
		}
		public static void init(string path, string name) {
			string fileName=String.Format("{0}/{1}_{2}.txt", path, name, DateTime.Now.ToString().Replace(":","_").Replace(".","_"));
			PatternLayout layout = new PatternLayout(@"[%d] %5p - %m%n");
			FileAppender appender=new FileAppender();
			appender.Layout = layout;
			appender.File = fileName;
			appender.AppendToFile = true;
			BasicConfigurator.Configure(appender);
			appender.ActivateOptions();
			logger=LogManager.GetLogger(name);			
		}

		public static void info(string str) {
			logger.Info(str);
		}

		public static void error(string str) {
			logger.Error(str);
		}

		public static void debug(string str) {
			logger.Debug(str);
		}

	}
}
