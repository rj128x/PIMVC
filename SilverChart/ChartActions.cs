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
using System.Windows.Printing;
using System.Windows.Media.Imaging;
using System.IO;
using FluxJpeg.Core;
using System.Collections.Generic;

namespace SilverChart
{
	public class ChartActions
	{
		private static ChartActions actions=null;
		private static List<Brush> PredefinedBrushes;
		private int brushIndex;
		private PrintDocument printDoc;
		private Control printElem;

		static ChartActions() {
			PredefinedBrushes = new List<Brush>();
			PredefinedBrushes.Add(new SolidColorBrush(Colors.Blue));
			PredefinedBrushes.Add(new SolidColorBrush(Colors.Green));
			PredefinedBrushes.Add(new SolidColorBrush(Colors.Red));
			PredefinedBrushes.Add(new SolidColorBrush(Colors.Cyan));
			PredefinedBrushes.Add(new SolidColorBrush(Colors.DarkGray));
			PredefinedBrushes.Add(new SolidColorBrush(Colors.Orange));
			PredefinedBrushes.Add(new SolidColorBrush(Colors.Magenta));
			PredefinedBrushes.Add(new SolidColorBrush(Colors.Purple));
			PredefinedBrushes.Add(new SolidColorBrush(Colors.Black));
		}


		private ChartActions() {
			printDoc = new PrintDocument();
			printDoc.PrintPage += new EventHandler<PrintPageEventArgs>(printDoc_PrintPage);
			printDoc.EndPrint += new EventHandler<EndPrintEventArgs>(printDoc_EndPrint);
		}

		public void print(Control printElem) {
			this.printElem = printElem;
			printDoc.Print("Chart");
		}


		private void printDoc_PrintPage(object sender, PrintPageEventArgs e) {
			printElem.Width = e.PrintableArea.Width;
			printElem.Height = e.PrintableArea.Height;
			e.PageVisual = printElem;

			e.HasMorePages = false;
		}

		void printDoc_EndPrint(object sender, EndPrintEventArgs e) {
			printElem.ClearValue(Grid.WidthProperty);
			printElem.ClearValue(Grid.HeightProperty);
		}

		public void SaveToImage(UIElement chart) {
			try {
				WriteableBitmap bitmap = new WriteableBitmap(chart, null);
				if (bitmap != null) {
					SaveFileDialog saveDlg = new SaveFileDialog();
					saveDlg.Filter = "JPEG Files (*.jpeg)|*.jpeg";
					saveDlg.DefaultExt = ".jpeg";
					if ((bool)saveDlg.ShowDialog()) {
						using (Stream fs = saveDlg.OpenFile()) {
							MemoryStream stream = GetImageStream(bitmap);
							byte[] binaryData = new Byte[stream.Length];
							long bytesRead = stream.Read(binaryData, 0, (int)stream.Length);
							fs.Write(binaryData, 0, binaryData.Length);
						}
					}
				}
			} catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine("Note: Please make sure that Height and Width of the chart is set properly.");
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		public static MemoryStream GetImageStream(WriteableBitmap bitmap) {
			byte[][,] raster = ReadRasterInformation(bitmap);
			return EncodeRasterInformationToStream(raster, ColorSpace.RGB);
		}

		public static byte[][,] ReadRasterInformation(WriteableBitmap bitmap) {
			int width = bitmap.PixelWidth;
			int height = bitmap.PixelHeight;
			int bands = 3;
			byte[][,] raster = new byte[bands][,];
			for (int i = 0; i < bands; i++) {
				raster[i] = new byte[width, height];
			}

			for (int row = 0; row < height; row++) {
				for (int column = 0; column < width; column++) {
					int pixel = bitmap.Pixels[width * row + column];
					raster[0][column, row] = (byte)(pixel >> 16);
					raster[1][column, row] = (byte)(pixel >> 8);
					raster[2][column, row] = (byte)pixel;
				}
			}
			return raster;
		}

		public static MemoryStream EncodeRasterInformationToStream(byte[][,] raster, ColorSpace colorSpace) {
			ColorModel model = new ColorModel { colorspace = ColorSpace.RGB };
			FluxJpeg.Core.Image img = new FluxJpeg.Core.Image(model, raster);
			MemoryStream stream = new MemoryStream();
			FluxJpeg.Core.Encoder.JpegEncoder encoder = new FluxJpeg.Core.Encoder.JpegEncoder(img, 100, stream);
			encoder.Encode();
			stream.Seek(0, SeekOrigin.Begin);
			return stream;
		}

		public static ChartActions Actions() {
			if (actions==null){
				actions=new ChartActions();
			}
			return actions;
		}

		public static void showMessage(string message) {
			System.Windows.Browser.HtmlPage.Window.Invoke("showMessage", message);
		}

		public static void logMessage(string message) {
			System.Windows.Browser.HtmlPage.Window.Invoke("logMessage", message);
		}

		public Brush getNextColor() {
			int index = (brushIndex++) % PredefinedBrushes.Count;
			return PredefinedBrushes[index];
		}

		public void ToggleFullScreen() {
			Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
		}
	}
}
