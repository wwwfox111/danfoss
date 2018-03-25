using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using System.ComponentModel;
using System.Collections;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Danfoss.Core.Utility
{
    /// <summary>
    /// Excel操作帮助类
    /// </summary>
    public class ExcelHelper
    {
        private string fileName = null; //文件名
        private IWorkbook workbook = null;
        private FileStream fs = null;
        private bool disposed;

        public ExcelHelper(string fileName)
        {
            this.fileName = fileName;
            disposed = false;
        }

        /// <summary>
        /// 读取Excel文件的内容，并转换为DataTable
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DataTable ReadAsDatatable(Stream s)
        {
            HSSFWorkbook wb = new HSSFWorkbook(s);
            return NPOIHelper.ConvertToDataTable(wb);
        }

        /// <summary>
        /// 导出到EXCEL
        /// </summary>
        /// <param name="dtSource"></param>
        /// <returns></returns>
        public static byte[] ExportToExcel(DataTable dtSource)
        {
            HSSFWorkbook workbook = NPOIHelper.InitializeBlankWorkbook("www.danfoss.com", "danfoss", "danfoss export file");
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

            //设置单元格显示格式
            HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            dateStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("m/d/yy");
            HSSFCellStyle intStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            intStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0");
            HSSFCellStyle doubleStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            doubleStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            HSSFCellStyle strStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            strStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
            HSSFCellStyle genStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            genStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("General");

            HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            HSSFFont font = (HSSFFont)workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            headStyle.SetFont(font);

            //设置列宽   同列标题宽度
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            //for (int i = 0; i < dtSource.Rows.Count; i++)
            //{
            //    for (int j = 0; j < dtSource.Columns.Count; j++)
            //    {
            //        int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
            //        if (intTemp > arrColWidth[j])
            //        {
            //            arrColWidth[j] = intTemp;
            //        }
            //    }
            //}

            //设置列格式
            HSSFCellStyle[] arrColStyle = new HSSFCellStyle[dtSource.Columns.Count];
            foreach (DataColumn column in dtSource.Columns)
            {
                switch (column.DataType.ToString())
                {
                    case "System.DateTime"://日期类型   
                        arrColStyle[column.Ordinal] = dateStyle;//日期格式化显示  m/d/yy 
                        break;
                    case "System.Int16"://整型
                    case "System.Int32":
                    case "System.Int64":
                        arrColStyle[column.Ordinal] = intStyle;
                        break;
                    case "System.Decimal"://浮点数
                    case "System.Double":
                        arrColStyle[column.Ordinal] = doubleStyle;
                        break;
                    case "System.String"://字符串类型   
                        arrColStyle[column.Ordinal] = strStyle;
                        break;
                    case "System.Byte":
                    case "System.Boolean"://布尔型
                    case "System.DBNull"://空值处理   
                    default:
                        arrColStyle[column.Ordinal] = genStyle;
                        break;
                }
            }


            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 超出行数则新建表，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = (HSSFSheet)workbook.CreateSheet();
                    }

                    #region 列头及样式
                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽，列格式 
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 4) * 256);
                            sheet.SetDefaultColumnStyle(column.Ordinal, arrColStyle[column.Ordinal]);
                        }
                    }
                    #endregion
                    rowIndex = 1;
                }
                #endregion

                #region 填充内容
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);
                    if (row[column].GetType() == typeof(DBNull))
                    {
                        continue;
                    }
                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型   
                            newCell.SetCellValue((string)row[column]);
                            //newCell.CellStyle = strStyle;
                            break;
                        case "System.DateTime"://日期类型   
                            newCell.SetCellValue((DateTime)row[column]);
                            //newCell.CellStyle = dateStyle;//日期格式化显示  m/d/yy 
                            break;
                        case "System.Boolean"://布尔型
                            newCell.SetCellValue((bool)row[column]);
                            break;
                        case "System.Int16"://整型
                            newCell.SetCellValue((Int16)row[column]);
                            //newCell.CellStyle = intStyle;
                            break;
                        case "System.Int32":
                            newCell.SetCellValue((Int32)row[column]);
                            //newCell.CellStyle = intStyle;
                            break;
                        case "System.Int64":
                            newCell.SetCellValue((Int64)row[column]);
                            //newCell.CellStyle = intStyle;
                            break;
                        case "System.Byte":
                            newCell.SetCellValue((Byte)row[column]);
                            //newCell.CellStyle = intStyle;
                            break;
                        case "System.Decimal"://浮点数
                                              //double doubV = 0;
                                              //double.TryParse(drValue, out doubV);
                            newCell.SetCellValue((double)(System.Decimal)row[column]);
                            //newCell.CellStyle = doubleStyle;
                            break;
                        case "System.Double":
                            newCell.SetCellValue((double)row[column]);
                            //newCell.CellStyle = doubleStyle;
                            break;
                        case "System.DBNull"://空值处理   
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }
                #endregion
                rowIndex++;
            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            return ms.GetBuffer();
            //return workbook.GetBytes();
        }

        #region 数据集互操作

        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list)
        {
            return ToDataTable<T>(list, null);
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="propertyName">需要返回的列的列名</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);

            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable,列名按照DisplayName属性
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTableWithDisplayName<T>(IList<T> list)
        {
            return ToDataTableWithDisplayName<T>(list, null);
        }
        /// <summary>
        /// 将泛型集合类转换成DataTable,列名按照DisplayName属性
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="propertyName">需要返回的列的列名</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTableWithDisplayName<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
                propertyNameList.AddRange(propertyName);

            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    DisplayNameAttribute dna = pi.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;
                    if (dna != null)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            result.Columns.Add(dna.DisplayName, pi.PropertyType);
                        }
                        else
                        {
                            if (propertyNameList.Contains(dna.DisplayName))
                                result.Columns.Add(dna.DisplayName, pi.PropertyType);
                        }
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }




        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        #endregion

    }

    public static class NPOIHelper
    {
        public static HSSFWorkbook InitializeWorkbook(string path)
        {
            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            //book1.xls is an Excel-2007-generated file, so some new unknown BIFF records are added. 
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                file.Close();
                return hssfworkbook;
            }
        }

        public static HSSFWorkbook InitializeBlankWorkbook(string company, string author, string title)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = company;
            hssfworkbook.DocumentSummaryInformation = dsi;

            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Author = author;
            si.Title = title;
            si.CreateDateTime = DateTime.Now;
            return hssfworkbook;
        }

        public static void WriteToFile(string strPath, HSSFWorkbook hssfworkbook)
        {
            using (FileStream fs = new FileStream(strPath, FileMode.Create, FileAccess.Write))
            {
                hssfworkbook.Write(fs);
                fs.Close();
            }
        }
        public static DataTable ConvertToDataTable(HSSFWorkbook hssfworkbook)
        {
            HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            //预设表头
            rows.MoveNext();
            HSSFRow header = (HSSFRow)rows.Current;
            System.Data.DataTable dt = new System.Data.DataTable();
            for (int j = 0; j < header.Cells.Count; j++)
            {
                dt.Columns.Add(header.Cells[j].ToString());
            }

            while (rows.MoveNext())
            {
                HSSFRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();

                for (int i = 0; i < row.LastCellNum; i++)
                {
                    HSSFCell cell = (HSSFCell)row.GetCell(i);
                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        if (cell.CellStyle.DataFormat == 0xe)//预设日期单元格处理
                        {
                            dr[i] = cell.DateCellValue.ToShortDateString();
                        }
                        else if (cell.CellStyle.DataFormat == 0x7)//预设金额处理
                        {
                            dr[i] = cell.NumericCellValue.ToString();
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
