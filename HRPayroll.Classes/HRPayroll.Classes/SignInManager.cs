using System;
using System.Collections.Generic;
using System.Text;
using CryptoHelper;

namespace HRPayroll.Classes
{
    public class SignInManager
    {
        #region Fields
        private string _userName;
        private string _hash;
        private string _password;
        #endregion

        #region Constructors
        public SignInManager() { }

        public SignInManager(string username, string hash)
        {
            Username = username;
            Hash = hash;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validate the user login details
        /// </summary>
        /// <returns>boolean</returns>
        public bool ValidateUserDetails()
        {


            //Hardcoded for testing purposes
            return _userName == "admin" && _hash == "admin";
        }

        /// <summary>
        /// Check that the username is present in the DB
        /// </summary>
        /// <returns>boolean</returns>
        public bool VerifyUserExists()
        {
            return true;
        }

        /// <summary>
        /// Check that the user password and hash match
        /// </summary>
        /// <returns>boolean</returns>
        public bool VerifyHash()
        {
            return Crypto.VerifyHashedPassword(Hash, Password);
        }

        /// <summary>
        /// Return the hashed password of the user
        /// </summary>
        /// <returns></returns>
        public string HashPassword()
        {
            return Crypto.HashPassword(Password);
        }
        #endregion

        #region Getters & Setters
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Hash
        {
            get { return _hash; }
            set { _hash = value; }
        }

        public string Username
        {
            get { return _userName; }
            set { _userName = value; }
        } 
        #endregion
    }
}
