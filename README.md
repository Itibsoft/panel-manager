# Panel Manager

<span style="background-color: rgb(0, 0, 0); color: rgb(255, 255, 255);">Внимание: Докуметация устаревшая, требует обновление информации (можно прочесть для понятия основ работы с системой, так как core остался преженим)</span>  
  
Это система предназначена для работы с User Interfaces (UI). Позволяет создавать интерфейсы не думаю о том, как вызвать то или иное окно, так как всем занимается менеджер.

**Фич лист:**

- [x] Паттерн работы с панелями на основе View + Controller
- [x] Поддерживает Window, Overlay
- [x] Кэширование панелей
- [x] Панели имеют свои методы выполнения
- [x] Поддерживает расширение функционала подменой
- [x] Система Event Callback
- [x] Методы неявного вызовы (Constructor, OnOpen, OnClose)
- [x] Добавить систему калбеков для View
- [ ] Transitions (Способы перемещения, открытия, закрытия)
- [x] Отслеживание состояний панелей
- [ ] Отслеживание текущей открытой панели по ее типу
- [ ] Разделение по группам
- [ ] Создание новых канвасов для конкретных групп
- [x] Order Layers для панелей
- [x] Сделать реализацию работы на Addressables
- [x] Сделать освобождение памяти (Release)
- [x] SafeArea
- [x] Кеширование рефлексии
- [x] Оптимизация обработчика панелей
- [x] Реализация работы с Zenject
- [ ] Реализация работы с VContainer
- [X] Реализация работы с Reflex
- [ ] Реализация работы с UniRx (Добаваить методы расширения)

#### **Интеграция пакета**

Чтобы интегрировать систему в проект, достаточно через Package Manager добавить ссылку на пакет. Это обеспечивает чистоту проекта, гибкость измнения пакета, так как есть возможность нажать одну кнопку "Update" для получения самой актуальной версии.  
  
