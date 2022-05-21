using UserServices.Models;

namespace UserServices.GraphQL
{
    public record UserCreate
    (
   
         int? Id,
        string FullName,
         string Email,
          string UserName,
           string Password,
         List<UserRoleData> UserRoleDatas
    );
}
