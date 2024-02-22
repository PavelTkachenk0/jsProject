<?php
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Получаем данные из формы
    $course = $_POST['course'];
    $description = $_POST['description'];
    $duration = $_POST['duration'];
    $instructors = $_POST['instructors'];

    // Открываем файл для записи
    $file = fopen("courses.txt", "a");

    // Формируем строку с данными
    $data = "$course|$description|$duration|$instructors\n";

    // Записываем данные в файл
    fwrite($file, $data);

    // Закрываем файл
    fclose($file);
}

// Перенаправляем пользователя обратно на страницу курсов
header("Location: courses.php");
?>
