using System.Collections.Generic;
namespace BLL.Security.Infrastructure
{
    public interface IValidation
    {
        bool IsValid(string str, List<string> errors);
    }
}
