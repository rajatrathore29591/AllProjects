using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.DataModel;
using TV.CraveASAP.DataModel.UnitOfWork;

namespace TV.CraveASAP.BusinessServices.HelperClass
{
    public class ManageMultipleDevices
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ManageMultipleDevices()
        {
            _unitOfWork = new UnitOfWork();
        }


        public string InsertDeviceToken(string deviceToken, string devicePlatform, string entity, string id)
        {
            string token = string.Empty;
            if (entity == "UserEntity")
            {
                var user = _unitOfWork.UserRepository.GetByID(Convert.ToInt32(id));
                if (user != null)
                {
                    if (user.deviceToken != null && user.deviceToken != "" && user.deviceToken.Length > 25 && user.devicePlatform != null && user.devicePlatform != "")
                    {

                        List<string> listOfDevTkn = SplitCommaSeperatedString(user.deviceToken);
                        if (!listOfDevTkn.Contains(deviceToken))
                            listOfDevTkn.Insert(listOfDevTkn.Count, deviceToken);
                        token = CreateCommaSeperatedString(listOfDevTkn);
                    }
                    else
                    {
                        token = deviceToken;
                    }
                }
            }
            else
            {
                var vendor = _unitOfWork.VendorRepository.GetAll().Where(d => d.vendorId == Convert.ToInt32(id)).FirstOrDefault();
                if (vendor != null)
                {
                    if (vendor.deviceToken != null && vendor.deviceToken != "" && vendor.deviceToken.Length > 25 && vendor.devicePlatform != null && vendor.devicePlatform != "")
                    {
                        List<string> listOfDevTkn = SplitCommaSeperatedString(vendor.deviceToken);
                        if (!listOfDevTkn.Contains(deviceToken))
                            listOfDevTkn.Insert(listOfDevTkn.Count, deviceToken);
                        token = CreateCommaSeperatedString(listOfDevTkn);
                    }
                    else
                    {
                        token = deviceToken;
                    }
                }
            }
            return token;
        }

        public string InsertDevicePlt(string deviceToken, string devicePlatform, string entity, string id)
        {
            string token = string.Empty;
            if (entity == "UserEntity")
            {
                var user = _unitOfWork.UserRepository.GetByID(Convert.ToInt32(id));
                if (user != null)
                {
                    if (user.deviceToken != null && user.deviceToken != "" && user.deviceToken.Length > 25 && user.devicePlatform != null && user.devicePlatform != "")
                    {

                        List<string> listOfDevTkn = SplitCommaSeperatedString(user.deviceToken);
                        List<string> listOfDevPlt = SplitCommaSeperatedString(user.devicePlatform);
                        if (!listOfDevTkn.Contains(deviceToken))
                        {
                            listOfDevPlt.Insert(listOfDevPlt.Count, devicePlatform);
                        }
                        token = CreateCommaSeperatedString(listOfDevPlt);
                    }
                    else
                    {
                        token = devicePlatform;
                    }
                }
            }
            else
            {
                var vendor = _unitOfWork.VendorRepository.GetAll().Where(d => d.vendorId == Convert.ToInt32(id)).FirstOrDefault();
                if (vendor != null)
                {
                    if (vendor.deviceToken != null && vendor.deviceToken != "" && vendor.deviceToken.Length > 25 && vendor.devicePlatform != null && vendor.devicePlatform != "")
                    {
                        List<string> listOfDevTkn = SplitCommaSeperatedString(vendor.deviceToken);
                        List<string> listOfDevPlt = SplitCommaSeperatedString(vendor.devicePlatform);
                        if (!listOfDevTkn.Contains(deviceToken))
                        {
                            listOfDevPlt.Insert(listOfDevPlt.Count, devicePlatform);
                        }
                        token = CreateCommaSeperatedString(listOfDevPlt);
                    }
                    else
                    {
                        token = devicePlatform;
                    }
                }
            }
            return token;
        }

