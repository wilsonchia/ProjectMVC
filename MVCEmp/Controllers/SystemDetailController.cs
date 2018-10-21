using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCEmp.Models;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;


namespace MVCEmp.Controllers
{
    public class SystemDetailController : Controller
    {
        SystemDetailModels sdModels = new SystemDetailModels();
        App_Start.ClassDataBase dbClass = new App_Start.ClassDataBase();

        // GET: /SystemDetail/
        public ActionResult Index(SystemDetailModels viewModel)
        {
            GetBindData("", "1", viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, SystemDetailModels viewModel)
        {
            string funSearchValue = form["searchValue"].ToString();
            string funPageIndex = form["hidePageIndex"].ToString();
            GetBindData(funSearchValue, funPageIndex, viewModel);
            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public RedirectResult Create(FormCollection form)
        {
            List<string> listPostData = new List<string>();
            listPostData.Add(form["textSystemData"].ToString());
            listPostData.Add(form["textSystemValue"].ToString());
            listPostData.Add(form["textSystemTitle"].ToString());
            listPostData.Add(form["textSystemNotation"].ToString());
            listPostData.Add(form["textSystemRemark"].ToString());
            listPostData.Add(form["hideSystemStatus"].ToString());
            ViewBag.valShowDetail = sdModels.DataCreate(listPostData);
            return Redirect("~/SystemDetail/Index");
        }

        public ActionResult Update(string cl, string vl)
        {
            ViewBag.valSystemClass = cl.ToString();
            ViewBag.valSystemValue = vl.ToString();
            List<listSystemDetail> detailListSystem = sdModels.detailListSystemDetail(cl, vl);
            ViewBag.valSystemTitle = detailListSystem[0].lSystemTitle.ToString();
            ViewBag.valSystemNotation = detailListSystem[0].lSystemNotation.ToString();
            ViewBag.valSystemRemark = detailListSystem[0].lSystemRemark.ToString();
            ViewBag.valSystemStatus = detailListSystem[0].lSystemStatus.ToString();
            return View();
        }

        [HttpPost]
        public RedirectResult Update(FormCollection form)
        {
            List<string> listPostData = new List<string>();
            listPostData.Add(form["hideSystemClass"].ToString());
            listPostData.Add(form["hideSystemValue"].ToString());
            listPostData.Add(form["textSystemTitle"].ToString());
            listPostData.Add(form["textSystemNotation"].ToString());
            listPostData.Add(form["textSystemRemark"].ToString());
            listPostData.Add(form["hideSystemStatus"].ToString());
            string fReturnValue = sdModels.DataUpdate(listPostData);
            return Redirect("~/SystemDetail/Index");
        }

        public RedirectResult Delete(string vClass, string vValue)
        {
            string valClass, valValue, returnExecuteValue;
            valClass = Request["cl"].ToString();
            valValue = Request["vl"].ToString();
            returnExecuteValue = sdModels.DataDelete(valClass, valValue);
            return Redirect("~/SystemDetail/Index");
        }

        [HttpPost]
        public void UpLoad(HttpPostedFileBase fileUpload)
        {
            string showDetailData = ""; DataTable dtSystemDetail = new DataTable();
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                string FilePath = System.Web.HttpContext.Current.Server.MapPath("~/excel");
                string FileName = fileUpload.FileName;
                fileUpload.SaveAs(string.Format(@"{0}\{1}", FilePath, FileName));
                dynamic hssfweb; DataTable tempDataTable = new DataTable(); tempDataTable.Clear();
                using (MemoryStream ms = new MemoryStream(dbClass.getFileToRead(string.Format(@"{0}\{1}", FilePath, FileName))))
                {
                    hssfweb = WorkbookFactory.Create(ms);
                }
                ISheet sheet = (ISheet)hssfweb.GetSheetAt(0);
                if (sheet.LastRowNum > 0)
                {
                    IRow rowT = (IRow)sheet.GetRow(0);
                    for (int i = 0; i < 6; i++)
                    {
                        DataColumn col = new DataColumn(rowT.GetCell(i).ToString(), typeof(string));
                        tempDataTable.Columns.Add(col);
                    }
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow rowD = (IRow)sheet.GetRow(i);
                        if (rowD != null)
                        {
                            DataRow tempRow = tempDataTable.NewRow();
                            for (int a = 0; a < 6; a++)
                            {
                                tempRow[a] = (rowD.GetCell(a).ToString() != null) ? rowD.GetCell(a).ToString() : "";
                            }
                        }
                    }
                    List<listSystemDetail> SystemDetailList = sdModels.reListSystemDetail();
                    string valSystemClass, valSystemValue, valSystemTitle, valSystemNotation, valSystemRemark, valSystemStatus;
                    if (tempDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow drtemp in tempDataTable.Rows)
                        {
                            List<string> DataList = new List<string>();
                            DataList.Add(drtemp["SystemClass"].ToString());
                            DataList.Add(drtemp["SystemValue"].ToString());
                            DataList.Add(drtemp["SystemTitle"].ToString());
                            DataList.Add(drtemp["SystemNotation"].ToString());
                            DataList.Add(drtemp["SystemRemark"].ToString());
                            DataList.Add(drtemp["SystemStatus"].ToString());
                            valSystemClass = drtemp["SystemClass"].ToString();
                            valSystemValue = drtemp["SystemValue"].ToString();
                            if (SystemDetailList.Where(x => x.lSystemClass == valSystemClass && x.lSystemValue == valSystemValue).Count() > 0)
                            {
                                sdModels.DataUpdate(DataList);
                            }
                            else
                            {
                                sdModels.DataUpdate(DataList);
                            }
                        }
                    }
                    List<listSystemDetail> SysDetailList = new List<listSystemDetail>();
                    if (SysDetailList.Count > 0)
                    {
                        foreach (listSystemDetail item in SysDetailList)
                        {
                            showDetailData += string.Format(@"{0}~{1}~{2}~{3}~{4}~{5},br />"
                                , item.lSystemClass, item.lSystemValue, item.lSystemTitle, item.lSystemNotation, item.lSystemRemark, item.lSystemStatus);
                        }
                    }
                }
            }
            Response.Write(showDetailData);
        }

