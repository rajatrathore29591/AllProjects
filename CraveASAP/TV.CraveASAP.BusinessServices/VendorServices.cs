using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.CraveASAP.BusinessEntities;
using TV.CraveASAP.DataModel;
using TV.CraveASAP.DataModel.UnitOfWork;
using TV.CraveASAP.BusinessServices.Interfaces;
using AutoMapper;
using System.Transactions;
using TV.CraveASAP.BusinessServices.HelperClass;
using System.Net.Mail;
using System.Web.Hosting;
using System.IO;
using System.Threading;
using System.Device.Location;


namespace TV.CraveASAP.BusinessServices
{
    public class VendorServices : IVendorService
    {
        private readonly UnitOfWork _unitOfWork;
        MailMessage message = new MailMessage();
        string Subject = string.Empty;
        string Body = string.Empty;
        bool check;
        /// <summary>
        /// Public constructor.
        /// </summary>
        public VendorServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Fetches all the products.
        /// </summary>
        /// <returns></returns>
        /// 


        public IEnumerable<SuperAdminLoginEntity> SuperAdminLogin(string emailId, string password)
        {

            var adminInfo = _unitOfWork.SuperAdminLoginRepository.GetByCondition(d => d.loginName.ToUpper() == emailId.Trim().ToUpper() && d.password == password.Trim()).ToList();
            if (adminInfo.Any())
            {
                Mapper.CreateMap<SuperAdminLogin, SuperAdminLoginEntity>();
                var adminInfoModel = Mapper.Map<List<SuperAdminLogin>, List<SuperAdminLoginEntity>>(adminInfo);
                return adminInfoModel;
            }
            return null;
        }

        public IEnumerable<VendorEntity> VendorLogin(string emailId, string password, string deviceToken, string devicePlatform)
        {
            var VendorInfo = _unitOfWork.VendorRepository.GetByCondition(d => d.loginVendorName.ToUpper() == emailId.Trim().ToUpper() && Encrypt_decrypt.DESDecrypt(d.password) == password.Trim() && d.isDeleted == false).ToList();
            if (VendorInfo.Any())
            {
                if (devicePlatform == "ios" || devicePlatform == "android")
                {
                    ManageMultipleDevices manageMultipleDevive = new ManageMultipleDevices();
                    var check = manageMultipleDevive.InsertDeviceToken(deviceToken, devicePlatform, "VendorEntity", VendorInfo[0].vendorId.ToString());
                    var checkPlt = manageMultipleDevive.InsertDevicePlt(deviceToken, devicePlatform, "VendorEntity", VendorInfo[0].vendorId.ToString());
                    var vendor = _unitOfWork.VendorRepository.GetByCondition(d => d.loginVendorName.ToUpper() == emailId.Trim().ToUpper() && Encrypt_decrypt.DESDecrypt(d.password) == password.Trim() && d.isDeleted == false).FirstOrDefault();
                    if (vendor != null)
                    {
                        vendor.devicePlatform = checkPlt;
                        vendor.deviceToken = check;
                    }
                    _unitOfWork.VendorRepository.Update(vendor);
                    _unitOfWork.Save();
                }
                Mapper.CreateMap<Vendor, VendorEntity>();
                Mapper.CreateMap<PromotionCode, PromotionCodeEntity>();
                Mapper.CreateMap<OpeningHour, OpeningHourEntity>();
                var VendorInfoModel = Mapper.Map<List<Vendor>, List<VendorEntity>>(VendorInfo);
                return VendorInfoModel;
            }
            return null;
        }

