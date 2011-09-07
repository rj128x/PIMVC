using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SilverChart.ChartDataServiceReference;
using Visifire.Charts;
using System.ComponentModel;
using System.Collections.Generic;

namespace SilverChart
{
	public class SilverChartSerie : INotifyPropertyChanged
	{
		public SilverChartControl silverChartControl;
		public DataSeries DataSerie { get; protected set; }
		public event PropertyChangedEventHandler PropertyChanged;
		public string TagName { get; protected set; }

		protected bool secondaryYAxis;
		public bool SecondaryYAxis {
			get { return secondaryYAxis; }
			set { 
				secondaryYAxis = value;
				DataSerie.AxisYType = secondaryYAxis ? AxisTypes.Secondary : AxisTypes.Primary;				
				NotifyChanged("SecondaryYAxis"); 
			}
		}

		protected bool enabled;
		public bool Enabled {
			get { return enabled; }
			set { 
				enabled = value;
				DataSerie.Enabled = enabled;
				NotifyChanged("Enabled");  
			}
		}

		protected bool labelEnabled;
		public bool LabelEnabled {
			get { return labelEnabled; }
			set {
				labelEnabled = value;
				DataSerie.LabelEnabled = labelEnabled;
				NotifyChanged("LabelEnabled");
			}
		}


		protected string name;
		public string Name {
			get { return name; }
			set { 
				name = value;
				DataSerie.Name = Name;
				NotifyChanged("Name"); 
			}
		}

		protected int opacity=100;
		public int Opacity {
			get { return opacity; }
			set {
				opacity = value;
				DataSerie.Opacity = Opacity/100.0;
				NotifyChanged("Opacity");
			}
		}

		protected int[] opacities=new int[] { 20,40,60,80,100 };
		public int[] Opacities{
			get {
				return opacities;
			}
		}

		protected ChartSerieType serieType;
		public ChartSerieType SerieType {
			get { return serieType; }
			set {				
				bool ok=changeType(value);
				if (ok) {
					serieType = value;
				}
				NotifyChanged("SerieType");				
			}
		}

		public string SerieTypeStr {
			get { return SilverChartSerieTypes.getByID(SerieType); }
			set {
				SerieType = SilverChartSerieTypes.getByName(value);
				NotifyChanged("SerieTypeStr");		
			}
		}

		public List<string> Types {
			get { return SilverChartSerieTypes.TypeNames; }
		}


		public SilverChartSerie(SilverChartControl silverChartControl) {
			this.silverChartControl = silverChartControl;
		}

		public void NotifyChanged(string propName) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		

