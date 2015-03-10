using System;

namespace MCM.Ios.Classic
{
	public class LoginService
	{
		public LoginService ()
		{
			_loggedIn = true;
		}

		private bool _loggedIn;
		public bool LoggedIn {
			get {
				return _loggedIn;
			}
		}
	}
}

