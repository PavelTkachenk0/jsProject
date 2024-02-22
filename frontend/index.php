<!DOCTYPE html>
<html lang="ru">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Главная - МГТУ им. Баумана</title>
    <link rel="stylesheet" href="./css/styles.css">
    <link rel="icon" href="../images/favicon.ico" type="image/x-icon">
</head>
<body>
    <header>
        <h1>МГТУ им. Баумана</h1>
        <img src="./images/university_icon.png" alt="МГТУ им. Баумана">
    </header>
    <nav>
        <ul>
            <li><a href="index.php">Главная</a></li>
            <li><a href="./about/about.php"> О нас</a></li>
            <li><a href="./courses/courses.php">Курсы</a></li>
            <li><a href="./contact/contact.php">Контакты</a></li>
            <li><a href="./security/logout.php">Выход</a></li>
        </ul>
    </nav>
    <main>
        <?php
        // Проверяем, авторизован ли пользователь
        session_start();
        if (isset($_SESSION['username'])) {
            // Пользователь авторизован, отображаем содержимое страницы
            echo "<h2>Добро пожаловать, {$_SESSION['username']}!</h2>";
            echo "<p>МГТУ им. Баумана - один из ведущих технических университетов России.</p>";
            echo "<img src=\"./images/university_image.jpg\" width=\"1500px\" alt=\"Кампус МГТУ им. Баумана\">";
        } else {
            if(isset($_SESSION['error'])){
                echo "<h3> {$_SESSION['error']}</h3>";
            }
            // Пользователь не авторизован, отображаем форму входа
            echo "<div>";
            echo "<h2>Авторизация</h2>";
            echo "<form action=\"./security/login.php\" method=\"post\">";
            echo "<label for=\"username\">Имя пользователя:</label><br>";
            echo "<input type=\"text\" id=\"username\" name=\"username\" required><br>";
            echo "<label for=\"password\">Пароль:</label><br>";
            echo "<input type=\"password\" id=\"password\" name=\"password\" required><br>";
            echo "<button type=\"submit\">Войти</button>";
            echo "</form>";
            echo "<form action=\"./security/register.php\" method\"post\">";
            echo "<button type=\"submit\">Зарегистрироваться</button>";
            echo "</form>";
        }
        ?>
    </main>
    <footer>
        &copy; 2024 МГТУ им. Баумана. Все права защищены.
    </footer>
</body>
</html>
