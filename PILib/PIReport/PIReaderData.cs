using System;
using System.Collections.Generic;
using System.Linq;
using PISDK;
using PISDKCommon;
using PITimeServer;

namespace PILib.PIReport
{
	public class PIReaderData
	{
		public static void getRecordedValues(DateTime dt1, DateTime dt2, int maxPointsCount, SortedList<string, TagInfo> tagInfoArray, Report report) {
			/*dt1 = dt1.AddSeconds(-dt1.Second);
			dt2 = dt2.AddSeconds(-dt2.Second);*/
			foreach (TagInfoPI ti in tagInfoArray.Values) {
				PISDK.PIValues vals=null;
				if (ti.TagMode == TagModeEnum.tag) {					
					TagInfoTag tagInfo = ti as TagInfoTag;
					PISDK.PIPoint point = tagInfo.Point;
					PISDK.IPIData2 data = point.Data as PISDK.IPIData2;
					vals = data.RecordedValues(dt1.ToString(), dt2.ToString());					

					if (vals != null) {
						processVals(dt1, dt2, vals, ti, maxPointsCount, report,false);
					}
				} else {
					Tag tag=new Tag(ti, 0, dt1);
					report.addTag(tag);
				}
			}
		}

		public static void getValues2(DateTime dt1, DateTime dt2, string IntervalLen, int maxPointsCount, SortedList<string, TagInfo> tagInfoArray, Report report) {
			/*dt1 = dt1.AddSeconds(-dt1.Second);
			dt2 = dt2.AddSeconds(-dt2.Second);	*/		
			foreach (TagInfoPI ti in tagInfoArray.Values) {
				try {
					PISDKCommon.NamedValues nVals=null;
					if (ti.TagMode == TagModeEnum.tag) {
						TagInfoTag tagInfo = ti as TagInfoTag;

						Debug.trace(String.Format("Tag: {0}: {1} - {2}, {3}", ti.Title, tagInfo.TagName, ti.TagSummariesType, ti.TagCalculationBasis), report.debugId);
						bool log=ti.server.Connected;
						PISDK.PIPoint point = tagInfo.Point;

						PISDK.IPIData2 data = point.Data as PISDK.IPIData2;
						Debug.start("Summaries2", report.debugId);
						try {
						nVals = data.Summaries2(dt1.ToString(), dt2.ToString(),
							IntervalLen, tagInfo.TagSummariesType, tagInfo.TagCalculationBasis);
						} catch (Exception e) {
							nVals = null;
							Logger.error(String.Format("Summaries2 {0}, Исключение {1}", tagInfo.TagName, e.Message));
						}
						Debug.end("Summaries2", report.debugId);
					} else if (ti.TagMode == TagModeEnum.expr) {
						TagInfoExpr tagInfo = ti as TagInfoExpr;
						Debug.trace(String.Format("Expr: {0}: {1} - {2}, {3}, {4}, {5}", ti.Title, tagInfo.Expr, tagInfo.TagSummariesType,
							tagInfo.TagCalculationBasis, tagInfo.TagSampleType, tagInfo.TagSampleInterval), report.debugId);
						string expr = tagInfo.Expr;
						//Debug.trace(tagInfo.Expr);
						PISDK.Server server = tagInfo.server;
						PISDK.IPICalculation calc = server as PISDK.IPICalculation;
						Debug.start("ExpressionSummaries", report.debugId);
						try {
							nVals =
								calc.ExpressionSummaries(dt1.ToString(), dt2.ToString(), IntervalLen, expr, tagInfo.TagSummariesType, tagInfo.TagCalculationBasis,
								tagInfo.TagSampleType, tagInfo.TagSampleInterval);
						} catch (Exception e) {
							nVals = null;
							Logger.error(String.Format("ExpressionSummaries {0}, Исключение {1}", expr, e.Message));

						}
						Debug.end("ExpressionSummaries", report.debugId);
					}

					if (nVals != null) {

						object indexObj = 1;
						PISDK.PIValues vals = nVals[indexObj].Value as PISDK.PIValues;
						processVals(dt1, dt2, vals, ti, maxPointsCount, report);
					}
				} catch (Exception e) {
					Logger.error("Ошибка при получении данных: " + e.Message);

				}
			}
		}

		protected static void processVals(DateTime dt1, DateTime dt2, PIValues vals, TagInfo ti, int maxPointsCount, Report report,bool addMinutes=true) {
			if (vals.Count > maxPointsCount) {
				report.addTag(new Tag(ti, 0, dt1));
				report.addTag(new Tag(ti, 0, dt2));
				return;
			}
			Array vs;
			Array times;
			Array attrs;
			
			PISDK.IPIValues2 vals2=vals as PISDK.IPIValues2;
			vals2.GetValueArrays(out vs, out times, out attrs);
			
			for (int i=0; i < vs.Length; i++) {
				double time=(double)times.GetValue(i);
				PITimeServer.PITimeFormat tc=new PITimeServer.PITimeFormat();
				tc.UTCSeconds = time;	
				int add=addMinutes?ti.AddMinutes:0;
				DateTime dt=tc.LocalDate.AddMinutes(add);

				object v=vs.GetValue(i);
				double value=getDoubleVal(v);
				Tag tag = new Tag(ti, value, dt);
				report.addTag(tag);
			}
		}

		public static String getStringValue(PISDK.PIValue value, String numberFormat = "{0}") {
			if (value.Value.GetType().IsCOMObject) {
				PISDK.DigitalState myDigState;
				myDigState = value.Value as PISDK.DigitalState;
				if (value != null)
					return myDigState.Name;
				else
					return "&&&";
			} else {
				return String.Format(numberFormat, value.Value);
			};
		}

		public static double getDoubleVal(PISDK.PIValue value) {
			if (value.Value.GetType().IsCOMObject) {
				PISDK.DigitalState myDigState;
				myDigState = value.Value as PISDK.DigitalState;
				return myDigState.Code;

			} else {
				double v = Convert.ToDouble(value.Value);
				return v;
			};
		}

		public static double getDoubleVal(object value) {
			if (value.GetType().IsCOMObject) {
				PISDK.DigitalState myDigState;
				myDigState = value as PISDK.DigitalState;
				return myDigState.Code;

			} else {
				double v = Convert.ToDouble(value);
				return v;
			};
		}
	}
}