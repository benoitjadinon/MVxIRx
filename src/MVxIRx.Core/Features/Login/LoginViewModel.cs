using System;

namespace MVxIRx.Core.ViewModels.Login
{
    public class LoginViewModel : BaseStatefulViewModel<ILoginViewModelBloc, LoginViewModelState>, IAuth
    {
        private readonly IAuthBloc _authBloc;

        public LoginViewModel(IAuthBloc authBloc)
        {
            _authBloc = authBloc;
        }

        #region ILogin

        public void LogIn(string username, string password) => _authBloc.LogIn(username, password);

        public void LogOut() => _authBloc.LogOut();

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
