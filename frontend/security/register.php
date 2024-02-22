<?php
// Проверяем, была ли отправлена форма регистрации
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Проверяем, что все необходимые поля заполнены
    if (!empty($_POST['username']) && !empty($_POST['password'])) {
        // Получаем данные из формы
        $username = $_POST['username'];
        $password = $_POST['password'];
        
        // Проверяем, существует ли файл для хранения пользователей
        $usersFile = '../users.txt';
        
        // Открываем файл для добавления нового пользователя
        $file = fopen($usersFile, "a");
        if ($file) {
            // Формируем строку с данными о пользователе для записи в файл
            $userData = $username . '|' . $password . "\n";
            
            // Записываем данные в файл
            fwrite($file, $userData);
            
            // Закрываем файл
            fclose($file);
            
            // Перенаправляем пользователя на страницу авторизации
            header("Location: ../index.php");
            exit;
        } else {
            $errorMessage = "Ошибка при открытии файла.";
        }
    } else {
        $errorMessage = "Необходимо заполнить все поля формы.";
    }
}
?>

<!DOCTYPE html>
<html lang="ru">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Регистрация</title>
    <link rel="stylesheet" href="../css/styles.css">
</head>
<body>
    <header>
        <h1>Регистрация</h1>
    </header>
    <main>
        <?php if (isset($errorMessage)) : ?>
            <p><?php echo $errorMessage; ?></p>
        <?php endif; ?>
        <form action="<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>" method="post">
            <label for="username">Имя пользователя:</label><br>
            <input type="text" id="username" name="username" required><br>
            <label for="password">Пароль:</label><br>
            <input type="password" id="password" name="password" required><br>
            <button type="submit">Зарегистрироваться</button>
        </form>
        <form action="../index.php">
             <button type="submit">Войти</button>
        </form>
    </nav>
    </main>
</body>
</html>
