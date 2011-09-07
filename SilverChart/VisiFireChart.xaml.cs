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
using Visifire.Charts;
using Visifire.Commons;
using SilverChart.ChartDataServiceReference;
using System.IO;
using System.Windows.Media.Imaging;
using FluxJpeg.Core;
using System.Windows.Printing;

namespace SilverChart
{
	public partial class VisiFireChart : UserControl
	{
		protected ChartDataServiceClient client;
		public SilverChartControl silverChartControl { get; protected set; }

		public VisiFireChart() {
			InitializeComponent();
			client=new ChartDataServiceClient();
			client.getChartDataCompleted += new EventHandler<getChartDataCompletedEventArgs>(client_getChartDataCompleted);
			client.getRynokChartCompleted += new EventHandler<getRynokChartCompletedEventArgs>(client_getRynokChartCompleted);
		}

		

		public void init(StartupEventArgs e) {

			switch (App.target) {
				case "chart":
					App.fileName = e.InitParams["fileName"];
					App.propFileName = e.InitParams["propFileName"];
					client.getChartDataAsync(App.fileName, App.propFileName);
					break;
				case "rynokChart":
					App.propFileName = e.InitParams["propFileName"];
					DateTime dateStart=e.InitParams.Keys.Contains("dateStart") ? DateTime.Parse(e.InitParams["dateStart"]) : DateTime.Today;
					DateTime dateEnd=e.InitParams.Keys.Contains("dateEnd") ? DateTime.Parse(e.InitParams["dateEnd"]) : DateTime.Today.AddDays(1);
					string interval=e.InitParams.Keys.Contains("interval") ? e.InitParams["interval"] : "30m";
					bool recorded=e.InitParams.Keys.Contains("recorded") ? Boolean.Parse(e.InitParams["recorded"]) : false;
					client.getRynokChartAsync(dateStart, dateEnd, interval,recorded,App.propFileName);
					break;
			}
			cmbAllTypes.ItemsSource = SilverChartSerieTypes.TypeNames;
			//cmbAllTypesXAxis.DataContext = silverChartControl;						
		}
		

		void client_getRynokChartCompleted(object sender, getRynokChartCompletedEventArgs e) {
			ChartAnswer chartAnswer=e.Result;
			processChartAnswer(chartAnswer);
		}


		void client_getChartDataCompleted(object sender, getChartDataCompletedEventArgs e) {
			ChartAnswer chartAnswer=e.Result;
			processChartAnswer(chartAnswer);
		}

		private void processChartAnswer(ChartAnswer chartAnswer) {
			silverChartControl = new SilverChartControl(chartAnswer);
			SeriesGrid.ItemsSource = silverChartControl.ChartSeries;
			ChartGrid.Children.Add(silverChartControl.ChartControl);
			panelSilverChartProperties.DataContext = silverChartControl;
		}

		public static void showMessage(string message) {
			System.Windows.Browser.HtmlPage.Window.Invoke("showMessage", message);
		}

		public static void logMessage(string message) {
			System.Windows.Browser.HtmlPage.Window.Invoke("logMessage", message);
		}


		private void cmbAllTypes_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			string type=cmbAllTypes.SelectedItem.ToString();
			silverChartControl.SerieTypeStr = type;
		}

		private void cmbOpacityAll_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			ComboBoxItem item=cmbOpacityAll.SelectedItem as ComboBoxItem;
			string opacStr=item.Content.ToString();
			int opacity=Int32.Parse(opacStr);
			silverChartControl.Opacity = opacity;
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

		private void btn3D_Click(object sender, RoutedEventArgs e) {
			silverChartControl.View3D = !silverChartControl.View3D;
		}

		private void btnSave_Click(object sender, RoutedEventArgs e) {
			ChartActions.Actions().SaveToImage(ChartGrid);
		}

		private void btnPrint_Click(object sender, RoutedEventArgs e) {
			ChartActions.Actions().print(silverChartControl.ChartControl);
		}


		private void cmbXAxisFormat_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			ComboBoxItem item=cmbXAxisFormat.SelectedItem as ComboBoxItem;
			string format=item.Content.ToString();
			silverChartControl.FormatXAxis = format;
		}

		private void btnFullScreen_Click(object sender, RoutedEventArgs e) {
			ChartActions.Actions().ToggleFullScreen();
		}
	}
}
