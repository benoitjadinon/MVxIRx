using System;
using System.Reactive.Subjects;

namespace MVxIRx.Core.ViewModels.Login
{
    public interface IAuth
    {
        void LogIn(string username, string password);
        void LogOut();
    }


    public interface IAuthBloc : IStatefulBloc<IAuthState>, IAuth
    {
    }


    public class AuthBloc : BaseStatefulBloc<IAuthState>, IAuthBloc
    {
        public AuthBloc()
            : base(initialState:new LoggedOutState())
        {
        }

        public void LogIn(string username, string password)
        {
            if (true /*username == "Elon" && password == "xxx"*/)
                SetState(new LoggedInState(username));
            else
                SetState<LoginErrorState>(username);
        }

        public void LogOut() => SetState<LoggedOutState>();
    }


    public interface IAuthState {}

    public class BaseAuthState : IAuthState
    {
        public string Username { get; private set; }

        public BaseAuthState(string username)
        {
            Username = username;
        }
    }

    public class LoggedInState : BaseAuthState
    {
        public LoggedInState(string username)
            : base(username)
        {
        }
    }

    public class LoginErrorState : BaseAuthState
    {
        public Exception Exception { get; private set; }

        public LoginErrorState(string username = null, Exception exception = null)
            : base(username)
        {
            Exception = exception;
        }
    }

    public class LoggedOutState : IAuthState {}
}
