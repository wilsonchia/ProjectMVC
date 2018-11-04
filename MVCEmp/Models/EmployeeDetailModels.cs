using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCEmp.Models
{
    public class EmployeeDetailModels
    {

        public List<listEmployeeDetail> listEmployeeDetail = new List<listEmployeeDetail>();
        public string showEmpIndex { get; set; }      public string valEmpIndex { get; set; }
        public string showEmpName { get; set; }       public string valEmpName { get; set; }
        public string showEmpSex { get; set; }        public string valEmpSex { get; set; }
        public string showEmpEmail { get; set; }      public string valEmpEmail { get; set; }
        public string showEmpAddtess { get; set; }    public string valEmpAddtess { get; set; }
        public string showEmpMobile { get; set; }     public string valEmpMobile { get; set; }
        public string showEmpPhone { get; set; }      public string valEmpPhone { get; set; }
        public string showEmpNotation { get; set; }   public string valEmpNotation { get; set; }
        public string showEmpRemark { get; set; }     public string valEmpRemark { get; set; }
        public string showEmpStatus { get; set; }     public string valEmpStatus { get; set; }
        public string showEmpJoinDate { get; set; }   public string valEmpJoinDate { get; set; }
        public string showEmpLeaveDate { get; set; }  public string valEmpLeaveDate { get; set; }
        public string valEmpID { get; set; }          public string valEmpPassword { get; set; }
        public string showEmpID { get; set; }         public string showEmpPassword { get; set; }
        public string searchValue { get; set; }
        public string valSumDataCount { get; set; }
        public string valSumPageCount { get; set; }
        public string valPageDataCount { get; set; }
        public string valPageIndex { get; set; }
        public List<SelectListItem> selAreaCity { get; set; }
        public List<SelectListItem> selAreaZip { get; set; }
        public List<string> listEmployeeColumn = new List<string>() { 
            "EmpIndex", "EmpID", "EmpName", "EmpSex", "EmpEmail",
            "EmpAddress", "EmpMobile", "EmpPhone", "EmpNotation", "EmpRemark",
            "EmpStatus", "EmpJoinDate", "EmpLeaveDate", "EmpPassword" };

        App_Start.ClassDataBase classDB = new App_Start.ClassDataBase();

        public DataTable ReDataTable()
        {
            DataTable reDT = new DataTable(); string funcQuerySQL = "";
            funcQuerySQL = "select * from EmployeeDetail Where 1=1 and isnull(EmpIndex,'')<>'' ";
            reDT = classDB.getDataTableToDataBase(funcQuerySQL, null);
            return reDT;
        }

        public List<listEmployeeDetail> ReListEmployeeDetail()
        {
            List<listEmployeeDetail> returnListDetail = new List<Models.listEmployeeDetail>();
            DataTable reDT = new DataTable(); reDT = ReDataTable();
            if (reDT.Rows.Count > 0)
            {
                foreach (DataRow dr in reDT.Rows)
                {
                    listEmployeeDetail item = new listEmployeeDetail();
                    item.lEmpIndex = dr["EmpIndex"].ToString();
                    item.lEmpID = dr["EmpID"].ToString();
                    item.lEmpName = dr["EmpName"].ToString();
                    item.lEmpSex = dr["EmpSex"].ToString();
                    item.lEmpEmail = dr["EmpEmail"].ToString();
                    item.lEmpAddress = dr["EmpAddress"].ToString();
                    item.lEmpMobile = dr["EmpMobile"].ToString();
                    item.lEmpPhone = dr["EmpPhone"].ToString();
                    item.lEmpNotation = dr["EmpNotation"].ToString();
                    item.lEmpRemark = dr["EmpRemark"].ToString();
                    item.lEmpStatus = dr["EmpStatus"].ToString();
                    item.lEmpJoinDate = dr["EmpJoinDate"].ToString();
                    item.lEmpLeaveDate = dr["EmpLeaveDate"].ToString();
                    item.lEmpPassword = dr["EmpPassword"].ToString();
                    returnListDetail.Add(item);
                }
            }
            return returnListDetail;
        }

        public string DataCreate(List<string> SendDataList)
        {
            string funcExecuteSQL = ""; string ReturnValue = ""; string funcQuerySQL = "";
            funcQuerySQL = string.Format(@"select * from EmployeeDetail Where EmpIndex='{0}'", SendDataList[0].ToString());
            if (classDB.checkValueToDataBase(funcQuerySQL, null) == "X")
            {
                funcExecuteSQL = string.Format(@"Insert Into EmployeeDetail ( ");
                for (int i = 0; i < listEmployeeColumn.Count; i++)
                {
                    funcExecuteSQL += string.Format(@"{0}{1}", listEmployeeColumn[i].ToString(), (i < listEmployeeColumn.Count - 1) ? "," : "");
                }

                funcExecuteSQL += string.Format(@" ) values ( ");
                for (int i = 0; i < SendDataList.Count; i++)
                {
                    funcExecuteSQL += string.Format(@"'{0}'{1}", SendDataList[i].ToString(), (i < SendDataList.Count - 1) ? "," : "");
                }
                funcExecuteSQL += string.Format(@")");
                ReturnValue = classDB.execValueToDataBase(funcExecuteSQL, null);
                //ReturnValue = funcExecuteSQL;
            }            
            return ReturnValue;
        }

        public string DataUpdate(List<string> SendDataList)
        {
            string funcExecuteSQL = ""; string ReturnValue = ""; string funcQuerySQL = "";
            funcQuerySQL = string.Format(@"select * from EmployeeDetail Where EmpIndex='{0}'", SendDataList[0].ToString());
            if (classDB.checkValueToDataBase(funcQuerySQL, null) == "O")
            {
                funcExecuteSQL = string.Format(@" Update EmployeeDetail Set ");
                for (int i = 2; i < SendDataList.Count; i++) { funcExecuteSQL += string.Format(@" {0}='{1}'{2} ", listEmployeeColumn[i].ToString(), SendDataList[i].ToString(), (i < SendDataList.Count - 1) ? "," : ""); }
                funcExecuteSQL += string.Format(@" Where EmpIndex='{0}' and EmpID='{1}'", SendDataList[0].ToString(), SendDataList[1].ToString());
                ReturnValue = classDB.execValueToDataBase(funcExecuteSQL, null);
                //ReturnValue = funcExecuteSQL;
            }
            ReturnValue = funcExecuteSQL;
            return ReturnValue;
        }

        public string DataDelete(string funcEmpIndex)
        {
            string funcExecuteSQL = ""; string ReturnValue = "";
            funcExecuteSQL = string.Format(@"Delete from EmployeeDetail Where EmpIndex='{0}'", funcEmpIndex);
            ReturnValue = classDB.execValueToDataBase(funcExecuteSQL, null);
            return ReturnValue;
        }

        public string returnMaxEmpIndex()
        {
            string ReturnValue = ""; DataTable reDT = new DataTable(); reDT = ReDataTable();
            if (reDT.Rows.Count > 0)
            {
                ReturnValue = Convert.ToInt32(Convert.ToInt32(reDT.Compute("Max(EmpIndex)", "")) + 1).ToString().PadLeft(5, '0');
            }
            else { ReturnValue = "00001"; }
            return ReturnValue;
        }

        public List<SelectListItem> selEmpDataList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<listEmployeeDetail> edList = new List<listEmployeeDetail>();
            edList = ReListEmployeeDetail().Where(x => x.lEmpStatus == "O").ToList();
            list.Add(new SelectListItem { Value = "", Text = "請選擇員工" });
            if (edList.Count() > 0) {                
                for (int i = 0; i < edList.Count; i++) {
                    list.Add(new SelectListItem { Value = edList[i].lEmpIndex.ToString(), Text = string.Format(@"({0}){1}", edList[i].lEmpIndex.ToString(), edList[i].lEmpName.ToString()) });
                }
            }
            return list;
        }




    }

    public class listEmployeeDetail
    {
        public string lEmpIndex { get; set; }
        public string lEmpID { get; set; }
        public string lEmpName { get; set; }
        public string lEmpSex { get; set; }
        public string lEmpEmail { get; set; }
        public string lEmpAddress { get; set; }
        public string lEmpMobile { get; set; }
        public string lEmpPhone { get; set; }
        public string lEmpNotation { get; set; }
        public string lEmpRemark { get; set; }
        public string lEmpStatus { get; set; }
        public string lEmpJoinDate { get; set; }
        public string lEmpLeaveDate { get; set; }
        public string lEmpPassword { get; set; }
    }

}