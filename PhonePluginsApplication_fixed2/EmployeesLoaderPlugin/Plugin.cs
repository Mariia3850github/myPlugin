using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneApp.Domain;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;

namespace EmployeesLoaderPlugin
{ 

  class myclass
  {
    public string Name { get; set; }
    public string Phone { get; set; }
  }
  [Author(Name = "Ivan Petrov")]
  public class Plugin : IPluggable
  {
    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
    {
      logger.Info("Loading employees");
      var employeesList = args.Cast<EmployeesDTO>().ToList();
      var tmp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<myclass>>(EmployeesLoaderPlugin.Properties.Resources.EmployeesJson);
      for (int i = 0; i < tmp.Count; ++i)
      {
        EmployeesDTO e = new EmployeesDTO();
        e.Name = tmp[i].Name;
        e.AddPhone(tmp[i].Phone);
        employeesList.Add(e);
      }
      logger.Info($"Loaded {employeesList.Count()} employees");
      return employeesList.Cast<DataTransferObject>();
    }
  }
}
