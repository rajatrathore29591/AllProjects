using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TradeMark.App_Data;
using TradeMark.Models;

namespace TradeMark.DAL
{
    public class USClassProvider
    {
        /// <summary>
        /// Insert US class data in US class descriptions table
        /// </summary>
        /// <param name="oUSClass"></param>
        /// <returns>boolean result</returns>
        public bool AddUSClass(USClassModel oUSClass)
        {
            // SQL Data access process here, 
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            sqlcon.ConnectionString = connectionstring;
            try
            {               
                sqlcon.Open();
                sqlcom.Connection = sqlcon;
                //Put the sql stored procedure ........
                sqlcom.CommandText = "[usp_USClassDescriptions]";
                //add parametre   
                      
                sqlcom.Parameters.AddWithValue("@usClassDescription", oUSClass.USClassDescriptions);
                sqlcom.Parameters.AddWithValue("@classNo", oUSClass.ClassNo);
                sqlcom.CommandType = CommandType.StoredProcedure;
                sqlcom.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// US class is available or not
        /// </summary>
        /// <param name="usClassId"></param>
        /// <returns></returns>
        public bool CheckUSClassDescriptionAvailability(string usClassDescriptions)
        {
            var connectionstring = SqlHelper.GetConnectionString();
            SqlConnection sqlcon = new SqlConnection();

            sqlcon.ConnectionString = connectionstring;

            sqlcon.Open();
            SqlCommand cmd = new SqlCommand("select USClassDescription from USClassDescriptions where USClassDescription=@usClassDescription", sqlcon);
            cmd.Parameters.AddWithValue("@usClassDescription", usClassDescriptions);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}