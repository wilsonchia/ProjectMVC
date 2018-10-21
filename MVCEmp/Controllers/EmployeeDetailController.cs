using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using MVCEmp.Models;


namespace MVCEmp.Controllers
{
    public class EmployeeDetailController : Controller
    {
        EmployeeDetailModels edModel = new EmployeeDetailModels();
        App_Start.ClassDataBase dbClass = new App_Start.ClassDataBase();

        // GET: EmployeeDetail
        public ActionResult Index(EmployeeDetailModels viewModel)
        {
            GetBindData("", "1", viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, EmployeeDetailModels viewModel)
        {
            string funSearchValue = form["searchValue"].ToString();
            string funPageIndex = form["hidePageIndex"].ToString();
            GetBindData(funSearchValue, funPageIndex, viewModel);
            return View(viewModel);
        }

        public PartialViewResult GetBindData(string funSearchValue, string funPageIndex, EmployeeDetailModels viewModel)
        {
            List<listEmployeeDetail> empList = new List<listEmployeeDetail>();
            empList = edModel.ReListEmployeeDetail();
            if (!(string.IsNullOrWhiteSpace(funSearchValue)))
            {
                empList = empList.Where(x => x.lEmpIndex.Contains(funSearchValue) || x.lEmpName.Contains(funSearchValue)
                || x.lEmpEmail.Contains(funSearchValue) || x.lEmpMobile.Contains(funSearchValue)
                || x.lEmpPhone.Contains(funSearchValue) || x.lEmpNotation.Contains(funSearchValue)
                || x.lEmpRemark.Contains(funSearchValue) || x.lEmpJoinDate.Contains(funSearchValue)
                || x.lEmpLeaveDate.Contains(funSearchValue)).ToList();
            }
            viewModel.listEmployeeDetail = empList;
            viewModel.valSumDataCount = empList.Count.ToString();
            viewModel.valPageDataCount = "30";
            viewModel.valSumPageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(viewModel.valSumDataCount) / Convert.ToDecimal(viewModel.valPageDataCount))).ToString();
            viewModel.valPageIndex = (ViewBag.valPageIndex == null) ? "1" : viewModel.valPageIndex;
            viewModel.valPageIndex = funPageIndex;
            ViewBag.valPageIndex = viewModel.valPageIndex;
            ViewBag.valShowDetail = ViewBag.valPageIndex;
            ViewBag.valSumDataCount = viewModel.valSumDataCount;
            ViewBag.valPageDataCount = viewModel.valPageDataCount;
            ViewBag.valSumPageCount = viewModel.valSumPageCount;
            return PartialView("List", viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            List<string> listCreateData = new List<string>();
            listCreateData.Add(edModel.returnMaxEmpIndex().ToString());
            listCreateData.Add(form["textEmpName"].ToString());
            listCreateData.Add(form["hideEmpSex"].ToString());
            listCreateData.Add(form["textEmpEmail"].ToString());
            listCreateData.Add(form["textEmpAddress"].ToString());
            listCreateData.Add(form["textEmpMobile"].ToString());
            listCreateData.Add(form["textEmpPhone"].ToString());
            listCreateData.Add(form["textEmpNotation"].ToString());
            listCreateData.Add(form["textEmpRemark"].ToString());
            listCreateData.Add("O");
            listCreateData.Add(DateTime.Now.ToString("yyyy/mm/dd"));
            listCreateData.Add("");
            string returnExecValue = edModel.DataCreate(listCreateData);
            //ViewBag.showEmpNotation = returnExecValue;
            //return View();
            return Redirect("~/EmployeeDetail/Index");
        }

