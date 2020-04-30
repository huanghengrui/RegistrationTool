using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegistrationTool.Model;

namespace RegistrationTool.Data
{
  public  class DataGridDataInit
    {
        private static DataGridDataInit dataInit;

        public static DataGridDataInit Instance
        {
            get
            {
                if (dataInit == null)
                    dataInit = new DataGridDataInit();
                return dataInit;
            }
        }

        private DataGridDataInit()
        {
            RegistrationList = new List<RegistrationData>();
        }
        public List<RegistrationData> RegistrationList { get; set; }
    }
}
