using System;
using System.Reactive.Subjects;

namespace MVxIRx.Core.ViewModels.Login
{
    public interface ILogin
    {
        void LogIn(string username, string password);
        void LogOut();
    }


    public interface ILoginBloc : IStatefulBloc<ILoginState>, ILogin
    {
    }


    public class LoginBloc : BaseStatefulBloc<ILoginState>, ILoginBloc
    {
        public LoginBloc()
            : base(initialState:new LoggedOutState())
        {
        }

        public void LogIn(string username, string password)
        {
            if (username == "test" && password == "test")
                SetState(new LoggedInState(username));
            else
                SetState(new LoginErrorState());
        }

        public void LogOut() => SetState(new LoggedOutState());
    }


    public interface ILoginState {}

    public class LoggedInState : ILoginState
    {
        readonly string _username;

        public LoggedInState(string username)
        {
            _username = username;
        }
    }

    public class LoginErrorState : LoggedOutState
    {
        readonly Exception _exception;

        public LoginErrorState(Exception exception = null)
        {
            _exception = exception;
        }
    }

    public class LoggedOutState : ILoginState {}
}
