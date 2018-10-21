using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
using MVCEmp.Models;
using System.Web.Mvc;

namespace MVCEmp.Models
{
    public class EmployeeJobDetailModels
    {
        SystemDetailModels sdModel = new SystemDetailModels();
        EmployeeDetailModels edModel = new EmployeeDetailModels();
        App_Start.ClassDataBase dbClass = new App_Start.ClassDataBase();
        public List<listEmployeeJobDetail> ListEmployeeJobDetail = new List<listEmployeeJobDetail>();
        public string valEmpIndex { get; set; }
        public string valDeptIndex { get; set; }
        public string valDeptTitle { get; set; }
        public string valJobIndex { get; set; }
        public string valJobTitle { get; set; }
        public string valJobDateTime { get; set; }
        public string valJobClass { get; set; }
        public string valClassTitle { get; set; }
        public string valJobNotation { get; set; }
        public string valJobRemark { get; set; }
        public string valJobStatus { get; set; }
        public string valJobJoinDate { get; set; }
        public string valJobLeaveDate { get; set; }
        public string valEmpName { get; set; }
        public string valNowDateTime { get; set; }
        static string rtnValue { get; set; }
        static string executeSQL { get; set; }
        static string funcQuerySQL { get; set; }
        public string valSumDataCount { get; set; }
        public string valSumPageCount { get; set; }
        public string valPageDataCount { get; set; }
        public string valPageIndex { get; set; }
        public string valShowDetail { get; set; }
        static List<string> ListColumnName = new List<string>() { "EmpIndex", "DeptIndex", "JobIndex", "JobDateTime", "JobClass", "JobNotation", "JobRemark", "JobStatus", "JobJoinDate", "JobLeaveDate" };
        public List<SelectListItem> selDeptIndex { get; set; }
        public List<SelectListItem> selJobIndex { get; set; }
        public List<SelectListItem> selJobClass { get; set; }
        public List<SelectListItem> selJoinYY { get; set; }
        public List<SelectListItem> selJoinMM { get; set; }
        public List<SelectListItem> selJoinDD { get; set; }
        public List<SelectListItem> selEmpData { get; set; }

        public DataTable reDataTable()
        {
            DataTable returnDT = new DataTable();
            funcQuerySQL = "select * from EmployeeJobDetail ";
            returnDT = dbClass.getDataTableToDataBase(funcQuerySQL, null);
            return returnDT;
        }

        public List<listEmployeeJobDetail> reListEmployeeJobDetail()
        {
            List<listEmployeeJobDetail> list = new List<listEmployeeJobDetail>();
            DataTable returnDT = reDataTable();
            DataTable rtnDTS = sdModel.ReDataTable();
            DataTable rtnDTE = edModel.ReDataTable();
            list = (from ej in returnDT.AsEnumerable()
                    join ed in rtnDTE.AsEnumerable()
                        on ej.Field<string>("EmpIndex") equals ed.Field<string>("EmpIndex")
                    join sa in rtnDTS.AsEnumerable()
                        on ej.Field<string>("DeptIndex") equals sa.Field<string>("SystemValue")
                    join sb in rtnDTS.AsEnumerable()
                        on ej.Field<string>("JobIndex") equals sb.Field<string>("SystemValue")
                    join sc in rtnDTS.AsEnumerable()
                        on ej.Field<string>("JobClass") equals sc.Field<string>("SystemValue")
                    where sa.Field<string>("SystemClass") == "DeptIndex"
                    && sb.Field<string>("SystemClass") == "JobIndex"
                    && sc.Field<string>("SystemClass") == "JobClass"
                    select new listEmployeeJobDetail
                    {
                        lEmpIndex = ej.Field<string>("EmpIndex"),
                        lDeptIndex = ej.Field<string>("DeptIndex"),
                        lJobIndex = ej.Field<string>("JobIndex"),
                        lJobDateTime = ej.Field<string>("JobDateTime"),
                        lJobClass = ej.Field<string>("JobClass"),
                        lJobNotation = ej.Field<string>("JobNotation"),
                        lJobRemark = ej.Field<string>("JobRemark"),
                        lJobStatus = ej.Field<string>("JobStatus"),
                        lJobJoinDate = ej.Field<string>("JobJoinDate"),
                        lJobLeaveDate = ej.Field<string>("JobLeaveDate"),
                        lEmpName = ed.Field<string>("EmpName"),
                        lDeptTitle = sa.Field<string>("SystemTitle"),
                        lJobTitle = sb.Field<string>("SystemTitle"),
                        lJobClassTitle = sc.Field<string>("SystemTitle")
                    }).ToList();
            return list;
        }

