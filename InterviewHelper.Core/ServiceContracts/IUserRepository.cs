using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts;

public interface IUserRepository
{
    User AddUser(User newUSer);

    void EditUserDetails(User user);



}