        public string DeleteDeviceToken(string deviceToken, string entity, string id)
        {
            string token = string.Empty;
            deviceToken = deviceToken.Replace(" ", "");
            if (entity == "UserEntity")
            {
                var user = _unitOfWork.UserRepository.GetByID(id);
                if (user != null)
                {
                    if (user.deviceToken != null)
                    {
                        List<string> listOfDevTkn = SplitCommaSeperatedString(user.deviceToken);
                        int IndexDevice = listOfDevTkn.FindIndex(a => a.Contains(deviceToken));
                        if (IndexDevice >= 0)
                        {
                            listOfDevTkn.RemoveAt(IndexDevice);
                        }
                        token = CreateCommaSeperatedString(listOfDevTkn);
                    }
                    else
                    {
                        token = deviceToken;
                    }
                }
            }
            else
            {
                var vendor = _unitOfWork.VendorRepository.GetAll().Where(d => d.vendorId == Convert.ToInt32(id)).FirstOrDefault();
                if (vendor != null)
                {
                    if (vendor.deviceToken != null)
                    {
                        List<string> listOfDevTkn = SplitCommaSeperatedString(vendor.deviceToken);
                        int IndexDevice = listOfDevTkn.FindIndex(a => a.Contains(deviceToken));
                        if (IndexDevice >= 0)
                        {
                            listOfDevTkn.RemoveAt(IndexDevice);
                        }
                        token = CreateCommaSeperatedString(listOfDevTkn);
                    }
                    else
                    {
                        token = deviceToken;
                    }
                }
            }
            return token;
        }

        public string DeleteDevicePlatform(string deviceToken, string entity, string id)
        {
            string token = string.Empty;
            deviceToken = deviceToken.Replace(" ", "");
            if (entity == "UserEntity")
            {
                var user = _unitOfWork.UserRepository.GetByID(id);
                if (user != null)
                {
                    if (user.deviceToken != null)
                    {
                        List<string> listOfDevTkn = SplitCommaSeperatedString(user.deviceToken);
                        List<string> listOfDevPlt = SplitCommaSeperatedString(user.devicePlatform);
                        int IndexDevice = listOfDevTkn.FindIndex(a => a.Contains(deviceToken));
                        if (IndexDevice >= 0)
                        {
                            listOfDevPlt.RemoveAt(IndexDevice);
                        }
                        token = CreateCommaSeperatedString(listOfDevPlt);
                    }
                    else
                    {
                        token = user.devicePlatform;
                    }
                }
            }
            else
            {
                var vendor = _unitOfWork.VendorRepository.GetAll().Where(d => d.vendorId == Convert.ToInt32(id)).FirstOrDefault();
                if (vendor != null)
                {
                    if (vendor.deviceToken != null)
                    {
                        List<string> listOfDevTkn = SplitCommaSeperatedString(vendor.deviceToken);
                        List<string> listOfDevPlt = SplitCommaSeperatedString(vendor.devicePlatform);
                        int IndexDevice = listOfDevTkn.FindIndex(a => a.Contains(deviceToken));
                        if (IndexDevice >= 0)
                        {
                            listOfDevPlt.RemoveAt(IndexDevice);
                        }
                        token = CreateCommaSeperatedString(listOfDevPlt);
                    }
                    else
                    {
                        token = vendor.devicePlatform;
                    }
                }
            }
            return token;
        }

        private List<string> SplitCommaSeperatedString(string tokens)
        {
            List<string> listOfDevTkn = new List<string>();
            if (tokens.Contains(','))
            {
                string[] splittedToken = tokens.Split(',');
                listOfDevTkn = splittedToken.ToList<string>();
            }
            else
            {
                listOfDevTkn.Insert(0, tokens);
            }
            return listOfDevTkn;
        }

        private string CreateCommaSeperatedString(List<string> listOfTokens)
        {
            string token = string.Empty;
            int i = 0;
            foreach (string tkn in listOfTokens)
            {
                if (i == 0)
                    token = tkn;
                else
                    token += "," + tkn;
                i++;
            }
            return token;
        }
    }
}
