using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;
using System.Text.RegularExpressions;

namespace pdfExtrator
{
	public class Person
	{
		private string name = string.Empty;
		private string age = string.Empty;
		private string sex = string.Empty;
		private string address = string.Empty;
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		public string Age
		{
			get
			{
				return this.age;
			}
			set
			{
				this.age = value;
			}
		}
		public string Sex
		{
			get 
			{
				return this.sex;
			}
			set
			{
				this.sex = value;
			}
		}
		public string Address
		{
			get
			{
				return this.address;
			}
			set
			{
				this.address = value;
			}
		}
	}
	class MainClass
	{
		private static List<Person> people = new List<Person>();
		public static void Main (string[] args)
		{
			//Console.WriteLine(testPDF("../../docs/test2.pdf"));
			Person data = new Person();
			data.Name = "Name";
			data.Age = "Age";
			data.Address = "Address";
			data.Sex = "Sex";
			people.Add(data);
			testsearcharea("../../docs/testdata.pdf");
			CreatingCsvFiles();
			//Console.WriteLine(testStringSearch);
			//Console.WriteLine(ExtractTextFromPdf("../../docs/testdata.pdf"));
			Console.ReadLine();
		}
		public static string ExtractTextFromPdf(string path)
		{
			using (PdfReader reader = new PdfReader(path))
			{
				StringBuilder text = new StringBuilder();
				//Console.WriteLine(reader.NumberOfPages);
				for (int i = 1; i <= reader.NumberOfPages; i++)
				{
					//Console.WriteLine(text);
					text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
				}

				return text.ToString();
			}
		}

		public static string testPDF(string path)
		{
			var text = new StringBuilder();

			// The PdfReader object implements IDisposable.Dispose, so you can
			// wrap it in the using keyword to automatically dispose of it
			using (var pdfReader = new PdfReader(path))
			{
				// Loop through each page of the document
				for (var page = 1; page <= pdfReader.NumberOfPages; page++)
				{
					ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

					var currentText = PdfTextExtractor.GetTextFromPage(
						pdfReader, 
						page, 
						strategy);

					currentText =
						Encoding.UTF8.GetString(Encoding.Convert(
							Encoding.Default,
							Encoding.UTF8,
							Encoding.Default.GetBytes(currentText)));
					Console.WriteLine("text:" + currentText);
					text.Append(currentText);
				}
			}
			return text.ToString();
		}

		public static string testStringSearch(string str)
		{
			String St = "வாக்காளர் ெபயர் : பிரபாகர்வாக்காளர் ெபயர் : ெகளவாக்காளர் ெபயர் : அேசாமாகுக்ர் \nசந்தரா";
			int pFrom = St.IndexOf("ெபயர் : ") + "ெபயர் : ".Length;
			int pTo = St.LastIndexOf("ெபயர் : ");
			return St.Substring(pFrom, pTo - pFrom);
		}

		public static void testsearcharea(string path)
		{
			// In this example, I'll declare a pageNumber integer variable to 
			// only capture text from the page I'm interested in
			int pageNumber = 3;

			var text = new StringBuilder();

			// The PdfReader object implements IDisposable.Dispose, so you can
			// wrap it in the using keyword to automatically dispose of it
			using (var pdfReader = new PdfReader(path))
			{
				float distanceInPixelsFromLeft = 30;
				//float distanceInPixelsFromBottom = 710;
				float distanceInPixelsFromBottom = 70;
				//float width = 190;
				//float height = 70;
				float width = 190;
				float height = 700;

				var rect = new System.util.RectangleJ(
					distanceInPixelsFromLeft,
					distanceInPixelsFromBottom, 
					width, 
					height);

				var filters = new RenderFilter[1];
				filters[0] = new RegionTextRenderFilter(rect);

				ITextExtractionStrategy strategy =
					new FilteredTextRenderListener(
						new LocationTextExtractionStrategy(), 
						filters);

				var currentText = PdfTextExtractor.GetTextFromPage(
					pdfReader, 
					pageNumber, 
					strategy);
				currentText =
					Encoding.UTF8.GetString(Encoding.Convert(
						Encoding.Default,
						Encoding.UTF8,
						Encoding.Default.GetBytes(currentText)));

				text.Append(currentText);
			}
			string str = text.ToString();
			string[] lines = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
			//bool flag = false;
			Person p = new Person();
			string tName = "வாக்காளர் ெபயர் : ";
			string tAge = "வயது :  ";
			string tAddress = "திய /பைழய வட்டு எண் : ";
			string tMale = "இனம் : ஆண்";
			string tFemale = "இனம் : ெபண்";
			string temp = string.Empty;
			foreach(string line in lines)
			{
				if(line.Contains(tName))
				{
					p.Name = line.Replace(tName, string.Empty);
				}
				else if(line.Contains(tAddress))
				{
					p.Address = line.Replace(tAddress, string.Empty);
				}
				else if(line.Contains(tAge))
				{
					temp = string.Empty;
					if(line.Contains(tMale))
					{
						p.Sex = "M";
						temp = line.Replace(tMale, string.Empty);
					}
					else if(line.Contains(tFemale))
					{
						p.Sex = "F";
						temp = line.Replace(tFemale, string.Empty);
					}
					p.Age = temp.Replace(tAge, string.Empty);
					people.Add(p);
					p = new Person();
				}
				Console.WriteLine("test-->"+ line);
			}
		}

		public static void CreatingCsvFiles()
		{
			string filePath = "../../docs/" + "test2.csv";
			if (!File.Exists(filePath))
			{
				File.Create(filePath).Close();
			}
			string delimiter = ",";
			StringBuilder sb = new StringBuilder();
			foreach(Person person in people)
				sb.AppendLine(string.Join(delimiter, new string[]{person.Name,person.Age,person.Sex,person.Address}));
			File.AppendAllText(filePath, sb.ToString());
		}
	}
}
