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
            if (true/*username == "test" && password == "test"*/)
                SetState(new LoggedInState(username));
            else
                SetState<LoginErrorState>();
        }

        public void LogOut() => SetState<LoggedOutState>();
    }


    public interface IAuthState {}

    public class LoggedInState : IAuthState
    {
        public string Username { get; private set; }

        public LoggedInState(string username)
        {
            Username = username;
        }
    }

    public class LoginErrorState : LoggedOutState
    {
        public Exception Exception { get; private set; }

        public LoginErrorState(Exception exception = null)
        {
            Exception = exception;
        }
    }

    public class LoggedOutState : IAuthState {}
}
