using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace Restaurant.Core.ReportContext.Utils
{
    public class ExcelWriter
    {
        private const string DefaultNumericFormat = "0.00";
        private const string DefaultDateFormat = "DD/MM/YYYY";
        private const string DefaultPercentageFormat = "0%";
        private const string DefaultInt64Format = "0";
        private const string DefaultText = "@";

        public Dictionary<int, ICellStyle> ColumnStyles;

        public ExcelWriter(
            IWorkbook workbook = null,
            string dateFormat = DefaultDateFormat,
            string numericFormat = DefaultNumericFormat,
            string int64Format = DefaultInt64Format)
        {
            DateFormat = dateFormat;
            NumericFormat = numericFormat;
            IntFormat = int64Format;

            Workbook = workbook ?? new HSSFWorkbook();

            ColumnStyles = new Dictionary<int, ICellStyle>();
        }

        public IWorkbook Workbook { get; }
        protected string DateFormat { get; }
        protected string IntFormat { get; }
        protected string NumericFormat { get; }

        public static string GetCellFormat(Type t)
        {
            if (t == typeof(DateTime))
            {
                return DefaultDateFormat;
            }

            if (t == typeof(decimal))
            {
                return DefaultNumericFormat;
            }

            if (t == typeof(long))
            {
                return DefaultInt64Format;
            }

            return DefaultText;
        }

        public static void MergeCells(ISheet sheet, int firstRow, int lastRow, int firstCol, int lastCol)
        {
            var cra = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
            sheet.AddMergedRegion(cra);
        }

        public static void MergeCellsAndAddBorder(ISheet sheet, int firstRow, int lastRow, int firstCol, int lastCol)
        {
            var cra = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
            sheet.AddMergedRegion(cra);

            RegionUtil.SetBorderBottom(1, cra, sheet, sheet.Workbook);
            RegionUtil.SetBorderTop(1, cra, sheet, sheet.Workbook);
            RegionUtil.SetBorderLeft(1, cra, sheet, sheet.Workbook);
            RegionUtil.SetBorderRight(1, cra, sheet, sheet.Workbook);
        }

        public IWorkbook DataToExcel(DataTable dataTable, string sheetName = null, IDictionary<int, string> columnFormatting = null)
        {
            try
            {
                ISheet sheet;
                if (sheetName != null)
                    sheet = Workbook.CreateSheet(sheetName);
                else
                    sheet = Workbook.CreateSheet();

                var headerRow = sheet.CreateRow(0);

                foreach (DataColumn column in dataTable.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);

                var rowIndex = 1;
                foreach (DataRow row in dataTable.Rows)
                {
                    var dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        var cell = dataRow.CreateCell(column.Ordinal);

                        string columnType = null;
                        if (columnFormatting != null && columnFormatting.ContainsKey(column.Ordinal))
                            columnType = columnFormatting[ column.Ordinal ];

                        SetCellTypeAndValue(cell, column, row[ column ], columnType);
                    }

                    rowIndex++;
                }
            }
            catch (Exception e)
            {
                Debug.Fail($"A critical exception occurred while parsing data to excel: {e.Message}");
                throw;
            }

            return Workbook;
        }

        public int DataToSheet(
            DataTable dataTable,
            ISheet sheet,
            bool writeColumnNames = true,
            int leaveEmptyRowsCount = 0,
            IDictionary<int, string> columnFormatting = null)
        {
            var rowIndex = -1;
            using (dataTable)
            {
                rowIndex = leaveEmptyRowsCount;

                if (writeColumnNames)
                {
                    var headerRow = sheet.CreateRow(rowIndex);      // To add a row in the table

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
                    }

                    rowIndex++;
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    var dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        var cell = dataRow.CreateCell(column.Ordinal);

                        string columnType = null;
                        if (columnFormatting != null && columnFormatting.ContainsKey(column.Ordinal))
                            columnType = columnFormatting[ column.Ordinal ];

                        SetCellTypeAndValue(cell, column, row[ column ], columnType);
                    }

                    rowIndex++;
                }
            }

            return rowIndex;
        }

        /// <summary>
        /// Writes data to excel file and returns the result as stream
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="sheetName"></param>
        /// <param name="columnFormating"></param>
        /// <returns></returns>
        public MemoryStream DataToStream(DataTable dataTable, string sheetName = null, IDictionary<int, string> columnFormating = null)
        {
            var data = DataToExcel(dataTable, sheetName, columnFormating);

            return WorkBookToStream(data);
        }

        public MemoryStream WorkBookToStream(IWorkbook workbook)
        {
            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            return ms;
        }

        public ISheet WriteRowToSheet(ISheet sheet, string[] data, int rowIndex)
        {
            var dataRow = sheet.CreateRow(rowIndex);
            for (int i = 0; i < data.Length; i++)
            {
                dataRow.CreateCell(i).SetCellValue(data[ i ]);
            }

            return sheet;
        }

        public MemoryStream WriteToExcel(SimpleSheetData data)
        {
            MemoryStream ms = new MemoryStream();

            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(data.SheetName);
            IRow headerRow = sheet.CreateRow(0);      //To add a row in the table

            int columnIndex = 0;

            foreach (var header in data.ColumnHeaders)
            {
                IRow dataRow = sheet.CreateRow(0);
                ICell cell = dataRow.CreateCell(columnIndex);

                cell.SetCellValue(header);

                columnIndex++;
            }

            int rowIndex = 1;
            foreach (var row in data.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                for (int i = 0; i < row.Length; i++)
                {
                    ICell cell = dataRow.CreateCell(i);
                    cell.SetCellValue(row[ i ]);
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            return ms;
        }

        private void CopyCellValue(ICell oldCell, ICell newCell)
        {
            // Set the cell data value
            switch (oldCell.CellType)
            {
                case CellType.Blank:
                    newCell.SetCellValue(oldCell.StringCellValue);
                    break;

                case CellType.Boolean:
                    newCell.SetCellValue(oldCell.BooleanCellValue);
                    break;

                case CellType.Error:
                    newCell.SetCellErrorValue(oldCell.ErrorCellValue);
                    break;

                case CellType.Formula:
                    newCell.SetCellFormula(oldCell.CellFormula);
                    break;

                case CellType.Numeric:
                    newCell.SetCellValue(oldCell.NumericCellValue);
                    break;

                case CellType.String:
                    newCell.SetCellValue(oldCell.RichStringCellValue);
                    break;

                case CellType.Unknown:
                    newCell.SetCellValue(oldCell.StringCellValue);
                    break;
            }
        }

        private void CopyRow(HSSFWorkbook workbook, HSSFSheet worksheet, int sourceRowNum, int destinationRowNum)
        {
            // Get the source / new row
            IRow newRow = worksheet.GetRow(destinationRowNum);
            IRow sourceRow = worksheet.GetRow(sourceRowNum);

            // If the row exist in destination, push down all rows by 1 else create a new row
            if (newRow != null)
            {
                worksheet.ShiftRows(destinationRowNum, worksheet.LastRowNum, 1);
            }
            else
            {
                newRow = worksheet.CreateRow(destinationRowNum);
            }

            // Loop through source columns to add to new row
            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                ICell oldCell = sourceRow.GetCell(i);
                ICell newCell = newRow.CreateCell(i);

                // If the old cell is null jump to next cell
                if (oldCell == null)
                {
                    newCell = null;
                    continue;
                }

                // Copy style from old cell and apply to new cell
                ICellStyle newCellStyle = workbook.CreateCellStyle();
                newCellStyle.CloneStyleFrom(oldCell.CellStyle); ;
                newCell.CellStyle = newCellStyle;

                // If there is a cell comment, copy
                if (newCell.CellComment != null) newCell.CellComment = oldCell.CellComment;

                // If there is a cell hyperlink, copy
                if (oldCell.Hyperlink != null) newCell.Hyperlink = oldCell.Hyperlink;

                // Set the cell data type
                newCell.SetCellType(oldCell.CellType);

                CopyCellValue(oldCell, newCell);
            }

            // If there are are any merged regions in the source row, copy to new row
            for (int i = 0; i < worksheet.NumMergedRegions; i++)
            {
                CellRangeAddress cellRangeAddress = worksheet.GetMergedRegion(i);
                if (cellRangeAddress.FirstRow == sourceRow.RowNum)
                {
                    CellRangeAddress newCellRangeAddress = new CellRangeAddress(newRow.RowNum,
                                                                                (newRow.RowNum +
                                                                                 (cellRangeAddress.FirstRow -
                                                                                  cellRangeAddress.LastRow)),
                                                                                cellRangeAddress.FirstColumn,
                                                                                cellRangeAddress.LastColumn);
                    worksheet.AddMergedRegion(newCellRangeAddress);
                }
            }
        }

        private ICellStyle GetColumnStyle(int index, string format)
        {
            if (ColumnStyles.ContainsKey(index))
            {
                return ColumnStyles[ index ];
            }

            var cellStyle = Workbook.CreateCellStyle();
            if (string.IsNullOrEmpty(format))
            {
                ColumnStyles.Add(index, cellStyle);
                return cellStyle;
            }

            var cellFormat = Workbook.CreateDataFormat();
            cellStyle.DataFormat = cellFormat.GetFormat(format);

            ColumnStyles.Add(index, cellStyle);

            return cellStyle;
        }

        private void SetCellTypeAndValue(ICell cell, DataColumn column, object value, string cellFormat)
        {
            // If column formatiing is set
            if (cellFormat != null)
            {
                if (column.DataType == typeof(DateTime) || column.DataType == typeof(DateTime?) || cellFormat == DefaultDateFormat)
                    SetCellValueDateTime(cell, value);

                if (column.DataType == typeof(decimal))
                    cell.SetCellValue(value == DBNull.Value || value == null ? 0 : (double)(decimal)value);

                cell.CellStyle = GetColumnStyle(column.Ordinal, cellFormat);
                return;
            }

            // If no formatiing is set at all
            if (column.DataType == typeof(DateTime))
            {
                SetCellValueDateTime(cell, value);

                cell.CellStyle = GetColumnStyle(column.Ordinal, DefaultDateFormat);
                return;
            }

            if (column.DataType == typeof(decimal))
            {
                cell.SetCellValue(value == DBNull.Value || value == null ? 0 : (double)(decimal)value);
                cell.CellStyle = GetColumnStyle(column.Ordinal, DefaultNumericFormat); // Will get default format
                return;
            }

            if (column.DataType == typeof(Int64))
            {
                cell.SetCellValue(value == DBNull.Value || value == null ? 0 : (Int64)value);
                cell.CellStyle = GetColumnStyle(column.Ordinal, IntFormat); // Will get default format
                return;
            }

            cell.SetCellValue(value == DBNull.Value || value == null ? String.Empty : value.ToString());
            cell.SetCellType(CellType.String);
        }

        private void SetCellValueDateTime(ICell cell, object value)
        {
            if (value == DBNull.Value)
                cell.SetCellValue(string.Empty);
            else
            {
                var date = Convert.ToDateTime(value);
                cell.SetCellValue(date);
            }
        }
    }

    public class SimpleSheetData
    {
        public string[] ColumnHeaders { get; set; }
        public string SheetName { get; set; }
        public IList<string[]/*Index: column index; Value: cell value*/> Rows { get; set; }
    }
}
