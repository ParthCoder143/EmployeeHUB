using EmployeeDAA.Api.InfraStructure;
using EmployeeDAA.Api.Models;
using EmployeeDAA.Api.Models.Common;
using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain.Grid;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using OfficeOpenXml.Style;

using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ceTe.DynamicPDF.Conversion;
using EmployeeDAA.Data.Extensions;
using EmployeeDAA.Core.Infrastructure;
using EmployeeDAA.Core.Domain;

namespace EmployeeDAA.Api.Extensions
{
    public static class CommonExtensions
    {
        public static HttpContext HttpContextAccessor => new HttpContextAccessor().HttpContext;
        private static readonly DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static ApiResponse ToSingleResponse<T>(this T obj, string msg)
        {
            try
            {
                return ApiResponseHelper.GenerateResponse(obj is null ? ApiStatusCode.Status404NotFound : ApiStatusCode.Status200OK, obj is null ? Message.NoDataFound : string.Format(Message.ListLoadSuccessMessageTemplate, msg), obj);
            }
            catch (Exception)
            {
                return new ApiResponse();
            }
        }

        public static IList<SelectListItem> ToDropDown1<T>(this IList<T> drpList, string dropValColName = "Id", string dropDisColName = "CategoryName", string drpGrpColName = "")
        {
            if (dropValColName.Split('|').Length == 1)
            {
                return drpList.Select(x => new SelectListItem() { Value = (typeof(T).GetProperty(dropValColName).GetValue(x)).ToString(), Text = typeof(T).GetProperty(dropDisColName).GetValue(x).ToString(), Group = new SelectListGroup() { Name = !string.IsNullOrEmpty(drpGrpColName) ? typeof(T).GetProperty(drpGrpColName).GetValue(x).ToString() : "" } }).ToList();
            }
            else if (dropValColName.Split('|').Length == 2)
            {
                return drpList.Select(x => new SelectListItem() { Value = (typeof(T).GetProperty(dropValColName.Split('|')[0]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[1]).GetValue(x)).ToString(), Text = typeof(T).GetProperty(dropDisColName).GetValue(x).ToString(), Group = new SelectListGroup() { Name = !string.IsNullOrEmpty(drpGrpColName) ? typeof(T).GetProperty(drpGrpColName).GetValue(x).ToString() : "" } }).ToList();
            }
            else if (dropValColName.Split('|').Length == 4)
            {
                return drpList.Select(x => new SelectListItem() { Value = (typeof(T).GetProperty(dropValColName.Split('|')[0]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[1]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[2]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[3]).GetValue(x)).ToString(), Text = typeof(T).GetProperty(dropDisColName).GetValue(x).ToString(), Group = new SelectListGroup() { Name = !string.IsNullOrEmpty(drpGrpColName) ? typeof(T).GetProperty(drpGrpColName).GetValue(x).ToString() : "" } }).ToList();
            }
            else
            {
                return drpList.Select(x => new SelectListItem() { Value = (typeof(T).GetProperty(dropValColName.Split('|')[0]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[1]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[2]).GetValue(x)).ToString(), Text = typeof(T).GetProperty(dropDisColName).GetValue(x).ToString(), Group = new SelectListGroup() { Name = !string.IsNullOrEmpty(drpGrpColName) ? typeof(T).GetProperty(drpGrpColName).GetValue(x).ToString() : "" } }).ToList();
            }
        }
        public static IList<SelectListItem> ToDropDown2<T>(this IList<T> drpList, string dropValColName = "Id", string dropDisColName = "ProductName", string drpGrpColName = "")
        {
            if (dropValColName.Split('|').Length == 1)
            {
                return drpList.Select(x => new SelectListItem() { Value = (typeof(T).GetProperty(dropValColName).GetValue(x)).ToString(), Text = typeof(T).GetProperty(dropDisColName).GetValue(x).ToString(), Group = new SelectListGroup() { Name = !string.IsNullOrEmpty(drpGrpColName) ? typeof(T).GetProperty(drpGrpColName).GetValue(x).ToString() : "" } }).ToList();
            }
            else if (dropValColName.Split('|').Length == 2)
            {
                return drpList.Select(x => new SelectListItem() { Value = (typeof(T).GetProperty(dropValColName.Split('|')[0]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[1]).GetValue(x)).ToString(), Text = typeof(T).GetProperty(dropDisColName).GetValue(x).ToString(), Group = new SelectListGroup() { Name = !string.IsNullOrEmpty(drpGrpColName) ? typeof(T).GetProperty(drpGrpColName).GetValue(x).ToString() : "" } }).ToList();
            }
            else if (dropValColName.Split('|').Length == 4)
            {
                return drpList.Select(x => new SelectListItem() { Value = (typeof(T).GetProperty(dropValColName.Split('|')[0]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[1]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[2]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[3]).GetValue(x)).ToString(), Text = typeof(T).GetProperty(dropDisColName).GetValue(x).ToString(), Group = new SelectListGroup() { Name = !string.IsNullOrEmpty(drpGrpColName) ? typeof(T).GetProperty(drpGrpColName).GetValue(x).ToString() : "" } }).ToList();
            }
            else
            {
                return drpList.Select(x => new SelectListItem() { Value = (typeof(T).GetProperty(dropValColName.Split('|')[0]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[1]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[2]).GetValue(x)).ToString(), Text = typeof(T).GetProperty(dropDisColName).GetValue(x).ToString(), Group = new SelectListGroup() { Name = !string.IsNullOrEmpty(drpGrpColName) ? typeof(T).GetProperty(drpGrpColName).GetValue(x).ToString() : "" } }).ToList();
            }
        }

