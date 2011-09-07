using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Conversive.PHPSerializationLibrary;
using System.Collections;
namespace PILib.NNET
{
	public enum NNETMODEL { vges_nb }
	public class NNET
	{
		protected static SortedList<NNETMODEL,NNET>nnets;
		protected object nnetObj;

		protected SortedList<int,SortedList<int,double>> l1c,l2c,l1w,l2w;
		protected SortedList<int,double>l1b,l2b;

		protected NNET(NNETMODEL name) {
			init(name);	
		}

		static NNET() {
			nnets = new SortedList<NNETMODEL, NNET>();
		}

		public static NNET getNNET(NNETMODEL name) {
			if (!nnets.Keys.Contains(name)) {
				NNET nnet=new NNET(name);
				nnets.Add(name,nnet);
			}
			return nnets[name];
		}

		public void init(NNETMODEL name) {
			Serializer serializer=new Serializer();

			switch (name) {
				case NNETMODEL.vges_nb:
					nnetObj = serializer.Deserialize(nnet.vges_nb);
					break;
			}
			
			Hashtable nnetArr=nnetObj as Hashtable;
			l1c = this.readListListDouble(nnetArr["l1c"]);
			l2c = this.readListListDouble(nnetArr["l2c"]);
			l1w = this.readListListDouble(nnetArr["l1w"]);
			l2w = this.readListListDouble(nnetArr["l2w"]);
			l1b = this.readListDouble(nnetArr["l1b"]);
			l2b = this.readListDouble(nnetArr["l2b"]);	
		}

		protected SortedList<int,double> readListDouble(object inpObj) {
			ArrayList arrObj=inpObj as ArrayList;
			SortedList<int,double> result=new SortedList<int,double>();
			int index=-1;
			foreach (object elem in arrObj) {
				index++;
				double val=Double.Parse(elem.ToString().Replace(".",","));
				result.Add(index,val);
			}
			return result;
		}

		protected SortedList<int,SortedList<int,double>> readListListDouble(object inpObj) {
			ArrayList arrObj=inpObj as ArrayList;
			SortedList<int,SortedList<int,double>> result=new SortedList<int, SortedList<int, double>>();
			int index=-1;
			foreach (object elem in arrObj) {
				index++;
				SortedList<int,double>val=this.readListDouble(elem);
				result.Add(index,val);
			}
			return result;
		}

		public SortedList<int, double> calc(SortedList<int, double> inputVector) {
			SortedList<int,double>l1b,l2b;
			l1b = new SortedList<int, double>();
			l2b = new SortedList<int, double>();

			foreach (KeyValuePair<int,double> de in this.l1b) {
				l1b.Add(de.Key, de.Value);
			}
			foreach (KeyValuePair<int,double> de in this.l2b) {
				l2b.Add(de.Key, de.Value);
			}


			SortedList<int,double> outputVector=new SortedList<int, double>();
			
			for (int index=0;index<inputVector.Keys.Count;index++){
				int n=inputVector.Keys[index];
				double v=inputVector[n];			
				double newVal=(v-l1c[n][0])/l1c[n][1];
				inputVector[n] = newVal;
			}

			foreach (KeyValuePair<int,SortedList<int,double>>de in l1w) {
				int n=de.Key;
				SortedList<int, double>lw=de.Value;
				foreach (KeyValuePair <int,double> innerDE in lw) {
					int i=innerDE.Key;
					double w=innerDE.Value;
					l1b[n] += w * inputVector[i];					
				}
				l1b[n] = func(l1b[n]);
			}

			foreach (KeyValuePair<int,SortedList<int,double>>de in l2w) {
				int n=de.Key;
				SortedList<int, double>lw=de.Value;
				foreach (KeyValuePair <int,double> innerDE in lw) {
					int t=innerDE.Key;
					double w=innerDE.Value;
					l2b[n] += w * l1b[t];				
				}
				double val=func(l2b[n]);					
				outputVector.Add(n, val);
			}

			for (int index=0;index<outputVector.Keys.Count;index++){
				int n=outputVector.Keys[index];
				double v=outputVector[n];		
				outputVector[n] = v * l2c[n][0] + l2c[n][1];
			}

			return outputVector;

		}

		protected double func(double x) {
			return 1.0 / (1+Math.Exp(-x));
		}


	}
}
