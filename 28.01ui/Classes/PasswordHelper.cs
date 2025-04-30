using System;
using System.Security.Cryptography;
using System.Text;

namespace _28._01ui.Classes
{
	public static class PasswordHelper
	{
		public static string HashPassword(string password)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
				return Convert.ToBase64String(hashedBytes);
			}
		}

		public static bool VerifyPassword(string enteredPassword, string storedHash)
		{
			var hashOfEnteredPassword = HashPassword(enteredPassword);
			return hashOfEnteredPassword == storedHash;
		}
	}
}