        public static IList<SelectListItem> ToDropDown<T>(this IList<T> drpList, string dropValColName = "Id", string dropDisColName = "Name", string drpGrpColName = "")
        {
            if (dropValColName.Split('|').Length == 1)
            {
                return drpList.Select(x => new SelectListItem() { Value = (typeof(T).GetProperty(dropValColName).GetValue(x)).ToString(), Text = typeof(T).GetProperty(dropDisColName).GetValue(x).ToString(), Group = new SelectListGroup() { Name = !string.IsNullOrEmpty(drpGrpColName) ? typeof(T).GetProperty(drpGrpColName).GetValue(x).ToString() : "" } }).ToList();
            }
            else if (dropValColName.Split('|').Length == 2)
            {
                return drpList.Select(x => new SelectListItem() { Value = (typeof(T).GetProperty(dropValColName.Split('|')[0]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[1]).GetValue(x)).ToString(), Text = typeof(T).GetProperty(dropDisColName).GetValue(x).ToString(), Group = new SelectListGroup() { Name = !string.IsNullOrEmpty(drpGrpColName) ? typeof(T).GetProperty(drpGrpColName).GetValue(x).ToString() : "" } }).ToList();
            }
            else if (dropValColName.Split('|').Length == 4)
            {
                return drpList.Select(x => new SelectListItem() { Value = (typeof(T).GetProperty(dropValColName.Split('|')[0]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[1]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[2]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[3]).GetValue(x)).ToString(), Text = typeof(T).GetProperty(dropDisColName).GetValue(x).ToString(), Group = new SelectListGroup() { Name = !string.IsNullOrEmpty(drpGrpColName) ? typeof(T).GetProperty(drpGrpColName).GetValue(x).ToString() : "" } }).ToList();
            }
            else
            {
                return drpList.Select(x => new SelectListItem() { Value = (typeof(T).GetProperty(dropValColName.Split('|')[0]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[1]).GetValue(x)).ToString() + "|" + (typeof(T).GetProperty(dropValColName.Split('|')[2]).GetValue(x)).ToString(), Text = typeof(T).GetProperty(dropDisColName).GetValue(x).ToString(), Group = new SelectListGroup() { Name = !string.IsNullOrEmpty(drpGrpColName) ? typeof(T).GetProperty(drpGrpColName).GetValue(x).ToString() : "" } }).ToList();
            }
        }
        private static object GetNewDynamicClass(object obj)
        {
            IDictionary<string, object> propertyValues = obj as System.Dynamic.ExpandoObject;

