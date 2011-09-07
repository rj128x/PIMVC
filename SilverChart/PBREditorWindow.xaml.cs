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
using Visiblox.Charts;
using System.Windows.Data;

namespace SilverChart
{
	public partial class PBREditorWindow : ChildWindow
	{
		private Chart chart;
		public PBRData pbrData;

		public PBREditorWindow(PBRData pbrData) {
			this.pbrData = pbrData;
			this.Loaded += new RoutedEventHandler(PBREditorWindow_Loaded);
			this.HasCloseButton = false;			
			InitializeComponent();
		}

		void PBREditorWindow_Loaded(object sender, RoutedEventArgs e) {
			if (chart == null) {
				chart = new Chart();
				chart.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
				chart.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
				gridChart.Children.Add(chart);
				BindableDataSeries ds=new BindableDataSeries("ПБР");
				ds.ItemsSource = pbrData.PBR;
				ds.XValueBinding = new Binding() { Path = new PropertyPath("Date") };
				ds.YValueBinding = new Binding() { Path = new PropertyPath("Val") };
				LineSeries ls=new LineSeries();
				ls.DataSeries = ds;
				chart.Series.Add(ls);
			}
		}

		private void OKButton_Click(object sender, RoutedEventArgs e) {
			this.DialogResult = true;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e) {
			this.DialogResult = false;
		}

		private void btnDown_Click(object sender, RoutedEventArgs e) {
			ChartActions.logMessage(e.OriginalSource.ToString());
		}

		private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
			TextBox box=sender as TextBox;
			box.SelectAll();
		}
	}
}