        public bool VendorLogout(VendorEntity vendorEntity)
        {
            var success = false;
            if (vendorEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var vendor = _unitOfWork.VendorRepository.GetByID(vendorEntity.vendorId);

                    if (vendor != null)
                    {
                        ManageMultipleDevices manageMultipleDevive = new ManageMultipleDevices();
                        var check = manageMultipleDevive.DeleteDeviceToken(vendorEntity.deviceToken, "VendorEntity", vendor.vendorId.ToString());
                        var checkPlt = manageMultipleDevive.DeleteDevicePlatform(vendorEntity.deviceToken, "VendorEntity", vendor.vendorId.ToString());
                        if (check != null)
                        {
                            vendor.deviceToken = check;
                            vendor.devicePlatform = checkPlt;
                        }
                        _unitOfWork.VendorRepository.Update(vendor);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public bool ChangeVendorPassword(int vendorId, string password, string oldPassword)
        {
            var success = false;
            var VendorInfo = _unitOfWork.VendorRepository.GetByCondition(d => d.vendorId == vendorId).ToList();
            if (VendorInfo.Any())
            {
                if (Encrypt_decrypt.DESDecrypt(VendorInfo[0].password) == oldPassword)
                {
                    var vendor = _unitOfWork.VendorRepository.GetByID(vendorId);
                    if (vendor != null)
                    {
                        vendor.password = Encrypt_decrypt.DESEncrypt(password);
                    }
                    _unitOfWork.VendorRepository.Update(vendor);
                    _unitOfWork.Save();
                    success = true;
                }
            }
            return success;
        }

        public bool ChangeAdminPassword(int vendorId, string password, string oldPassword)
        {
            var success = false;
            var VendorInfo = _unitOfWork.SuperAdminLoginRepository.GetByCondition(d => d.loginId == vendorId).ToList();
            if (VendorInfo.Any())
            {
                if (VendorInfo[0].password == oldPassword)
                {
                    var vendor = _unitOfWork.SuperAdminLoginRepository.GetByID(vendorId);
                    if (vendor != null)
                    {
                        vendor.password = password;
                    }
                    _unitOfWork.SuperAdminLoginRepository.Update(vendor);
                    _unitOfWork.Save();
                    success = true;
                }
            }
            return success;
        }

        public bool ForgotVendorPassword(string email)
        {
            var success = false;
            var newPassword = EmailUtility.RandomString(6);

            var VendorInfo = _unitOfWork.VendorRepository.GetByCondition(d => d.email == email.Trim() && d.isDeleted == false).ToList();
            if (VendorInfo.Any())
            {
                string filePath = string.Empty;
                filePath = HostingEnvironment.MapPath("~/Pictures/");
                message = new MailMessage();
                string str = "";
                str = "FROM: info@eddee.it" + Environment.NewLine;
                str += "TO: " + email + Environment.NewLine;
                str += "SUBJECT: Your New Eddee Password" + Environment.NewLine;
                str += "Hi " + VendorInfo[0].loginVendorName + "," + Environment.NewLine + Environment.NewLine +
                    "It seems that you forgot your password, please find your new password below. If you want to set your own password, you can always login to the e-vendor app and set your own password there. " + Environment.NewLine + Environment.NewLine +
                    "User Name: " + VendorInfo[0].loginVendorName + Environment.NewLine + "New Password: " + newPassword + Environment.NewLine + Environment.NewLine + "If you need any help, please contact us at info@frontiersweden.com" + Environment.NewLine + "Sincerely" + Environment.NewLine +
                    "Vendor Support Team";
                if (File.Exists(@"C:\inetpub\wwwroot\eddee\Pictures\Email.txt"))
                {
                    File.Delete(@"C:\inetpub\wwwroot\eddee\Pictures\Email.txt");
                }
                string path = @"C:\inetpub\wwwroot\eddee\Pictures\Email.txt";
                File.AppendAllLines(path, new[] { str });
               
                FileInfo fi = new FileInfo(@"C:\inetpub\wwwroot\eddee\Pictures\Email.txt");
                fi.CopyTo(@"C:\inetpub\mailroot\Pickup\Email1.txt", true);
               
                    var vendor = _unitOfWork.VendorRepository.GetByID(VendorInfo[0].vendorId);
                    if (vendor != null)
                    {
                        VendorInfo[0].password = Encrypt_decrypt.DESEncrypt(newPassword);
                    }
                    _unitOfWork.VendorRepository.Update(vendor);
                    _unitOfWork.Save();
                    success = true;
            }
            return success;
        }

        public IEnumerable<VendorListEntity> GetAllVendors()
        {
            var vendors = _unitOfWork.VendorRepository.GetByCondition(d => d.isDeleted == false).OrderBy(x => x.companyName).ToList();
            if (vendors.Any())
            {
                Mapper.CreateMap<Vendor, VendorListEntity>();
                var vendorsModel = Mapper.Map<List<Vendor>, List<VendorListEntity>>(vendors);
                return vendorsModel;
            }
            return null;
        }

        public IEnumerable<VendorCategoryEntity> GetAllVendorByCategoryId(int categoryId, int userId, string userLocationLat, string userLocationLong)
        {
            List<VendorCategoryEntity> vendorCategoryEntity = new List<VendorCategoryEntity>();
            var vicinityVendor = onLocationChanged(Convert.ToDouble(userLocationLat), Convert.ToDouble(userLocationLong), 1);
            if (vicinityVendor != null)
            {
                var vendorCategory = _unitOfWork.VendorRepository.GetByCondition(d => d.businessCategory == categoryId).Join(vicinityVendor, p => p.vendorId, v => v.vendorId, (p, v) => new { p, v }).ToList();
                if (vendorCategory.Count() > 0)
                {
                    foreach (var Item in vendorCategory)
                    {
                        var favourite = _unitOfWork.UserFavoriteRepository.GetByCondition(f => f.userId == userId && f.vendorId == Item.v.vendorId).FirstOrDefault();
                        vendorCategoryEntity.Add(new VendorCategoryEntity()
                        {
                            vendorId = Item.v.vendorId,
                            loginVendorName = Item.v.loginVendorName,
                            companyName = Item.v.companyName,
                            businessCategory = Item.v.businessCategory,
                            shortDescription = Item.v.shortDescription,
                            fullDescription = Item.v.fullDescription,
                            email = Item.v.email,
                            phoneNo = Item.v.phoneNo,
                            contactPerson = Item.v.contactPerson,
                            contactPhoneNo = Item.v.contactPhoneNo,
                            contactEmail = Item.v.contactEmail,
                            streetName = Item.v.streetName,
                            postCode = Item.v.postCode,
                            buildingName = Item.v.buildingName,
                            floor = Item.v.floor,
                            area = Item.v.area,
                            city = Item.v.city,
                            latitude = Item.v.latitude,
                            longitude = Item.v.longitude,
                            password = Item.v.password,
                            taxId = Item.v.taxId,
                            logoImg = Item.v.logoImg,
                            deviceToken = Item.v.deviceToken,
                            isVendorActive = false,
                            favouriteId = favourite != null ? favourite.favoriteRestaurantId : 0,
                            isFavourite = favourite != null ? Convert.ToBoolean(favourite.isActive) : false,
                            fromday = fromday(Item.v.vendorId),
                            today = today(Item.v.vendorId),
                            fromtime = fromtime(Item.v.vendorId),
                            totime = totime(Item.v.vendorId),

                        });
                    }
                    return vendorCategoryEntity;
                }
            }
            return vendorCategoryEntity;
        }

        public IEnumerable<VendorEntity> onLocationChanged(double latCurrent, double lngCurrent, int typeFillter)
        {
            List<VendorEntity> VendorLists = new List<VendorEntity>();
            IEnumerable<Vendor> vendarlistfull = new List<Vendor>(_unitOfWork.VendorRepository.GetByCondition(d => d.isDeleted == false));


            if (vendarlistfull.Count() > 0)
            {
                foreach (var elementList in vendarlistfull)
                {
                    var latSave = elementList.latitude;
                    var longSave = elementList.longitude;
                    if (latSave != null && longSave != null)
                    {
                        VendorLists.Add(new VendorEntity()
                        {
                            vendorId = elementList.vendorId,
                            loginVendorName = elementList.companyName,
                            companyName = elementList.companyName,
                            businessCategory = elementList.businessCategory,
                            shortDescription = elementList.shortDescription,
                            fullDescription = elementList.fullDescription,
                            email = elementList.email,
                            phoneNo = elementList.phoneNo,
                            contactPerson = elementList.contactPerson,
                            contactPhoneNo = elementList.contactPhoneNo,
                            contactEmail = elementList.contactEmail,
                            streetName = elementList.streetName,
                            postCode = elementList.postCode,
                            buildingName = elementList.buildingName,
                            floor = elementList.floor,
                            area = elementList.area,
                            city = elementList.city,
                            longitude = elementList.longitude,
                            latitude = elementList.latitude,
                            password = elementList.password,
                            logoImg = elementList.logoImg,
                            Distance = distance(Convert.ToDouble(latSave), Convert.ToDouble(longSave), latCurrent, lngCurrent)
                        });
                    }
                }
            }
            if (typeFillter == 1)
            {
               // VendorLists = VendorLists.Where(x => x.Distance <= 2500.00).ToList();
                VendorLists = VendorLists.Where(x => x.Distance <= 10000.00).ToList();
            }
            var Shortlist = VendorLists.OrderBy(o => o.Distance).ThenBy(o => o.Distance).ToList();

            if (Shortlist.Any())
            {
                Mapper.CreateMap<Vendor, VendorEntity>();
                var vendorsModel = Mapper.Map<List<VendorEntity>, List<VendorEntity>>(Shortlist);
                return vendorsModel;
            }
            return null;
        }

        private double distance(double lat1, double lng1, double lat2, double lng2)
        {

            var firstCordinate = new GeoCoordinate(lat1, lng1);
            var secondCordinate = new GeoCoordinate(lat2, lng2);

            double distance = firstCordinate.GetDistanceTo(secondCordinate);
            return distance;
        }

        public string fromday(int id)
        {
            int i = 0;
            string day = string.Empty;
            var vendorOpeningHours = _unitOfWork.OpeningHourRepository.GetByCondition(x => x.fkvendorId == id).ToList();
            if (vendorOpeningHours != null)
            {
                foreach (var item in vendorOpeningHours)
                {
                    if (i == 0)
                    {
                        day += item.fromday.ToString();
                    }
                    else { day += "," + item.fromday.ToString(); }

                    i++;
                }
            }
            return day;
        }

        public string today(int id)
        {
            int i = 0;
            string day = string.Empty;
            var vendorOpeningHours = _unitOfWork.OpeningHourRepository.GetByCondition(x => x.fkvendorId == id).ToList();
            if (vendorOpeningHours != null)
            {
                foreach (var item in vendorOpeningHours)
                {
                    if (i == 0)
                    {
                        day += item.fromday.ToString();
                    }
                    else { day += "," + item.today.ToString(); }

                    i++;
                }
            }
            return day;
        }

        public string totime(int id)
        {
            int i = 0;
            string totime = string.Empty;
            var vendorOpeningHours = _unitOfWork.OpeningHourRepository.GetByCondition(x => x.fkvendorId == id).ToList();
            if (vendorOpeningHours != null)
            {
                foreach (var item in vendorOpeningHours)
                {
                    if (i == 0)
                    {
                        totime += item.totime.ToString();
                    }
                    else { totime += "," + item.totime.ToString(); }

                    i++;
                }
            }
            return totime;
        }

        public string fromtime(int id)
        {
            int i = 0;
            string fromtime = string.Empty;
            var vendorOpeningHours = _unitOfWork.OpeningHourRepository.GetByCondition(x => x.fkvendorId == id).ToList();
            if (vendorOpeningHours != null)
            {
                foreach (var item in vendorOpeningHours)
                {
                    if (i == 0)
                    {
                        fromtime += item.fromtime.ToString();
                    }
                    else { fromtime += "," + item.fromtime.ToString(); }

                    i++;
                }
            }
            return fromtime;
        }

        public IEnumerable<CategoryEntity> GetAllVendorCategory()
        {
            var category = _unitOfWork.CategoryRepository.GetAll().ToList();
            if (category.Any())
            {
                Mapper.CreateMap<Category, CategoryEntity>();
                Mapper.CreateMap<AppDefaultLandingPage, AppDefaultLandingPageEntity>();
                var categoryModel = Mapper.Map<List<Category>, List<CategoryEntity>>(category);
                return categoryModel;
            }
            return null;
        }

        /// <summary>
        /// Fetches all the products.
        /// </summary>
        /// <returns></returns>
        public VendorEntity GetVendorById(int ID)
        {
            var VendorInfo = _unitOfWork.VendorRepository.GetByCondition(d => d.vendorId == ID).FirstOrDefault();
            if (VendorInfo != null)
            {
                Mapper.CreateMap<Vendor, VendorEntity>().ForMember(d => d.password, o => o.MapFrom(s => s.password != null ? 1 : 0));
                Mapper.CreateMap<PromotionCode, PromotionCodeEntity>();
                Mapper.CreateMap<OpeningHour, OpeningHourEntity>();
                var VendorInfoModel = Mapper.Map<Vendor, VendorEntity>(VendorInfo);
                return VendorInfoModel;
            }
            return null;
        }

        /// <summary>
        /// Creates a product
        /// </summary>
        /// <param name="productEntity"></param>
        /// <returns></returns>
        public int CreateVendor(VendorEntity vendorEntity)
        {
            var id = 0;
            string[] fromday = null; string[] today = null; string[] fromtime = null; string[] totime = null;
            var vendorDetail = _unitOfWork.VendorRepository.GetAll().ToList();
            var number = 0;
            foreach (var item in vendorDetail)
            {
                if (item.loginVendorName == vendorEntity.loginVendorName.Trim())
                {
                    number = 1;
                }
                if (item.email == vendorEntity.email.Trim())
                {
                    number = 1;
                }

            }
            if (number == 0)
            {
                if (vendorEntity.fromday != null)
                {
                    fromday = vendorEntity.fromday.Split(','); today = vendorEntity.fromday.Split(','); fromtime = vendorEntity.fromtime.Split(','); totime = vendorEntity.totime.Split(',');
                }
                using (var scope = new TransactionScope())
                {
                    var vendor = new Vendor
                    {

                        loginVendorName = vendorEntity.loginVendorName != null ? vendorEntity.loginVendorName : "",
                        companyName = vendorEntity.companyName != null ? vendorEntity.companyName : "",
                        businessCategory = vendorEntity.businessCategory != 0 ? vendorEntity.businessCategory : 0,
                        shortDescription = vendorEntity.shortDescription != null ? vendorEntity.shortDescription : "",
                        fullDescription = vendorEntity.fullDescription != null ? vendorEntity.fullDescription : "",
                        email = vendorEntity.email != null ? vendorEntity.email : "",
                        phoneNo = vendorEntity.phoneNo != null ? vendorEntity.phoneNo : "",
                        contactPerson = vendorEntity.contactPerson != null ? vendorEntity.contactPerson : "",
                        contactPhoneNo = vendorEntity.contactPhoneNo != null ? vendorEntity.contactPhoneNo : "",
                        contactEmail = vendorEntity.contactEmail != null ? vendorEntity.contactEmail : "",
                        streetName = vendorEntity.streetName != null ? vendorEntity.streetName : "",
                        postCode = vendorEntity.postCode != null ? vendorEntity.postCode : "",
                        buildingName = vendorEntity.buildingName != null ? vendorEntity.buildingName : "",
                        floor = vendorEntity.floor != null ? vendorEntity.floor : "",
                        area = vendorEntity.area != null ? vendorEntity.area : "",
                        city = vendorEntity.city != null ? vendorEntity.city : "",
                        longitude = vendorEntity.longitude != null ? vendorEntity.longitude : "",
                        latitude = vendorEntity.latitude != null ? vendorEntity.latitude : "",
                        password = vendorEntity.password != null ? Encrypt_decrypt.DESEncrypt(vendorEntity.password) : "",
                        isDeleted = false
                    };

                    _unitOfWork.VendorRepository.Insert(vendor);
                    _unitOfWork.Save();
                    scope.Complete();

                    message = new MailMessage();
                    Subject = "Crave ASAP ";
                    Body = @"<table border='0' cellpadding='0' cellspacing='0'><tr><td>Hello  " + vendorEntity.companyName +
                                    "<br/><br/> Your  LoginId is <b>" + vendorEntity.loginVendorName + "</b><br/><br/>" +
                                    "<br/><br/> Your  password is <b>" + vendorEntity.password + "</b><br/><br/>" +
                                   "We are happy that you decided to join our successful community of Restaurant. Stay tuned for our valuable Updates!</br><br/></br><br/>Best regards,</br><br/>Crave ASAP Team </td></tr></table>";
                    message.Subject = Subject;
                    message.IsBodyHtml = true;
                    message.Body = Body;
                    message.To.Add(new MailAddress(vendorEntity.contactEmail));
                    message.From = new MailAddress("rajat.rathore@techvalens.com");
                    EmailUtility SendEmail = new EmailUtility();
                    SendEmailInBackgroundThread(message);
                    var vendorID = _unitOfWork.VendorRepository.GetByID(vendor.vendorId);
                    using (var scopes = new TransactionScope())
                    {

                        if (vendorEntity.logoImg != null)
                        {
                            vendor.logoImg = base64ToImage(vendorEntity.logoImg, vendorID.vendorId, vendorID.logoImg);
                            _unitOfWork.VendorRepository.Update(vendor);
                            _unitOfWork.Save();
                            scopes.Complete();
                        }
                    }
                    if (fromday != null)
                        if (fromday[0] != "All Day")
                        {
                            var i = 0;

                            using (var scopeTime = new TransactionScope())
                            {
                                foreach (var item in fromday)
                                {
                                    if (fromday[0].ToString() != "undefined")
                                    {
                                        var openingHour = new OpeningHour
                                        {
                                            fromday = fromday[i],
                                            today = today[i],
                                            fromtime = fromtime[i],
                                            totime = totime[i],
                                            fkvendorId = vendor.vendorId
                                        };
                                        _unitOfWork.OpeningHourRepository.Insert(openingHour);
                                        _unitOfWork.Save();

                                    }
                                    i = i + 1;
                                }
                                scopeTime.Complete();
                            }

                        }
                        else
                        {
                            using (var scopeTime = new TransactionScope())
                            {
                                var openingHour = new OpeningHour
                                {
                                    fromday = fromday[0],
                                    today = today[0],
                                    fromtime = fromtime[0],
                                    totime = totime[0],
                                    fkvendorId = vendor.vendorId
                                };
                                _unitOfWork.OpeningHourRepository.Insert(openingHour);
                                _unitOfWork.Save();
                                scopeTime.Complete();
                            }

                        }
                    return vendor.vendorId;
                }
            }
            return id;
        }

        public void SendEmailInBackgroundThread(MailMessage message)
        {
            EmailUtility SendEmail = new EmailUtility();
            Thread bgThread = new Thread(new ParameterizedThreadStart(SendEmail.SendMailToVendor));
            bgThread.IsBackground = true;
            bgThread.Start(message);
        }

        public string base64ToImage(string logImg, int MaxId, string oldImage)
        {
            string filePath = string.Empty;
            string image = string.Empty;
            var path = DateTime.Now;
            var data = String.Format("{0:d/M/yyyy HH:mm:ss}", path);
            data = data.Replace(@"/", "").Trim(); data = data.Replace(@":", "").Trim(); data = data.Replace(" ", String.Empty);
            if (!string.IsNullOrEmpty(logImg))
            {
                filePath = HostingEnvironment.MapPath("~/Pictures/");
                image = MaxId + "_VendorLogopic_" + data + ".png";
                if (File.Exists(filePath + oldImage))
                {
                    System.IO.File.Delete((filePath + oldImage));
                }
                byte[] bytes = System.Convert.FromBase64String(logImg);
                FileStream fs = new FileStream(filePath + image, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            return image;
        }

        public int CreateVendorBranch(VendorBranchEntity vendorBranchEntity)
        {
            using (var scope = new TransactionScope())
            {
                var vendorBranch = new VendorBranch
                {
                    businessName = vendorBranchEntity.businessName != null ? vendorBranchEntity.businessName : "",
                    taxId = vendorBranchEntity.taxId != null ? vendorBranchEntity.taxId : "",
                    location = vendorBranchEntity.location != null ? vendorBranchEntity.location : "",
                    fullAddress = vendorBranchEntity.fullAddress != null ? vendorBranchEntity.fullAddress : "",
                    deliveryAddress = vendorBranchEntity.deliveryAddress != null ? vendorBranchEntity.deliveryAddress : "",
                    phoneNo = vendorBranchEntity.phoneNo != null ? vendorBranchEntity.phoneNo : "",
                    contactPerson = vendorBranchEntity.contactPerson != null ? vendorBranchEntity.contactPerson : "",
                    additionalInfo = vendorBranchEntity.additionalInfo != null ? vendorBranchEntity.additionalInfo : "",
                    defaultBranch = vendorBranchEntity.defaultBranch != null ? vendorBranchEntity.defaultBranch : "",
                    email = vendorBranchEntity.email != null ? vendorBranchEntity.email : "",
                    vendorId = vendorBranchEntity.vendorId != null ? Convert.ToInt32(vendorBranchEntity.vendorId) : 0,
                    branchCode = vendorBranchEntity.branchCode != null ? vendorBranchEntity.branchCode : ""

                };
                _unitOfWork.VendorBranchRepository.Insert(vendorBranch);
                _unitOfWork.Save();
                scope.Complete();
                return vendorBranch.vendorBranchId;
            }
        }

        public string UpdateVendor(int vendorId, VendorEntity vendorEntity)
        {
            var success = false; string[] fromday = null; string[] today = null; string[] fromtime = null; string[] totime = null;
            var vendorDetail = _unitOfWork.VendorRepository.GetByCondition(d => d.vendorId != vendorEntity.vendorId).ToList();
            var number = 0;
            foreach (var item in vendorDetail)
            {
                if (item.loginVendorName == vendorEntity.loginVendorName.Trim())
                {
                    number = 1;
                }
                if (item.email == vendorEntity.email.Trim())
                {
                    number = 1;
                }

            }
            if (vendorEntity.password == null)
            {
                vendorEntity.password = "";
            }
            if (number == 0)
            {
                ResponseEntity response = new ResponseEntity();
                if (vendorEntity.fromday != null)
                {
                    fromday = vendorEntity.fromday.Split(','); today = vendorEntity.today.Split(','); fromtime = vendorEntity.fromtime.Split(','); totime = vendorEntity.totime.Split(',');
                }
                if (vendorEntity != null)
                {
                    using (var scope = new TransactionScope())
                    {
                        var opening = _unitOfWork.OpeningHourRepository.GetByCondition(v => v.fkvendorId == vendorId).ToList();
                        var vendor = _unitOfWork.VendorRepository.GetByID(vendorId);
                        if (vendor != null)
                        {
                            vendor.loginVendorName = vendorEntity.loginVendorName != null ? vendorEntity.loginVendorName : "";
                            vendor.companyName = vendorEntity.companyName != null ? vendorEntity.companyName : "";
                            vendor.businessCategory = vendorEntity.businessCategory != 0 ? Convert.ToInt32(vendorEntity.businessCategory) : 0;
                            vendor.shortDescription = vendorEntity.shortDescription != null ? vendorEntity.shortDescription : "";
                            vendor.fullDescription = vendorEntity.fullDescription != null ? vendorEntity.fullDescription : "";
                            vendor.email = vendorEntity.email != null ? vendorEntity.email : "";
                            vendor.phoneNo = vendorEntity.phoneNo != null ? vendorEntity.phoneNo : "";
                            vendor.contactPerson = vendorEntity.contactPerson != null ? vendorEntity.contactPerson : "";
                            vendor.contactPhoneNo = vendorEntity.contactPhoneNo != null ? vendorEntity.contactPhoneNo : "";
                            vendor.contactEmail = vendorEntity.contactEmail != null ? vendorEntity.contactEmail : "";
                            vendor.streetName = vendorEntity.streetName != null ? vendorEntity.streetName : "";
                            vendor.postCode = vendorEntity.postCode != null ? vendorEntity.postCode : "";
                            vendor.buildingName = vendorEntity.buildingName != null ? vendorEntity.buildingName : "";
                            vendor.floor = vendorEntity.floor != null ? vendorEntity.floor : "";
                            vendor.area = vendorEntity.area != null ? vendorEntity.area : "";
                            vendor.city = vendorEntity.city != null ? vendorEntity.city : "";
                            vendor.longitude = vendorEntity.longitude != null ? vendorEntity.longitude : "";
                            vendor.latitude = vendorEntity.latitude != null ? vendorEntity.latitude : "";
                            vendor.password = string.IsNullOrEmpty(vendorEntity.password) ? vendor.password : Encrypt_decrypt.DESEncrypt(vendorEntity.password);

                            _unitOfWork.VendorRepository.Update(vendor);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;


                            if (!string.IsNullOrEmpty(vendorEntity.logoImg))
                            {
                                using (var scopes = new TransactionScope())
                                {
                                    var vendorID = _unitOfWork.VendorRepository.GetByID(vendor.vendorId);
                                    if (vendorID != null)
                                    {
                                        vendor.logoImg = base64ToImage(vendorEntity.logoImg, vendorID.vendorId, vendorID.logoImg);
                                        _unitOfWork.VendorRepository.Update(vendor);
                                        _unitOfWork.Save();
                                        var vendor1 = _unitOfWork.VendorRepository.GetByID(vendorId);
                                        response.Image = vendor1.logoImg;
                                        scopes.Complete();
                                    }
                                }
                            }
                            else
                            {
                                var vendor1 = _unitOfWork.VendorRepository.GetByID(vendorId);
                                response.Image = vendor1.logoImg;
                            }

                        }
                        if (fromday != null)
                        {
                            using (var scopeTimeDel = new TransactionScope())
                            {
                                foreach (var item in opening)
                                {
                                    _unitOfWork.OpeningHourRepository.Delete(item.openingHoursId);
                                    _unitOfWork.Save();
                                }
                                scopeTimeDel.Complete();
                            }

                            using (var scopeTime = new TransactionScope())
                            {
                                int i = 0;

                                foreach (var item in fromday)
                                {
                                    if (fromday[i].ToString() != "null")
                                    {
                                        var openingHour = new OpeningHour
                                        {
                                            fromday = fromday[i],
                                            today = today[i],
                                            fromtime = fromtime[i],
                                            totime = totime[i],
                                            fkvendorId = vendorId
                                        };
                                        _unitOfWork.OpeningHourRepository.Insert(openingHour);
                                        _unitOfWork.Save();

                                    }
                                    i++;
                                }
                                scopeTime.Complete();
                            }

                        }
                    }
                }
                return response.Image;
            }
            return number.ToString();
        }

        public bool DeleteVendor(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var vendor = _unitOfWork.VendorRepository.GetByID(Id);
                    if (vendor != null)
                    {
                        vendor.isDeleted = true;
                        _unitOfWork.VendorRepository.Update(vendor);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
                using (var scope = new TransactionScope())
                {
                    var vendor = _unitOfWork.UserFavoriteRepository.GetByCondition(d => d.vendorId == Id).ToList();
                    foreach (var ven in vendor)
                    {
                        var deletedId = _unitOfWork.UserFavoriteRepository.GetByID(ven.favoriteRestaurantId);
                        if (deletedId != null)
                        {
                            _unitOfWork.UserFavoriteRepository.Delete(deletedId);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;
                        }
                    }
                }
            }
            return success;
        }

        public IEnumerable<PromotionCodeEntity> GetPromotionByVendorId(int Id)
        {
            DateTime UTCTime = System.DateTime.UtcNow;
            DateTime dates2 = UTCTime.AddHours(7.0);
            var promotionCodeMain = _unitOfWork.PromotionCodeRepository.GetByCondition(c => c.vendorId == Id && c.isDeleted == false).OrderByDescending(d => d.isActive).ThenBy(d => d.promotionCodeId).ToList(); 
            var promotionCode = promotionCodeMain.Where(d => d.isActive == true).ToList();
            promotionCode = promotionCode.OrderByDescending(d => d.promotionCodeId).ThenBy(d => d.promotionCodeId).ToList();
            var PinnedPromotionInfo = promotionCodeMain.Where(d => d.vendorId == Id && d.isActive == false && d.isDeleted == false && Convert.ToDateTime(d.createdDate).AddDays(45) > DateTime.Now).OrderByDescending(d => d.createdDate).OrderByDescending(d => d.isPinned).OrderByDescending(d => d.PinnedDate).ToList();
            promotionCode.AddRange(PinnedPromotionInfo);

            //changed logic to remove expired promtion from list start//

            var qtyPromo = promotionCode.Where(z => z.expiryDate == null).ToList(); var datePromo = promotionCode.Where(z => z.expiryDate != null && z.expiryDate >= dates2).ToList();
            promotionCode = null; promotionCode = qtyPromo; promotionCode.AddRange(datePromo);
            var promotionCodeMain1 = promotionCode;
            promotionCode = null; promotionCode = promotionCodeMain1.Where(d => d.isActive == true).ToList();
            promotionCode = promotionCode.OrderByDescending(d => d.promotionCodeId).ThenBy(d => d.promotionCodeId).ToList();
            var PinnedPromotionInfo1 = promotionCodeMain1.Where(d => d.vendorId == Id && d.isActive == false && d.isDeleted == false && Convert.ToDateTime(d.createdDate).AddDays(45) > DateTime.Now).OrderByDescending(d => d.createdDate).OrderByDescending(d => d.isPinned).OrderByDescending(d => d.PinnedDate).ToList();
            promotionCode.AddRange(PinnedPromotionInfo1);

            //changed logic to remove expired promtion from list End//

            var subCategory = _unitOfWork.SubCategoryRepository.GetAll().ToList();
            var optionalCategory = _unitOfWork.OptionalCategoryRepository.GetAll().ToList();
            var vendorImg = _unitOfWork.VendorRepository.GetByCondition(p => promotionCode.Select(v => v.vendorId).Contains(p.vendorId)).ToList();
            if (promotionCode.Any())
            {
                AutoMapper.Mapper.Reset();
                AutoMapper.Mapper.CreateMap<PromotionCode, PromotionCodeEntity>().ForMember(d => d.logoImg, o => o.ResolveUsing(s => s.vendorId == 0 ? "" : Convert.ToString(vendorImg.Where(z => z.vendorId.Equals(s.vendorId)).FirstOrDefault().logoImg)))
                    .ForMember(d => d.subCategoryname, o => o.ResolveUsing(s => s.subCategoryId == 0 ? "" : Convert.ToString(subCategory.Where(z => z.subCategoryId.Equals(s.subCategoryId)).FirstOrDefault().subCategoryName)))
                    .ForMember(d => d.optionalCategoryName, o => o.ResolveUsing(s => s.optCategoryId == 0 ? "" : Convert.ToString(optionalCategory.Where(z => z.optCategoryId.Equals(s.optCategoryId)).FirstOrDefault().optCategoryName)))
                    .ForMember(d => d.expiryDate, o => o.ResolveUsing(s => s.expiryDate == null ? "" : String.Format("{0:dd/MM/yyyy h:mm:ss tt}", s.expiryDate)))
                    .ForMember(d => d.createdDate, o => o.ResolveUsing(s => s.createdDate == null ? "" : String.Format("{0:dd/MM/yyyy h:mm:ss tt}", s.createdDate)))
                    .ForMember(d => d.expiryDate1, o => o.ResolveUsing(s => s.expiryDate == null ? "" : String.Format("{0:dd/MM/yyyy HH:mm:ss}", s.expiryDate)))
                    .ForMember(d => d.createdDate1, o => o.ResolveUsing(s => s.createdDate == null ? "" : String.Format("{0:dd/MM/yyyy HH:mm:ss}", s.createdDate)))
                    .ForMember(d => d.serverTime1, o => o.ResolveUsing(s => s.promotionCodeId == null ? "date" : String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.UtcNow.AddHours(7.0))))
                    .ForMember(d => d.serverTime, o => o.ResolveUsing(s => s.promotionCodeId == null ? "date" : String.Format("{0:dd/MM/yyyy h:mm:ss tt}", DateTime.UtcNow.AddHours(7.0))))
                    .ForMember(d => d.day, p => p.ResolveUsing(s => s.code != "" ? LeftTime(Convert.ToInt32(s.quantity), Convert.ToString(s.expiryDate), Convert.ToString(DateTime.UtcNow.AddHours(7.0))) : "Day"));
                var userPromotionModel = Mapper.Map<List<PromotionCode>, List<PromotionCodeEntity>>(promotionCode);
                AutoMapper.Mapper.Reset();
                return userPromotionModel;
            }
            return null;
        }

        //public IEnumerable<PromotionCodeEntity> GetPromotionByVendorId(int ID)
        //{
        //    List<PromotionCodeEntity> promotionsCodeEntity = new List<PromotionCodeEntity>();
        //    var PromotionInfoMain = _unitOfWork.AdminManageActivePromotionRepository.GetByCondition(d => d.vendorId == ID && d.isDeleted == false).OrderByDescending(d => d.isActive).ThenBy(d => d.promotionCodeId).ToList();
        //    var PromotionInfo = PromotionInfoMain.Where(d => d.isActive == true).ToList();
        //    PromotionInfo = PromotionInfo.OrderByDescending(d => d.promotionCodeId).ThenBy(d => d.promotionCodeId).ToList();
        //    if (PromotionInfo != null)
        //    {
        //        foreach (var Item in PromotionInfo)
        //        {
        //            DateTime UTCTime = System.DateTime.UtcNow;
        //            DateTime dates2 = UTCTime.AddHours(7.0);
        //            var subCategory = _unitOfWork.SubCategoryRepository.GetByID(Item.subCategoryId);
        //            var optionalCategory = _unitOfWork.OptionalCategoryRepository.GetByID(Item.optCategoryId);
        //            var vendorImg = _unitOfWork.VendorRepository.GetByID(Item.vendorId);
        //            promotionsCodeEntity.Add(new PromotionCodeEntity()
        //            {
        //                promotionCodeId = Item.promotionCodeId,
        //                code = Item.code,
        //                name = Item.name,
        //                descriptionEnglish = Item.descriptionEnglish,
        //                descriptionThai = Item.descriptionThai,
        //                createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.createdDate),
        //                expiryDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.expiryDate),
        //                isActive = Item.isActive,
        //                vendorId = Item.vendorId,
        //                quantity = Item.quantity != null ? Item.quantity : 0,
        //                recommendation = Item.recommendation,
        //                isPinned = Item.isPinned,
        //                categoryId = Item.categoryId,
        //                shareFacebookCount = Item.shareFacebookCount,
        //                shareInstagramCount = Item.shareInstagramCount,
        //                shareTwitterCount = Item.shareTwitterCount,
        //                price = Item.price,
        //                useCount = Item.useCount,
        //                viewCount = Item.viewCount,
        //                recommendationCount = Item.recommendationCount,
        //                promotionImage = Item.promotionImage,
        //                subCategoryId = Item.subCategoryId,
        //                optCategoryId = Item.optCategoryId,
        //                subCategoryname = subCategory != null ? subCategory.subCategoryName : "",
        //                optionalCategoryName = optionalCategory != null ? optionalCategory.optCategoryName : "",
        //                day = LeftTime(Convert.ToInt32(Item.quantity), Convert.ToString(Item.expiryDate), Convert.ToString(dates2)),
        //                logoImg = vendorImg.logoImg == null ? "" : vendorImg.logoImg,
        //                serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2)
        //            });
        //        }
        //        var PinnedPromotionInfo = PromotionInfoMain.Where(d => d.vendorId == ID && d.isActive == false && d.isDeleted == false && Convert.ToDateTime(d.createdDate).AddDays(45) > DateTime.Now).OrderByDescending(d => d.createdDate).OrderByDescending(d => d.isPinned).OrderByDescending(d => d.PinnedDate).ToList();
        //        foreach (var Item in PinnedPromotionInfo)
        //        {
        //            DateTime UTCTime = System.DateTime.UtcNow;
        //            DateTime dates2 = UTCTime.AddHours(7.0);
        //            var subCategory = _unitOfWork.SubCategoryRepository.GetByID(Item.subCategoryId);
        //            var optionalCategory = _unitOfWork.OptionalCategoryRepository.GetByID(Item.optCategoryId);
        //            var vendorImg = _unitOfWork.VendorRepository.GetByID(Item.vendorId);
        //            promotionsCodeEntity.Add(new PromotionCodeEntity()
        //            {
        //                promotionCodeId = Item.promotionCodeId,
        //                code = Item.code,
        //                name = Item.name,
        //                descriptionEnglish = Item.descriptionEnglish,
        //                descriptionThai = Item.descriptionThai,
        //                createdDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.createdDate),
        //                expiryDate = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", Item.expiryDate),
        //                isActive = Item.isActive,
        //                vendorId = Item.vendorId,
        //                quantity = Item.quantity != null ? Item.quantity : 0,
        //                recommendation = Item.recommendation,
        //                isPinned = Item.isPinned,
        //                categoryId = Item.categoryId,
        //                shareFacebookCount = Item.shareFacebookCount,
        //                shareInstagramCount = Item.shareInstagramCount,
        //                shareTwitterCount = Item.shareTwitterCount,
        //                price = Item.price,
        //                useCount = Item.useCount,
        //                viewCount = Item.viewCount,
        //                recommendationCount = Item.recommendationCount,
        //                promotionImage = Item.promotionImage,
        //                subCategoryId = Item.subCategoryId,
        //                optCategoryId = Item.optCategoryId,
        //                subCategoryname = subCategory != null ? subCategory.subCategoryName : "",
        //                optionalCategoryName = optionalCategory != null ? optionalCategory.optCategoryName : "",
        //                logoImg = vendorImg.logoImg == null ? "" : vendorImg.logoImg,
        //                serverTime = String.Format("{0:dd/MM/yyyy h:mm:ss tt}", dates2),
        //                day = LeftTime(Convert.ToInt32(Item.quantity), Convert.ToString(Item.expiryDate), Convert.ToString(dates2)),
        //            });
        //        }
        //        return promotionsCodeEntity;
        //    }
        //    return promotionsCodeEntity;
        //}


        public string LeftTime(int qty, string expiryDate,string serverTime)
        {
            string returndata = string.Empty;
            if (qty == 0 && expiryDate != "")
            {
                TimeSpan ts = Convert.ToDateTime(expiryDate) - Convert.ToDateTime(serverTime);
                if (ts.Days == 0) { returndata = Convert.ToString(ts.Hours) + "h" + ":" + Convert.ToString(ts.Minutes) + "m"; }
                else if (ts.Days > 0 && ts.Minutes > 0) { returndata = Convert.ToString(ts.Days + 1); }
                else { returndata = Convert.ToString(ts.Days); }

            }
            else { returndata = Convert.ToString(qty); }
            return returndata;
        }

        public IEnumerable<CategoryMapEntity> GetAllCategoryByCategoryId(int ID)
        {
            List<CategoryMapEntity> categoryList = new List<CategoryMapEntity>();
            var category = _unitOfWork.CategoryRepository.GetByCondition(d => d.categoryId == ID).ToList();
            var subCategory = _unitOfWork.SubCategoryRepository.GetByCondition(d => d.categoryId == ID && d.isDeleted == false).OrderBy(s => s.subCategoryName).ThenBy(s => s.subCategoryName).ToList();
            var optCategory = _unitOfWork.OptionalCategoryRepository.GetByCondition(d => d.categoryId == ID && d.isDeleted == false).ToList();

            List<SubCategoryEntity> subCategoryList = new List<SubCategoryEntity>();
            List<OptionalCategoryEntity> optCategoryList = new List<OptionalCategoryEntity>();
            foreach (var cat in category)
            {
                foreach (var sub in subCategory)
                {
                    subCategoryList.Add(new SubCategoryEntity()
                    {
                        subCategoryId = sub.subCategoryId,
                        subCategoryName = sub.subCategoryName,
                        language = sub.language,
                        description = sub.description

                    });
                }
                foreach (var opt in optCategory)
                {
                    optCategoryList.Add(new OptionalCategoryEntity()
                    {
                        optCategoryId = opt.optCategoryId,
                        optCategoryName = opt.optCategoryName,
                        language = opt.language,
                        description = opt.description
                    });
                }

                categoryList.Add(new CategoryMapEntity()
                {
                    categoryId = cat.categoryId,
                    categoryName = cat.categoryName,
                    description = cat.description,
                    language = cat.language,
                    SubCategories = subCategoryList,
                    OptionalCategories = optCategoryList

                });
            }
            return categoryList;
            //if (category != null)
            //{
            //    Mapper.CreateMap<Category, CategoryMapEntity>();
            //    Mapper.CreateMap<SubCategory, SubCategoryEntity>().ForMember(d => d.isDeleted,o => o.MapFrom(s => s.isDeleted == false));
            //    Mapper.CreateMap<OptionalCategory, OptionalCategoryEntity>();
            //    var CategoryInfoModel = Mapper.Map<List<Category>, List<CategoryMapEntity>>(category);
            //    return CategoryInfoModel;
            //}
            //return null;
        }

        public SubCategoryEntity GetSubCategoryById(int Id)
        {
            var subCategory = _unitOfWork.SubCategoryRepository.GetByCondition(d => d.subCategoryId == Id && d.isDeleted == false).FirstOrDefault();
            if (subCategory != null)
            {
                Mapper.CreateMap<SubCategory, SubCategoryEntity>();
                var subCategoryModel = Mapper.Map<SubCategory, SubCategoryEntity>(subCategory);
                return subCategoryModel;
            }
            return null;
        }

        public int AddSubCategory(SubCategoryEntity subCategoryEntity)
        {
            using (var scope = new TransactionScope())
            {
                var subCategory = new SubCategory
                {
                    subCategoryName = subCategoryEntity.subCategoryName != null ? subCategoryEntity.subCategoryName : "",
                    categoryId = subCategoryEntity.categoryId != null ? subCategoryEntity.categoryId : 0,
                    description = subCategoryEntity.description != null ? subCategoryEntity.description : "",
                    language = subCategoryEntity.language != null ? subCategoryEntity.language : "English",
                    isDeleted = false
                };
                _unitOfWork.SubCategoryRepository.Insert(subCategory);
                _unitOfWork.Save();
                scope.Complete();
                return subCategory.subCategoryId;
            }
        }

        public bool UpdateSubCategory(SubCategoryEntity subCategoryEntity)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var subCategory = _unitOfWork.SubCategoryRepository.GetByID(subCategoryEntity.subCategoryId);
                if (subCategory != null)
                {
                    subCategory.subCategoryName = subCategoryEntity.subCategoryName == null ? "" : subCategoryEntity.subCategoryName;
                    subCategory.categoryId = subCategoryEntity.categoryId;
                    subCategory.description = subCategoryEntity.description == null ? "" : subCategoryEntity.description;
                    subCategory.language = subCategoryEntity.language == null ? "English" : subCategoryEntity.language;
                }
                _unitOfWork.SubCategoryRepository.Update(subCategory);
                _unitOfWork.Save();
                scope.Complete();
                success = true;
            }
            return success;
        }


        public bool DeleteSubCategory(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var category = _unitOfWork.SubCategoryRepository.GetByID(Id);
                    if (category != null)
                    {
                        category.isDeleted = true;
                        _unitOfWork.SubCategoryRepository.Update(category);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
                using (var scope = new TransactionScope())
                {
                    var category = _unitOfWork.UserPrefrenceRepository.GetByCondition(d => d.prefrencesId == Id && d.type == "sub").ToList();
                    foreach (var cat in category)
                    {
                        var deletedId = _unitOfWork.UserPrefrenceRepository.GetByID(cat.id);
                        if (deletedId != null)
                        {
                            _unitOfWork.UserPrefrenceRepository.Delete(deletedId);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;
                        }
                    }
                }
            }
            return success;
        }

        public OptionalCategoryEntity GetOptCategoryById(int Id)
        {
            var optCategory = _unitOfWork.OptionalCategoryRepository.GetByCondition(d => d.optCategoryId == Id && d.isDeleted == false).FirstOrDefault();
            if (optCategory != null)
            {
                Mapper.CreateMap<OptionalCategory, OptionalCategoryEntity>();
                var optCategoryModel = Mapper.Map<OptionalCategory, OptionalCategoryEntity>(optCategory);
                return optCategoryModel;
            }
            return null;
        }

        public int AddOptionalCategory(OptionalCategoryEntity optCategoryEntity)
        {
            using (var scope = new TransactionScope())
            {
                var optCategory = new OptionalCategory
                {
                    optCategoryName = optCategoryEntity.optCategoryName != null ? optCategoryEntity.optCategoryName : "",
                    categoryId = optCategoryEntity.categoryId != null ? optCategoryEntity.categoryId : 0,
                    subCategoryId = optCategoryEntity.subCategoryId != null ? optCategoryEntity.subCategoryId : 0,
                    description = optCategoryEntity.description != null ? optCategoryEntity.description : "",
                    language = optCategoryEntity.language != null ? optCategoryEntity.language : "English",
                    isDeleted = false
                };
                _unitOfWork.OptionalCategoryRepository.Insert(optCategory);
                _unitOfWork.Save();
                scope.Complete();
                return optCategory.optCategoryId;
            }
        }

        public bool UpdateOptCategory(OptionalCategoryEntity optCategoryEntity)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var optCategory = _unitOfWork.OptionalCategoryRepository.GetByID(optCategoryEntity.optCategoryId);
                if (optCategory != null)
                {
                    optCategory.optCategoryName = optCategoryEntity.optCategoryName == null ? "" : optCategoryEntity.optCategoryName;
                    optCategory.categoryId = optCategoryEntity.categoryId;
                    optCategory.subCategoryId = optCategoryEntity.subCategoryId == null ? 0 : optCategoryEntity.subCategoryId;
                    optCategory.description = optCategoryEntity.description == null ? "" : optCategoryEntity.description;
                    optCategory.language = optCategoryEntity.language == null ? "English" : optCategoryEntity.language;
                }
                _unitOfWork.OptionalCategoryRepository.Update(optCategory);
                _unitOfWork.Save();
                scope.Complete();
                success = true;
            }
            return success;
        }

