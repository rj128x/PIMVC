using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SilverChart.ChartDataServiceReference;
using System.Windows.Data;
using Visiblox.Charts;
using System.Windows.Printing;
using System.ComponentModel;
namespace SilverChart
{
	public partial class GraphVyrabToolkit : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected ChartDataServiceClient client;
		private DateTime dateStart;
		private DateTime dateEnd;
		private VisibloxChartControl silverChart;
		private DateTime lastUpdate;
		private int RefreshSeconds=30;
		private EventHandler handlerRefresh;
		private LineSeries DateSerie;
		private DateTime dateReport;
		

		public GraphVyrabToolkit() {
			InitializeComponent();
			client = new ChartDataServiceClient();
			client.getGraphVyrabDataCompleted += new EventHandler<getGraphVyrabDataCompletedEventArgs>(client_getGraphVyrabDataCompleted);
			handlerRefresh = new EventHandler(CompositionTarget_Rendering);
			pnlRefresh.DataContext = this;
		}

		private bool autoRefresh;
		public bool AutoRefresh {
			get {
				return autoRefresh;
			}
			set {
				autoRefresh = value;
				if (!autoRefresh) {
					CompositionTarget.Rendering -= handlerRefresh;
				} else {
					CompositionTarget.Rendering += handlerRefresh;
				}
				NotifyChanged("AutoRefresh");
			}
		}

		private bool readyForUpdate;
		public bool ReadyForUpdate {
			get {
				return readyForUpdate;
			}
			set {
				readyForUpdate = value;
				NotifyChanged("ReadyForUpdate");
			}
		}

		protected int refreshTime;
		public int RefreshTime {
			get {
				return refreshTime;
			}
			set {
				refreshTime = value;
				NotifyChanged("RefreshTime");				
			}
		}

		void CompositionTarget_Rendering(object sender, EventArgs e) {
			int time = (int)(lastUpdate.Ticks + RefreshSeconds * 10000000 - DateTime.Now.Ticks)/10000000;
			RefreshTime = time < 0 ? 0 : time;
			if ((RefreshTime <= 0) && (ReadyForUpdate)) {				
				refreshChart();
			}
		}

		protected void refreshChart() {
			ReadyForUpdate = false;
			client.getGraphVyrabDataAsync(dateStart, dateEnd, App.propFileName);
		}

		public static void showMessage(string message) {
			System.Windows.Browser.HtmlPage.Window.Invoke("showMessage", message);
		}

		public static void logMessage(string message) {
			System.Windows.Browser.HtmlPage.Window.Invoke("logMessage", message);
		}	

		void client_getGraphVyrabDataCompleted(object sender, getGraphVyrabDataCompletedEventArgs e) {
			ChartAnswer chartAnswer=e.Result.chartAnswer;
			dateReport = e.Result.date;

			processChartAnswer(chartAnswer);
			DiffGrid.ItemsSource = e.Result.CurrentVals;
			DiffHoursGrid.ItemsSource = e.Result.HourVals;
			txtDate.Text = e.Result.date.ToString();			
			lastUpdate = DateTime.Now;
			ReadyForUpdate = true;
		}



		public void init(StartupEventArgs e) {
			App.propFileName = e.InitParams["propFileName"];
			dateStart = e.InitParams.Keys.Contains("dateStart") ? DateTime.Parse(e.InitParams["dateStart"]) : DateTime.Today;
			dateEnd = e.InitParams.Keys.Contains("dateEnd") ? DateTime.Parse(e.InitParams["dateEnd"]) : DateTime.Today.AddDays(1);
			refreshChart();
		}

		protected void processChartAnswer(ChartAnswer answer) {

			if (silverChart == null) {
				silverChart = new VisibloxChartControl(answer);
				SeriesGrid.ItemsSource = silverChart.ChartSeries;
				ChartGrid.Children.Add(silverChart.ChartControl);
				silverChart.ChartControl.XAxis.IsAutoMarginEnabled = false;
				silverChart.ChartControl.YAxis.IsAutoMarginEnabled = false;
				silverChart.ChartControl.SecondaryYAxis.IsAutoMarginEnabled = false;
				DateSerie=new LineSeries();
				DateSerie.LineStrokeThickness = 1;
				DateSerie.LineStroke = new SolidColorBrush (Colors.Black);								

				silverChart.ChartControl.Series.Add(DateSerie);

			} else {
				silverChart.refresh(answer);
			}
			DataSeries<DateTime,double> dataSeries=new DataSeries<DateTime, double>();
			double max=((double)silverChart.ChartControl.YAxis.ActualRange.Maximum);
			dataSeries.Add(new DataPoint<DateTime, double>(dateReport, 0));
			dataSeries.Add(new DataPoint<DateTime, double>(dateReport, max));

			(silverChart.ChartControl.XAxis as DateTimeAxis).MinorTickIntervalType = DateTimeAxisIntervalSpan.FifteenMinutes;
			(silverChart.ChartControl.XAxis as DateTimeAxis).MajorTickIntervalType = DateTimeAxisIntervalSpan.Hours;
			DateSerie.DataSeries = dataSeries;
		}

		private void btnHide_Click(object sender, RoutedEventArgs e) {
			if (GridPanel.Visibility == System.Windows.Visibility.Collapsed) {
				GridPanel.Visibility = System.Windows.Visibility.Visible;
				btnHide.Content = ">>";
			} else {
				GridPanel.Visibility = System.Windows.Visibility.Collapsed;
				btnHide.Content = "<<";
			}
		}


		private void btnSave_Click(object sender, RoutedEventArgs e) {
			//silverChart.SaveToImage(silverChart.ChartControl);
			ChartActions.Actions().SaveToImage(ChartGrid);
		}

		private void btnPrint_Click(object sender, RoutedEventArgs e) {
			ChartActions.Actions().print(silverChart.ChartControl);
		}		

		private void btnRefresh_Click(object sender, RoutedEventArgs e) {
			refreshChart();
		}

		private void btnFullScreen_Click(object sender, RoutedEventArgs e) {
			ChartActions.Actions().ToggleFullScreen();
		}

		public void NotifyChanged(string propName) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
	}
}