        [HttpPost]
        public void OutExcelData()
        {
            string ExportFileName = "SystemData.xlsx"; string ExportFileTitle = "Data";
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            XSSFWorkbook NpoiWB = new XSSFWorkbook();
            XSSFCellStyle xCellStyle = (XSSFCellStyle)NpoiWB.CreateCellStyle();
            XSSFDataFormat NpoiFormat = (XSSFDataFormat)NpoiWB.CreateDataFormat();
            xCellStyle.SetDataFormat(NpoiFormat.GetFormat("[DbNum2][$-804]0"));
            XSSFCellStyle cellStyleFontColor = (XSSFCellStyle)NpoiWB.CreateCellStyle();
            XSSFFont font1 = (XSSFFont)NpoiWB.CreateFont(); font1.Color = (short)10; font1.IsBold = true;
            cellStyleFontColor.SetFont(font1);
            ISheet xSheet = NpoiWB.CreateSheet(ExportFileTitle);
            List<string> listColumn = new List<string>() { "SystemClass", "systemValue", "SystemTitle", "SystemNotation", "SystemRemark", "SystemStatus" };
            IRow xRowT = xSheet.CreateRow(0); xRowT.HeightInPoints = 40;
            for (int i = 0; i < listColumn.Count; i++)
            {
                ICell xCellT = xRowT.CreateCell(i);
                xCellT.SetCellValue(listColumn[i]);
            }
            List<listSystemDetail> systemDetailList = new List<listSystemDetail>();
            systemDetailList = sdModels.reListSystemDetail();
            for (int i = 0; i < systemDetailList.Count; i++)
            {
                listSystemDetail dr = systemDetailList[i];
                List<string> listData = new List<string>();
                listData.Add(dr.lSystemClass.ToString());
                listData.Add(dr.lSystemValue.ToString());
                listData.Add(dr.lSystemTitle.ToString());
                listData.Add(dr.lSystemNotation.ToString());
                listData.Add(dr.lSystemRemark.ToString());
                listData.Add(dr.lSystemStatus.ToString());
                IRow xRowD = xSheet.CreateRow(i + 1);
                xRowD.HeightInPoints = 40;
                for (int b = 0; b < listData.Count; b++)
                {
                    ICell xCellData = xRowD.CreateCell(b);
                    xCellData.SetCellValue(listData[b]);
                }
            }
            MemoryStream MS = new MemoryStream(); NpoiWB.Write(MS);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + ExportFileName + "");
            Response.BinaryWrite(MS.ToArray());
            NpoiWB = null; MS.Close(); MS.Dispose();
            Response.Flush();
            Response.End();
        }

        public PartialViewResult GetBindData(string funSearchValue, string funPageIndex, SystemDetailModels viewModel)
        {
            List<listSystemDetail> seaList = new List<listSystemDetail>();
            seaList = sdModels.reListSystemDetail();
            if (!(string.IsNullOrWhiteSpace(funSearchValue)))
            {
                seaList = seaList.Where(x => x.lSystemClass.Contains(funSearchValue)
                    || x.lSystemValue.Contains(funSearchValue) || x.lSystemTitle.Contains(funSearchValue)
                    || x.lSystemNotation.Contains(funSearchValue) || x.lSystemRemark.Contains(funSearchValue)
                ).ToList();
            }
            viewModel.listSystemDetail = seaList;
            viewModel.valSumDataCount = viewModel.listSystemDetail.Count.ToString();
            viewModel.valPageDataCount = "30";
            viewModel.valSumPageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(viewModel.valSumDataCount) / Convert.ToDecimal(viewModel.valPageDataCount))).ToString();
            viewModel.valPageIndex = funPageIndex;
            ViewBag.valPageIndex = viewModel.valPageIndex;
            ViewBag.valShowDetail = ViewBag.valPageIndex;
            ViewBag.valSumDataCount = viewModel.valSumDataCount;
            ViewBag.valPageDataCount = viewModel.valPageDataCount;
            ViewBag.valSumPageCount = viewModel.valSumPageCount;
            return PartialView("List", viewModel);
        }

    }
}
