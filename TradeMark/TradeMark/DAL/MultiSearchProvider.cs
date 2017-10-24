using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradeMark.Models;
using TradeMark.App_Data;
using System.Data.SqlClient;
using System.Data;
using TradeMark.Utility;
using System.Configuration;

namespace TradeMark.DAL
{
    public class MultiSearchProvider
    {
        /// <summary>
        /// Insert bulk search request data 
        /// </summary>
        /// <param name="oBulkSearchRequest"></param>
        /// <returns></returns>
        public string AddBulkSearchRequest(BulkSearchRequestModel oBulkSearchRequest)
        {
            // SQL Data access process here, 
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            try
            {
                Guid userId;
                sqlcon.Open();

                sqlcom.Connection = sqlcon;
                //Put the sql stored procedure ........
                sqlcom.CommandText = "[usp_SaveBulkSearchRequest]";
                //add parametre    
                sqlcom.Parameters.AddWithValue("@userId",new Guid(oBulkSearchRequest.UserId));
                sqlcom.Parameters.AddWithValue("@email", oBulkSearchRequest.Email);
                sqlcom.Parameters.AddWithValue("@status", oBulkSearchRequest.Status);
                sqlcom.Parameters.AddWithValue("@excelName", oBulkSearchRequest.ExcelName);
                sqlcom.Parameters.Add("@requestId", SqlDbType.BigInt);
                sqlcom.Parameters["@requestId"].Direction = ParameterDirection.Output;
                sqlcom.CommandType = CommandType.StoredProcedure;
                sqlcom.ExecuteNonQuery();
                sqlcon.Close();
                var Id = sqlcom.Parameters["@requestId"].Value.ToString();

                return Id;
            }
            catch (Exception ex)
            {
                return "something went wrong";
            }



        }

        /// <summary>
        /// method to return datatable with filtered data 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetRecordByQuery(string query)
        {
            DataTable dtTM = SqlHelper.FillDataTable(query);
            return dtTM;
        }

        /// <summary>
        /// method to return datatable with all data of table
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllMarkToSearch()
        {
            string query = "select distinct RequestId,GoodsService from MarkToBeSearch order by RequestId asc";

            DataTable dtServices = SqlHelper.FillDataTable(query);
            return dtServices;
        }

    }

}
