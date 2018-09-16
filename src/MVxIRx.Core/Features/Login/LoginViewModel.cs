using System;

namespace MVxIRx.Core.ViewModels.Login
{
    public class LoginViewModel : BaseStatefulViewModel<ILoginViewModelBloc, LoginViewModelState> { }


    public interface ILoginViewModelBloc : IStatefulBloc<LoginViewModelState>, IAuth {}

    // ViewModel Bloc : used to combine blocs into one testable 'viewmodel' bloc,
    // so there is no logic in the viewmodel, can be reused as an upper level bloc somewhere else
    public class LoginViewModelBloc : BaseStatefulBloc<LoginViewModelState>, ILoginViewModelBloc
    {
        private readonly IAuthBloc _authBloc;

        public LoginViewModelBloc(IAuthBloc authBloc)
        {
            _authBloc = authBloc;
        }

        #region IAuth

        public void LogIn(string username, string password) => _authBloc.LogIn(username, password);

        public void LogOut() => _authBloc.LogOut();

        #endregion
    }


    public class LoginViewModelState
    {
    }
}
