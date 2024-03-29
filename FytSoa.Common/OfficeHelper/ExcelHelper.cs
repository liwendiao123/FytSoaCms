﻿using Microsoft.AspNetCore.Mvc;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace FytSoa.Common.OfficeHelper
{
   public class ExcelHelper
    {
        #region Excel导出方法 ExcelDownload

        /// <summary>
        /// Excel导出下载
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="list">数据源</param>
        /// <param name="columnJson">导出列</param>
        /// <param name="fileName">下载文件名称</param>
        //public ActionResult ExcelDownload<T>(IEnumerable<T> list, IEnumerable<ExcelGridModel> columnList, string fileName)
        //{
        //    var excelConfig = ConvertExcelGridModelToConfig(columnList, fileName);

        //    var rowData = list.ToDataTable(columnList.Select(i => i.name));

        //    var stream = ExportMemoryStream(rowData, excelConfig);
        //    return new FileStreamResult(stream, MIMEType.xls) { FileDownloadName = JointXls(fileName) };

        //}

        /// <summary>
        /// Excel导出下载
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public ActionResult ExcelDownload(DataTable dtSource, IEnumerable<ExcelGridModel> columnList, string fileName)
        {
            var excelConfig = ConvertExcelGridModelToConfig(columnList, fileName);
            var stream = ExportMemoryStream(dtSource, excelConfig);
            return new FileStreamResult(stream, MIMEType.xls) { FileDownloadName = JointXls(fileName) };
        }

        /// <summary>
        /// Excel导出下载
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public ActionResult ExcelDownload(DataTable dtSource, ExcelConfig excelConfig, string fileName)
        {
            var stream = ExportMemoryStream(dtSource, excelConfig);
            return new FileStreamResult(stream, MIMEType.xls) { FileDownloadName = JointXls(fileName) };
        }

        #endregion Excel导出方法 ExcelDownload

        #region DataTable导出到Excel文件excelConfig中FileName设置为全路径

        /// <summary>
        /// DataTable导出到Excel文件 Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public string ExcelExportToFile(DataTable dtSource, ExcelConfig excelConfig, string fileName)
        {
            fileName = JointXls(fileName);
            using (MemoryStream ms = ExportMemoryStream(dtSource, excelConfig))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }

            return fileName;
        }


        #endregion DataTable导出到Excel文件excelConfig中FileName设置为全路径

        #region DataTable导出到Excel的MemoryStream

        /// <summary>
        /// DataTable导出到Excel的MemoryStream Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public MemoryStream ExportMemoryStream(DataTable dtSource, ExcelConfig excelConfig)
        {
            var columnEntity = excelConfig.ColumnEntity;
            if (columnEntity == null || columnEntity.Count == 0)
            {
                if (columnEntity == null)
                {
                    columnEntity = new List<ColumnModel>();
                }

                foreach (DataColumn dc in dtSource.Columns)
                {
                    columnEntity.Add(new ColumnModel
                    {
                        Alignment = "center",
                        Column = dc.ColumnName,
                        ExcelColumn = dc.ColumnName
                    });
                }
            }
            else
            {
                int colint = 0;
                for (int i = 0; i < dtSource.Columns.Count;)
                {
                    DataColumn column = dtSource.Columns[i];
                    if (excelConfig.ColumnEntity[colint].Column != column.ColumnName)
                    {
                        dtSource.Columns.Remove(column.ColumnName);
                    }
                    else
                    {
                        i++;
                        if (colint < excelConfig.ColumnEntity.Count - 1)
                        {
                            colint++;
                        }
                    }
                }
            }

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            #region 右击文件 属性信息

            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = ""; //填加xls文件作者信息
                si.ApplicationName = ""; //填加xls文件创建程序信息
                si.LastAuthor = ""; //填加xls文件最后保存者信息
                si.Comments = ""; //填加xls文件作者信息
                si.Title = ""; //填加xls文件标题信息
                si.Subject = "";//填加文件主题信息
                si.CreateDateTime = System.DateTime.Now;
                workbook.SummaryInformation = si;
            }

            #endregion 右击文件 属性信息

            #region 设置标题样式

            ICellStyle headStyle = workbook.CreateCellStyle();
            int[] arrColWidth = new int[dtSource.Columns.Count];
            string[] arrColName = new string[dtSource.Columns.Count];//列名
            ICellStyle[] arryColumStyle = new ICellStyle[dtSource.Columns.Count];//样式表
            headStyle.Alignment = HorizontalAlignment.Center; // ------------------
            if (excelConfig.Background != new Color())
            {
                if (excelConfig.Background != new Color())
                {
                    headStyle.FillPattern = FillPattern.SolidForeground;
                    headStyle.FillForegroundColor = GetXLColour(workbook, excelConfig.Background);
                }
            }
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = excelConfig.TitlePoint;
            if (excelConfig.ForeColor != new Color())
            {
                font.Color = GetXLColour(workbook, excelConfig.ForeColor);
            }
            font.Boldweight = 700;
            headStyle.SetFont(font);

            #endregion 设置标题样式

            #region 列头及样式

            ICellStyle cHeadStyle = workbook.CreateCellStyle();
            cHeadStyle.Alignment = HorizontalAlignment.Center; // ------------------
            IFont cfont = workbook.CreateFont();
            cfont.FontHeightInPoints = excelConfig.HeadPoint;
            cHeadStyle.SetFont(cfont);

            #endregion 列头及样式

            #region 设置内容单元格样式

            foreach (DataColumn item in dtSource.Columns)
            {
                ICellStyle columnStyle = workbook.CreateCellStyle();
                columnStyle.Alignment = HorizontalAlignment.Center;
                arrColWidth[item.Ordinal] = Encoding.UTF8.GetBytes(item.ColumnName.ToString()).Length;
                arrColName[item.Ordinal] = item.ColumnName.ToString();
                if (excelConfig.ColumnEntity != null)
                {
                    ColumnModel columnentity = excelConfig.ColumnEntity.Find(t => t.Column == item.ColumnName);
                    if (columnentity != null)
                    {
                        arrColName[item.Ordinal] = columnentity.ExcelColumn;
                        if (columnentity.Width != 0)
                        {
                            arrColWidth[item.Ordinal] = columnentity.Width;
                        }
                        if (columnentity.Background != new Color())
                        {
                            if (columnentity.Background != new Color())
                            {
                                columnStyle.FillPattern = FillPattern.SolidForeground;
                                columnStyle.FillForegroundColor = GetXLColour(workbook, columnentity.Background);
                            }
                        }
                        if (columnentity.Font != null || columnentity.Point != 0 || columnentity.ForeColor != new Color())
                        {
                            IFont columnFont = workbook.CreateFont();
                            columnFont.FontHeightInPoints = 10;
                            if (columnentity.Font != null)
                            {
                                columnFont.FontName = columnentity.Font;
                            }
                            if (columnentity.Point != 0)
                            {
                                columnFont.FontHeightInPoints = columnentity.Point;
                            }
                            if (columnentity.ForeColor != new Color())
                            {
                                columnFont.Color = GetXLColour(workbook, columnentity.ForeColor);
                            }
                            columnStyle.SetFont(font);
                        }
                        columnStyle.Alignment = getAlignment(columnentity.Alignment);
                    }
                }
                arryColumStyle[item.Ordinal] = columnStyle;
            }
            if (excelConfig.IsAllSizeColumn)
            {
                #region 根据列中最长列的长度取得列宽

                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    for (int j = 0; j < dtSource.Columns.Count; j++)
                    {
                        if (arrColWidth[j] != 0)
                        {
                            int intTemp = Encoding.UTF8.GetBytes(dtSource.Rows[i][j].ToString()).Length;
                            if (intTemp > arrColWidth[j])
                            {
                                arrColWidth[j] = intTemp;
                            }
                        }
                    }
                }

                #endregion 根据列中最长列的长度取得列宽
            }

            #endregion 设置内容单元格样式

            int rowIndex = 0;

            #region 表头及样式

            if (excelConfig.Title != null)
            {
                IRow headerRow = sheet.CreateRow(rowIndex);
                rowIndex++;
                if (excelConfig.TitleHeight != 0)
                {
                    headerRow.Height = (short)(excelConfig.TitleHeight * 20);
                }
                headerRow.HeightInPoints = 25;
                headerRow.CreateCell(0).SetCellValue(excelConfig.Title);
                headerRow.GetCell(0).CellStyle = headStyle;
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1)); // ------------------
            }

            #endregion 表头及样式

            #region 列头及样式

            {
                IRow headerRow = sheet.CreateRow(rowIndex);
                rowIndex++;

                #region 如果设置了列标题就按列标题定义列头，没定义直接按字段名输出

                foreach (DataColumn column in dtSource.Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(arrColName[column.Ordinal]);
                    headerRow.GetCell(column.Ordinal).CellStyle = cHeadStyle;
                    //设置列宽
                    sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                }

                #endregion 如果设置了列标题就按列标题定义列头，没定义直接按字段名输出
            }

            #endregion 列头及样式

            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 65535)
                {
                    sheet = workbook.CreateSheet();
                    rowIndex = 0;

                    #region 表头及样式

                    {
                        if (excelConfig.Title != null)
                        {
                            IRow headerRow = sheet.CreateRow(rowIndex);
                            rowIndex++;
                            if (excelConfig.TitleHeight != 0)
                            {
                                headerRow.Height = (short)(excelConfig.TitleHeight * 20);
                            }
                            headerRow.HeightInPoints = 25;
                            headerRow.CreateCell(0).SetCellValue(excelConfig.Title);
                            headerRow.GetCell(0).CellStyle = headStyle;
                            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1)); // ------------------
                        }
                    }

                    #endregion 表头及样式

                    #region 列头及样式

                    {
                        IRow headerRow = sheet.CreateRow(rowIndex);
                        rowIndex++;

                        #region 如果设置了列标题就按列标题定义列头，没定义直接按字段名输出

                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(arrColName[column.Ordinal]);
                            headerRow.GetCell(column.Ordinal).CellStyle = cHeadStyle;
                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }

                        #endregion 如果设置了列标题就按列标题定义列头，没定义直接按字段名输出
                    }

                    #endregion 列头及样式
                }

                #endregion 新建表，填充表头，填充列头，样式

                #region 填充内容

                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);
                    newCell.CellStyle = arryColumStyle[column.Ordinal];
                    string drValue = row[column].ToString();
                    SetCell(newCell, dateStyle, column.DataType, drValue);
                }

                #endregion 填充内容

                rowIndex++;
            }
            //using (MemoryStream ms = new MemoryStream())
            {
                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }

        #endregion DataTable导出到Excel的MemoryStream


        #region 设置表格内容

        private void SetCell(ICell newCell, ICellStyle dateStyle, Type dataType, string drValue)
        {
            switch (dataType.ToString())
            {
                case "System.String"://字符串类型
                    newCell.SetCellValue(drValue);
                    break;

                case "System.DateTime"://日期类型
                    System.DateTime dateV;
                    if (System.DateTime.TryParse(drValue, out dateV))
                    {
                        newCell.SetCellValue(dateV);
                    }
                    else
                    {
                        newCell.SetCellValue("");
                    }
                    newCell.CellStyle = dateStyle;//格式化显示
                    break;

                case "System.Boolean"://布尔型
                    bool boolV = false;
                    bool.TryParse(drValue, out boolV);
                    newCell.SetCellValue(boolV);
                    break;

                case "System.Int16"://整型
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(drValue, out intV);
                    newCell.SetCellValue(intV);
                    break;

                case "System.Decimal"://浮点型
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(drValue, out doubV);
                    newCell.SetCellValue(doubV);
                    break;

                case "System.DBNull"://空值处理
                    newCell.SetCellValue("");
                    break;

                default:
                    newCell.SetCellValue("");
                    break;
            }
        }

        #endregion 设置表格内容

        #region 从Excel导入
        /// <summary>
        /// 读取excel ,默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public DataTable ExcelImport(string strFileName)
        {
            return ExcelImport(strFileName, 0);
        }

        /// <summary>
        /// 读取excel ,默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <param name="headerRowNo">标题行号，以0开始</param>
        /// <returns></returns>
        public DataTable ExcelImport(string strFileName, int headerRowNo)
        {
            ISheet sheet;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (strFileName.IndexOf(".xlsx", StringComparison.Ordinal) == -1)//2003
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(0);
                }
                else//2007
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(0);
                }
            }

            return ReadSheetToDataTable(headerRowNo, sheet);
        }

        /// <summary>
        /// 读取excel ,默认第一行为标头
        /// </summary>
        /// <param name="fileStream">文件数据流</param>
        /// <returns></returns>
        public DataTable ExcelImport(Stream fileStream, string flieType)
        {
            return ExcelImport(fileStream, flieType, 0);
        }

        /// <summary>
        /// 读取excel ,默认第一行为标头
        /// </summary>
        /// <param name="fileStream">文件数据流</param>
        /// <param name="headerRowNo">标题行号从0开始</param>
        /// <returns></returns>
        public DataTable ExcelImport(Stream fileStream, string flieType, int headerRowNo)
        {
            DataTable dt = new DataTable();
            ISheet sheet = null;
            if (flieType == ".xls")
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(fileStream);
                sheet = hssfworkbook.GetSheetAt(0);
            }
            else
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fileStream);
                sheet = xssfworkbook.GetSheetAt(0);
            }
            return ReadSheetToDataTable(headerRowNo, sheet);
        }

        /// <summary>
        /// 从sheet中读取数据到DataTable
        /// </summary>
        /// <param name="headerRowNo">标题行号（数据行号=标题行号+1）</param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private DataTable ReadSheetToDataTable(int headerRowNo, ISheet sheet)
        {
            var dt = new DataTable();
            IRow headerRow = sheet.GetRow(headerRowNo);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (headerRowNo + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = 0; j < cellCount; j++)
                {
                    if (row.GetCell(j) == null)
                    {
                        continue;
                    }

                    ICell cell = row.GetCell(j);
                    if (cell.CellType == CellType.Error)
                    {
                        throw new Exception($"第{i + 1}行，列【{dt.Columns[j].ColumnName}】,单元格格式错误");
                    }
                    else if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                    {
                        dataRow[j] = cell.DateCellValue;
                    }
                    else if (cell.CellType == CellType.Numeric)
                    {
                        dataRow[j] = cell.NumericCellValue;
                    }
                    else if (cell.CellType == CellType.Blank)
                    {
                        dataRow[j] = "";
                    }
                    else
                    {
                        dataRow[j] = cell.StringCellValue;
                    }

                    //dataRow[j] = row.GetCell(j).ToString();
                }

                bool existsValue = false;
                foreach (DataColumn column in dt.Columns)
                {
                    if (dataRow[column.ColumnName] == null || string.IsNullOrEmpty(dataRow[column.ColumnName].ToString()))
                    {
                        continue;
                    }

                    existsValue = true;
                    break;
                }
                if (existsValue)
                {
                    dt.Rows.Add(dataRow);
                }
            }
            return dt;
        }

        #endregion 从Excel导入

        #region RGB颜色转NPOI颜色

        private short GetXLColour(HSSFWorkbook workbook, Color SystemColour)
        {
            short s = 0;
            HSSFPalette XlPalette = workbook.GetCustomPalette();
            NPOI.HSSF.Util.HSSFColor XlColour = XlPalette.FindColor(SystemColour.R, SystemColour.G, SystemColour.B);
            if (XlColour == null)
            {
                if (NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE < 255)
                {
                    XlColour = XlPalette.FindSimilarColor(SystemColour.R, SystemColour.G, SystemColour.B);
                    s = XlColour.Indexed;
                }
            }
            else
            {
                s = XlColour.Indexed;
            }

            return s;
        }

        #endregion RGB颜色转NPOI颜色

        #region 设置列的对齐方式

        /// <summary>
        /// 设置对齐方式
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private HorizontalAlignment getAlignment(string style)
        {
            switch (style)
            {
                case "center":
                    return HorizontalAlignment.Center;

                case "left":
                    return HorizontalAlignment.Left;

                case "right":
                    return HorizontalAlignment.Right;

                case "fill":
                    return HorizontalAlignment.Fill;

                case "justify":
                    return HorizontalAlignment.Justify;

                case "centerselection":
                    return HorizontalAlignment.CenterSelection;

                case "distributed":
                    return HorizontalAlignment.Distributed;
            }
            return NPOI.SS.UserModel.HorizontalAlignment.General;
        }

        #endregion 设置列的对齐方式


        #region 辅助方法
        /// <summary>
        /// 如果文件名中没有后缀名，增加文件后缀名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string JointXls(string fileName)
        {
            if (!fileName.EndsWith(".xls"))
            {
                fileName += ".xls";
            }

            return fileName;
        }

        private ExcelConfig ConvertExcelGridModelToConfig(IEnumerable<ExcelGridModel> columnList, string fileName)
        {
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = fileName;
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 15;
            excelconfig.IsAllSizeColumn = true;
            excelconfig.ColumnEntity = new List<ColumnModel>();
            foreach (ExcelGridModel columnModel in columnList)
            {
                excelconfig.ColumnEntity.Add(new ColumnModel()
                {
                    Column = columnModel.name,
                    ExcelColumn = columnModel.label,
                    Alignment = columnModel.align,
                });
            }

            return excelconfig;
        }

        /// <summary>
        /// MIME文件类型
        /// </summary>
        class MIMEType
        {
            public const string xls = "application/ms-excel";
        }
        #endregion
    }
}

