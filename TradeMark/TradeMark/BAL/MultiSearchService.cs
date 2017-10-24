using System;
using System.Collections.Generic;
using System.Data;
using TradeMark.DAL;
using TradeMark.Models;

namespace TradeMark.BAL
{

    public class MultiSearchService
    {
        MultiSearchProvider oProvider = new MultiSearchProvider();
        /// <summary>
        /// add record in BulkSearchRequest
        /// </summary>
        /// <param name="oBulkSearchRequest"></param>
        /// <returns></returns>
        public string AddBulkSearchRequest(BulkSearchRequestModel oBulkSearchRequest)
        {
            //var Id = 
            return oProvider.AddBulkSearchRequest(oBulkSearchRequest);
        }

        /// <summary>
        /// get the data from query
        /// </summary>
        /// <returns></returns>
        public DataTable GetRecordByQuery(string query)
        {
            DataTable dtTM = oProvider.GetRecordByQuery(query);
            return dtTM;

        }
      

        /// <summary>
        /// get all data of maerktobesearch table
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllMarkToSearch()
        {
            DataTable dtServices = oProvider.GetAllMarkToSearch();
            return dtServices;
        }
    }
}