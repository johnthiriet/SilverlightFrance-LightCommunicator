using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightCommunicator.App
{
    internal sealed class ApplicationContext
    {
        private static readonly ApplicationContext _context = new ApplicationContext();

        private ApplicationContext()
        {
        }

        public static ApplicationContext Instance
        {
            get
            {
                return _context;
            }
        }

        public bool IsLogged { get; set; }

        public string Login { get; set; }
    }
}