		public bool changeType(ChartSerieType type) {
			try {
				switch (type) {
					case ChartSerieType.area:
						DataSerie.RenderAs = RenderAs.Area;
						LabelEnabled = false;
						break;					
					case ChartSerieType.bar:
						DataSerie.RenderAs = RenderAs.Bar;
						LabelEnabled = true;
						break;
					case ChartSerieType.column:
						DataSerie.RenderAs = RenderAs.Column;
						LabelEnabled = true;
						break;
					case ChartSerieType.line:
						DataSerie.RenderAs = RenderAs.Line;
						LabelEnabled = false;
						break;					
					case ChartSerieType.pie:
						DataSerie.RenderAs = RenderAs.Pie;
						LabelEnabled = true;
						SecondaryYAxis = false;
						break;	
					case ChartSerieType.quickLine:
						DataSerie.RenderAs = RenderAs.QuickLine;
						LabelEnabled = false;
						break;
					case ChartSerieType.radar:
						DataSerie.RenderAs = RenderAs.Radar;
						LabelEnabled = false;
						break;					
					case ChartSerieType.spline:
						DataSerie.RenderAs = RenderAs.Spline;
						LabelEnabled = false;
						break;
					case ChartSerieType.stackedArea:
						DataSerie.RenderAs = RenderAs.StackedArea;
						LabelEnabled = false;
						break;
					case ChartSerieType.stackedArea100:
						DataSerie.RenderAs = RenderAs.StackedArea100;
						LabelEnabled = false;
						break;
					case ChartSerieType.stackedBar:
						DataSerie.RenderAs = RenderAs.StackedBar;
						LabelEnabled = true;
						break;
					case ChartSerieType.stackedBar100:
						DataSerie.RenderAs = RenderAs.StackedBar100;
						LabelEnabled = true;
						break;
					case ChartSerieType.stackedColumn:
						DataSerie.RenderAs = RenderAs.StackedColumn;
						LabelEnabled = true;
						break;
					case ChartSerieType.stackedColumn100:
						DataSerie.RenderAs = RenderAs.StackedColumn100;
						LabelEnabled = true;
						break;
					case ChartSerieType.stepLine:
						DataSerie.RenderAs = RenderAs.StepLine;
						LabelEnabled = false;
						break;		
					case ChartSerieType.pyramid:
						DataSerie.RenderAs = RenderAs.Pyramid;
						labelEnabled = true;
						SecondaryYAxis = false;
						break;
					case ChartSerieType.sectionFunnel:
						DataSerie.RenderAs = RenderAs.SectionFunnel;
						labelEnabled = true;
						SecondaryYAxis = false;
						break;
					case ChartSerieType.streamlineFunnel:
						DataSerie.RenderAs = RenderAs.StreamLineFunnel;
						labelEnabled = true;
						SecondaryYAxis = false;
						break;
				}				
				return true;
			} catch (Exception) {
				return false;
			}
		}


		public void init(ChartDataSerie serieData, ChartSerieProperties serieProp) {
			TagName = serieData.Name;
			//SerieProp = serieProp;
			DataSerie = new DataSeries();

			refresh(serieData);
					
			
			SerieType = serieProp.SerieType;

			silverChartControl.ChartControl.Series.Add(DataSerie);

			SecondaryYAxis = serieProp.YAxisIndex!=0;
			Name = serieProp.Title;
			Enabled = serieProp.Enabled;
			DataSerie.ToolTipText = "#Series: (#AxisXLabel, #YValue)";			
			DataSerie.LineThickness = serieProp.LineWidth;
			DataSerie.MarkerEnabled = serieProp.Marker;

			if (serieProp.Color != null) {
				string[] colors=serieProp.Color.Split('-');
				byte r=Byte.Parse(colors[0]);
				byte g=Byte.Parse(colors[1]);
				byte b=Byte.Parse(colors[2]);
				Brush br=new SolidColorBrush(Color.FromArgb(255,r,g,b));
				DataSerie.Color=br;
			}

			
		}

		protected void addAutoPoint(string x, double y) {			
			try {
				DataPoint pt=new DataPoint();
				pt.AxisXLabel = x;
				pt.YValue = y;
				DataSerie.DataPoints.Add(pt);
			} catch { }
		}

		protected void addDatePoint(string x, double y) {
			try {
				DataPoint pt=new DataPoint();
				pt.XValue = DateTime.Parse(x);
				pt.YValue = y;
				DataSerie.DataPoints.Add(pt);
			} catch { }
		}

		public void refresh(ChartDataSerie serieData) {
			DataSerie.DataPoints.Clear();			
			switch (silverChartControl.TypeXAxis) {
				case XAxisTypeEnum.auto:
					foreach (ChartDataPoint point in serieData.Points) {
						addAutoPoint(point.XVal, point.YVal);
					}
					DataSerie.XValueType = ChartValueTypes.Auto;
					break;
				case XAxisTypeEnum.datetime:
					foreach (ChartDataPoint point in serieData.Points) {
						addDatePoint(point.XVal, point.YVal);
					}
					DataSerie.XValueType = ChartValueTypes.DateTime;
					break;
			}
		}
	}
}
