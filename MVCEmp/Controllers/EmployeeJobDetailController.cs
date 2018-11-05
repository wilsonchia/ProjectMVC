using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCEmp.Models;
using System.Data;

namespace MVCEmp.Controllers
{
    public class EmployeeJobDetailController : Controller
    {
        EmployeeJobDetailModels jdModel = new EmployeeJobDetailModels();
        EmployeeDetailModels edModel = new EmployeeDetailModels();
        SystemDetailModels sdModel = new SystemDetailModels();
        App_Start.ClassDataBase dbClass = new App_Start.ClassDataBase();
        List<listEmployeeDetail> edDetail = new List<listEmployeeDetail>();
        List<listEmployeeJobDetail> jobList = new List<listEmployeeJobDetail>();

        // GET: EmployeeJobDetail
        public ActionResult Index(EmployeeJobDetailModels viewModel)
        {
            getBindData("", "1", viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, EmployeeJobDetailModels viewModel)
        {
            string funSearchValue = form["searchValue"].ToString();
            string funPageIndex = form["hidePageIndex"].ToString();
            getBindData(funSearchValue, funPageIndex, viewModel);
            return View(viewModel);
        }

        public ActionResult Create()
        {
            var selEmpData = new List<SelectListItem>();
            selEmpData = edModel.selEmpDataList();
            ViewBag.selEmpData = selEmpData;
            ViewBag.valNowDateTime = System.DateTime.Now.ToShortDateString();
            var seldeptdata = new List<SelectListItem>();
            seldeptdata = sdModel.rtnSysDataList("DeptIndex", "請選擇部門");
            ViewBag.selDeptIndex = seldeptdata;
            var seljobdata = new List<SelectListItem>();
            seljobdata = sdModel.rtnSysDataList("JobIndex", "請選擇職位");
            ViewBag.selJobIndex = seljobdata;
            var seljobclass = new List<SelectListItem>();
            seljobclass = sdModel.rtnSysDataList("JobClass", "請選擇類別");
            ViewBag.selJobClass = seljobclass;
            return View();
        }

        [HttpPost]
        public RedirectResult Create(FormCollection form)
        {
            List<string> DataList = new List<string>();
            DataList.Add(form["hideEmpIndex"].ToString());
            DataList.Add(form["hideDeptIndex"].ToString());
            DataList.Add(form["hideJobIndex"].ToString());
            DataList.Add(form["hideJobDateTime"].ToString());
            DataList.Add(form["hideJobClass"].ToString());
            DataList.Add(form["textJobNotation"].ToString());
            DataList.Add(form["textJobRemark"].ToString());
            DataList.Add(form["hideJobStatus"].ToString());
            DataList.Add(form["hideJobJoinDate"].ToString());
            DataList.Add("");
            string execValue = jdModel.DataCreate(DataList);
            return Redirect("~/EmployeeJobDetail/Index");
        }

        public ActionResult Update(string fEmpIndex, string fDeptIndex, string fJobIndex)
        {
            modelDataClear();
            jobList = jdModel.detailEmployeeJobDetail(fEmpIndex, fDeptIndex, fJobIndex);
            ViewBag.valEmpIndex = fEmpIndex;
            ViewBag.valDeptIndex = fDeptIndex;
            ViewBag.valJobIndex = fJobIndex;
            if (jobList.Count() > 0)
            {
                ViewBag.valEmpName = jobList[0].lEmpName.ToString();
                ViewBag.valDeptTitle = jobList[0].lDeptTitle.ToString();
                ViewBag.valJobTitle = jobList[0].lJobTitle.ToString();
                ViewBag.valJobClass = jobList[0].lJobClass.ToString();
                ViewBag.valJobNotation = jobList[0].lJobNotation.ToString();
                ViewBag.valJobRemark = jobList[0].lJobRemark.ToString();
                ViewBag.valJoinDateTime = jobList[0].lJobDateTime.ToString();
                ViewBag.valJobJoinDate = jobList[0].lJobJoinDate.ToString();
                ViewBag.valJobStatus = jobList[0].lJobStatus.ToString();
                ViewBag.valJobLeaveDate = jobList[0].lJobLeaveDate.ToString();
                ViewBag.valJobClassTitle = jobList[0].lJobClassTitle.ToString();
            }
            ViewBag.valJobLeaveDate = (ViewBag.valJobStatus == "O") ? "" : (ViewBag.valJobLeaveDate != "") ? ViewBag.valJobLeaveDate : System.DateTime.Now.ToShortDateString();
            List<SelectListItem> seljobclass = new List<SelectListItem>();            
            seljobclass = sdModel.rtnSysDataList("JobClass", "請選擇類別");
            //seljobclass.Where(x => x.Value == ViewBag.valJobClass.Trim()).First().Selected = true;
            ViewBag.selJobClass = seljobclass;            
            return View();
        }

        [HttpPost]
        public RedirectResult Update(FormCollection form)
        {
            List<string> DataList = new List<string>();
            DataList.Add(form["hideEmpIndex"].ToString());
            DataList.Add(form["hideDeptIndex"].ToString());
            DataList.Add(form["hideJobIndex"].ToString());
            DataList.Add(form["hideJobDateTime"].ToString());
            DataList.Add(form["hideJobClass"].ToString());
            DataList.Add(form["textJobNotation"].ToString());
            DataList.Add(form["textJobRemark"].ToString());
            DataList.Add(form["hideJobStatus"].ToString());
            DataList.Add(form["hideJobJoinDate"].ToString());
            DataList.Add(form["hideJobLeaveDate"].ToString());
            string execValue = jdModel.DataUpdate(DataList);
            return Redirect("~/EmployeeJobDetail/Index");
        }

        public RedirectResult Delete(string fEmpIndex, string fDeptIndex, string fJobIndex)
        {
            string execValue = jdModel.DataDelete(fEmpIndex, fDeptIndex, fJobIndex);
            return Redirect("~/EmployeeJobDetail/Index");
        }

        public PartialViewResult getBindData(string funSearchValue, string funPageIndex, EmployeeJobDetailModels viewModel)
        {

            jobList = jdModel.reListEmployeeJobDetail();
            if (!(string.IsNullOrWhiteSpace(funSearchValue)))
            {
                jobList = jobList.Where(x => x.lEmpIndex.Contains(funSearchValue)
                || x.lDeptIndex.Contains(funSearchValue) || x.lJobNotation.Contains(funSearchValue)
                || x.lJobRemark.Contains(funSearchValue)).ToList();
            }
            viewModel.ListEmployeeJobDetail = jobList;
            viewModel.valSumDataCount = jobList.Count.ToString();
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

        private void modelDataClear() {
            ViewBag.valEmpIndex = ""; ViewBag.valDeptIndex = ""; ViewBag.valJobIndex = ""; ViewBag.valEmpName = "";
            ViewBag.valDeptTitle = ""; ViewBag.valJobTitle = ""; ViewBag.valJobClass = ""; ViewBag.valJobNotation = "";
            ViewBag.valJobRemark = ""; ViewBag.valJoinDateTime = ""; ViewBag.valJobJoinDate = ""; ViewBag.valJobStatus = "";
            ViewBag.valJobLeaveDate = "";
        }

    }
}