        public bool DeleteOptCategory(int Id)
        {
            var success = false;
            if (Id > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var optCategory = _unitOfWork.OptionalCategoryRepository.GetByID(Id);
                    if (optCategory != null)
                    {
                        optCategory.isDeleted = true;
                        _unitOfWork.OptionalCategoryRepository.Update(optCategory);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
                using (var scope = new TransactionScope())
                {
                    var category = _unitOfWork.UserPrefrenceRepository.GetByCondition(d => d.prefrencesId == Id && d.type == "opt").ToList();
                    foreach (var cat in category)
                    {
                        var deletedId = _unitOfWork.UserPrefrenceRepository.GetByID(cat.id);
                        if (deletedId != null)
                        {
                            _unitOfWork.UserPrefrenceRepository.Delete(deletedId);
                            _unitOfWork.Save();
                            scope.Complete();
                            success = true;
                        }
                    }
                }
            }
            return success;
        }

        public IEnumerable<UserFavouriteEntity> GetRestaurants()
        {
            var category = _unitOfWork.VendorRepository.GetByCondition(d => d.isDeleted == false).OrderBy(x => x.companyName).ToList();
            List<UserFavouriteEntity> Restaurant = new List<UserFavouriteEntity>();
            foreach (var item in category)
            {
                Restaurant.Add(new UserFavouriteEntity
                {
                    vendorId = item.vendorId,
                    companyName = item.companyName
                });
            }
            return Restaurant;
        }

        public IEnumerable<ContentManagementEntity> GetAllVideoWebApp()
        {
            var ContentMgtInfo = _unitOfWork.ContentMgtRepository.GetByCondition(d => d.isActive == true).ToList();
            if (ContentMgtInfo != null)
            {
                Mapper.CreateMap<ContentManagement, ContentManagementEntity>();
                var ContentMgtModel = Mapper.Map<List<ContentManagement>, List<ContentManagementEntity>>(ContentMgtInfo);
                return ContentMgtModel;
            }
            return null;
        }
    }
}
