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
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.IO;
using FluxJpeg.Core;
using System.ComponentModel;
using System.Windows.Printing;
using Visiblox.Charts;

namespace SilverChart
{

	public class VisibloxChartControl : INotifyPropertyChanged
	{		
		public event PropertyChangedEventHandler PropertyChanged;
		public ChartAnswer Answer { get; set; }
		public Chart ChartControl{get;protected set;}
		public List<VisibloxChartSerie> ChartSeries;
		public Dictionary<int,LinearAxis> Axes;
		public TrackballBehaviour TrackBehaviour { get; protected set; }

		public VisibloxChartControl(ChartAnswer answer) {
			CreateChart();
			processChartAnswer(answer);			
		}

		public VisibloxChartControl() {
			CreateChart();
		}

		protected void processChartAnswer(ChartAnswer chartAnswer) {
			Answer = chartAnswer;
			ChartData data=Answer.Data;
			ChartProperties prop=Answer.Properties;
			
			CreateChart();
			LinearAxis hiddenYAxis=new LinearAxis();
			hiddenYAxis.Visibility = Visibility.Collapsed;
			ChartControl.AdditionalSecondaryYAxes.Add(hiddenYAxis);
			Axes = new Dictionary<int, LinearAxis>();
			foreach (ChartAxisProperties ax in chartAnswer.Properties.Axes) {
				LinearAxis axis=new LinearAxis();
				axis.AutoScaleToVisibleData = true;
				axis.LabelFormatString = "### ### ##0.##";
				axis.ShowGridlines = ax.Index == 0;
				if (ax.Interval != 0) {
					axis.MajorTickInterval = ax.Interval;
				}
				if (!ax.Auto) {
					axis.Range = new DoubleRange(ax.Min, ax.Max);
				}
				if (ax.Index > 1) {
					ChartControl.AdditionalSecondaryYAxes.Add(axis);
				}
				Axes.Add(ax.Index, axis);
			}
			ChartControl.YAxis = Axes[0];
			ChartControl.SecondaryYAxis = Axes[1];

			ChartSeries = new List<VisibloxChartSerie>();
			foreach (ChartSerieProperties serieProp in prop.Series) {
				foreach (ChartDataSerie serieData in data.Series) {
					if (serieData.Name == serieProp.TagName) {
						VisibloxChartSerie chartSerie=new VisibloxChartSerie(this);
						chartSerie.init(serieData, serieProp);
						ChartSeries.Add(chartSerie);
					}
				}
			}
								
		}
		

		private void CreateChart() {
			ChartControl = new Chart();						

			BehaviourManager manager=new BehaviourManager();
			manager.AllowMultipleEnabled = true;
			manager.IsEnabled = true;

			TrackballBehaviour track=new TrackballBehaviour();
			manager.Behaviours.Add(track);
			track.IsEnabled = true;
			track.TrackingMode = TrackingPointPattern.LineOnX;

			ZoomBehaviour zoom=new ZoomBehaviour();
			manager.Behaviours.Add(zoom);
			zoom.IsEnabled = true;
			
			TrackBehaviour = track;

			ChartControl.LegendVisibility = Visibility.Collapsed;

			ChartControl.Behaviour = manager;			
		}		

		public void NotifyChanged(string propName) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		

		public void refresh(ChartAnswer answer) {
			Answer = answer;
			foreach (ChartDataSerie serieData in answer.Data.Series) {
				foreach (VisibloxChartSerie serie in ChartSeries) {
					if (serie.TagName == serieData.Name) {
						serie.refresh(serieData);
					}
				}
			}
		}
		
	}
}
