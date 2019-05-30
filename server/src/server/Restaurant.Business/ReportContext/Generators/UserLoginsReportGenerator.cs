using Marten;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Optional;
using Restaurant.Business.ReportContext._Base;
using Restaurant.Core.ReportContext.Commands;
using Restaurant.Core.ReportContext.Enums;
using Restaurant.Core.ReportContext.Utils;
using Restaurant.Domain;
using Restaurant.Domain.Events.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Business.ReportContext.Generators
{
    public class UserLoginsReportGenerator : ExcelReportBase<UserLoginsReportRequest>
    {
        // CONSTANTS
        private const int ColumnHeaderRowsCount = 1;
        private const int ColumnHeaderStartPosition = 1;
        private const int RowsDefaultHeight = 400;
        private const string ValuesDataFormat = "#,##0.00";

        private static readonly byte[] YellowBackgroundRgb = { 253, 184, 19 };
        private static readonly byte[] LightYellowBackgroundRgb = { 244, 200, 91 };
        private static readonly byte[] LightGrayBackgroundRgb = { 103, 112, 119 };
        private static readonly byte[] BlackBackgroundRgb = { 0, 0, 0 };
        private static readonly byte[] BlueBackgroundRgb = { 4, 23, 72 };
        private static readonly byte[] WhiteForegroundRgb = { 255, 255, 255 };

        private static int ValueStartRowIndex => ColumnHeaderRowsCount + ColumnHeaderStartPosition + 1;

        // Dependencies
        private readonly IDocumentSession _session;

        private ISheet _sheet;
        private ICellStyle _valueCellStyle;

        public UserLoginsReportGenerator(IWorkbook workbook, IDocumentSession session)
            : base(workbook)
        {
            _session = session;
        }

        protected override Option<IWorkbook, Error> PrepareReport(UserLoginsReportRequest model) =>
            CreateReportSheet().FlatMap(sheet =>
            InitializeData().FlatMap(logIns =>
            WriteHeaders(sheet).FlatMap(_ =>
            WriteValueRows(sheet, logIns.ToArray())))).Map(_ => Workbook);

        protected override bool DecorateCells()
        {
            _sheet.DisplayGridlines = false;
            for (var i = 0; i < _sheet.GetRow(ColumnHeaderStartPosition).Cells.Count; i++)
            {
                _sheet.SetColumnWidth(i, ColumnWidthXXL);
            }

            return true;
        }

        protected override string GetReportFileName() =>
            $"user_logins_report_{DateTime.Now.Date.ToShortDateString()}.xlsx";

        private Option<IEnumerable<UserLoggedIn>, Error> InitializeData() =>
            _session.Events.QueryRawEventDataOnly<UserLoggedIn>().Some<IEnumerable<UserLoggedIn>, Error>();

        private Option<ISheet, Error> CreateReportSheet()
        {
            _sheet = Workbook.CreateSheet("Report");
            return _sheet.Some<ISheet, Error>();
        }

        private Option<IRow, Error> WriteHeaders(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(ColumnHeaderStartPosition);
            var userLoggedInInfoHeaders = new[] { nameof(UserLoggedIn.UserId), nameof(UserLoggedIn.DateTime)};

            for (var columnIndex = 0; columnIndex < userLoggedInInfoHeaders.Length; columnIndex++)
            {
                // Here we merge on row level ie 4 rows from current column will be merged
                ExcelWriter.MergeCellsAndAddBorder(
                    sheet,
                    ColumnHeaderStartPosition,
                    ColumnHeaderStartPosition + ColumnHeaderRowsCount - 1,
                    columnIndex,
                    columnIndex);

                CreateHeaderCell(headerRow, columnIndex, userLoggedInInfoHeaders[ columnIndex ], HeaderCellBackgroundColor.Yellow);
            }

            return headerRow.Some<IRow, Error>();
        }

        private ICell CreateHeaderCell(IRow row, int columnIndex, string value, HeaderCellBackgroundColor color)
        {
            var cell = row.CreateCell(columnIndex);

            switch (color)
            {
                case HeaderCellBackgroundColor.Yellow:
                    cell.CellStyle = GetYellowHeaderStyle();
                    break;
                case HeaderCellBackgroundColor.LightGray:
                    cell.CellStyle = GetLightGrayHeaderStyle();
                    break;
                case HeaderCellBackgroundColor.DarkGray:
                    cell.CellStyle = GetDarkGrayHeaderStyle();
                    break;
                case HeaderCellBackgroundColor.Blue:
                    cell.CellStyle = GetBlueHeaderStyle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }

            cell.Row.Height = -1;

            cell.SetCellValue(value);

            return cell;
        }

        private Option<ISheet, Error> WriteValueRows(ISheet sheet, UserLoggedIn[] loggedIns)
        {
            var rowIndex = ValueStartRowIndex;
            var columnIndex = 0;
            foreach (var userLoggedIn in loggedIns)
            {
                var row = sheet.CreateRow(rowIndex);
                CreateUserDetailCell(row, columnIndex, userLoggedIn.UserId.ToString());
                columnIndex++;
                CreateUserDetailCell(row, columnIndex, userLoggedIn.DateTime.ToString());

                // new row
                rowIndex++;
                columnIndex = 0;
            }

            return sheet.Some<ISheet, Error>();
        }

        private ICell CreateUserDetailCell(IRow row, int columnIndex, string value)
        {
            var cell = row.CreateCell(columnIndex);

            cell.SetCellValue(value);
            cell.CellStyle = ValueCellStyle;
            cell.Row.Height = RowsDefaultHeight;

            return cell;
        }

        #region CellsDecoration

        private XSSFCellStyle GetYellowHeaderStyle()
        {
            var style = GetElementColumnHeaderCellStyleDarkText();
            style.SetFillForegroundColor(new XSSFColor(YellowBackgroundRgb));
            style.FillPattern = FillPattern.SolidForeground;

            return style;
        }

        private XSSFCellStyle GetLightYellowHeaderStyle()
        {
            var style = GetElementColumnHeaderCellStyleDarkText();
            style.SetFillForegroundColor(new XSSFColor(LightYellowBackgroundRgb));
            style.FillPattern = FillPattern.SolidForeground;

            return style;
        }

        private XSSFCellStyle GetLightGrayHeaderStyle()
        {
            var style = GetElementColumnHeaderCellStyle();
            style.SetFillForegroundColor(new XSSFColor(LightGrayBackgroundRgb));
            style.FillPattern = FillPattern.SolidForeground;

            return style;
        }

        private XSSFCellStyle GetDarkGrayHeaderStyle()
        {
            var style = GetElementColumnHeaderCellStyle();
            style.SetFillForegroundColor(new XSSFColor(BlackBackgroundRgb));
            style.FillPattern = FillPattern.SolidForeground;

            return style;
        }

        private XSSFCellStyle GetBlueHeaderStyle()
        {
            var style = GetElementColumnHeaderCellStyle();
            style.SetFillForegroundColor(new XSSFColor(BlueBackgroundRgb));
            style.FillPattern = FillPattern.SolidForeground;

            return style;
        }

        private XSSFCellStyle GetElementColumnHeaderCellStyle()
        {
            var style = (XSSFCellStyle)Workbook.CreateCellStyle();

            style = AddBordersToStyle(style);
            var font = GetHeaderFont();
            ((XSSFFont)font).SetColor(new XSSFColor(WhiteForegroundRgb));
            style.SetFont(font);
            style.WrapText = true;

            return style;
        }

        private XSSFCellStyle GetElementColumnHeaderCellStyleDarkText()
        {
            var style = (XSSFCellStyle)Workbook.CreateCellStyle();

            style = AddBordersToStyle(style);
            var font = GetHeaderFont();
            ((XSSFFont)font).SetColor(new XSSFColor(BlackBackgroundRgb));
            style.SetFont(font);
            style.WrapText = true;

            return style;
        }

        private XSSFCellStyle AddBordersToStyle(XSSFCellStyle style)
        {
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;

            return style;
        }

        private IFont GetHeaderFont(short fontSize = 12)
        {
            var font = (XSSFFont)Workbook.CreateFont();
            font.FontHeightInPoints = fontSize;
            font.FontName = "Calibri";
            font.Boldweight = (short)FontBoldWeight.Bold;

            return font;
        }

        public ICellStyle ValueCellStyle
        {
            get => _valueCellStyle ?? (_valueCellStyle = GetValueCellStyle());

            set => _valueCellStyle = value;
        }

        private XSSFCellStyle GetValueCellStyle()
        {
            var style = (XSSFCellStyle)Workbook.CreateCellStyle();
            style = AddBordersToStyle(style);
            var font = GetValueFont();
            style.SetFont(font);
            style.DataFormat = Workbook.CreateDataFormat().GetFormat(ValuesDataFormat);

            return style;
        }

        private IFont GetValueFont(short fontSize = 10)
        {
            var font = Workbook.CreateFont();
            font.FontHeightInPoints = fontSize;
            font.FontName = "Calibri";
            font.Boldweight = (short)FontBoldWeight.Normal;

            return font;
        }

        #endregion
    }
}