            List<DynamicClassField> Fields = new();
            foreach (var property in propertyValues)
            {
                if (property.Key.ToUpper() != "ID")
                    Fields.Add(new DynamicClassField { FieldName = property.Key, FieldType = typeof(string) });
            }
            DynamicClassCreator o = new();
            return o.CreateNewObject(Fields);
        }
        public static ApiResponse ToSingleResponse<T, TModel>(this T obj, string msg)
        where T : BaseEntity
        where TModel : Models.Common.BaseModel
        {
            try
            {
                return ApiResponseHelper.GenerateResponse(obj is null ? ApiStatusCode.Status404NotFound : ApiStatusCode.Status200OK, obj is null ? Message.NoDataFound : string.Format(Message.ListLoadSuccessMessageTemplate, msg), obj.MapTo<TModel>());
            }
            catch (Exception)
            {
                return new ApiResponse();
            }
        }
        public static object ToGridResponse<T>(this IPagedList<T> GridData, GridRequestModel objGrid, string FileName, string Message = "", object ExtraData = null)
        {
            try
            {
                return objGrid.ExportType == 0 ? new ApiResponse { StatusCode = 200, Message = Message, Data = GridData.ToGrid(), ExtraData = ExtraData } : GridData.ExportToGrid(objGrid, FileName);
            }
            catch (Exception ex)
            {
                return new object();
            }
        }
        public static GridResponseModel ToGrid<T>(this IPagedList<T> GridData)
        {
            try
            {
                return new GridResponseModel() { Data = GridData, RecordsFiltered = GridData.TotalCount, RecordsTotal = GridData.TotalCount, PageIndex = GridData.PageIndex };
            }
            catch (Exception)
            {
                return new GridResponseModel();
            }
        }
        public static System.IO.Stream ExportToGrid<T>(this IPagedList<T> GridData, GridRequestModel GridParam, string FileName)
        {
            return GridParam.ExportType == ExportType.Excel ?
            ("SLA Report" == FileName ? ExportToExcelEpPlus(GridData, GridParam, FileName) :


            ExportToExcel(GridData, GridParam, FileName)) : ExportToPdf(GridData, GridParam);
        }
        private static System.IO.MemoryStream ExportToPdf<T>(IPagedList<T> GridData, GridRequestModel GridParam)
        {
            StringBuilder html = new();
            html.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
               "<head><meta http-equiv='Content-Type' content='text/html; charset=UTF-8'><meta name='viewport' content='width=device-width'><meta http-equiv='X-UA-Compatible' content='IE=edge'>" +
               "<title>SamplePdf</title></head><body><table cellpadding='5' border='1' style='border-spacing:0px;'>");

