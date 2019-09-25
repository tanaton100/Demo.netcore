
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Demo.Models;
using Demo.Repository;

namespace Demo.Services
{
    public interface IUserService
    {
        IEnumerable<Users> GetAllUsers();
        Users GetByIdUsers(int id);
        Users AddUsers(Users users);
        Users UpdateUser(Users users);
        bool DeleteUserById(int id);
        string Login(string userName, string pass);
        string ChangePass(string pass, int id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public string Login( string userName, string pass)
        {
            var passEn = GetHash(pass);
            var user = _userRepository.Login(userName, passEn);
            return user.Id>0 ? "Suscess" : "Fail";
        }

        public string ChangePass(string pass ,int id)
        {
            var passEn = GetHash(pass);
            var user = _userRepository.ChangePass(passEn ,id);
            return user > 0 ? pass : "Fail";
        }

        public IEnumerable<Users> GetAllUsers()
        {
            var users = _userRepository.GetAll().ToList();
            return users;
        }

        public Users GetByIdUsers(int id)
        {
            return _userRepository.FindById(id);
        }

        public Users AddUsers(Users users)
        {
            var checkDuplicate = ValidateUserNameExist(users);

            if (checkDuplicate)
                throw new Exception("DuplicateName");
            var passHash = GetHash(users.Password);
            users.Password = passHash;
            var id = _userRepository.Add(users);
            users.Id = id;
            return id > 0 ? users : new Users();
        }

        public Users UpdateUser(Users users)
        {
            var checkDuplicate = ValidateUserNameExist(users);

            if (checkDuplicate)
                throw new Exception("DuplicateName");
            var passHash = GetHash(users.Password);
            users.Password = passHash;
            var result = _userRepository.Update(users) > 0;
            if (!result)
            {
                throw new Exception("Cannot Update");
            }

            var viewResult = new Users
            {
                Email = users.Email,
                Id = users.Id,
                LastName = users.LastName,
                UserName = users.UserName,
                FirstName = users.FirstName,
                Password = users.Password,
                Tel = users.Tel
            };
            return viewResult;
        }

        public bool DeleteUserById(int id)
        {
            return _userRepository.Delete(id) > 0;
        }

        private bool ValidateUserNameExist(Users users)
        {
            var userList = _userRepository.GetAll();
            var duplicated = userList.Where(d => d.UserName.ToLower().Trim() == users.UserName.ToLower().Trim() && d.Id != users.Id);

            return duplicated.Any();
        }

        private string GetHash(string password)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
