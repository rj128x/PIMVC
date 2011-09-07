using System;
using System.Collections.Generic;
using System.Linq;

namespace PILib
{
	public class Debug
	{
		protected static Debug empty;
		protected static Dictionary<string,Debug>debugs;
		public Dictionary<string, long> vals;
		public Dictionary<string, long> starts;
		public List<string> messages;
		public static Random random=new Random();

		static Debug(){
			debugs = new Dictionary<string, Debug>();
		}
		protected Debug() {
			vals = new Dictionary<string, long>();
			starts = new Dictionary<string, long>();
			messages = new List<string>();
		}

		protected void start(string name) {
			if (!starts.Keys.Contains(name))
				starts.Add(name, DateTime.Now.Ticks);
			starts[name] = DateTime.Now.Ticks;
		}

		protected void end(string name) {
			if (!vals.Keys.Contains(name))
				vals.Add(name, 0);
			vals[name] = vals[name]+DateTime.Now.Ticks - starts[name];
		}

		protected void traceVal(string name, string allName) {
			//HttpContext.Current.Trace.Write(String.Format("{0}: {1:0.00}/{2:0.00} ({3:0.00})", name, vals[name], vals[allName], (float)vals[name] / (float)vals[allName] * 100));
			double valName=vals[name] / 10000000.0;
			double valAllName=vals[allName] / 10000000.0;
			//System.Diagnostics.Trace.Write(String.Format("{0}: {1:0.00}/{2:0.00} ({3:0.00})", name, valName, valAllName, (float)vals[name] / (float)vals[allName] * 100));
			//HttpContext.Current.Trace.Write(String.Format("{0}: {1:0.00}/{2:0.00} ({3:0.00})", name, valName, valAllName, (float)vals[name] / (float)vals[allName] * 100));
			Logger.debug(String.Format("{0}: {1:0.00}/{2:0.00} ({3:0.00})", name, valName, valAllName, (double)vals[name] / (double)vals[allName] * 100.0));
		}

		protected void traceAll(string allName) {
			Logger.debug("==============================================");
			foreach (KeyValuePair<string, long> de in vals) {
				traceVal(de.Key, allName);
			}
			foreach (string m in messages) {
				Logger.debug(String.Format("{0}",m));
			}
			Logger.debug("==============================================");
		}

		protected void trace(string str) {
			messages.Add(str);
		}


		protected static Debug getDebug(string idObj){
			Debug debug;
			if (idObj==null){
				if (empty==null)
					empty=new Debug();
				return empty;
			}
			else {
				if (!debugs.Keys.Contains(idObj)){
					debug=new Debug();
					debugs.Add(idObj,debug);
				}else{
					debug=debugs[idObj];
				}
				return debug;
			}
		}

		public static void start(string name,string idObj=null) {
			getDebug(idObj).start(name);
		}

		public static void end(string name,string idObj=null ) {
			getDebug(idObj).end(name);
		}


		public static void traceAll(string allName,string idObj=null,bool remove = true) {
			getDebug(idObj).traceAll(allName);
			if (remove) {
				try {
					Debug.debugs.Remove(idObj);
				} catch { }
			}
		}

		public static void trace(string str,string idObj=null) {
			getDebug(idObj).trace(String.Format("{0}: {1}",DateTime.Now,str));
		}		
		
	}

}