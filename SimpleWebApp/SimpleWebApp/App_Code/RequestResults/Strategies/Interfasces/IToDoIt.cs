
using System.Web;
namespace SimpleWebApp.RequestResults.Strategies.Interfasces
{
    public interface IToDoIt
    {
        void ToDoIt(HttpContext context, Routes? target, bool isAjax = true);
    }
}
