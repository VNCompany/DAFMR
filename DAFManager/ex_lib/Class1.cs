using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;

namespace ex_lib
{
    public class ExcelWorker
    {
        public static string[][] Parse(string filename)
        {
            List<string[]> vs = new List<string[]>();
            using (var pack = new ExcelPackage(new FileInfo(filename)))
            {
                using (ExcelWorksheet sheet = pack.Workbook.Worksheets[1])
                {
                    int count = sheet.Dimension.Rows;
                    if (sheet.Dimension.Columns != 5) throw new Exception();
                    for (int i = 1; i <= count; i++)
                    {
                        vs.Add(new string[5] { sheet.Cells[i, 1].Text, sheet.Cells[i, 2].Text, sheet.Cells[i, 3].Text, sheet.Cells[i, 4].Text, sheet.Cells[i, 5].Text });
                    }
                }
            }
            return vs.ToArray();
        }
    }
}