**Ссылка на пакет: [https://github.com/Itibsoft/panel-manager.git](https://github.com/Itibsoft/panel-manager.git)**

**[![Screenshot_2.jpg](http://docs.itibsoft.ru/uploads/images/gallery/2023-09/scaled-1680-/screenshot-2.jpg)](http://docs.itibsoft.ru/uploads/images/gallery/2023-09/screenshot-2.jpg)**

#### **Начало работы**

##### **Создание PanelManager**

Для того, чтобы получить экземпляр IPanelManger через который уже и будет происходить управление панелями, нужно его создать через `PanelManagerBuilder`. У него есть методы, которые можно выполнить по желанию. Они требуются для настройки зависимостей PanelManger'a.

```c#
public class PanelManagerBuilder
{
      private IPanelControllerFactory _panelControllerFactory;
      private PanelDispatcher _panelDispatcher;
      
      public static PanelManagerBuilder Create()
      {
          return new PanelManagerBuilder();
      }
  
      public PanelManagerBuilder SetupPanelDispatcher(PanelDispatcher panelDispatcher)
      {
          _panelDispatcher = panelDispatcher;
  
          return this;
      }
  
      public PanelManagerBuilder SetupPanelControllerFactory(IPanelControllerFactory panelControllerFactory)
      {
          _panelControllerFactory = panelControllerFactory;
  
          return this;
      }
  
      public PanelManager Build()
      {
          return new PanelManager(_panelControllerFactory, _panelDispatcher);
      }
 }
```

`<strong>Create()</strong>` - создает `PanelManagerBuilder`. В котором уже можно будет создать сам объект IPanelManager.

`<strong>SetupPanelDispacher(PanelDispatcher panelDispatcher)</strong>` - это метод, который позволяет указать экземпляр PanelDispacher. Это просто MonoBehaviou объект, который управляет панелями, перемещая их между Window, Overlay, Cached контентами. По умолчанию, если его не указать, то PanelManager создаст его самостоятельно. В данном случае это просто Canvas объект с определенными настройками, в котором уже в виде чилдов находятся пустые объекты.

[![Screenshot_3.jpg](http://docs.itibsoft.ru/uploads/images/gallery/2023-09/scaled-1680-/screenshot-3.jpg)](http://docs.itibsoft.ru/uploads/images/gallery/2023-09/screenshot-3.jpg)

**`SetupPanelControllerFactory(IPanelControllerFactory panelControllerFactory)`** - это метод, который позволяет заменить фабрику, которая создает создает панель и для нее котроллер. По стандарту используется фабрика, которая ищет панель по AssetId в Resources. После этого спавнить и создает по конкретному типу контроллер и этому контроллеру прокидывает эту панель для управления.

Кастомная логика фабрики может быть полезна, если вы хотите поменять систему загрузки панелей. Например перейти с Resources на Addressables. Что очень важно, когда идет речь о управлении памяти.

```c#
public class DefaultPanelControllerFactory : IPanelControllerFactory
{
    public TPanelController Create<TPanelController>(PanelAttribute meta) where TPanelController : IPanelController
    {
        var type = typeof(TPanelController);
        
        var panelPrefab = Resources.Load<PanelBase>(meta.AssetId);

        if (panelPrefab == default)
        {
            throw new Exception($"Not found asset in Resources for path: {meta.AssetId}");
        }

        var panel = Object.Instantiate(panelPrefab);
        
        var extraArguments = new object[]
        {
            panel
        };

        var controller = Activator.CreateInstance(type, extraArguments);

        return (TPanelController)controller;
    }
}
```

**`Build() `-** этот метод создает экземпляр IPanelManger с уже прокинутыми настройками (если не прокидывали, то будут дефолтные)  
  
**Пример использования c базовой логикой:**

```c#
private class StartApp
{
    //В большинстве случаев это не требуется делать, так как это просто объект, который управляет панелями на сцене
    //Можно разширить класс PanelDispatcher, просто наследуясь от него (Если нужна кастомная логика)
    [SerializeField] private PanelDispatcher _panelDispatcher; //Наш кастомные PanelDispatcher
  
    private void Start()
    {
        //Создаем IPanelManager со стандартным настройками
        var panelManagerDefault = PanelManagerBuilder
          .Create() //Создаем экземпляр PanelManagerBuilder 
          .Build(); //Создаем экземпялр IPanelManger
    
        //Создаем IPanelManager с кастымными настройками
        var panelManagerCustom = PanelManagerBuilder
          .Create() //Создаем экземпляр PanelManagerBuilder 
          .SetupPanelDispatcher(_panelDispatcher) //Устанавливаем кастомный PanelDispatcher
          .SetupPanelControllerFactory(new CustomPanelControllerFactory()) //Устанавливаем кастомную фабрику
          .Build(); //Создаем экземпялр IPanelManger
    }
  
}

//Кастомная фабрика, которая загружает префаб панели, создает ее инстанс и после создается контроллер, передав ему панель.
private class CustomPanelControllerFactory : IPanelControllerFactory
{
    public TPanelController Create<TPanelController>(PanelAttribute meta) where TPanelController : IPanelController
    {
        var panelPrefab = Resources.Load<PanelBase>(meta.AssetId);
        
        var panel = Instantiate(panelPrefab);

        var controller = Activator.CreateInstance(typeof(TPanelController), new { panel });

        return (TPanelController)controller;
    }
}
```

##### **Создание Panel (View)**

Панелей может быть на данный момент только два вида (Window, Overlay), но настройка типа панели происходит уже на этапе создания контроллера, поэтому там это и разберем подробнее.

View часть панели не имеет никакой логики, это просто интерфейс, который может обрабатывать все отклики от пользователя, например нажатие на кнопку, ввод, скролл и так далее.

Поэтому чтобы создать панель, достаточно наследоваться от класса PanelBase, который является наследником MonoBehaviour

```c#
//Class которые создается для View панели, под тип Window 
//Обычно дописываем в конце название типа, чтобы сразу иметь понимание, что это View
public class TestWindow : PanelBase //Базовый класс, которые определяет поведение панели
{
    //Логика обработки взаимодействия с панелью
}

//Class которые создается для View панели, под тип Overlay 
//Обычно дописываем в конце название типа, чтобы сразу иметь понимание, что это View
public class TestOverlay : PanelBase //Базовый класс, которые определяет поведение панели
{
    //Логика обработки взаимодействия с панелью
}
```

Однако, при работе с панелью, использование методов, таких как Awake, Start, OnEnable, OnDisable - не рекомендуется. Так как у панелей своий цикл инитициализаций. Поэтому мы предоставили для вас, альтернативные методы, которые можете использоваться.  
  
**Рекомендация:** Всем этим методам нужно добавить аттрибут `[UsedImplicitly]`, так как эти методы вызываются через рефлексию самостоятельно. И чтобы Rider понимал, что эти методы вызываются не явно.

1. **Constructor**  
    Это метод, который вызывается только раз, при создании панели. В нем можно выполнять ту логику, которую вы обычно выполняли в Awake или Start.  
      
    ```c#
    public class TestWindow : PanelBase
    {
        [UsedImplicitly]
        private void Constructor() //Данный метод вызывается при первом открытии панели
        {
            Debug.Log("TestWindow.Constructor");
        }
    }
    ```
2. **OnOpen** Это метод, который вызывается каждый раз, когда панелька открывается. Удобно делать подписку на какие-то ивенты (Например подписка на нажатие кнопки и тд). Аналог метода **OnEnable**.  
      
    ```c#
    public class TestWindow : PanelBase
    {
        [UsedImplicitly]
        private void OnOpen() //Данный метод взывается каждый раз при открытии панели
        {
            Debug.Log("TestWindow.OnOpen");
        }
    }
    ```
3. **OnClose**  
    Это метод, который вызывается каждый раз, когда панелька закрывается. Удобно делать отписку от всех ивентов (Например отписаться от нажатия на кнопку и тд). Аналог метода **OnDisable.** ```c#
    public class TestWindow : PanelBase
    {
        [UsedImplicitly]
        private void OnClose() //Данный метод взывается каждый раз при закрытии панели
        {
            Debug.Log("TestWindow.OnClose");
        }
    }
    ```
4. **Dispose** Этот метод можно переопределить, если нужно выполнить какую-то логику при полном уничтожении панели.  
    Когда панелька закрывается, то она просто кешируется и потом открывается при запросе внось. Если же мы делаем полное уничтожение через **Relese** (Об этом потом будет расписано), то панель освобождает память и уничтожается, но перед этим вызывает метод **Dispose**. Аналог метода **OnDestory.** ```c#
    public class TestWindow : PanelBase
    {
        public override void Dispose() //Данный метод вызывается при полном уничтожении панели
        {
            //Рекомендуется оставлять базовый вызов
            //Так как в будущих версиях PanelManager, мы можем туда вносить какую-то логику.
            base.Dispose(); 
        }
    }
    ```

##### **Создание PanelController (Controller)**

Вся логика, которая должна происходить при взаимодействием с панелью (View), должна обрабатываться исключительно в контроллере, так как это отделяет представление от логики, что делает код стабильнее.

Контороллер не только обрабатывает логику панели, но и определяется ее вид, через мета информацию, посредоством аттрибута.

Так же контроллер является обычным классом, который наследует бозовую логику абстрактного класса `PanelControllerBase<TPanelView>`.

```c#
//Мы создаем класс, под конкретную панель, где будет обрабатываться вся логика
public class TestWindowController : PanelControllerBase<TestWindow> //Джинериком передается тип панели, которую будет обрабатывать
{
    public TestWindowController(TestWindow panel) : base(panel)
    {
        //Инициализация
    }

    //Логика обработки панели...
}
```

Над классом контроллера нужно обязательно подписывать аттрибут: `[Panel]`  
Он требуется для того, чтобы задать некоторую необходимую мета информацию для работы PanelManager.

Аттрибут принимает в себя:

1. **PanelType** - Это тип панели (В зависимости от него, панель в **PanelDispatcher** будет находиться в соответсвуюещем контенте) 
    - **Window** - Панель, которая в данный момент является главной. Обычно она не закрывается, до перехода на другое окно.
    - **Overlay** - Панель, которая открывается поверх **Window**. Вспомогательные окна, поп-апы.
2. <div>**AssetId** - Это адресс по которому будет загружаться панель. По стандартной фабрике, это тот путь, по которому будет браться панелька из папки Resources. Тот самый путь, который вы бы писали, использую Resources.Load&lt;PanelBase&gt;(assetId);</div>

```c#
//Добавляем мета информацию.
[Panel(PanelType = PanelType.Window, AssetId = "Windows/Test")]
public class TestWindowController : PanelControllerBase<TestWindow>
{
    public TestWindowController(TestWindow panel) : base(panel)
    {
    }
}
```
