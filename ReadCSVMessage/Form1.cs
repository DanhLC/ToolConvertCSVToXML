using CsvHelper;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ReadCSVMessage
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				ofdFile.Filter = "CSV files (*.csv)|*.csv";
				ofdFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				ofdFile.Title = "Chọn tệp CSV";
				ofdFile.FileName = "";

				if (ofdFile.ShowDialog() == DialogResult.OK)
				{
					var filePath = ofdFile.FileName;

					using (var folderBrowserDialog = new FolderBrowserDialog())
					{
						folderBrowserDialog.Description = "Chọn thư mục lưu trữ tệp XML";
						folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

						if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
						{
							var saveFolder = folderBrowserDialog.SelectedPath;
							ReadCsvAndConvertToXml(filePath, saveFolder);
						}
					}
				}

				MessageBox.Show("Tạo dữ liệu xml thành công");
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ReadCsvAndConvertToXml(string filePath, string saveFolder)
		{
			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
			{
				csv.Read();
				csv.ReadHeader();

				while (csv.Read())
				{
					var mTDiep = csv.GetField<string>("MTDiep");
					var tDiep = csv.GetField<string>("TDiep");
					var date = csv.GetField<DateTime>("Date");

					if (!string.IsNullOrEmpty(mTDiep) && !string.IsNullOrEmpty(tDiep))
					{
						var xml = XElement.Parse(tDiep);
						var folderPath = Path.Combine(saveFolder, string.Format("{0:yyyy}/{0:MM}.{0:yyyy}", date));

						if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

						var subFolderPath = Path.Combine(folderPath, "01TH.HĐĐT");

						if (!Directory.Exists(subFolderPath)) Directory.CreateDirectory(subFolderPath);

						var xmlFileName = Path.Combine(subFolderPath, string.Format("{0}.xml", mTDiep.Trim()));
						xml.Save(xmlFileName);
					}
				}
			}
		}
	}
}
