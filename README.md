# Inclusive Map — CHILLGO

> Інтерактивна карта доступності міста Львів для людей з інвалідністю.

## Стек технологій:
- ASP.NET Core Web API
- MongoDB
- Razor Pages (Front)
- Google Maps API
- JWT Authentication (Bearer Token)

---

## Структура проекту

| Каталог / Файл | Опис |
|----------------|------|
| Controllers/ | API контролери (Auth, User, Point, Review, Transport, PointRequest, LoiRequest) |
| MongoDB/ | Collections (Моделі даних) + MongoDbContext |
| Repositories/ | CRUD репозиторії для кожної моделі (Interfaces + Implementations) |
| Pages/ | Razor Pages для UI (Login, Register, Profile, Index, Admin) |
| wwwroot/css/ | Стилі для фронту |
| Program.cs | Dependency Injection, JWT, MongoDB налаштування |
| appsettings.json | Конфігурація проєкту |

---

## Основний функціонал

### Авторизація та Реєстрація
- Register → POST /api/auth/register
- Login → POST /api/auth/login
- Авторизація через JWT токен
- Паролі зберігаються через BCrypt

---

### Користувач (User)
- Перегляд та редагування профілю
- Можна редагувати:
  - Ім'я
  - Пароль
  - Інклюзивність (з випадаючого списку)
- При виборі інклюзивності → роль користувача = inclusive
- Список збережених точок (Points)
- Збереження / Відміна збереження точки:
  - POST /api/user/savePoint/{id}
  - DELETE /api/user/removeSavedPoint/{id}

---

### Точки (Points)
- Отримання всіх точок → GET /api/point
- CRUD для admin
- Звичайний користувач може подати заявку на додавання точки (PointRequest)
- Автоматичний розрахунок LOI:
  - 0-3 → червоний маркер
  - 4-7 → синій маркер
  - 8-10 → зелений маркер
- ManualLOI → задається адміністратором при approve LoiRequest

---

### Відгуки (Reviews)
- CRUD для зареєстрованих користувачів
- Оцінка у вигляді зірок (1-5)
- Відображення усіх відгуків під Point

---

### PointRequest — заявки на додавання точки
- Створення → POST /api/pointrequest/create
- Approve / Reject → тільки admin
- При approve → додається Point в основну колекцію

---

### LoiRequest — заявки на зміну LOI
- Створення → POST /api/loirequest/create
- Approve / Reject → тільки admin
- При approve → оновлення ManualLOI в Point
- ManualLOI пріоритетніше за автоматичний LOI

---

### Маршрути (Routing)
- Побудова маршруту на мапі між точками A та B
- Google Maps Directions API
- Вибір типу маршруту:
  - Інклюзивний транспорт
  - Пішки
  - Будь-який транспорт
- Пріоритезація маршрутів з whitelist транспорту

---

## Ролі користувачів
| Роль | Можливості |
|------|------------|
| user | Звичайний користувач |
| inclusive | Може задавати рівень інклюзивності |
| admin | Повний CRUD по всіх моделях, схвалення заявок |

---

## Запуск проекту локально
1. Створити файл `appsettings.json`
2. Додати MongoDB connection string
3. Додати Google Maps API Key
4. Запуск:
```bash
dotnet run
