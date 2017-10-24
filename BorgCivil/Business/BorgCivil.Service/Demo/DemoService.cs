using BorgCivil.Framework.Entities;
using BorgCivil.Repositories;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BorgCivil.Service
{
    public class DemoService : IDemoService
    {
        #region Dependencies Injection with initialization

        //Initialized interface object. 
        IUnitOfWork unitOfWork;

        //Initialized Parameterized Constructor.
        public DemoService(IUnitOfWork _unitOfWork) { unitOfWork = _unitOfWork; }

        #endregion

        #region DemoService

        /// <summary>
        /// getting all demo from demo table
        /// </summary>
        /// <returns></returns>
        public List<Demo> GetAllDemo()
        {
            try
            {
                return unitOfWork.DemoRepository.GetAll<Demo>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// this method is used for getting demo detail by demoId
        /// </summary>
        /// <param name="DemoId"></param>
        /// <returns></returns>
        public Demo GetDemoByDemoId(Guid DemoId)
        {
            try
            {
                //map entity.
                return unitOfWork.DemoRepository.GetById<Demo>(DemoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        public bool AddDemo(DemoModel demoModel)
        {
            try
            {
                //map entity.
                Demo entity = new Demo()
                {
                    DemoId = Guid.NewGuid(),
                    Name = demoModel.Name,
                    Address = demoModel.Address,
                    CurrentDate = demoModel.CurrentDate == null ? DateTime.UtcNow : demoModel.CurrentDate,
                    RadioGender = demoModel.RadioGender,
                    CheckBoxGender = demoModel.CheckBoxGender,
                    DropDownGender = demoModel.DropDownGender

                };
                //add record from in database.
                unitOfWork.DemoRepository.Insert<Demo>(entity);
                ////save changes in database.
                unitOfWork.DemoRepository.Commit();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EditDemo(DemoModel demoModel)
        {
            try
            {
                //get existing record.
                Demo entity = GetDemoByDemoId(demoModel.DemoId);

                //check entity is null.
                if (entity != null)
                {
                    //map entity
                    entity.Name = demoModel.Name;
                    entity.Address = demoModel.Address;
                    entity.CurrentDate = DateTime.UtcNow;
                    entity.RadioGender = demoModel.RadioGender;
                    entity.CheckBoxGender = demoModel.CheckBoxGender;
                    entity.DropDownGender = demoModel.DropDownGender;

                    //update record from existing entity in database.
                    unitOfWork.DemoRepository.Update<Demo>(entity);
                    ////save changes in database.
                    unitOfWork.DemoRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool DeleteDemo(Guid DemoId)
        {
            try
            {
                //get existing record.
                Demo entity = GetDemoByDemoId(DemoId);
                //check entity is null.
                if (entity != null)
                {
                    //delete record from existing entity in database.
                    unitOfWork.DemoRepository.Delete<Demo>(entity);
                    ////save changes in database.
                    unitOfWork.DemoRepository.Commit();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
    }
}
