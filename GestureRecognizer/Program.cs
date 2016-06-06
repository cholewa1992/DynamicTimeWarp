using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using dk.itu.jbec.DTW;
using System.Collections.Concurrent;
using Microsoft.AspNet.SignalR.Client;

namespace Recognizer
{		
	class MainClass
	{
		public static void Main (string[] args)
		{
			var templates = new TemplateCollection<IList<double>> ();
			var l = 6;

			var connection = new HubConnection ("http://watchboard.azurewebsites.net");
			var proxy = connection.CreateHubProxy ("EchoHub");
			connection.Start ();

			using (var reader = new StreamReader ("templates.csv")) {
				
				while (!reader.EndOfStream) {

					string line = reader.ReadLine ();


					int pointer = 0;

					string[] lines = line.Split (',');
					string label = lines [lines.Length - 1];

					double[][] template = new double[(lines.Length - 1) / l] [];

					for (int n = 0; n < template.Length; n++) {
						template [n] = new double[l];
					}


					for (int n = 0; n < template.Length; n++) {
						for (int i = 0; i < template[n].Length; i++) {
							template [n] [i] = double.Parse (lines [pointer++]);
						}
					}

					templates.Add (label, template);
				}
			}



			var cap = templates.Max (t => t.Item2.Count());
			Console.WriteLine ("Template length: " + cap);
		
			var queue = new FixedSizedQueue<IList<double>>(cap);

			var dtw = new PathlessDynamicTimeWrap<IList<double>>(cap * 10 / 100, DistanceFunctions.Euclidean);
			var classifier = new DynamicTimeWrapClassifier<IList<double>> (dtw);
			var recognizer = new Recognizer<IList<double>> (classifier, templates);
			recognizer.AddFilter (new Normalizer ());


		
				
			Console.WriteLine ($"{templates.Count()} templates was loaded");

			var conf = recognizer.ConfussionMatrix ();
			var labels = recognizer.GetLabels ();

			for (int i = 0; i < conf.Length; i++) {
				for (int j = 0; j < conf[i].Length; j++) {
					Console.Write (conf [i][j] + " ");
				}
				Console.WriteLine (labels [i]);
			}
				

			using (var reader = new StreamReader("/tmp/data")) {
			//using (var reader = Console.In) {
				int count = 0;
				while (true) {

					var line = reader.ReadLine ();
					if (line == null || string.IsNullOrEmpty (line))
						continue;
					
					string[] lines = line.Split (',');

					if (lines.Length != 7)
						continue;

					double[] values = new double[lines.Length-1];

					for (int i = 0; i < values.Length; i++) {
						values [i] = double.Parse (lines [i+1]);
					}
					
					queue.Enqueue (values);



					if (queue.Count == queue.Capacity && ++count % (queue.Capacity * 75 / 100) == 0) {
						 
						var result = recognizer.TryRecognize (queue);

						if (result.Item1.Contains ("still") || result.Item2 > 2.5) {
							count--;
							continue;
						}
							
						Console.WriteLine ($"Closest label is {result.Item1} with distance {result.Item2}");
						try{ proxy.Invoke ("Send", result.Item1); }
						catch{}
					}
				}
			}
		}
	}
}
