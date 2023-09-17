# Panel Manager

Это система предназначена для работы с User Interfaces (UI). Позволяет создавать интерфейсы не думаю о том, как вызвать то или иное окно, так как всем занимается менеджер.

**Фич лист:**

- [x] Паттерн работы с панелями на основе View + Controller
- [x] Поддерживает Window, Overlay
- [x] Кэширование панелей
- [x] Панели имеют свои методы выполнения
- [x] Поддерживает работу с Zenject (Extenject)
- [x] Поддерживает работу с Addressables
- [x] Система Event Callback
- [ ] Transitions (Способы перемещения, открытия, закрытия)
- [ ] Отслеживание состояний панелей
- [ ] Отслеживание текущей открытой панели по ее типу

#### **Интеграция пакета**

Чтобы интегрировать систему в проект, достаточно через Package Manager добавить ссылку на пакет. Это обеспечивает чистоту проекта, гибкость измнения пакета, так как есть возможность нажать одну кнопку "Update" для получения самой актуальной версии.  
  
**Ссылка на пакет: [https://github.com/Itibsoft/panel-manager.git](https://github.com/Itibsoft/panel-manager.git)**

**[![Screenshot_2.jpg](http://docs.itibsoft.ru/uploads/images/gallery/2023-09/scaled-1680-/screenshot-2.jpg)](http://docs.itibsoft.ru/uploads/images/gallery/2023-09/screenshot-2.jpg)**

#### **Начало работы**

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