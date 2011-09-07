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
using System.Windows.Controls.Primitives;
namespace SilverChart
{
	

	public partial class PrognozNB : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected ChartDataServiceClient client;		
		private VisibloxChartControl silverChart;
		private DateTime lastUpdate;
		private int RefreshSeconds=30;
		private EventHandler handlerRefresh;
		private LineSeries DateSerie;
		private DateTime dateReport;		
		private PBREditorWindow pbrEditor;
		private PrognozProperties prognozProp;

		public PrognozNB() {
			InitializeComponent();
			client = new ChartDataServiceClient();
			client.getPrognozNBFaktCompleted += new EventHandler<getPrognozNBFaktCompletedEventArgs>(client_getPrognozNBFaktCompleted);
			handlerRefresh = new EventHandler(CompositionTarget_Rendering);
			pnlRefresh.DataContext = this;
			prognozProp = new PrognozProperties(this);			
			
			prognozProp.PBRZad = new PBRData();
			pbrEditor = new PBREditorWindow(prognozProp.PBRZad);
			pbrEditor.DataContext = prognozProp.PBRZad;
			pnlPrognozProperties.DataContext = prognozProp;
			
		}



		void client_getPrognozNBFaktCompleted(object sender, getPrognozNBFaktCompletedEventArgs e) {
			ChartAnswer chartAnswer=e.Result.Chart;
			prognozProp.PBRZad.Data = e.Result.PBR30;
			dateReport = e.Result.Date;
			txtDate.Text = e.Result.Date.ToString();					
			processChartAnswer(chartAnswer);
			lastUpdate = DateTime.Now;
			ReadyForUpdate = true;			
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
				prognozProp.ReadyForUpdate = value;
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
			client.getPrognozNBFaktAsync(prognozProp.Date, prognozProp.Now,prognozProp.Hour,prognozProp.Minute,prognozProp.PrognozType,
				prognozProp.PBRZad.Data,prognozProp.SutkiCount,prognozProp.RashodCalcType,prognozProp.FaktMinutes);
		}

		public void init(StartupEventArgs e) {
			try {
				prognozProp.Date = e.InitParams.Keys.Contains("date") ? DateTime.Parse(e.InitParams["date"]) : DateTime.Today;
				prognozProp.Now = e.InitParams.Keys.Contains("now") ? Boolean.Parse(e.InitParams["now"]) : true;
				prognozProp.Hour = e.InitParams.Keys.Contains("hour") ? Int32.Parse(e.InitParams["hour"]) : 0;
				prognozProp.Minute = e.InitParams.Keys.Contains("minute") ? Int32.Parse(e.InitParams["minute"]) : 0;
			} catch (Exception ex) {
				ChartActions.logMessage(ex.StackTrace);
			}
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
				
				DateSerie = new LineSeries();
				DateSerie.LineStrokeThickness = 1;
				DateSerie.LineStroke = new SolidColorBrush(Colors.Black);

				silverChart.ChartControl.Series.Add(DateSerie);
			} else {
				silverChart.refresh(answer);
			}
			DataSeries<DateTime,double> dataSeries=new DataSeries<DateTime, double>();
			double max=((double)silverChart.ChartControl.YAxis.ActualRange.Maximum);
			double min=((double)silverChart.ChartControl.YAxis.ActualRange.Minimum);
			dataSeries.Add(new DataPoint<DateTime, double>(dateReport, min));
			dataSeries.Add(new DataPoint<DateTime, double>(dateReport, max));
			DateSerie.DataSeries = dataSeries;

			(silverChart.ChartControl.XAxis as DateTimeAxis).MinorTickIntervalType = DateTimeAxisIntervalSpan.FifteenMinutes;
			(silverChart.ChartControl.XAxis as DateTimeAxis).MajorTickIntervalType = DateTimeAxisIntervalSpan.Hours;
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

		private void btnApplyParams_Click(object sender, RoutedEventArgs e) {
			try {
				refreshChart();
			} catch (Exception ex) {
				ChartActions.logMessage(ex.StackTrace);
			}
			
		}

		private void btnChangePBR_Click(object sender, RoutedEventArgs e) {
			pbrEditor.Show();
		}

