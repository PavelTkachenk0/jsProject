<?php
session_start();

// Проверяем, была ли отправлена форма авторизации
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Проверяем, что все необходимые поля заполнены
    if (!empty($_POST['username']) && !empty($_POST['password'])) {
        // Получаем данные из формы
        $username = $_POST['username'];
        $password = $_POST['password'];
        
        // Проверяем, существует ли файл для хранения пользователей
        $usersFile = '../users.txt';
        if (file_exists($usersFile)) {
            // Открываем файл и ищем пользователя
            $file = fopen($usersFile, "r");
            if ($file) {
                $userFound = false;
                while (($line = fgets($file)) !== false) {
                    $data = explode('|', $line);
                    if (trim($data[0]) === $username && trim($data[1]) === $password) {
                        // Пользователь найден, устанавливаем флаг и завершаем цикл
                        $userFound = true;
                        break;
                    }
                }
                fclose($file);
                
                // Перенаправляем пользователя на главную страницу, если пользователь найден
                if ($userFound) {
                    $_SESSION['username'] = $username;
                    header("Location: ../index.php");
                    exit;
                } else {
                    $errorMessage = "Неверное имя пользователя или пароль.";
                }
            } else {
                $errorMessage = "Ошибка при открытии файла.";
            }
        } else {
            $errorMessage = "Файл пользователей не найден.";
        }
    } else {
        $errorMessage = "Необходимо заполнить все поля формы.";
    }
    $_SESSION['error'] = $errorMessage;
    header("Location: ../index.php");
    exit;
}
?>
