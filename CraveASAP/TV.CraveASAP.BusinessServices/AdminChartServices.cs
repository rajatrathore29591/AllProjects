using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Hosting;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.BusinessServices.Interfaces;
using TV.CraveASAP.DataModel;
using TV.CraveASAP.DataModel.UnitOfWork;
using System.Web.Script.Serialization;

namespace TV.CraveASAP.BusinessServices
{
    public class AdminChartServices : IAdminChartServices
    {
        //string connString = "data source=204.93.197.136;initial catalog=CraveASAPDB; User ID=valens1_mm26;Password=techvalens@123;";
        string connString = "data source=EDDEEVM;initial catalog=DB_9D9221_eddee; User ID=eddeedb;Password=eddee##85;";
        private readonly UnitOfWork _unitOfWork;
        public AdminChartServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public string GetAllChart()
        {
            
            string JsonData = DataTableToJsonObj(GetStoreProcByID("usp_Analytics", "0", "All", "N", DateTime.Now, DateTime.Now));
            return JsonData;
        }

        public string GetAllChartByVendorID(string id, string fromDate, string toDate)
        {
            string JsonData = DataTableToJsonObj(GetStoreProc("usp_AnalyticsPromotion"));
            return JsonData;
        }

        public string GetAllChartByTime(string id, string CategoryType, string time, string fromDate, string toDate)
        {
            return DataTableToJsonObj(GetStoreProcByID("usp_Analytics", id, CategoryType, time, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate)));
        }

        public string GetTrackUsage()
        {
            return DataTableToJsonObj(GetStoreProc("usp_AnalyticsTrackUsage"));
        }
        public string GetMultipleUsage()
        {
            return DataTableToJsonObj(GetStoreProc("usp_AnalyticsUsagePerDay"));
        }

        public string GetSocialMedia()
        {
            return DataTableToJsonObj(GetStoreProc("usp_AnalyticsSocialMedia"));
        }

        public string GetUserPerDayUse()
        {
            return DataTableToJsonObj(GetStoreProc("usp_AnalyticsUserPerDayUseCategory"));
        }

        public DataTable GetStoreProc(string procName)
        {
            DataTable dt = new DataTable();

            string sql = procName;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = new SqlCommand(sql, conn);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        DataSet ds = new DataSet();
                        da.Fill(ds, "result_name");

                        dt = ds.Tables["result_name"];
                        string JSONresult;
                        JSONresult = DataTableToJsonObj(dt);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            return dt;
        }

        public DataTable GetStoreProcVendorId(string procName)
        {
            DataTable dt = new DataTable();

            string sql = procName;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = new SqlCommand(sql, conn);
                        da.SelectCommand.Parameters.Add("@vendorId", SqlDbType.Int).Value = 0;
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        DataSet ds = new DataSet();
                        da.Fill(ds, "result_name");

                        dt = ds.Tables["result_name"];
                        string JSONresult;
                        JSONresult = DataTableToJsonObj(dt);

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            return dt;
        }

        public DataTable GetStoreProcByID(string procName, string id, string CategoryType, string time, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            string sql = "EXEC " + procName + " @vendorId=" + id + ", @CategoryType='" + CategoryType + "', @time='" + @time + "', @fromDate='" + fromDate + "', @toDate ='" + toDate + "'";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = new SqlCommand(sql, conn);

                        DataSet ds = new DataSet();
                        da.Fill(ds, "result_name");
                        dt = ds.Tables["result_name"];
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine("Error: " + e.Message);
                }
            }
            return dt;
        }

        public string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }

    }
}