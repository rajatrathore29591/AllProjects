using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeMark.App_Data;
using System.Data.SqlClient;
using System.Data;

namespace TradeMark.DAL
{
   public class SearchProvider
    {
        public DataTable GetTradeMark_EnterMark(string whereCondition)
        {
            
            string query = "select Top 10 SerialNo, Registrationno,Status,MarkIdentification,OwnerName,Casefilestatementstext from TradeMark where " + whereCondition + " Order by Registrationno desc, SerialNo desc";
            
            DataTable dtTM= SqlHelper.FillDataTable(query);
            return dtTM;
        }
        /// <summary>
        ///  /// <summary>
        /// get the data from multiple query
        /// </summary>
        /// <param name="whereCondition">Pass multiple query</param>
        /// <returns></returns>
       
        public DataSet GetTradeMark_EnterMarkDataSet(string query)
        {
            //string query = string.Empty;
            var connectionString = SqlHelper.GetConnectionString();
            //for (int i=0; i<whereCondition.Length; i++)
            //{
            //     query += "select Top 10 SerialNo, Registrationno,Status,MarkIdentification,OwnerName from TradeMark where " + whereCondition[i] + " Order by SerialNo desc ";
            //    query += " ";
            //}
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandTimeout = 0;
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);

                            return ds;
                        }
                       
                    }
                }
            }
            
        }
        public DataTable GetAssociatedUsclassIds(int usClassDescriptionId)
        {
            string query = "sp_AssociatedUSClassDescriptionIds  "+usClassDescriptionId;
            DataTable dtTM = SqlHelper.FillDataTable(query);
            return dtTM;
        }

        /// <summary>
        /// To get associated us class decriptions 
        /// </summary>
        /// <param name="usClassDescriptionId"></param>
        /// <returns></returns>
        public DataTable GetAssociatedUsclassDescriptions(int usClassDescriptionId, string near5)
        {
            string uspQuery = "usp_GetAssociatedUSClassDescriptions  " + usClassDescriptionId+",'"+ near5 + "'" ;
            DataTable dtAssociatedUSDescriptions = SqlHelper.FillDataTable(uspQuery);
            return dtAssociatedUSDescriptions;
        }

        /// <summary>
        /// method to return datatable with data to bind the auto fill drop down for goods and services
        /// </summary>
        /// <returns></returns>
        public DataTable BindddlGoodsServices()
        {
            string query = "select (convert(varchar,ClassNo)+','+convert(varchar,USClassDescriptionId)) as UsClassno, USClassDescription from USClassDescriptions";

            DataTable dtGoodsServices = SqlHelper.FillDataTable(query);
            return dtGoodsServices;
        }

        ///// <summary>
        ///// Get the usclass description
        ///// </summary>
        ///// <param name="goodsServicesId=usclassdescriptionid"></param>
        ///// <returns></returns>
        //public string GetUSClassDescription(string goodsServicesId)
        //{
        //   string query= "select USClassDescription  from USClassDescriptions where USClassDescriptionId =" + Convert.ToInt32(goodsServicesId);
           
        //    return query;
        //}

        /// <summary>
        /// Method to return Search params details 
        /// </summary>
        /// <param name="guid">string search guid</param>
        /// <returns>datatable</returns>
        /// 
        public DataTable GetSeachParams(string guid)
        {
            string query = "select * from SaveMarkSearch Where SearchGuid='"+ guid + "'";

            DataTable dtSeachParams = SqlHelper.FillDataTable(query);
            return dtSeachParams;
        }
        public string GetUsclassdescription(string uSClassDescriptionId)
        {
            string query = "Select USClassDescription from USClassDescriptions where USClassDescriptionId=" + uSClassDescriptionId + "";

            DataTable dtSeachParams = SqlHelper.FillDataTable(query);
            if(dtSeachParams!=null && dtSeachParams.Rows.Count>0)
            {
                return dtSeachParams.Rows[0]["USClassDescription"].ToString();
            }
            return "";
        }
        /// <summary>
        /// Method to get the search log report for Admin
        /// </summary>
        /// <returns>Datatable</returns>
        public DataTable GetSearchLogReport(string startDate, string endDate)
        {
            var endDatetime = string.Empty;
            var startDatetime = string.Empty;
            
            // check if startdate nad end date is null then take a current date and show result accordingly.
            if ((startDate == "null" || startDate == "") && (endDate == "null" || endDate == ""))
            {
                endDatetime = DateTime.Now.ToString("MM-dd-yyyy");
                startDatetime = DateTime.Now.AddDays(-30).ToString("MM-dd-yyyy");
            }
            else
            {
                //var  endDatetime1 = Convert.ToDateTime(endDate);
                //var  startDatetime1 = Convert.ToDateTime(startDate);

                endDatetime = endDate;
                //Convert.ToDateTime(endDate).ToString("MM-dd-yyyy"); ;
                startDatetime = startDate;
                    //Convert.ToDateTime(startDate).ToString("MM-dd-yyyy"); ;
            }
            string query = "Select * from Vw_GetSearchLogReport where convert(Date,SearchDate,101) between  convert(Date,\'" + startDatetime + "\',101) " + "AND convert(Date," + " \'" + endDatetime + "\',101) order by SearchDate desc";

            DataTable dtSearchLog = SqlHelper.FillDataTable(query);
            return dtSearchLog;
        }
    }
}
