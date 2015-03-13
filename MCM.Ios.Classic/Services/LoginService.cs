using System;

namespace MCM.Ios.Classic
{
	public class LoginService : ILoginService
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

