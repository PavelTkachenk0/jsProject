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
    <title>Контакты - МГТУ им. Баумана</title>
    <link rel="stylesheet" href="../css/styles.css">
    <link rel="icon" href="../images/favicon.ico" type="image/x-icon">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script>
    $(document).ready(function() {
    // Функция для отправки данных формы при отправке сообщения
    $("#contact_form").submit(function(event) {
        event.preventDefault(); // Предотвращаем стандартное поведение отправки формы
        
        // Получаем данные из формы и создаем объект JavaScript
        var formData = {
            name: "<?php echo isset($_SESSION['username']) ? $_SESSION['username'] : '' ?>",
            email: $("#email").val(),
            message: $("#message").val(),
        };
        
        // Преобразуем данные в формат JSON
        var jsonData = JSON.stringify(formData);
        
        // Отправляем асинхронный POST-запрос на сервер для отправки сообщения
        $.ajax({
            type: "POST",
            url: "http://localhost:5131/api/contact", // URL сервера для отправки сообщений
            data: jsonData,
            contentType: "application/json", // Указываем тип контента как JSON
            success: function(response) {
                // Обработка успешного ответа от сервера
                console.log(response);
                // Очищаем поля формы после успешной отправки сообщения
                $("#email").val("");
                $("#message").val("");
                // Загружаем обновленные сообщения
                loadMessages();
            },
            error: function(xhr, status, error) {
                // Обработка ошибки при отправке сообщения
                console.log("Ошибка отправки сообщения:");
                console.log(error);
            }
        });
    });
});

    </script>
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
            <li><a href="contact.php">Контакты</a></li>
            <li><a href="../security/logout.php">Выход</a></li>
        </ul>
    </nav>
    <main>
        <h2>Контакты</h2>
        <p><strong>Адрес:</strong> Москва, 2-я Бауманская улица, д. 5</p>
        <p><strong>Телефон:</strong> +7 (495) 500-00-00</p>
        <p><strong>Email:</strong> info@bmstu.ru</p>

        <!-- Форма для отправки сообщения -->
        <div class="contact-form">
            <h2>Отправить сообщение</h2>
            <form id="contact_form" method="post">
                <label for="email">Ваш email:</label><br>
                <input type="text" id="email" name="email" required placeholder="email"><br>
                <label for="message">Сообщение:</label><br>
                <textarea id="message" name="message" rows="4" required placeholder="сообщение"></textarea><br>
                <button type="submit">Отправить</button>
            </form>
        </div>
        <div>
            <?php
                echo "<form action=\"messages.php\" method\"get\">";
                echo "<button type=\"pick\">Просмотр сообщений</button>";
                echo "</form>";
            ?>
        </div>
    </main>
    <footer>
        &copy; 2024 МГТУ им. Баумана. Все права защищены.
    </footer>
</body>
</html>
