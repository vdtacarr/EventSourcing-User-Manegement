using EventSourcing.Api.Entities;
using EventSourcing.Api.Events;
using EventSourcing.Api.Exception;

namespace EventSourcing.Api.Aggregate
{
    public class UserAggregate : Aggregate
    {
        //Kullanıcı oluşturulduğunda
        public void Created(User model)
        {
            if (CheckStreamName())
                throw new StreamNotFoundException();

            UserCreated userCreated = new()
            {
                UserId = model.Id,
                Email = model.Email,
                EmailApprove = model.EmailApprove,
                Name = model.Name,
                UserName = model.UserName
            };
            events.Add(userCreated);
        }
        //Kullanıcı adı değiştirildiğinde
        public void NameChanged(string newName, int userId)
        {
            if (CheckStreamName())
                throw new StreamNotFoundException();

            UserNameChanged userNameChanged = new()
            {
                NewName = newName,
                UserId = userId
            };
            events.Add(userNameChanged);
        }
        //Kullanıcı email onaylandığında
        public void EmailApproved(int userId)
        {
            if (CheckStreamName())
                throw new StreamNotFoundException();

            UserEmailApproved userEmailApproved = new()
            {
                UserId = userId
            };
            events.Add(userEmailApproved);
        }
    }
}
