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
using SilverChart.CustomReportService;
using System.Collections.ObjectModel;
namespace SilverChart
{
	

	public partial class CustomReportSettings : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected Dictionary<string,CustomReportDataString> selectedData=new Dictionary<string,CustomReportDataString>();
		protected CustomReportTags currentReportTags;
		protected CustomReport report;
		protected CustomReportServiceClient client;
		protected string fileName;		
		protected bool submitAfterSave=false;

		protected string status;
		public String Status {
			get {
				return status;
			}
			set {
				status = value;
				NotifyChanged("Status");
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
		

		public CustomReportSettings(StartupEventArgs e) {
			init(e);
			InitializeComponent();
			System.Windows.Browser.HtmlPage.RegisterScriptableObject("CustomReportSettings", this);
			client = new CustomReportServiceClient();
			client.getReportCompleted += new EventHandler<getReportCompletedEventArgs>(client_getReportCompleted);
			client.getReportTagsCompleted += new EventHandler<getReportTagsCompletedEventArgs>(client_getReportTagsCompleted);
			client.createReportXMLCompleted += new EventHandler<createReportXMLCompletedEventArgs>(client_createReportXMLCompleted);
			client.createReportFromXMLCompleted += new EventHandler<createReportFromXMLCompletedEventArgs>(client_createReportFromXMLCompleted);
			GridPanel.DataContext = this;
			LayoutRoot.DataContext = this;
			Status = "Получение данных отчета";
			ReadyForUpdate = false;
			client.createReportFromXMLAsync(fileName);
		}

		void client_createReportFromXMLCompleted(object sender, createReportFromXMLCompletedEventArgs e) {
			Status = "Обработка данных отчета";
			CustomReportStructure structure=e.Result;
			selectedData = new Dictionary<string, CustomReportDataString>();
			foreach (CustomReportDataString tag in structure.SelectedTags) {
				selectedData.Add(tag.TagName, tag);
			}			
			Status = "Получение структуры тэгов";
			client.getReportAsync();
		}

		public void init(StartupEventArgs e) {
			fileName = e.InitParams["fileName"];				
		}

		void client_createReportXMLCompleted(object sender, createReportXMLCompletedEventArgs e) {
			Status = "Готово";
			//bool ok=e.Result;			
			if (submitAfterSave) {
				bool ok=selectedData.Count > 0;
				if (ok){
					System.Windows.Browser.HtmlPage.Window.Invoke("submitCustomReportForm");
				}
			}
			ReadyForUpdate = true;
		}

		void client_getReportTagsCompleted(object sender, getReportTagsCompletedEventArgs e) {
			Status = "Обработка тэгов";
			currentReportTags=e.Result;
			gridTags.ItemsSource = currentReportTags.Tags;
			refreshGridData();
			Status = "Готово";
			ReadyForUpdate = true;
		}

		void client_getReportCompleted(object sender, getReportCompletedEventArgs e) {
			Status = "Обработка структуры тэгов";
			report=e.Result;					
			treeSections.ItemsSource = report.MainSection.Children;
			Status = "Готово";
			ReadyForUpdate = true;
		}



		private void btnHide_Click(object sender, RoutedEventArgs e) {
			if (GridPanel.Visibility == System.Windows.Visibility.Collapsed) {
				GridPanel.Visibility = System.Windows.Visibility.Visible;
				btnHide.Content = "<<";
			} else {
				GridPanel.Visibility = System.Windows.Visibility.Collapsed;
				btnHide.Content = ">>";
			}
		}	

		public void NotifyChanged(string propName) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		private void treeSections_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
			CustomReportSection section=treeSections.SelectedItem as CustomReportSection;
			if (section.FindString.ToLower().Equals("selected")) {
				refreshSelectedData();
				gridTags.ItemsSource = selectedData.Values;
			} else {
				ReadyForUpdate = false;
				refreshSelectedData();

				Status = "Получение тэгов";
				client.getReportTagsAsync(section);
			}
		}

		private void refreshSelectedData() {
			if (currentReportTags == null)
				return;
			foreach (CustomReportDataString tag in currentReportTags.Tags) {
				refreshTag(tag);
			}
		}

		private void refreshTag(CustomReportDataString tag) {
			bool exist=tag.MaxData || tag.MinData || tag.AvgData;
			if (exist) {
				if (!selectedData.Keys.Contains(tag.TagName)) {
					selectedData.Add(tag.TagName, tag);
				}
			} else {
				if (selectedData.Keys.Contains(tag.TagName)) {
					selectedData.Remove(tag.TagName);
				}
			}
		}

		private void refreshGridData() {
			if (currentReportTags == null) {
				return;
			}
			foreach (CustomReportDataString tag in currentReportTags.Tags) {
				bool exist=selectedData.Keys.Contains(tag.TagName);
				if (exist) {					
					tag.AvgData=selectedData[tag.TagName].AvgData;
					tag.MaxData=selectedData[tag.TagName].MaxData;
					tag.MinData=selectedData[tag.TagName].MinData ;
				} 
			}
		}

		private void btnApply_Click(object sender, RoutedEventArgs e) {
			ReadyForUpdate = false;
			Status = "Сохранение отчета";
			submitAfterSave = false;
			refreshSelectedData();			
			client.createReportXMLAsync(fileName,selectedData);
		}

		[System.Windows.Browser.ScriptableMember()]
		public void saveAndSubmit() {
			if (ReadyForUpdate) {
				ReadyForUpdate = false;
				Status = "Сохранение отчета";
				submitAfterSave = true;
				refreshSelectedData();
				client.createReportXMLAsync(fileName, selectedData);
			}
		}

		private void btnClear_Click(object sender, RoutedEventArgs e) {
			if (currentReportTags != null) {
				foreach (CustomReportDataString tag in currentReportTags.Tags) {
					tag.AvgData = false;
					tag.MaxData = false;
					tag.MinData = false;
				}
			}
			foreach (CustomReportDataString tag in selectedData.Values) {
				tag.AvgData = false;
				tag.MaxData = false;
				tag.MinData = false;
			}			
		}
	}
}
