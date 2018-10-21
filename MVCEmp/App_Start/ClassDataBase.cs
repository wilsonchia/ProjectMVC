using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace MVCEmp.App_Start
{
    public class ClassDataBase
    {
        static string WebConn = "Data Source=localhost;User ID=admSQL;password=t/6ru8jo3;Initial Catalog=dbemployee";
        static System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
        static System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand();
        static System.Data.SqlClient.SqlDataAdapter sDA = new System.Data.SqlClient.SqlDataAdapter();
        static System.Data.DataSet rtnDS = new System.Data.DataSet();
        static System.Data.DataTable rtnDT = new System.Data.DataTable();
        static string rtnValue { get; set; }
        static string rtnExecValue { get; set; }
        static Dictionary<string, object> dicPara { get; set; }

        public DataTable getDataTableToDataBase(string rtnQuerySQL, Dictionary<string,object> rtnPara) {
            rtnDS.Clear(); rtnDT.Clear();
            try{
                conn = new SqlConnection(WebConn); conn.Open();
                comm = new SqlCommand(rtnQuerySQL, conn);
                if (rtnPara != null) {
                    foreach (KeyValuePair<string,object> item in rtnPara) {
                        comm.Parameters.AddWithValue(item.Key.ToString(), item.Value.ToString());
                    }
                }
                sDA = new SqlDataAdapter(comm);
                sDA.Fill(rtnDS, "table");
                rtnDT = rtnDS.Tables["table"];
            } catch (Exception ex){
                rtnDT.Clear();
            } finally {
                conn.Dispose(); comm.Dispose();
            }
            return rtnDT;
        }

        public string checkValueToDataBase(string rtnQuerySQL, Dictionary<string, object> rtnPara) {
            try {
                rtnValue = ""; rtnDT.Clear();
                rtnDT = getDataTableToDataBase(rtnQuerySQL, rtnPara);
                rtnValue = (rtnDT.Rows.Count > 0) ? "O" : "X";
            } catch (Exception ex) {
                rtnValue = ex.Message;
            }
            return rtnValue;
        }

        public string execValueToDataBase(string rtnExecuteSQL, Dictionary<string, object> execPara) {
            try {
                rtnExecValue = "";
                conn = new SqlConnection(WebConn); conn.Open();
                comm = new SqlCommand(rtnExecuteSQL, conn);
                rtnExecValue = (comm.ExecuteNonQuery() == 1) ? "O" : "X";
            } catch (Exception ex) {
                rtnExecValue = ex.Message;
            }
            return rtnExecValue;
        }

        public byte[] getFileToRead(string sendValue)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(sendValue);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length) throw new System.IO.IOException(sendValue);
            return data;
        }

    }
}