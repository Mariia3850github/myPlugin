using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PhoneApp.Domain;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;

namespace myPlugin
{
    [Author(Name = "Ivan Petrov")]
    public class Plugin : IPluggable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
        {
            var employeesList = args.Cast<EmployeesDTO>().ToList();
            try
            {
                long len = new System.IO.FileInfo("users.json").Length;
                char[] json = new char[len];

                StreamReader r = new StreamReader("users.json");
                r.ReadBlock(json, 0, (int)len);
                string tmp = "";
                bool st = false;
                string u_Name = "";
                for (int i = 0; i < json.Length; ++i)
                {
                    if (!st && (json[i] == '{' || json[i] == ',' || json[i] == '[' || json[i] == '}' || json[i] == ']'))
                    {
                        if (tmp.Contains("firstName:"))
                        {
                            tmp = tmp.Substring("firstName:".Length);
                            u_Name = tmp + ' ';
                        }
                        else if (tmp.Contains("lastName:"))
                        {
                            tmp = tmp.Substring("lastName:".Length);
                            u_Name = u_Name + tmp;
                        }
                        else if (tmp.Contains("phone:"))
                        {
                            tmp = tmp.Substring("phone:".Length);
                            EmployeesDTO employee = new EmployeesDTO();
                            employee.Name = u_Name;
                            employee.AddPhone(tmp);
                            employeesList.Add(employee);
                            u_Name = "";
                        }
                        tmp = "";
                    }
                    else
                    {
                        if (json[i] != '\"')
                            tmp = tmp + json[i];
                    }

                    if (json[i] == '\"')
                    {
                        st = !st;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
            return employeesList.Cast<DataTransferObject>();
        }
    }
}
