using ClosedXML.Excel;
using System;
using System.IO;

public class ExcelProcessor
{
    public static void ProcessExcel(string inputPath, string outputPath)
    {
        using (var workbook = new XLWorkbook(inputPath))
        {
            var worksheet = workbook.Worksheet(1); // Get the first worksheet
            var newWorkbook = new XLWorkbook();
            var newWorksheet = newWorkbook.AddWorksheet("ProcessedData");

            // Iterate rows and process the column "col1"
            int col1Index = 0;

            var headerRow = worksheet.Row(1); // Find the header row
            foreach (var cell in headerRow.CellsUsed())
            {
                if (cell.Value.ToString() == "col1")
                {
                    col1Index = cell.Address.ColumnNumber;
                    break;
                }
            }

            if (col1Index == 0)
            {
                Console.WriteLine("Column 'col1' not found.");
                return;
            }

            // Copy the "col1" data to the new worksheet
            int newRowNumber = 1;
            foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip the header row
            {
                newWorksheet.Cell(newRowNumber++, 1).Value = row.Cell(col1Index).Value;
            }

            // Save the new Excel file
            newWorkbook.SaveAs(outputPath);
        }
    }
}