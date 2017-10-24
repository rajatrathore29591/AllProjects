using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeMark.DAL;
using System.Data.SqlClient;
using System.Data;

namespace TradeMark.BAL
{
    
    public class SearchService
    {
        /// <summary>
        /// Method to get trade marks with option first "Enter Mark"
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public DataTable GetTradeMark_EnterMark(string whereCondition)
        {
            SearchProvider oProvider = new SearchProvider();
            DataTable dtTM = oProvider.GetTradeMark_EnterMark(whereCondition);
            return dtTM;

        }
        /// <summary>
        /// get the data from multiple query
        /// </summary>
        /// <param name="whereCondition">Pass multiple query</param>
        /// <returns></returns>
        public DataSet GetTradeMark_EnterMarkDataSet(string  query)
        {
            SearchProvider oProvider = new SearchProvider();
            DataSet dtTM = oProvider.GetTradeMark_EnterMarkDataSet(query);
            return dtTM;

        }

        /// <summary>
        /// method to get associated us class ids for US-Class description id
        /// </summary>
        /// <param name="usClassDescriptionId"></param>
        /// <returns></returns>
        public DataTable GetAssociatedUsclassIds(int usClassDescriptionId)
        {
            SearchProvider oProvider = new SearchProvider();
            DataTable dtTM = oProvider.GetAssociatedUsclassIds(usClassDescriptionId);
            return dtTM;

        }

        /// <summary>
        /// method to get associated us class decriptions for US-Class description id
        /// </summary>
        /// <param name="usClassDescriptionId"></param>
        /// <returns></returns>
        public DataTable GetAssociatedUsclassDescriptions(int usClassDescriptionId , string near5)
        {
            SearchProvider oProvider = new SearchProvider();
            DataTable dtAssociatedUSDescriptions = oProvider.GetAssociatedUsclassDescriptions(usClassDescriptionId, near5);
            return dtAssociatedUSDescriptions;

        }
        public DataTable BindddlGoodsServices()
        {
            SearchProvider oProvider = new SearchProvider();
            DataTable dtGoodsServices = oProvider.BindddlGoodsServices();
            return dtGoodsServices;
        }
        /// <summary>
        /// Get the usclass description
        /// </summary>
        /// <param name="goodsServicesId=usclassdescriptionid"></param>
        /// <returns></returns>
        public string GetUSClassDescription(string goodsServicesId)
        {
            SearchProvider oProvider = new SearchProvider();
            string usClassDescription = oProvider.GetUsclassdescription(goodsServicesId);
            return usClassDescription;
        }

        /// <summary>
        /// Method to get the search parameters to show save search result
        /// </summary>
        /// <param name="guid">search Guid</param>
        /// <returns>Datatable</returns>
        public DataTable GetSeachParams(string guid)
        {
            SearchProvider oProvider = new SearchProvider();
            DataTable dtSeachParams = oProvider.GetSeachParams(guid);
            return dtSeachParams;

        }

        /// <summary>
        /// Method to get the search parameters to show save search result
        /// </summary>
        /// <param name="uSClassDescriptionId">uS Class Description Id</param>
        /// <returns>Datatable</returns>
        public string GetUsclassdescription(string uSClassDescriptionId)
        {
            SearchProvider oProvider = new SearchProvider();
            string description = oProvider.GetUsclassdescription(uSClassDescriptionId);
            return description;

        }

        /// <summary>
        /// Method to get the search log report for Admin
        /// </summary>
        /// <returns>Datatable</returns>
        public DataTable GetSearchLogReport(string startDate, string endDate)
        {
            SearchProvider oProvider = new SearchProvider();
            DataTable dtSearchLog = oProvider.GetSearchLogReport(startDate, endDate);
            return dtSearchLog;
        }

    }
}