        public string DataCreate(List<string> SendDataList)
        {
            executeSQL = string.Format(@" insert into EmployeeJobDetail ( ");
            for (int i = 0; i < ListColumnName.Count; i++)
            {
                executeSQL += string.Format(@" {0}{1}", ListColumnName[i].ToString(), (i < ListColumnName.Count() - 1) ? "," : "");
            }
            executeSQL += string.Format(@" ) values ( ");
            for (int i = 0; i < SendDataList.Count; i++)
            {
                executeSQL += string.Format(@" '{0}'{1}", SendDataList[i].ToString(), (i < SendDataList.Count() - 1) ? "," : "");
            }
            executeSQL += string.Format(@" )");
            rtnValue = dbClass.execValueToDataBase(executeSQL, null);
            return rtnValue;
        }

        public string DataUpdate(List<string> SendDataList)
        {
            executeSQL = string.Format(@"Update EmployeeJobDetail set ");
            for (int i = 3; i < ListColumnName.Count; i++)
            {
                executeSQL += string.Format(@" {0}='{1}'{2}", ListColumnName[i].ToString(), SendDataList[i].ToString(), (i < ListColumnName.Count - 1) ? "," : "");
            }
            executeSQL += string.Format(@" where ");
            for (int i = 0; i < 3; i++)
            {
                executeSQL += string.Format(@" {2}{0}='{1}' ", ListColumnName[i].ToString(), SendDataList[i].ToString(), (i == 0) ? "" : " and ");
            }
            rtnValue = dbClass.execValueToDataBase(executeSQL, null);
            return rtnValue;
        }

        public string DataDelete(string fEmpIndex, string fDeptIndex, string fJobIndex)
        {
            executeSQL = string.Format(@"delete from EmployeeJobDetail 
                where EmpIndex='{0}' and DeptIndex='{1}' and JobIndex='{2}'"
            , fEmpIndex, fDeptIndex, fJobIndex);
            rtnValue = dbClass.execValueToDataBase(executeSQL, null);
            return rtnValue;
        }

        public List<listEmployeeJobDetail> detailEmployeeJobDetail(string fEmpIndex, string fDeptIndex, string fJobIndex)
        {
            List<listEmployeeJobDetail> delist = new List<listEmployeeJobDetail>();
            delist = reListEmployeeJobDetail();
            delist = delist.Where(x => x.lEmpIndex == fEmpIndex).ToList();
            if (fDeptIndex != "") { delist = delist.Where(x => x.lDeptIndex.Contains(fDeptIndex)).ToList(); }
            if (fJobIndex != "") { delist = delist.Where(x => x.lJobIndex.Contains(fJobIndex)).ToList(); }
            return delist;
        }
    }

    public class listEmployeeJobDetail
    {
        public string lEmpIndex { get; set; }
        public string lDeptIndex { get; set; }
        public string lJobIndex { get; set; }
        public string lJobDateTime { get; set; }
        public string lJobClass { get; set; }
        public string lJobNotation { get; set; }
        public string lJobRemark { get; set; }
        public string lJobStatus { get; set; }
        public string lJobJoinDate { get; set; }
        public string lJobLeaveDate { get; set; }
        public string lEmpName { get; set; }
        public string lDeptTitle { get; set; }
        public string lJobTitle { get; set; }
        public string lJobClassTitle { get; set; }
        public string lJobStatusTitle { get; set; }

    }

}