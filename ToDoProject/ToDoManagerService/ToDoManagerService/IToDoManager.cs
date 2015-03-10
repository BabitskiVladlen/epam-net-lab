using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ToDoManagerService.Entities;

namespace ToDoManagerService
{
    [ServiceContract]
    public interface IToDoManager
    {
        [OperationContract]
        List<Entities.ToDoItem> GetTodoList(int userId);

        [OperationContract]
        void UpdateToDoItem(Entities.ToDoItem todo);

        [OperationContract]
        void CreateToDoItem(Entities.ToDoItem todo);

        [OperationContract]
        void DeleteToDoItem(int todoItemId);

        [OperationContract]
        int CreateUser(string name);
    }
}
