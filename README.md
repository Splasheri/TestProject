# Используемый паттерн.
Используется паттерн MVP, где presenter отслеживает изменения в модели, вносит изменения во вью, получает от вью данные о коллизии и сообщает об этом модели. В MVP подразумевается, что презентер должен плучать от вью данные об инпуте, но я посчитал, что в данном примере будет удобнее реализовать инпут непосредственно в презентере.

Модель наследуется от базового класса SpaceObjectModel, в котором реализованы методы UpdatePosition(изменение позиции), OutOfBounds(выход за рамки экрана) и виртуальный метод Save(пример бинарной сериализации класса в строку, которую можно сохранить с помощью PlayerPrefs. Не используется.). Если у модели должен быть показатель здоровья, то используется интерфейс ISpaceHp, который в свою очередь наследуется от интерфейса ISpaceDestroyable - интерфейса, позволяющего отправлять сообщения об уничтожении объекта.

Вью наследуется от базового класса SpaceObjectView, который наследуется от MonoBehaviour и в котором реализованы функции коллизии, самоуничтожения и изменения позиции на экране. Есть также один уникальный вью, служащий исключительно для изменения количества набранных очков.

Уничтожение объектов, выстрелы, добавление очков и завершение игры реализованы с помощью системы сообщений.
Используется библиотека UniRx.

# Используемые сущности.

Spaceship - имеет показатель хп, позицию на экране, может стрелять. При коллизии с астероидом корабль теряет хп. При уничтожении - отправляет сообщение SpaceshipPresenter и GameManager. Первый уничтожает вью корабля, второй завершает игру. Корабль перемещается клавишами wasd или стрелочками. Выстрел произовится нажатием на пробел. При выстреле презентер отправляет сообщение BulletPullPresenter, который создает экземпляр пули. Стартовое положение пули передано в сообщении.

Астероид - имеет показатель хп, тип астероида, позицию на экране. При коллизии с пулей астероид теряет хп. При уничтожении - отправляет сообщение AsteroidPresenter, тот уничтожает view и отправляет сообщение AsteroidPullPresenter, AsteroidPullPresenter удаляет AsteroidPresenter из реактивной коллекции и тот самоуничтожается.

Пуля - имеет позицию на экране. При коллизии с астероидом самоуничтожается. Механизм уничтожения такой же как и у астероида.

Гейм менеджер. При запуске создает презентер пула астероидов, презентер пула пуль, презентер корабля. Текущий уровень передается с помощью статической переменной класса LvlManager, он влияет на размер астероида, его хп и скорость спавна.  Спавнер астероидов реализован с помощью подписки на таймер. Если игрок набирает достаточное количество очков или корабль игрока погибает , то вызывается GamеOver(true или false соответственно). Этот метод останавливает игру, путем рассылки сообщений гейм овер, которые диспозят все подписки у всех презентеров. Кроме этого появляется окно с соответствующей надписью и кнопка "выйти в меню".

Главное меню. В нем есть четыре кнопки(три уровня и выход) и LvlManager. Лвл менеджер регулирует какие уровни доступны игроку с помощью переменной сохраненной в PlayerPrefs. При нажатии на кнопку уровня загружается сцена с геймплеем и статическая переменная получает значение соответствующее номеру уровня. 
