using FieldService.Models;
using RoverMob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService
{
    public static class Initializer
    {
        public static Application<Technician> LoadApplication()
        {
            var application = new Application<Technician>();
            application.Load(new Technician(Guid.NewGuid()));
            return application;
        }

        public static Application<Technician> LoadDesignModeApplication()
        {
            var application = new Application<Technician>();
            application.Load(new Technician(Guid.NewGuid()));
            return application;
        }
    }
}
