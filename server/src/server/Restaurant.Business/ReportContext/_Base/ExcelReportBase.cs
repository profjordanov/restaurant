using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Optional;
using Restaurant.Core._Base;
using Restaurant.Domain;
using System.Collections.Generic;
using System.IO;

namespace Restaurant.Business.ReportContext._Base
{
    public abstract class ExcelReportBase<TReportModel>
        where TReportModel : ICommand<HttpFile>
    {
        // CONSTANTS
        private const string ApplicationXls = "application/vnd.ms-excel";
        private const string ApplicationXlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        protected const int ColumnWidthM = 3500;
        protected const int ColumnWidthX = 5000;
        protected const int ColumnWidthXL = 8000;
        protected const int ColumnWidthXXL = 10000;

        protected List<ISheet> Sheets;
        protected IWorkbook Workbook;

        protected ExcelReportBase(IWorkbook workbook)
        {
            Workbook = workbook;
        }

        public virtual HttpFile GetReport(TReportModel model)
        {
            if (!PrepareReport(model).HasValue)
            {
                return null;
            }

            DecorateCells();

            return WriteToFile();
        }

        protected abstract bool DecorateCells();

        protected abstract string GetReportFileName();

        protected abstract Option<IWorkbook, Error> PrepareReport(TReportModel model);

        protected void ResizeColumns(int size)
        {
            foreach (var sheet in Sheets)
            {
                for (var i = 0; i < sheet.GetRow(0)?.Cells.Count; i++)
                {
                    sheet.SetColumnWidth(i, size);
                }
            }
        }

        protected HttpFile WriteToFile()
        {
            var mimeType = string.Empty;
            switch (Workbook)
            {
                case XSSFWorkbook _:
                    mimeType = ApplicationXlsx;
                    break;
                case HSSFWorkbook _:
                    mimeType = ApplicationXls;
                    break;
            }

            using (var stream = new MemoryStream())
            {
                Workbook.Write(stream);
                return new HttpFile(stream.ToArray(), GetReportFileName(), mimeType);
            }
        }

        protected int GetColumnWidthFromPoints(int widthInPoints)
        {
            return (widthInPoints * 256) + 200;
        }
    }
}