using BorgCivil.Framework.Entities;
using BorgCivil.Utils.Models;
using System;
using System.Collections.Generic;

namespace BorgCivil.Service
{
    public interface IDemoService : IService
    {
        List<Demo> GetAllDemo();
        Demo GetDemoByDemoId(Guid DemoId);
        bool AddDemo(DemoModel demoModel);
        bool EditDemo(DemoModel demoModel);
        bool DeleteDemo(Guid DemoId);
    }
}
