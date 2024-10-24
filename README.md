# [ РУС ]
# Мультиагентная модель распространения вируса
## Описание:
Это программа на C#, реализующая симуляцию эпидемии с использованием системы сборки make и компилятора mcs (mono). Она использует WinForms и XNA (FNA) фреймворки для графического интерфейса. Вы можете настроить различные начальные параметры, такие как летальность заболевания, скорость передачи, продолжительность и популяция.
Приложение предлагает три разных сценария для увеличения исследовательского потенциала:
- симуляция одиночного сообщества,
- симуляция точки интереса (магазин),
- симуляция мультигруппового сообщества.
Пользователи могут наблюдать за распространением вируса в реальном времени и отслеживать прогресс через графики. В конце симуляции создается файл с логами.

## Требования:
- Компилятор C# - требуется `mono`
- `XNA` или `FNA` фреймворк
- команда `make` (опционально, для компиляции)

## Запуск на Windows:
В папке `/bin` запустите `start.vbs`

## Запуск на Linux:
Введите в консоль `make run`, находясь в директории программы

## Доступные команды make:
- `make` - собирает весь проект (приложение и тесты)
- `make compile` - собирает только приложение
- `make run` - запускает приложение
- `make test` - компилирует и запускает тесты
- `make compile_test` - компилирует только тесты

## Дополнительная информация:
- Создано в качестве программы для курсовой работы в 2024 г. Никитой Смирновым (РТУ МИРЭА)
- [Полезный гайд](https://www.youtube.com/watch?v=Jbld5ZrW3ls) на случай запуска в Windows

---


# [ ENG ]
# Multi-Agent Virus Spread Model
## Description:
This is a C# program that simulates an epidemic using the make build system and the mcs (mono) compiler. It utilizes WinForms and XNA (FNA) frameworks for the graphical user interface. You can configure various initial parameters, such as disease lethality, transmission rate, duration, and population size.
The application offers three different scenarios to enhance research potential:
- single community simulation,
- point of interest (store) simulation,
- multi-group community simulation.
Users can observe the virus spread in real-time and track progress through graphs. At the end of the simulation, a log file is created.

## Requirements:
- C# compiler - `mono` is required
- `XNA` or `FNA` framework
- `make` command (optional, for compilation)

## Running on Windows:
Run `start.vbs` in the `/bin` folder.

## Running on Linux:
Run `make run` from the program's directory.

## Available make commands:
- `make` - builds the entire project (application and tests)
- `make compile` - builds only the application
- `make run` - runs the application
- `make test` - compiles and runs tests
- `make compile_test` - compiles only tests

## Additional Information:
- Created as a course project in 2024 by Nikita Smirnov (RTU MIREA)
- [Helpful guide](https://www.youtube.com/watch?v=Jbld5ZrW3ls) for running on Windows

---
