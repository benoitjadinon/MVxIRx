using System;

namespace MVxIRx.Core.ViewModels.Login
{
    public class LoginViewModel : BaseStatefulViewModel<ILoginViewModelBloc, LoginViewModelState>, ILogin
    {
        private readonly ILoginBloc _loginBloc;

        public LoginViewModel(ILoginBloc loginBloc)
        {
            _loginBloc = loginBloc;
        }

        #region ILogin

        public void LogIn(string username, string password) => _loginBloc.LogIn(username, password);

        public void LogOut() => _loginBloc.LogOut();

        #endregion
    }


    public interface ILoginViewModelBloc : IStatefulBloc<LoginViewModelState>{}

    // ViewModel Bloc : used to combine blocs into one testable 'viewmodel' bloc,
    // so there is no logic in the viewmodel, can be reused as an upper level bloc somewhere else
    public class LoginViewModelBloc : BaseStatefulBloc<LoginViewModelState>, ILoginViewModelBloc {}


    public class LoginViewModelState
    {
    }
}
