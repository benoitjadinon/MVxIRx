
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

- MVVM Issues
    - each property updates the view at own time
        - refresh after an error : loading == true AND error != nil

- combinelatest(sink1, othersink).subscribe(stateBehaviorSubject);

- ViewState is a statemachine ?

- les views sont testables avec des states tout faits, c'est juste des valeurs dans un POCO
    - un viewmodel est bcp plus chaud à faker, car il contient souvent de la logique business (et c'est mal)

- idée pour pas avoir de présenter mais une sorte de presenter-bloc-vm ?
    
    - `BaseStateBloc<T:MonState>`
        - `RenderStream<T> { get };`

    - `HomePresenterBloc<HomeState> : BaseStateBloc`
        - `RefreshSink(blah);`
    
    - inconvénient : la vue doit subsribe sur le render mais c'est le seul truc qu'elle a à faire

    - ViewState [ios-architecture-an-state-container-based-approach](https://jobandtalent.engineering/ios-architecture-an-state-container-based-approach-4f1a9b00b82e)

- `AppModel<AppState> ??
    AppState{
        isLoggedIn ? -> trigger l'affichage de authcontroller ?
    }`
	- comme ça, on peut mocker et aller direct où il faut

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