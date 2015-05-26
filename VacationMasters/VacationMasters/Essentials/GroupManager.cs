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

        public List<string> GetUserGroup(string userName)
        {
            return _dbWrapper.RunCommand(command =>
            {
                command.CommandText = string.Format("Select Name from Groups, ChooseGroups, Users where " +
                                                    "Groups.ID = ChooseGroups.IDGroup and " +
                                                    "ChooseGroups.IDUser = Users.ID and UserName = '{0}'; ",
                    userName);
          
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
