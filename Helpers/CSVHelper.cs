using System.Data;
using Microsoft.VisualBasic.FileIO;

namespace dashboard_api.Helpers
{
    public class CSVHelper
    {
        public static DataTable GetDataTable(string csvFile)
        {
            DataTable csvData = new DataTable();

            try
            {
                using (TextFieldParser csvParser = new TextFieldParser(csvFile))
                {
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;
                    string[] columns = csvParser.ReadFields();
                    foreach (var col in columns)
                    {
                        DataColumn dataCol = new DataColumn(col);
                        dataCol.AllowDBNull = true;
                        csvData.Columns.Add(dataCol);
                    }
                    while (!csvParser.EndOfData)
                    {
                        string[] fieldData = csvParser.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (System.Exception)
            {
                return null;
            }

            return csvData;
        }
    }
}