﻿using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Zhuang.NPOI.Excel
{
    public static class WorkbookExtensions
    {
        public static void Download4Web(this IWorkbook workbook, string fileName)
        {
            string contentType = string.Empty;

            if (workbook.GetType() == typeof(HSSFWorkbook))
            {
                //Office2003
                contentType = "application/vnd.ms-excel";
            }
            else if ((workbook.GetType() == typeof(XSSFWorkbook)))
            {
                //Office2007
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }

            var request = HttpContext.Current.Request;
            if (request.UserAgent.ToLower().IndexOf("msie") > -1)
            {
                fileName = System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
            }
            else if (request.UserAgent.ToLower().IndexOf("firefox") > -1)
            {
                fileName = "\"" + fileName + "\"";
            }

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = contentType;
            response.Charset = "uft-8";
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}",fileName));

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.WriteTo(response.OutputStream);
            }
            
            response.Flush();
            response.End();

        }

        public static void SaveAs(this IWorkbook workbook, string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }
    }
}