            bool IsDynamic = false;
            if (GridData[0].GetType() == typeof(System.Dynamic.ExpandoObject))
                IsDynamic = true;
            System.Reflection.PropertyInfo[] PropList = IsDynamic ? GetNewDynamicClass(GridData[0]).GetType().GetProperties() : GridData[0].GetType().GetProperties();
            #region :: Get Custom Attribute ::
            List<string> _dict = new();
            foreach (GridColumn prop in GridParam.Columns)
            {
                if (!string.IsNullOrEmpty(prop.Data) && !string.IsNullOrEmpty(prop.Name))
                {
                    _dict.Add(prop.Data);
                }
            }
            #endregion
            //add header row
            html.Append("<tr>");
            foreach (string propInfo in _dict)
            {
                html.Append("<th>" + propInfo + "</th>");
            }
            html.Append("</tr>");
            //add rows
            int j = 2; //from 2 bcs row one is header            
            foreach (T item in GridData)
            {
                int i = 0;
                html.Append("<tr>");
                System.Reflection.PropertyInfo[] propInfoList = item.GetType().GetProperties();
                foreach (string prop in _dict)
                {
                    if (IsDynamic)
                    {
                        html.Append("<td>").Append((item as System.Dynamic.ExpandoObject).FirstOrDefault(x => x.Key == prop).Value).Append("</td>");
                        i++;
                    }
                    else
                    {
                        if (propInfoList.Any(x => x.Name == prop))
                        {
                            System.Reflection.PropertyInfo propInfo = propInfoList.FirstOrDefault(x => x.Name == prop);
                            if (propInfo.GetValue(item, null) != null && propInfo.PropertyType == typeof(System.DateTime))
                            {
                                DateTime Coldatetime = Convert.ToDateTime(propInfo.GetValue(item)).ToLocalDateTime(GridParam.TimeZone);
                                if (propInfo.PropertyType.Name == "DateTime")
                                {
                                    html.Append("<td>" + Convert.ToString(Coldatetime) + "</td>");
                                }
                            }
                            else
                                html.Append("<td>" + propInfo.GetValue(item) + "</td>");
                            i++;
                        }
                    }

                }
                html.Append("</tr>");
                j++;
            }
            html.Append("</table></body></html>");
            HtmlConverter htmlConverter = new(html.ToString());
            byte[] outputByte = htmlConverter.Convert();
            MemoryStream ms = new(outputByte);
            ms.Position = 0;
            return ms;
        }
        private static System.IO.Stream ExportToExcel<T>(IPagedList<T> GridData, GridRequestModel GridParam, string FileName)
        {

            bool isSLAReport = "SLA Report" == FileName;
            System.Reflection.PropertyInfo[] PropList = GridData[0].GetType().GetProperties();
            #region :: Get Custom Attribute ::
            string[] stringArray = { "Id" };
            List<string> _dict = new();
            foreach (GridColumn prop in GridParam.Columns)
            {
                if (!stringArray.Contains(prop.Data))
                {
                    if (!string.IsNullOrEmpty(prop.Data) && !string.IsNullOrEmpty(prop.Name))
                    {
                        _dict.Add(prop.Data);
                    }
                }
            }
            #endregion
            #region worksheet
            using XLWorkbook excel = new();
            IXLWorksheet workSheet = excel.Worksheets.Add(FileName?.Length == 0 ? "Sheet1" : FileName);// set sheet name
            int col = 1, ColJ = 1;
            int rn = 0;
            foreach (GridColumn propInfo in GridParam.Columns)
            {
                if (!stringArray.Contains(propInfo.Data))
                {
                    if (!string.IsNullOrEmpty(propInfo.Data) && !string.IsNullOrEmpty(propInfo.Name))
                    {
                        rn++;
                        workSheet.Cell(1, col).Value = propInfo.Name;
                        workSheet.Cell(1, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        col++;
                    }
                }
            }

            IXLRow headerRwo = workSheet.Row(1);
            headerRwo.Style.Font.Bold = true;

            workSheet.Row(1).Height = 30;
            int j = 2; //from 2 bcs row one is header
            if (isSLAReport)
            {
                j = j + 1;
                foreach (T item in GridData)
                {
                    int i = 0;
                    foreach (var propInfo in item.GetType().GetProperties())
                    {
                        workSheet.Cell(j, i + 1).Value = propInfo.GetValue(item);
                        workSheet.Cell(j, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        i++;
                    }
                    workSheet.Row(j).Height = 30;
                    j++;
                }
            }
            else
            {
                foreach (T item in GridData)
                {
                    int i = 0;
                    System.Reflection.PropertyInfo[] propInfoList = item.GetType().GetProperties();
                    foreach (string prop in _dict)
                    {
                        if (propInfoList.Any(x => x.Name == prop))
                        {
                            System.Reflection.PropertyInfo propInfo = propInfoList.FirstOrDefault(x => x.Name == prop);
                            if (propInfo.GetValue(item, null) != null && propInfo.PropertyType == typeof(System.DateTime))
                            {
                                DateTime Coldatetime = Convert.ToDateTime(propInfo.GetValue(item)).ToLocalDateTime(GridParam.TimeZone);
                                if (propInfo.PropertyType.Name == "DateTime")
                                {
                                    propInfo.SetValue(item, Coldatetime, default);
                                }
                                else
                                {
                                    propInfo.SetValue(item, Coldatetime.ToString(), null);
                                }
                            }
                            workSheet.Cell(j, i + 1).Value = propInfo.GetValue(item);
                            workSheet.Cell(j, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            i++;
                        }
                    }
                    workSheet.Row(j).Height = 30;
                    j++;
                }
            }
            System.IO.Stream spreadsheetStream = new System.IO.MemoryStream();
            excel.SaveAs(spreadsheetStream);
            spreadsheetStream.Position = 0;
            return spreadsheetStream;
            #endregion
        }
        private static System.IO.Stream ExportToExcelEpPlus<T>(IPagedList<T> GridData, GridRequestModel GridParam, string FileName)
        {

            bool isSLAReport = "SLA Report" == FileName;
            #region :: Get Custom Attribute ::
            string[] stringArray = { "Id" };
            List<string> _dict = new();
            foreach (GridColumn prop in GridParam.Columns)
            {
                if (!stringArray.Contains(prop.Data))
                {
                    if (!string.IsNullOrEmpty(prop.Data) && !string.IsNullOrEmpty(prop.Name))
                    {
                        _dict.Add(prop.Data);
                    }
                }
            }
            #endregion
            #region worksheet
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            OfficeOpenXml.ExcelPackage excel = new();
            var workSheet = excel.Workbook.Worksheets.Add(FileName?.Length == 0 ? "Sheet1" : FileName);// set sheet name            
            int col = 1, ColJ = 1;
            if (isSLAReport)
            {
                ColJ++;
                using (ExcelRange Rng = workSheet.Cells[col, 1, col, 16])
                {
                    Rng.Value = "";
                    Rng.Merge = true;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Color.SetColor(Color.Gray);
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                };

                using (ExcelRange Rng = workSheet.Cells[col, 17, col, 34])
                {
                    Rng.Value = "Order Verification";
                    Rng.Merge = true;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Color.SetColor(Color.Black);
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Rng.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                };

                using (ExcelRange Rng = workSheet.Cells[col, 35, col, 56])
                {
                    Rng.Value = "Regn Verification";
                    Rng.Merge = true;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Color.SetColor(Color.White);
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Rng.Style.Fill.BackgroundColor.SetColor(Color.Navy);
                };

                using (ExcelRange Rng = workSheet.Cells[col, 57, col, 59])
                {
                    Rng.Value = "Delivered";
                    Rng.Merge = true;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Color.SetColor(Color.White);
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Rng.Style.Fill.BackgroundColor.SetColor(Color.Green);
                };
                ColJ++;
            }
            int rn = 0;
            foreach (GridColumn propInfo in GridParam.Columns)
            {
                if (!stringArray.Contains(propInfo.Data))
                {
                    if (!string.IsNullOrEmpty(propInfo.Data) && !string.IsNullOrEmpty(propInfo.Name))
                    {
                        rn++;
                        workSheet.Cells[2, col].Value = propInfo.Name;
                        workSheet.Cells[2, col].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[2, col].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[2, col].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[2, col].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        if (rn <= 13 && rn > 0)
                        {
                            workSheet.Cells[2, col].Style.Font.Size = 12;
                            using ExcelRange Rng = workSheet.Cells[2, col];
                            Rng.Style.Font.Color.SetColor(Color.Green);
                        }
                        if (rn <= 16 && rn > 13)
                        {
                            workSheet.Cells[2, col].Style.Font.Size = 12;
                            using ExcelRange Rng = workSheet.Cells[2, col];
                            Rng.Style.Font.Color.SetColor(Color.Yellow);
                        }
                        if (rn > 16)
                        {
                            workSheet.Cells[2, col].Style.Font.Size = 12;
                            using ExcelRange Rng = workSheet.Cells[2, col];
                            Rng.Style.Font.Color.SetColor(Color.Gray);
                        }
                        col++;
                    }
                }
            }
            using (ExcelRange Rng = workSheet.Cells[2, 1, 2, 13])
            {
                Rng.Style.Font.Color.SetColor(Color.White);
                Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                Rng.Style.Fill.BackgroundColor.SetColor(Color.Green);
            };
            using (ExcelRange Rng = workSheet.Cells[2, 14, 2, 16])
            {
                Rng.Style.Font.Color.SetColor(Color.Black);
                Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                Rng.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            };
            using (ExcelRange Rng = workSheet.Cells[2, 17, 2, 63])
            {
                Rng.Style.Font.Color.SetColor(Color.White);
                Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                Rng.Style.Fill.BackgroundColor.SetColor(Color.Gray);
            };
            workSheet.Row(1).Height = 30;
            int j = 2; //from 2 bcs row one is header
            j++;
            foreach (T item in GridData)
            {
                int i = 0;
                System.Reflection.PropertyInfo[] propInfoList = item.GetType().GetProperties();
                foreach (string prop in _dict)
                {
                    if (propInfoList.Any(x => x.Name == prop))
                    {
                        System.Reflection.PropertyInfo propInfo = propInfoList.FirstOrDefault(x => x.Name == prop);
                        if (propInfo.GetValue(item, null) != null && propInfo.PropertyType == typeof(System.DateTime))
                        {
                            DateTime Coldatetime = Convert.ToDateTime(propInfo.GetValue(item)).ToLocalDateTime(GridParam.TimeZone);
                            if (propInfo.PropertyType.Name == "DateTime")
                            {
                                propInfo.SetValue(item, Coldatetime, default);
                            }
                            else
                            {
                                propInfo.SetValue(item, Coldatetime.ToString(), null);
                            }
                        }
                        workSheet.Cells[j, i + 1].Value = propInfo.GetValue(item);
                        workSheet.Cells[j, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[j, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[j, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[j, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        i++;
                    }
                }
                workSheet.Row(j).Height = 30;
                j++;
            }
            // foreach (T item in GridData)
            // {
            //     int i = 0;
            //     foreach (var propInfo in item.GetType().GetProperties())
            //     {
            //         workSheet.Cells[j, i + 1].Value = propInfo.GetValue(item);
            //         workSheet.Cells[j, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //         workSheet.Cells[j, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            //         workSheet.Cells[j, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //         workSheet.Cells[j, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //         i++;
            //     }
            //     workSheet.Row(j).Height = 30;
            //     j++;
            // }
            System.IO.Stream spreadsheetStream = new System.IO.MemoryStream();
            excel.SaveAs(spreadsheetStream);
            spreadsheetStream.Position = 0;
            return spreadsheetStream;
            #endregion
        }
        public static DateTime ToLocalDateTime(this DateTime OnDate, string TimeZone)
        {
            TimeSpan ts = string.IsNullOrEmpty(TimeZone) ? new TimeSpan() : TimeSpan.Parse(TimeZone.Replace("+", ""));
            return OnDate.Add(ts);
        }
        public static List<int> ToIntList(this object a, string separator)
        {
            try
            {
                return string.IsNullOrWhiteSpace(Convert.ToString(a)) ? new List<int>() : a.ToString().Split(separator.ToCharArray()).Select(int.Parse).ToList();
            }
            catch (Exception)
            {
                return new List<int>();
            }
        }
        #region  :: Permission for Page ::
        #endregion

        //public static async Task<string> GenerateToken(User user, string Secret, int Minutes, string permissions)
        //{
        //    return await Task.Run(() =>
        //    {
        //        #region :: Generate Token ::
        //        JwtSecurityTokenHandler tokenHandler = new();
        //        byte[] key = Encoding.ASCII.GetBytes(Secret);
        //        SecurityTokenDescriptor tokenDescriptor = new()
        //        {
        //            Subject = new ClaimsIdentity(new Claim[]
        //            {
        //                       new Claim("Id",EncryptionUtility.EncryptText(user.Id.ToString(),SecurityHelper.EnDeKey)),
        //                       new Claim("Permissions",EncryptionUtility.EncryptText(permissions,SecurityHelper.EnDeKey)),
        //                       new Claim("UserName",EncryptionUtility.EncryptText(user.UserName,SecurityHelper.EnDeKey)),
        //                       new Claim("UserToken",EncryptionUtility.EncryptText(user.UserToken,SecurityHelper.EnDeKey)),
        //                       new Claim("RoleId", EncryptionUtility.EncryptText(user.RoleId.ToString(),SecurityHelper.EnDeKey)),
        //                       new Claim("RoleType", EncryptionUtility.EncryptText(((int)user.RoleType).ToString(),SecurityHelper.EnDeKey)),
        //                       new Claim("RoleName", EncryptionUtility.EncryptText(user.Role,SecurityHelper.EnDeKey)),
        //                       new Claim("FullName", EncryptionUtility.EncryptText(user.FirstName + (user.LastName != null ? " " + user.LastName : ""),SecurityHelper.EnDeKey))

        //            }),
        //            Expires = DateTime.UtcNow.AddMinutes(Minutes),
        //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //        };
        //        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        //        #endregion
        //    });
        //}
        //#endregion

        #region :: Check Permission ::
        public static bool CheckPermission(PageName pageCode, PagePermission permit)
        {
            ClaimsPrincipal currentUser = HttpContextAccessor.User;
            if (currentUser.HasClaim(c => c.Type == "RoleId"))
            {
                if (EncryptionUtility.DecryptText(currentUser.Claims.FirstOrDefault(c => c.Type == "RoleType").Value, SecurityHelper.EnDeKey) == "-1")
                {
                    return true;
                }
                else
                {
                    if (currentUser.HasClaim(c => c.Type == "Permissions"))
                    {
                        string permissions = EncryptionUtility.DecryptText(currentUser.Claims.FirstOrDefault(c => c.Type == "Permissions").Value, SecurityHelper.EnDeKey);
                        string[] AllPermissions = permissions.Split(',');
                        foreach (string permission in AllPermissions)
                        {
                            if (permission.StartsWith(pageCode.ToDescription() + "|"))
                            {
                                string[] perms = permission.Split('|');
                                if (permit == PagePermission.Add)
                                {
                                    return perms[1] == "1";
                                }
                                else if (permit == PagePermission.Edit)
                                {
                                    return perms[2] == "1";
                                }
                                else if (permit == PagePermission.Delete)
                                {
                                    return perms[3] == "1";
                                }
                                else if (permit == PagePermission.View)
                                {
                                    return perms[4] == "1";
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        #endregion
        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }
        public static void ReplaceTemplateStringByToken(ref string EmailTemplateWithToken, string ReplaceTokenKey, string ReplaceTokenValue)
        {
            EmailTemplateWithToken = EmailTemplateWithToken.Replace("{{" + ReplaceTokenKey + "}}", string.IsNullOrWhiteSpace(ReplaceTokenValue) ? "" : ReplaceTokenValue);
        }
        public static void WriteToFile(string Message)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
                if (!File.Exists(filepath))
                {
                    // Create a file to write to.   
                    using StreamWriter sw = File.CreateText(filepath);
                    sw.WriteLine(Message);
                }
                else
                {
                    using StreamWriter sw = File.AppendText(filepath);
                    sw.WriteLine(Message);
                }
            }
            catch (Exception)
            {
            }
        }
        public static async Task<string> GenerateToken(User user, string Secret, int Minutes, string permissions)
        {
            return await Task.Run(() =>
            {
                #region :: Generate Token ::
                JwtSecurityTokenHandler tokenHandler = new();
                byte[] key = Encoding.ASCII.GetBytes(Secret);
                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                               new Claim("Id",EncryptionUtility.EncryptText(user.Id.ToString(),SecurityHelper.EnDeKey)),
                               new Claim("Permissions",EncryptionUtility.EncryptText(permissions,SecurityHelper.EnDeKey)),
                               new Claim("UserName",EncryptionUtility.EncryptText(user.UserName,SecurityHelper.EnDeKey)),
                               new Claim("UserToken",EncryptionUtility.EncryptText(user.UserToken,SecurityHelper.EnDeKey)),
                               new Claim("RoleId", EncryptionUtility.EncryptText(user.RoleId.ToString(),SecurityHelper.EnDeKey)),
                               new Claim("RoleType", EncryptionUtility.EncryptText(((int)user.RoleType).ToString(),SecurityHelper.EnDeKey)),
                               new Claim("RoleName", EncryptionUtility.EncryptText(user.Role,SecurityHelper.EnDeKey)),
                               new Claim("FullName", EncryptionUtility.EncryptText(user.FirstName + (user.LastName != null ? " " + user.LastName : ""),SecurityHelper.EnDeKey))

                    }),
                    Expires = DateTime.UtcNow.AddMinutes(Minutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
                #endregion
            });
        }


    }
}
