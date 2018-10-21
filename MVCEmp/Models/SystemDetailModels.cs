using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace MVCEmp.Models
{
    public class SystemDetailModels
    {

        public List<listSystemDetail> listSystemDetail = new List<listSystemDetail>();
        public string valSystemClass { get; set; }
        public string valSystemValue { get; set; }
        public string valSystemTitle { get; set; }
        public string valSystemNotation { get; set; }
        public string valSystemRemark { get; set; }
        public string valSystemStatus { get; set; }
        public string searchValue { get; set; }
        public string valSumDataCount { get; set; }
        public string valSumPageCount { get; set; }
        public string valPageDataCount { get; set; }
        public string valPageIndex { get; set; }
        public string funQuerySQL { get; set; }
        public string funExecuteSQL { get; set; }
        public string funExecuteValue { get; set; }
        public string funCheckValue { get; set; }
        public List<string> listColumnName = new List<string>() { "SystemClass", "SystemValue", "SystemTitle", "SystemNotation", "SystemRemark", "SystemStatus" };

        App_Start.ClassDataBase dbClass = new App_Start.ClassDataBase();

        public DataTable ReDataTable()
        {
            DataTable returnDT = new DataTable(); string funQuerySQL = "";
            funQuerySQL = "select * from SystemDetail where 1=1 ";
            returnDT = dbClass.getDataTableToDataBase(funQuerySQL, null);
            return returnDT;
        }

        public List<listSystemDetail> reListSystemDetail()
        {
            List<listSystemDetail> reList = new List<listSystemDetail>();
            DataTable reDt = ReDataTable();
            if (reDt.Rows.Count > 0)
            {
                foreach (DataRow dr in reDt.Rows)
                {
                    listSystemDetail item = new listSystemDetail();
                    item.lSystemClass = dr["SystemClass"].ToString();
                    item.lSystemValue = dr["SystemValue"].ToString();
                    item.lSystemTitle = dr["SystemTitle"].ToString();
                    item.lSystemNotation = dr["SystemNotation"].ToString();
                    item.lSystemRemark = dr["SystemRemark"].ToString();
                    item.lSystemStatus = dr["SystemStatus"].ToString();
                    reList.Add(item);
                }
            }
            return reList;
        }

        public List<listSystemDetail> detailListSystemDetail(string fSystemClass, string fSystemValue)
        {
            List<listSystemDetail> deList = new List<listSystemDetail>();
            deList = reListSystemDetail();
            deList = deList.Where(x => x.lSystemClass == fSystemClass && x.lSystemValue == fSystemValue).ToList();
            return deList;
        }

        public string DataCreate(List<string> listSysDetail)
        {
            funExecuteSQL = ""; funExecuteValue = "";
            funExecuteSQL = string.Format(@"Insert Into SystemDetail ( ");
            for (int i = 0; i < listColumnName.Count; i++)
            {
                funExecuteSQL += string.Format(@" {0}{1}", listColumnName[i].ToString(), (i < listColumnName.Count() - 1) ? "," : "");
            }
            funExecuteSQL += string.Format(@" ) values ( ");
            for (int i = 0; i < listSysDetail.Count; i++)
            {
                funExecuteSQL += string.Format(@" '{0}'{1}", listSysDetail[i].ToString(), (i < listSysDetail.Count() - 1) ? "," : "");
            }
            funExecuteSQL += string.Format(@")");
            funExecuteValue = dbClass.execValueToDataBase(funExecuteSQL, null);
            return funExecuteValue;
        }

        public string DataUpdate(List<string> listSysDetail)
        {
            funExecuteSQL = ""; funExecuteValue = ""; funCheckValue = ""; funQuerySQL = "";
            funQuerySQL = string.Format(@" select * from SystemDetail Where SystemClass='{0}' and SystemValue='{1}' "
            , listSysDetail[0].ToString(), listSysDetail[1].ToString());
            funCheckValue = dbClass.checkValueToDataBase(funQuerySQL, null);
            if (funCheckValue == "O")
            {
                funExecuteSQL = string.Format(@"Update SystemDetail Set ");
                for (int i = 2; i < listSysDetail.Count; i++)
                {
                    funExecuteSQL += string.Format(@" {0}='{1}'{2}", listColumnName[i].ToString(), listSysDetail[i].ToString(), (i < listSysDetail.Count - 1) ? "," : "");
                }
                funExecuteSQL += string.Format(@" Where ");
                for (int i = 0; i < 2; i++)
                {
                    funExecuteSQL += string.Format(@" {0}='{1}'{2}", listColumnName[i].ToString(), listSysDetail[i].ToString(), (i < 1) ? " and " : "");
                }
            }
            funExecuteValue = dbClass.execValueToDataBase(funExecuteSQL, null);
            return funExecuteValue;
        }

        public string DataDelete(string fSystemClass, string fSystemValue)
        {
            funExecuteValue = ""; funExecuteSQL = "";
            funExecuteSQL = string.Format(@"Delete from SystemDetail Where SystemClass='{0}' and SystemValue='{1}'"
                , fSystemClass, fSystemValue);
            funExecuteValue = dbClass.execValueToDataBase(funExecuteSQL, null);
            return funExecuteValue;
        }

        public List<SelectListItem> rtnSysDataList(string funSystemClass, string funNullTitle, string funSystemValue)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<listSystemDetail> sdList = new List<listSystemDetail>();
            sdList = reListSystemDetail();
            if (sdList.Where(x => x.lSystemClass == funSystemClass).Count() > 0)
            {
                sdList = sdList.Where(x => x.lSystemClass == funSystemClass).ToList();
                list.Add(new SelectListItem { Value = "", Text = funNullTitle });
                for (int i = 0; i < sdList.Count; i++)
                {
                    list.Add(new SelectListItem { Value = sdList[i].lSystemValue.ToString(), Text = sdList[i].lSystemTitle.ToString(), Selected = (sdList[i].lSystemValue.ToString() == funSystemValue) });
                }
            }
            return list;
        }


    }

    public class listSystemDetail
    {
        public string lSystemClass { get; set; }
        public string lSystemValue { get; set; }
        public string lSystemTitle { get; set; }
        public string lSystemNotation { get; set; }
        public string lSystemRemark { get; set; }
        public string lSystemStatus { get; set; }
    }

}