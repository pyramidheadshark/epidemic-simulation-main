# [ РУС ]
# Мультиагентная модель распространения вируса
## Описание:
Это программа на C#, реализующая симуляцию эпидемии с использованием системы сборки make и компилятора mcs (mono). Она использует WinForms и XNA (FNA) фреймворки для графического интерфейса. Вы можете настроить различные начальные параметры, такие как летальность заболевания, скорость передачи, продолжительность и популяция.
Приложение предлагает три разных сценария для увеличения исследовательского потенциала:
- симуляция одиночного сообщества,
- симуляция точки интереса (магазин),
- симуляция мультигруппового сообщества.
Пользователи могут наблюдать за распространением вируса в реальном времени и отслеживать прогресс через графики. В конце симуляции создается файл с логами.

## Доступные команды:
- `make` - собирает весь проект (приложение и тесты)
- `make compile` - собирает только приложение
- `make run` - запускает приложение
- `make test` - компилирует и запускает тесты
- `make compile_test` - компилирует только тесты

## Требования:
- Компилятор C# - рекомендуется использовать `mono`
- `XNA` или `FNA` фреймворк
- команда `make`

## Дополнительная информация:
- Создано в качестве программы для курсовой работы в 2024 г. Никитой Смирновым (РТУ МИРЭА)
- [Полезный гайд](https://www.youtube.com/watch?v=Jbld5ZrW3ls) на случай запуска в Windows

---


# [ ENG ]
# ABM Virus spreading model
## Description:
This app is a C# implementation of an epidemic simulation using a make system. It utilizes WinForms and XNA (FNA) frameworks for the GUI. You can set various initial parameters such as disease lethality, communicability, duration, and simulated population.
The app offers three different scenarios to enhance the user experience: 
- single community simulation,
- shopping community simulation,
- and multigroup community simulation.
Users can observe the spread of the disease in real-time and track progress through graphs. At the end of the simulation, a file containing logs is generated.

## Available commands
- `make` - builds the entire project (application and test)
- `make compile` - builds only the application
- `make run` - runs the application
- `make test` - compiles and runs test
- `make compile_test` - compiles only test

## Requirements:
- C# implementation - `mono` recommended
- `XNA` or `FNA` framework
- `make` command

## Additional info:
- Built as app for term paper in 2024 by Nikita Smirnov (RTU MIREA)
- [A useful guide](https://www.youtube.com/watch?v=Jbld5ZrW3ls) if you're building in Windows

---