        public ActionResult Update(string vIndex)
        {
            ViewBag.valEmpIndex = vIndex.ToString();
            List<listEmployeeDetail> empDetail = new List<listEmployeeDetail>();
            empDetail = edModel.ReListEmployeeDetail().Where(x => x.lEmpIndex == vIndex.ToString()).ToList();
            if (empDetail.Count > 0)
            {
                ViewBag.valEmpName = empDetail[0].lEmpName.ToString();
                ViewBag.valEmpSex = empDetail[0].lEmpSex.ToString();
                ViewBag.valEmpMobile = empDetail[0].lEmpMobile.ToString();
                ViewBag.valEmpPhone = empDetail[0].lEmpPhone.ToString();
                ViewBag.valEmpEmail = empDetail[0].lEmpEmail.ToString();
                ViewBag.valEmpAddress = empDetail[0].lEmpAddress.ToString();
                ViewBag.valEmpNotation = empDetail[0].lEmpNotation.ToString();
                ViewBag.valEmpRemark = empDetail[0].lEmpRemark.ToString();
                ViewBag.valEmpStatus = empDetail[0].lEmpStatus.ToString();
                ViewBag.valEmpJoinDate = empDetail[0].lEmpJoinDate.ToString();
                ViewBag.valEmpLeaveDate = empDetail[0].lEmpLeaveDate.ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Update(FormCollection form)
        {
            List<string> listUpdateData = new List<string>();
            listUpdateData.Add(form["hideEmpIndex"].ToString());
            listUpdateData.Add(form["textEmpName"].ToString());
            listUpdateData.Add(form["hideEmpSex"].ToString());
            listUpdateData.Add(form["textEmpEmail"].ToString());
            listUpdateData.Add(form["textEmpAddress"].ToString());
            listUpdateData.Add(form["textEmpMobile"].ToString());
            listUpdateData.Add(form["textEmpPhone"].ToString());
            listUpdateData.Add(form["textEmpNotation"].ToString());
            listUpdateData.Add(form["textEmpRemark"].ToString());
            listUpdateData.Add(form["hideEmpStatus"].ToString());
            listUpdateData.Add(form["hideEmpJoinDate"].ToString());
            listUpdateData.Add(form["textEmpLeaveDate"].ToString());
            string showData = "";
            for (int i = 0; i < listUpdateData.Count; i++)
            {
                showData += string.Format(@"{0}-", listUpdateData[i].ToString());
            }
            string returnExecValue = edModel.DataUpdate(listUpdateData);
            return Redirect("~/EmployeeDetail/Index");
        }

        [HttpPost]
        public void OutExcelData()
        {
            string ExportFileName = "EmpData.xlsx"; string ExportFileTitle = "Data"; Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            XSSFWorkbook NpoiWB = new XSSFWorkbook();
            XSSFCellStyle xCellStyle = (XSSFCellStyle)NpoiWB.CreateCellStyle();
            XSSFDataFormat NpoiFormat = (XSSFDataFormat)NpoiWB.CreateDataFormat();
            xCellStyle.SetDataFormat(NpoiFormat.GetFormat("[DbNum2][$-804]0"));
            XSSFCellStyle cellStyleFontColor = (XSSFCellStyle)NpoiWB.CreateCellStyle();
            XSSFFont font1 = (XSSFFont)NpoiWB.CreateFont(); font1.Color = (short)10; font1.IsBold = true;
            cellStyleFontColor.SetFont(font1);
            ///     進行產生Excel檔案流程
            ISheet xSheet = NpoiWB.CreateSheet(ExportFileTitle);
            List<string> listColumn = edModel.listEmployeeColumn;
            ///     建立標題列
            IRow xRowT = xSheet.CreateRow(0); xRowT.HeightInPoints = 40;
            for (int i = 0; i < listColumn.Count; i++) { ICell xCellT = xRowT.CreateCell(i); xCellT.SetCellValue(listColumn[i]); }
            ///     讀取資料庫資料
            List<listEmployeeDetail> ListEmpData = new List<listEmployeeDetail>();
            ListEmpData = edModel.ReListEmployeeDetail();
            if (ListEmpData != null && ListEmpData.Count > 0)
            {
                for (int i = 0; i < ListEmpData.Count; i++)
                {
                    listEmployeeDetail item = ListEmpData[i];
                    List<string> list = new List<string>();
                    list.Add(item.lEmpIndex.ToString());
                    list.Add(item.lEmpName.ToString());
                    list.Add(item.lEmpSex.ToString());
                    list.Add(item.lEmpEmail.ToString());
                    list.Add(item.lEmpAddress.ToString());
                    list.Add(item.lEmpMobile.ToString());
                    list.Add(item.lEmpPhone.ToString());
                    list.Add(item.lEmpNotation.ToString());
                    list.Add(item.lEmpRemark.ToString());
                    list.Add(item.lEmpStatus.ToString());
                    list.Add(item.lEmpJoinDate.ToString());
                    list.Add(item.lEmpLeaveDate.ToString());
                    IRow xRowD = xSheet.CreateRow(i + 1); xRowD.HeightInPoints = 30;
                    for (int b = 0; b < list.Count; b++) { ICell xCellData = xRowD.CreateCell(b); xCellData.SetCellValue(list[b]); }
                }

            }
            MemoryStream MS = new MemoryStream(); NpoiWB.Write(MS);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + ExportFileName + "");
            Response.BinaryWrite(MS.ToArray());
            //     ----------------------------------------------------------------------------------------------
            //     釋放記憶體參數
            NpoiWB = null; MS.Close(); MS.Dispose();
            Response.Flush(); Response.End();
        }

        //[HttpPost]
        //public void DataUpload(HttpPostedFileBase fileUpload)
        //{
        //    string showDetailData = ""; DataTable dtTemp = new DataTable();
        //    if (fileUpload != null && fileUpload.ContentLength > 0)
        //    {
        //        string FilePath = System.Web.HttpContext.Current.Server.MapPath("~/excel");
        //        string FileName = fileUpload.FileName;
        //        fileUpload.SaveAs(string.Format(@"{0}\{1}", FilePath, FileName));
        //        dynamic hssfweb; dtTemp.Clear();
        //        using (MemoryStream ms = new MemoryStream(dbClass.GetFile(string.Format(@"{0}\{1}", FilePath, FileName))))
        //        {
        //            hssfweb = WorkbookFactory.Create(ms);
        //        }
        //        ISheet sheet = (ISheet)hssfweb.GetSheetAt(0);
        //        if (sheet.LastRowNum > 0)
        //        {
        //            IRow rowT = (IRow)sheet.GetRow(0);
        //            for (int d = 0; d < 12; d++)
        //            {
        //                DataColumn col = new DataColumn(rowT.GetCell(d).ToString(), typeof(string));
        //                dtTemp.Columns.Add(col);
        //            }
        //            for (int i = 1; i <= sheet.LastRowNum; i++)
        //            {
        //                IRow rowD = (IRow)sheet.GetRow(i);
        //                if (rowD != null)
        //                {
        //                    DataRow tempRow = dtTemp.NewRow();
        //                    for (int a = 0; a < 12; a++)
        //                    {
        //                        //Response.Write(rowD.GetCell(a).ToString() + "<br>");
        //                        tempRow[a] = (rowD.GetCell(a) != null) ? rowD.GetCell(a).ToString() : "";
        //                    }
        //                    dtTemp.Rows.Add(tempRow);
        //                }
        //            }
        //        }
        //        Response.Write(dtTemp.Rows.Count.ToString() + dtTemp.Columns.Count.ToString());
        //        //List<listEmployeeDetail> listEmpDetail = new List<listEmployeeDetail>();
        //        //listEmpDetail = edModel.ReListEmployeeDetail();
        //        //if (dtTemp.Rows.Count > 0)
        //        //{
        //        //    foreach (DataRow dr in dtTemp.Rows)
        //        //    {
        //        //        List<string> execDataList = new List<string>();
        //        //        execDataList.Add(dr["EmpIndex"].ToString());
        //        //        execDataList.Add(dr["EmpName"].ToString());
        //        //        execDataList.Add(dr["EmpSex"].ToString());
        //        //        execDataList.Add(dr["EmpEmail"].ToString());
        //        //        execDataList.Add(dr["EmpAddress"].ToString());
        //        //        execDataList.Add(dr["EmpMobile"].ToString());
        //        //        execDataList.Add(dr["EmpPhone"].ToString());
        //        //        execDataList.Add(dr["EmpNotation"].ToString());
        //        //        execDataList.Add(dr["EmpRemark"].ToString());
        //        //        execDataList.Add(dr["EmpStatus"].ToString());
        //        //        execDataList.Add(dr["EmpJoinDate"].ToString());
        //        //        execDataList.Add(dr["EmpLeaveDate"].ToString());
        //        //        if (listEmpDetail.Where(x =>x.lEmpIndex == dr["EmpIndex"].ToString()).Count() == 0) {
        //        //            edModel.DataCreate(execDataList);
        //        //        } else {
        //        //            edModel.DataUpdate(execDataList);
        //        //        }
        //        //    }                    
        //        //}
        //    }
        //}

    }
}
