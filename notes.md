
#IDEAS

##MVI
- user -> Intent -> model -> view -> user -> intent -> ... !!!
	- [introducing MVI @jsconf](https://www.youtube.com/watch?v=1zj7M1LnJV4)

- intent = source of data (click ui, backend data ,...)

- sink -> stream -> sink -> stream

```
	button           producer     observable
	textview         receiver       consumer
	textedit    producer&receiver    subject
```

##BLoC !!!

- user -> view -> bloc sink (intent ?) -> bloc stream -> view -> user

##ViewState
- http://hannesdorfmann.com/android/mosby3-mvi-1
    - classic : loading + error at the same time (because of bug in mvvm impl)
    - send last state to raygun
    - state history (keep last 3 states in memory ?)
    - state to json for comparison or send to raygun
    - state is IEquatable ?

- package by feature
    - /login/
        login(Navigation)Bloc.cs
        loginView.cs
        loginViewBloc.cs
      /formsnavigator.cs

- MVVM Issues
    - each property updates the view at own time
        - refresh after an error : loading == true AND error != nil

- use combinelatest in vm to reduce the number of setState calls
	- combinelatest(bloc1.sink1, bloc2.othersink).subscribe(setState);

- ViewState is a statemachine ?

- views are testable with hardcoded states, to work on ui before having business logic
    - a viewmodel is harder to fake, and usually contains business logic (and shouldn't)

- ViewState [ios-architecture-an-state-container-based-approach](https://jobandtalent.engineering/ios-architecture-an-state-container-based-approach-4f1a9b00b82e)

- `AppModel<AppState> ??
    AppState{
        isLoggedIn ? -> triggers show some auth screen ?
    }`
	- so that even the nav is mockable, and can show directly one specific screen
	- `userBloc.onUserObs.Where(u => u == null).Subscribe(app.showLoginPage());`
		- logout : `userBloc.logout() => user = null;` : will show the login screen

- blocs can export LoadableStates
    - multiple loadable states can be merged into the viewmodelbloc to group loadings together

- TODO : debug.writeline a state could show hierarchy and values

##Pseudo-code

```csharp
public enum Status {
    SUCCESS,
    ERROR,
    LOADING
}

public class Resource<T> {

    public final Status status;
    public final String message;
    public final T data;

    public Resource(@NonNull Status status, @Nullable T data, @Nullable String message) {
        this.status = status;
        this.data = data;
        this.message = message;
    }

    public static <T> Resource<T> success(@Nullable T data) {
        return new Resource<>(SUCCESS, data, null);
    }

    public static <T> Resource<T> error(String msg, @Nullable T data) {
        return new Resource<>(ERROR, data, msg);
    }

    public static <T> Resource<T> loading(@Nullable T data) {
        return new Resource<>(LOADING, data, null);
    }
}
```