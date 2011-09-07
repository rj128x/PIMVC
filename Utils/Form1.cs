using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PILib.Rashod;
using PILib.PBR;
using PILib.NNET;

namespace Utils
{
	public partial class Form1 : Form
	{
		public Form1() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e) {
			NNET nnet=new NNET();
			SortedList<int,double> inputVector=new SortedList<int, double>();
			inputVector.Add(0, 2010);
			inputVector.Add(1,130);
			inputVector.Add(2, 3.3);
			inputVector.Add(3, 85.621171875);
			inputVector.Add(4, 85.63);
			inputVector.Add(5, 85.63);
			inputVector.Add(6, 85.63);
			inputVector.Add(7, 1586.6361084);
			inputVector.Add(8, 1565.75964355);
			inputVector.Add(9, 1564.30651855);
			inputVector.Add(10, 1462.03149414);
			inputVector.Add(11, 1235.34301758);

			inputVector.Add(12, 1230.233);
			inputVector.Add(13, 1230.233);
			inputVector.Add(14, 1230.233);
			inputVector.Add(15, 1230.233);
			inputVector.Add(16, 1230.233);
			inputVector.Add(17, 1230.233);
			inputVector.Add(18, 1230.233);
			inputVector.Add(19, 1230.233);
			inputVector.Add(20, 1230.233);
			inputVector.Add(21, 1230.233);
			inputVector.Add(22, 1230.233);
			inputVector.Add(23, 1230.233);
			inputVector.Add(24, 1230.233);
			inputVector.Add(25, 1230.233);
			inputVector.Add(26, 1230.233);
			inputVector.Add(27, 1230.233);
			inputVector.Add(28, 1230.233);
			inputVector.Add(29, 1230.233);
			inputVector.Add(30, 1230.233);
			inputVector.Add(31, 1230.233);
			inputVector.Add(32, 1230.233);
			inputVector.Add(33, 1230.233);
			inputVector.Add(34, 1230.233);

			inputVector.Add(35, 67.0405371094);
			inputVector.Add(36, 66.9921337891);
			inputVector.Add(37, 66.9668994141);
			inputVector.Add(38, 66.9457324219);

			SortedList<int,double> output=nnet.calc(inputVector);			
			
		}
	}
}
