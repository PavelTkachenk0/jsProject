<?php
session_start();

// Проверяем, был ли пользователь авторизован
if (!isset($_SESSION['username'])) {
    // Если пользователь не авторизован, возвращаем ошибку
    echo "error";
    exit;
}

// Получаем данные курса для удаления
$courseData = $_POST['courseData'];

// Открываем файл для чтения и записи
$file = fopen("courses.txt", "r+");
if ($file) {
    // Считываем строки файла
    $data = file("courses.txt");

    // Ищем строку курса для удаления
    foreach ($data as $key => $line) {
        if ($line == $courseData) {
            // Удаляем строку из массива данных
            unset($data[$key]);
            break;
        }
    }

    // Перезаписываем файл без удаленной строки
    file_put_contents("courses.txt", implode("", $data));

    // Закрываем файл
    fclose($file);

    // Возвращаем успешный статус удаления
    echo "success";
} else {
    // В случае ошибки при открытии файла возвращаем ошибку
    echo "error";
}
?>
