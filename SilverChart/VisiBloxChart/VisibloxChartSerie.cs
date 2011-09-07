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
using Visiblox.Charts;

namespace SilverChart
{
	public class VisibloxChartSerie : INotifyPropertyChanged
	{
		public VisibloxChartControl silverChartControl { get; protected set; }
		public event PropertyChangedEventHandler PropertyChanged;
		public string TagName { get; protected set; }
		public ChartSeriesBase Serie { get; protected set; }

		public int SerieIndex { get; protected set; }

		protected int yAxisIndex;
		public int YAxisIndex {
			get { return yAxisIndex; }
			set {
				yAxisIndex = value;
				if (!enabled) {
					Serie.YAxis = silverChartControl.ChartControl.AdditionalSecondaryYAxes[0];
				} else {
					Serie.YAxis = silverChartControl.Axes[value];
				}
				NotifyChanged("YAxisIndex");
			}
		}

		protected IDataPoint currentPoint;
		public IDataPoint CurrentPoint {
			get {
				return currentPoint;
			}
			set {
				currentPoint = value;
				NotifyChanged("CurrentPoint"); 
			}
		}

		protected bool enabled;
		public bool Enabled {
			get { return enabled; }
			set { 				
				enabled = value;
				if (enabled) {
					YAxisIndex = yAxisIndex;					
					Serie.Visibility = Visibility.Visible;
					silverChartControl.ChartControl.DoInvalidate();
				} else {
					YAxisIndex = yAxisIndex;
					Serie.Visibility = Visibility.Collapsed;
				}

				NotifyChanged("Enabled");  
			}
		}

		protected string name;
		public string Name {
			get { return name; }
			set { 
				name = value;
				NotifyChanged("Name"); 
			}
		}		

		protected ChartSerieType serieType;
		public ChartSerieType SerieType {
			get { return serieType; }
			set {			
				serieType=value;
				NotifyChanged("SerieType");				
			}
		}

		protected Brush lineStroke;
		public Brush LineStroke {
			get { return lineStroke; }
			set {
				lineStroke = value;
				NotifyChanged("LineStroke");
			}
		}

		public VisibloxChartSerie(VisibloxChartControl silverChartControl) {
			this.silverChartControl = silverChartControl;
		}

		public void NotifyChanged(string propName) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		
		public void init(ChartDataSerie serieData, ChartSerieProperties serieProp) {
			SerieIndex = silverChartControl.ChartSeries.Count;
			TagName = serieData.Name;
			Name = serieProp.Title;
			
			DataSeries<DateTime,double> dataSeries=new DataSeries<DateTime, double>{Title=Name};
			foreach (ChartDataPoint point in serieData.Points) {
				dataSeries.Add(new DataPoint<DateTime,double>(DateTime.Parse(point.XVal),point.YVal));
			}
			Serie=null;
			Brush br=new SolidColorBrush(Color.FromArgb(255,0,0,0));
			if (serieProp.Color != null) {
				string[] colors=serieProp.Color.Split('-');
				byte r=Byte.Parse(colors[0]);
				byte g=Byte.Parse(colors[1]);
				byte b=Byte.Parse(colors[2]);
				br = new SolidColorBrush(Color.FromArgb(255, r, g, b));
			} else {
				br = ChartActions.Actions().getNextColor();
			}
			switch (serieProp.SerieType){
				case ChartSerieType.line:
					LineSeries lineSerie=new LineSeries{DataSeries=dataSeries};
					lineSerie.LineStrokeThickness=serieProp.LineWidth+1;
					lineSerie.ToolTipEnabled = true;
					lineSerie.LineStroke = br;
					LineStroke = lineSerie.LineStroke;
					//GraphVyrabToolkit.logMessage(lineSerie.LineStroke.ToString());
					Serie = lineSerie;
					
					break;
				case ChartSerieType.stepLine:
					StaircaseSeries stairSerie=new StaircaseSeries{DataSeries=dataSeries};
					stairSerie.LineStrokeThickness=serieProp.LineWidth+1;
					stairSerie.ToolTipEnabled = true;
					stairSerie.LineStroke = br;
					LineStroke = stairSerie.LineStroke;
					Serie = stairSerie;
					break;			
			}
			
			silverChartControl.ChartControl.Series.Add(Serie);

			YAxisIndex = serieProp.YAxisIndex;			
			Enabled = serieProp.Enabled;			
			silverChartControl.TrackBehaviour.PropertyChanged += new PropertyChangedEventHandler(TrackBehaviour_PropertyChanged);
		}

		void TrackBehaviour_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			CurrentPoint = silverChartControl.TrackBehaviour.CurrentPoints[SerieIndex];
		}

		public void refresh(ChartDataSerie serieData) {
			DataSeries<DateTime,double> dataSeries=new DataSeries<DateTime, double> { Title = Name };
			foreach (ChartDataPoint point in serieData.Points) {
				dataSeries.Add(new DataPoint<DateTime, double>(DateTime.Parse(point.XVal), point.YVal));
			}
			Serie.DataSeries = dataSeries;
		}
		
	}
}
