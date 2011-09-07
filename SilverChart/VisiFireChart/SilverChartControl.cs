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
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.IO;
using FluxJpeg.Core;
using System.ComponentModel;
using System.Windows.Printing;

namespace SilverChart
{

	public class SilverChartControl : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public ChartAnswer Answer { get; set; }
		public Chart ChartControl{get;protected set;}
		public List<SilverChartSerie> ChartSeries;

		protected bool view3D;
		public bool View3D {
			get {
				return view3D;
			}
			set {
				view3D = value;
				ChartControl.View3D = view3D;
				NotifyChanged("View3D");
			}
		}

		public string SerieTypeStr {
			set {
				foreach (SilverChartSerie serie in ChartSeries) {
					serie.SerieTypeStr = value;
				}
				NotifyChanged("SerieTypeStr");
			}
		}

		public List<string> Types {
			get { return SilverChartSerieTypes.TypeNames; }
		}


		public int Opacity {
			set {
				foreach (SilverChartSerie serie in ChartSeries) {
					serie.Opacity = value;
				}
				NotifyChanged("SerieTypeStr");
			}
		}

		public XAxisTypeEnum typeXAxis;
		public XAxisTypeEnum TypeXAxis {
			get {
				return typeXAxis;
			}
			set {
				typeXAxis = value;				
			}
		}


		protected string formatXAxis;
		public string FormatXAxis {
			get {
				return formatXAxis;
			}
			set {
				formatXAxis = value;
				try {
					if (TypeXAxis == XAxisTypeEnum.datetime) {
						ChartControl.AxesX[0].ValueFormatString = formatXAxis;
						if ((value == "MMMM") || (value == "MMMM yyyy")) {
							ChartControl.AxesX[0].IntervalType = IntervalTypes.Months;
							ChartControl.AxesX[0].Interval = 1;
						} else {
							ChartControl.AxesX[0].ClearValue(Axis.IntervalProperty);
							ChartControl.AxesX[0].ClearValue(Axis.IntervalTypeProperty);
						}
					}
				} catch { };
				NotifyChanged("FormatXAxis");
			}
		}


		protected double mainYMin;
		public double MainYMin {
			get {
				return mainYMin;
			}
			set {
				mainYMin = value;
				refreshMainYAxes();
				NotifyChanged("MainYMin");
			}
		}

		protected double mainYMax;
		public double MainYMax {
			get {
				return mainYMax;
			}
			set {
				mainYMax = value;
				refreshMainYAxes();
				NotifyChanged("MainYMax");
			}
		}

		protected bool mainYAuto;
		public bool MainYAuto {
			get {
				return mainYAuto;
			}
			set {
				mainYAuto = value;
				refreshMainYAxes();
				NotifyChanged("MainYAuto");
			}
		}

		protected double secondaryYMin;
		public double SecondaryYMin {
			get {
				return secondaryYMin;
			}
			set {
				secondaryYMin = value;
				refreshSecondaryYAxes();
				NotifyChanged("SecondaryYMin");
			}
		}

		protected double secondaryYMax;
		public double SecondaryYMax {
			get {
				return secondaryYMax;
			}
			set {
				secondaryYMax = value;
				refreshSecondaryYAxes();
				NotifyChanged("SecondaryYMax");
			}
		}

		protected bool secondaryYAuto;
		public bool SecondaryYAuto {
			get {
				return secondaryYAuto;
			}
			set {
				secondaryYAuto = value;
				refreshSecondaryYAxes();
				NotifyChanged("SecondaryYAuto");
			}
		}

		public SilverChartControl(ChartAnswer answer) {
			CreateChart();
			processChartAnswer(answer);
		}

		protected void processChartAnswer(ChartAnswer chartAnswer) {
			Answer = chartAnswer;
			ChartData data=Answer.Data;
			ChartProperties prop=Answer.Properties;
			CreateChart();
			TypeXAxis = chartAnswer.Properties.XAxisType;
			ChartSeries = new List<SilverChartSerie>();
			foreach (ChartSerieProperties serieProp in prop.Series) {
				foreach (ChartDataSerie serieData in data.Series) {
					if (serieData.Name == serieProp.TagName) {
						SilverChartSerie chartSerie=new SilverChartSerie(this);
						chartSerie.init(serieData, serieProp);
						ChartSeries.Add(chartSerie);
					}
				}
			}
			
			MainYAuto = true;
			SecondaryYAuto = true;
			MainYMax = 100;
			SecondaryYMax = 100;
			formatXAxis = "dd.MM.yy HH:mm";
		}
		

		private void CreateChart() {
			ChartControl = new Chart();
			ChartControl.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
			ChartControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			ChartControl.ScrollingEnabled = false;
			ChartControl.ZoomingEnabled = true;
			ChartControl.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
			ChartControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			//ChartControl.DataContext = this;


			DataSeries serie=new DataSeries();
			ChartControl.Series.Add(serie);
			serie.ShowInLegend = false;
			serie.Enabled = false;
					

			ChartControl.AnimatedUpdate = false;
			ChartControl.AnimationEnabled = false;
			ChartControl.IndicatorEnabled = false;
			ChartControl.SmartLabelEnabled = false;						

			ChartControl.AxesY.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AxesY_CollectionChanged);
			ChartControl.AxesX.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AxesX_CollectionChanged);
		}

		void AxesX_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
			FormatXAxis = formatXAxis;
			try {
				Axis axis= ChartControl.AxesX[0];
				if (axis.Grids.Count == 0) {
					ChartGrid grid=new ChartGrid();
					axis.Grids.Add(grid);
					grid.Enabled = true;
				}
			} catch { }
		}

		void AxesY_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
			foreach (Axis axis in ChartControl.AxesY) {
				axis.StartFromZero = false;
			}
		}


		protected void refreshAxis(Axis axis, double min, double max, bool auto) {
			if (!auto) {
				axis.AxisMinimum = min;
				axis.AxisMaximum = max;
			} else {
				axis.ClearValue(Axis.AxisMinimumProperty);
				axis.ClearValue(Axis.AxisMaximumProperty);
			}
		}

		protected void refreshMainYAxes() {
			try {
				refreshAxis(ChartControl.AxesY[0], MainYMin, MainYMax, MainYAuto);
			} catch { }
		}

		protected void refreshSecondaryYAxes() {
			try {
				refreshAxis(ChartControl.AxesY[1], SecondaryYMin, SecondaryYMax, SecondaryYAuto);
			} catch { }
		}

		public void NotifyChanged(string propName) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
				

		public void refresh(ChartAnswer answer) {
			Answer = answer;			
			foreach (ChartDataSerie serieData in answer.Data.Series) {
				foreach (SilverChartSerie serie in ChartSeries) {
					if (serie.TagName == serieData.Name) {
						serie.refresh(serieData);
					}
				}
			}
		}

		
	}
}