		private void clnDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e) {

		}

		private void rbUsePBRGekon_Click(object sender, RoutedEventArgs e) {
			changePrognozType();
		}

		public void changePrognozType() {
			if (rbUsePBRGekon.IsChecked.Value)
				prognozProp.PrognozType = PrognozDataType.PBRGekon;
			else if (rbUsePBRUser.IsChecked.Value)
				prognozProp.PrognozType = PrognozDataType.PBRUser;
			else if (rbUsePFakt.IsChecked.Value)
				prognozProp.PrognozType = PrognozDataType.PFakt;
		}

		public void changedPrognozType() {
			switch (prognozProp.PrognozType) {
				case PrognozDataType.PBRGekon:
					rbUsePBRGekon.IsChecked = true;
					btnChangePBR.Visibility = Visibility.Collapsed;
					break;
				case PrognozDataType.PBRUser:
					rbUsePBRUser.IsChecked = true;
					btnChangePBR.Visibility = Visibility.Visible;
					break;
				case PrognozDataType.PFakt:
					rbUsePFakt.IsChecked = true;
					btnChangePBR.Visibility = Visibility.Collapsed;
					break;
			}
		}

		public void changeRashodCalcType() {
			if (rbQAvg.IsChecked.Value)
				prognozProp.RashodCalcType = PrognozRashodCalcType.avg;
			else if (rbQFakt.IsChecked.Value)
				prognozProp.RashodCalcType = PrognozRashodCalcType.fakt;
			else if (rbQMax.IsChecked.Value)
				prognozProp.RashodCalcType = PrognozRashodCalcType.max;
			else if (rbQMin.IsChecked.Value)
				prognozProp.RashodCalcType = PrognozRashodCalcType.min;
		}

		public void changedRashodCalcType() {
			switch (prognozProp.RashodCalcType) {
				case PrognozRashodCalcType.min:
					rbQMin.IsChecked = true;
					break;
				case PrognozRashodCalcType.max:
					rbQMax.IsChecked = true;
					break;
				case PrognozRashodCalcType.avg:
					rbQAvg.IsChecked = true;
					break;
				case PrognozRashodCalcType.fakt:
					rbQFakt.IsChecked = true;
					break;
				
			}
		}

		private void rbQAvg_Click(object sender, RoutedEventArgs e) {
			changeRashodCalcType();
		}

		private void rbQMin_Click(object sender, RoutedEventArgs e) {
			changeRashodCalcType();
		}


		private void rbQMax_Click(object sender, RoutedEventArgs e) {
			changeRashodCalcType();
		}

		private void rbQFakt_Click(object sender, RoutedEventArgs e) {
			changeRashodCalcType();
		}

	}

	public class PrognozProperties : INotifyPropertyChanged
	{
		public PrognozNB parent;
		public event PropertyChangedEventHandler PropertyChanged;
		private PBRData pbrData;
		private DateTime date;
		private bool now;
		private int hourStart;
		private int minuteStart;
		private int sutkiCount=1;
		private int faktMinutes=10;
		private PrognozDataType prognozType=PrognozDataType.PBRGekon;
		private PrognozRashodCalcType rashodCalcType=PrognozRashodCalcType.avg;
		private bool readyForUpdate;

		public PrognozProperties(PrognozNB parent) {
			this.parent = parent;
		}

		public bool ReadyForUpdate {
			get {
				return readyForUpdate;
			}
			set {
				readyForUpdate = value;
				NotifyChanged("ReadyForUpdate");
			}
		}


		public PBRData PBRZad {
			get {
				return pbrData;
			}
			set {
				pbrData = value;
				NotifyChanged("PBRZad");
			}
		}

		public DateTime Date {
			get {
				return date;
			}
			set {
				date = value;
				if (date.Date != DateTime.Today) {
					Now = false;
				} else if (!Now) {					
					Now = true;
				}
				NotifyChanged("Date");
			}
		}

		public bool Now {
			get {
				return now;
			}
			set {
				now = value;
				if (now) {
					Date = DateTime.Today;
					PrognozType = PrognozDataType.PBRGekon;
				} else {
					PrognozType = PrognozDataType.PFakt;
				}
				NotifyChanged("Now");
			}
		}

		public int Hour {
			get {
				return hourStart;
			}
			set {
				hourStart = value;
				hourStart = hourStart < 0 ? 0 : hourStart;
				hourStart = hourStart > 23 ? 0 : hourStart;
				NotifyChanged("Hour");
			}
		}

		public int Minute {
			get {
				return minuteStart;
			}
			set {
				minuteStart = value;
				minuteStart = minuteStart < 0 ? 0 : minuteStart;
				minuteStart = minuteStart > 59 ? 59 : minuteStart;
				NotifyChanged("Minute");
			}
		}

		public int SutkiCount {
			get {
				return sutkiCount;
			}
			set {
				sutkiCount = value;
				NotifyChanged("SutkiCount");
			}
		}

		public int FaktMinutes {
			get {
				return faktMinutes;
			}
			set {
				faktMinutes = value;
				NotifyChanged("FaktMinutes");
			}
		}

		public PrognozRashodCalcType RashodCalcType {
			get {
				return rashodCalcType;
			}
			set {
				rashodCalcType = value;
				parent.changedRashodCalcType();
				NotifyChanged("RashodCalcType");
			}
		}

		public PrognozDataType PrognozType {
			get {
				return prognozType;
			}
			set {
				prognozType = value;
				if (prognozType != PrognozDataType.PFakt) {
					RashodCalcType = PrognozRashodCalcType.avg;
				}
				parent.changedPrognozType();
				NotifyChanged("PrognozType");
			}
		}

		public void NotifyChanged(string propName) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		
	}
}
