using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationMasters.Wrappers;

namespace VacationMasters.Essentials
{
    public class GroupManager
    {
        private readonly IDbWrapper _dbWrapper;
        public GroupManager(IDbWrapper dbWrapper)
        {
            _dbWrapper = dbWrapper;
        }

        public List<string> GetAllGroups()
        {
            return _dbWrapper.RunCommand(command =>
            {
                command.CommandText = "Select Name from Groups";
                var reader = command.ExecuteReader();
                var list = new List<string>();
                while (reader.Read())
                {
                    var name = reader.GetString(0);
                    list.Add(name);
                }
                return list;
            });
        }
    }
}
