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
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SilverChart
{
	public class PBRPoint : INotifyPropertyChanged
	{
		protected PBRData parent;
		public event PropertyChangedEventHandler PropertyChanged;
		protected DelegateCommand commandDown;
		protected DelegateCommand commandUp;
		protected DelegateCommand commandPrev;
		protected DelegateCommand commandNext;
		
		public PBRPoint(DateTime date, double val, PBRData parent) {
			Date = date;
			Val = val;
			this.parent = parent;
			commandDown = new DelegateCommand(downDataFunc, canExec);
			commandUp = new DelegateCommand(upDataFunc, canExec);
			commandPrev = new DelegateCommand(prevDataFunc, canExec);
			commandNext = new DelegateCommand(nextDataFunc, canExec);
		}

		private void downDataFunc(object param) {
			parent.doDown(this);
		}

		private void upDataFunc(object param) {
			parent.doUp(this);
		}

		private void prevDataFunc(object param) {
			parent.doPrev(this);
		}

		private void nextDataFunc(object param) {
			parent.doNext(this);
		}

		private bool canExec(object parameter) {
			return true;
		}

		public void NotifyChanged(string propName) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
		protected DateTime date;
		public DateTime Date {
			get {
				return date;
			}
			set {
				date = value;
				NotifyChanged("Date");
			}
		}

		protected double val;
		public double Val {
			get {
				return val;
			}
			set {
				val = value;
				NotifyChanged("Val");
			}
		}

		public DelegateCommand doDown {
			get {
				return commandDown;
			}
		}

		public DelegateCommand doUp {
			get {
				return commandUp;
			}
		}

		public DelegateCommand doPrev {
			get {
				return commandPrev;
			}
		}

		public DelegateCommand doNext {
			get {
				return commandNext;
			}
		}
	}

	public class PBRData : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private ObservableCollection<PBRPoint> pbr=new ObservableCollection<PBRPoint>();
		public ObservableCollection<PBRPoint> PBR {
			get {
				return pbr;
			}
			set {
				pbr = PBR;
				NotifyChanged("PBR");
			}
		}

		public Dictionary<DateTime, double> Data {
			set {
				pbr.Clear();
				foreach (KeyValuePair<DateTime,double> de in value) {
					if (de.Key.Minute == 0) {
						pbr.Add(new PBRPoint(de.Key, de.Value,this));
					}
				}
				NotifyChanged("PBR");
			}
			get {
				Dictionary<DateTime,double> result=new Dictionary<DateTime, double>();
				foreach (PBRPoint point in pbr) {
					result.Add(point.Date, point.Val);
				}
				return result;
			}
		}

		public void NotifyChanged(string propName) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		public void doDown(PBRPoint point) {
			int index=PBR.IndexOf(point);
			for (int i=index; i < PBR.Count; i++) {
				PBR[i].Val = PBR[index].Val;
			}
		}

		public void doUp(PBRPoint point) {
			int index=PBR.IndexOf(point);
			for (int i=0; i < index; i++) {
				PBR[i].Val = PBR[index].Val;
			}
		}

		public void doNext(PBRPoint point) {
			int index=PBR.IndexOf(point);
			if (index < PBR.Count) {
				PBR[index].Val = PBR[index + 1].Val;
			}
		}

		public void doPrev(PBRPoint point) {
			int index=PBR.IndexOf(point);
			if (index > 0) {
				PBR[index].Val = PBR[index - 1].Val;
			}
		}


				
	}
}
