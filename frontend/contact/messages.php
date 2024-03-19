<?php
session_start();

// Проверяем, был ли пользователь авторизован
if (!isset($_SESSION['username'])) {
    // Если пользователь не авторизован, перенаправляем его на страницу авторизации
    header("Location: ../index.php");
    exit;
}
?>

<!DOCTYPE html>
<html lang="ru">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Просмотр сообщений - МГТУ им. Баумана</title>
    <!-- Подключение Bootstrap CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <link rel="stylesheet" href="../css/styles.css">
    <link rel="icon" href="../images/favicon.ico" type="image/x-icon">
    <style>
        /* Стили для выравнивания заголовка по центру */
        .center-text {
            text-align: center;
        }
    </style>
</head>
<body>
    <header>
        <h1>МГТУ им. Баумана</h1>
        <img src="../images/university_icon.png" alt="МГТУ им. Баумана">
    </header>
    <nav>
        <ul>
            <li><a href="../index.php">Главная</a></li>
            <li><a href="../about/about.php"> О нас</a></li>
            <li><a href="../courses/courses.php">Курсы</a></li>
            <li><a href="../contact/contact.php">Контакты</a></li>
            <li><a href="../security/logout.php">Выход</a></li>
        </ul>
    </nav>
    <main>
        <!-- Заголовок выровненный по центру -->
        <h2 class="center-text">Просмотр сообщений</h2>
        <!-- Таблица для отображения сообщений -->
        <div class="container">
            <div class="row">
                <div class="col">
                    <table class="table">
                        <thead>
                            <tr>
                                <th scope="col">Имя</th>
                                <th scope="col">Email</th>
                                <th scope="col">Сообщение</th>
                            </tr>
                        </thead>
                        <tbody id="messages-container"></tbody>
                    </table>
                </div>
            </div>
        </div>
    </main>
    <footer>
        &copy; 2024 МГТУ им. Баумана. Все права защищены.
    </footer>

    <!-- Подключение jQuery и Bootstrap JS -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script>
        $(document).ready(function() {
            // Функция для загрузки сообщений с сервера
            function loadMessages() {
                $.ajax({
                    type: "GET",
                    url: "http://localhost:5131/api/contact", // URL для загрузки сообщений с сервера ASP.NET
                    success: function(messages) {
                        // Очищаем контейнер для сообщений перед загрузкой новых сообщений
                        $("#messages-container").empty();

                        // Получаем текущего пользователя
                        var currentUser = "<?php echo $_SESSION['username']; ?>";

                        // Вставляем каждое сообщение в контейнер
                        messages.forEach(function(message) {
                            // Проверяем, совпадает ли отправитель с текущим пользователем
                            if (message.name === currentUser) {
                                // Создаем строку для таблицы с данными сообщения
                                var row = "<tr>" +
                                            "<td>" + message.name + "</td>" +
                                            "<td>" + message.email + "</td>" +
                                            "<td>" + message.message + "</td>" +
                                          "</tr>";
                                // Вставляем строку в таблицу
                                $("#messages-container").append(row);
                            }
                        });
                    },
                    error: function(xhr, status, error) {
                        // Выводим сообщение об ошибке, если не удалось загрузить сообщения
                        $("#messages-container").html("<tr><td colspan='3'>Ошибка загрузки сообщений. Пожалуйста, попробуйте позже.</td></tr>");
                    }
                });
            }

            // Вызываем функцию загрузки сообщений при загрузке страницы
            loadMessages();
        });
    </script>

</body>
</html>
