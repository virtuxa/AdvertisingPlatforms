# Рекламные площадки

REST API для подбора рекламных площадок по регионам.

## Подготовка

1. Восстановите зависимости:
```bash
dotnet restore
```

## Запуск

```bash
dotnet run
```
Сервер запустится на http://localhost:5234

## API

1. Загрузка площадок:
```bash
curl -X POST -F "file=@platforms.txt" http://localhost:5234/api/advertisingplatforms/upload
```

2. Поиск по локации:
```bash
curl http://localhost:5234/api/advertisingplatforms/search?location=/ru/msk
```

## Формат файла с площадками
```
Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
Крутая реклама:/ru/svrd 
```